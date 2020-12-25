using System;
using System.Collections.Generic;
using System.Text;

namespace OOFileSystem
{
    public class Entity
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public int Size { get; set; }

        public List<Entity> Entities { get; set; }

        public Entity Parent { get; set; }

        public Entity(string type, string name, string path)
        {
            if (type == "Drives")
            {
                Parent = null;
            }
            Type = type;
            Name = name;
            Path = path;
            Entities = new List<Entity>();

        }


        public void UpdateSize()
        {
            int size = 0;
            foreach (var entity in Entities)
            {

                size += entity.Size;
            }
            if (this.Type == "Zip files")
            {
                Size = size / 2;
            }
            else
            {
                Size = size;
            }
        }
    }
}
