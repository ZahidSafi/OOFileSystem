using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OOFileSystem
{

    public class FileSystem
    {
        public static List<Entity> drives;

        /// <summary>
        /// 
        /// </summary>
        public FileSystem()
        {
            drives = new List<Entity>
            {
                new Entity("Drives", "C:", "C:")
            };
        }
        /// <summary>
        /// Create either a Drive, Folder, Zip or Text at the given parent path. 
        /// If an empty string path is put in, and the type is "Drive" then we make it a drive
        /// </summary>
        /// <param name="Type">Type of the entity</param>
        /// <param name="Name">Name of the entity</param>
        /// <param name="ParentPath">Location to create the entity</param>
        public void Create(string Type, string Name, string ParentPath)
        {
            if (Type == "Drive" && ParentPath.Length == 0)
            {
                drives.Add(new Entity(Type, Name, Name));
            }
            else if (Type == "Drive" && ParentPath.Length != 0)
            {
                throw new Exception("Illegal File System Operation");
            }
            else
            {
                CheckFilePath(ParentPath);
                string[] path = ParentPath.Split('\\');
                Entity parent = TraverseFileSystem(path);
                Entity child = new Entity(Type, Name, ParentPath + "\\" + Name);
                foreach (var entity in parent.Entities)
                {
                    if (entity.Path == child.Path)
                    {
                        throw new Exception("Path already exists");
                    }
                }
                if (parent.Type == "Text")
                {
                    throw new Exception("Illegal File System Operation");
                }
                parent.Entities.Add(child);
                child.Parent = parent;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Entity TraverseFileSystem(string[] path)
        {
            Entity drive = null;
            //loop through the drives to find the drive that matches
            foreach (var d in drives)
            {
                if (d.Name == path[0])
                {
                    drive = d;
                    break;
                }
            }
            if (drive == null)
            {
                throw new Exception("Path not found");
            }
            Entity current = drive;
            for (int i = 1; i < path.Length; i++)
            {
                string EntityName = path[i];
                bool EntityFound = false;
                foreach (var entity in current.Entities)
                {
                    if (entity.Name == EntityName)
                    {
                        current = entity;
                        EntityFound = true;
                        break;
                    }
                }
                if (EntityFound == false)
                {
                    throw new Exception("Path not found");
                }
            }
            return current;
        }


        /// <summary>
        /// Iterate through the input path and make sure a drive 
        /// is not a child entity and to make sure its the 
        /// first entity in the path
        /// </summary>
        /// <param name="ParentPath">Input file path</param>
        public void CheckFilePath(string ParentPath)
        {
            string[] path = ParentPath.Split('\\');
            if (!Regex.IsMatch(path[0], "^[A-Z]:$"))
            {
                throw new Exception("Illegal File System Operation");
            }
            for (int i = 1; i < path.Length; i++)
            {
                if (Regex.IsMatch(path[i], "^[A-Z]:$"))
                {
                    throw new Exception("Illegal File System Operation");
                }
            }
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
            parent.Entities.Remove(ToRemove);
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
            foreach (var entity in Destination.Entities)
            {
                //check if file path exists
                if (Source.Name == entity.Name)
                {
                    throw new Exception("Path already exists");
                }
            }

            Entity ToRemove = Source;
            Entity SourceParent = ToRemove.Parent;//store the parent
            SourceParent.Entities.Remove(ToRemove);//remove the old source
            /*from here add source to destination, and update 
             * parents and path as needed
            */
            Source.Parent = Destination;
            Source.Path = Destination.Path + "\\" + Source.Name;
            UpdateChildFilePath(Source);
            Destination.Entities.Add(Source);
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
        /// along the way
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
            Queue<List<Entity>> queue = new Queue<List<Entity>>();
            queue.Enqueue(temp.Entities);
            while (queue.Count != 0)
            {
                List<Entity> CurrentList = queue.Dequeue();
                foreach (var ent in CurrentList)
                {
                    ent.UpdatePath();
                    if (ent.Entities.Count != 0)
                    {
                        queue.Enqueue(ent.Entities);
                    }
                }
            }
        }
    }
}
