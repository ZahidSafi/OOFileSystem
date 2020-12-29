using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OOFileSystem
{

    public class FileSystem
    {
        public static Dictionary<string, Entity> drives;

        /// <summary>
        /// Initializes a file system with a starting "Drive" called C:
        /// </summary>
        public FileSystem()
        {
            drives = new Dictionary<string, Entity>
            {
                { "C:", new Entity("Drive", "C:", "C:") }
            };
        }
        /// <summary>
        /// Create either a Drive, Folder, Zip or Text at the given parent path. 
        /// If an empty string path is put in, and the type is "Drive" then we make it a drive.
        /// </summary>
        /// <param name="Type">Type of the entity to be created</param>
        /// <param name="Name">Name of the entity to be created</param>
        /// <param name="ParentPath">Location to create the entity</param>
        public void Create(string Type, string Name, string ParentPath)
        {
            if (Type == "Drive" && ParentPath.Length == 0)
            {
                if (drives.ContainsKey(Name))
                {
                    throw new Exception("Path already exists");
                }
                else
                {
                    drives.Add(Name, new Entity(Type, Name, Name));
                }
            }
            else if (Type == "Drive" && ParentPath.Length != 0)
            {
                //exception is thrown, as they are adding it as a child entity
                throw new Exception("Illegal File System Operation");
            }
            else
            {
                string[] path = ParentPath.Split('\\');
                Entity parent = TraverseFileSystem(path);
                Entity child = new Entity(Type, Name, ParentPath + "\\" + Name);

                //check if file exists already
                if (parent.Entities.ContainsKey(child.Name))
                {
                    throw new Exception("Path already exists");
                }
                if (parent.Type == "Text")
                {
                    //throw exception, if adding as a child of a text file
                    throw new Exception("Illegal File System Operation");
                }
                parent.Entities.Add(child.Name, child);
                child.Parent = parent;
            }
        }

        /// <summary>
        /// Traverse the file system to using the "path" to return the entity
        /// </summary>
        /// <param name="path">Location of the entity</param>
        /// <returns></returns>
        private Entity TraverseFileSystem(string[] path)
        {
            Entity drive;
            //loop through the drives to find the drive that matches
            if (drives.ContainsKey(path[0]))
            {
                drive = drives[path[0]];
            }
            else
            {
                throw new Exception("Path not found");
            }
            /*from here traverse the rest of the file path
             * and check to see if the entity name exists
             */
            Entity current = drive;
            for (int i = 1; i < path.Length; i++)
            {
                string EntityName = path[i];
                //check if path exists
                if (current.Entities.ContainsKey(EntityName))
                {
                    current = current.Entities[EntityName];
                }
                else
                {
                    throw new Exception("Path not found");
                }
            }
            return current;
        }

        /// <summary>
        /// Traverse the file path and remove the entity
        /// </summary>
        /// <param name="path">Location of entity to remove</param>
        public void Delete(string path)
        {
            string[] PathToRemove = path.Split('\\');
            Entity ToRemove = TraverseFileSystem(PathToRemove);
            Entity parent = ToRemove.Parent;
            parent.Entities.Remove(ToRemove.Name);
        }

        /// <summary>
        /// Traverse the file system to get both the destination 
        /// and source entity, and move source to destination. 
        /// Remove source entity from old file path once done.
        /// </summary>
        /// <param name="SourcePath">Location of entity to move</param>
        /// <param name="DestinationPath">Location to store it</param>
        public void Move(string SourcePath, string DestinationPath)
        {
            string[] source = SourcePath.Split('\\');
            string[] dest = DestinationPath.Split('\\');
            Entity Source = TraverseFileSystem(source);
            Entity Destination = TraverseFileSystem(dest);
            if (Destination.Entities.ContainsKey(Source.Name))
            {
                throw new Exception("Path already exists");
            }

            /*Here we want to store source, get its parent, 
             * and then remove source from the parents list of 
             * children
            */
            Entity ToRemove = Source;
            Entity SourceParent = ToRemove.Parent;
            SourceParent.Entities.Remove(ToRemove.Name);

            /*from here add source to destination, and update 
             * parents and path as needed
            */
            Source.Parent = Destination;
            Source.Path = Destination.Path + "\\" + Source.Name;
            UpdateChildFilePath(Source);
            Destination.Entities.Add(Source.Name, Source);
            /*
             * Update the sizes after the move
             */
            UpdateEntitySize(SourceParent);
            UpdateEntitySize(Destination);
        }


        /// <summary>
        /// Traverse the file system and write the content to the entity
        /// </summary>
        /// <param name="Path">File path for the entity</param>
        /// <param name="Content">Text for the file</param>
        public void WriteToFile(string Path, string Content)
        {
            string[] path = Path.Split('\\');
            Entity entity = TraverseFileSystem(path);
            if (entity.Type != "Text")
            {
                throw new Exception("Not a text file");
            }
            entity.Content = Content;
            entity.Size = Content.Length;
            UpdateEntitySize(entity.Parent);
        }


        /// <summary>
        /// Link list traversal and update the size of each entity 
        /// all the way up to the drives
        /// </summary>
        /// <param name="entity">The starting entity</param>
        public void UpdateEntitySize(Entity entity)
        {
            Entity temp = entity;
            while (temp != null)
            {
                temp.UpdateSize();
                temp = temp.Parent;
            }
        }

        /// <summary>
        /// Start from "entity" and use a breadth first search to traverse all the children and update
        /// all the file paths accordingly after a move.
        /// </summary>
        /// <param name="entity">starting entity for child entities</param>
        private void UpdateChildFilePath(Entity entity)
        {
            Entity temp = entity;
            Queue<Dictionary<string, Entity>> queue = new Queue<Dictionary<string, Entity>>();
            queue.Enqueue(temp.Entities);
            while (queue.Count != 0)
            {
                Dictionary<string, Entity> CurrentList = queue.Dequeue();
                foreach (var key in CurrentList.Keys)
                {
                    //iterate through and update the path, and add any children non empty children
                    Entity current = CurrentList[key];
                    current.UpdatePath();
                    if (current.Entities.Count != 0)
                    {
                        queue.Enqueue(current.Entities);
                    }
                }
            }
        }
    }
}
