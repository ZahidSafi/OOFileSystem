using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OOFileSystem
{

    public class FileSystem
    {
        public static List<Entity> drives;

        public FileSystem()
        {
            drives = new List<Entity>
            {
                new Entity("Drives", "C:", "C:")
            };
        }

        public void Create(string Type, string Name, string ParentPath)
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
            parent.Entities.Add(child);
            child.Parent = parent;
        }

        public Entity TraverseFileSystem(string[] path)
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
                if (current.Type == "Text files" && current.Entities.Count != 0)
                {
                    throw new Exception("Illegal File System Operation");
                }
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

        public void Delete(string path)
        {
            string[] PathToRemove = path.Split('\\');
            Entity ToRemove = TraverseFileSystem(PathToRemove);
            Entity parent = ToRemove.Parent;
            parent.Entities.Remove(ToRemove);
        }


        public void Move(string SourcePath, string DestinationPath)
        {
            string[] source = SourcePath.Split('\\');
            string[] dest = DestinationPath.Split('\\');
            Entity Source = TraverseFileSystem(source);
            Entity Destination = TraverseFileSystem(dest);
            foreach (var entity in Destination.Entities)
            {
                if (Source.Path == entity.Path)
                {
                    throw new Exception("Path already exists");
                }
            }
            Destination.Entities.Add(Source);
            Source.Parent = Destination;
            Entity ToRemove = Source;
            Entity SourceParent = Source.Parent;
            SourceParent.Entities.Remove(ToRemove);
            UpdateEntitySize(SourceParent);
            UpdateEntitySize(Destination);
        }

        public void WriteToFile(string Path, string Content)
        {
            string[] path = Path.Split('\\');
            Entity entity = TraverseFileSystem(path);
            if (entity.Type != "Text files")
            {
                throw new Exception("Not a text file");
            }
            entity.Content = Content;
            entity.Size = Content.Length;
            UpdateEntitySize(entity.Parent);
        }

        public void UpdateEntitySize(Entity entity)
        {
            Entity temp = entity;
            while (temp.Parent != null)
            {
                temp.UpdateSize();
                temp = temp.Parent;
            }
        }
    }
}
