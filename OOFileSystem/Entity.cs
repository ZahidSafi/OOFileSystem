using System;
using System.Collections.Generic;
using System.Text;

namespace OOFileSystem
{
    public class Entity
    {
        /// <summary>
        /// Type of entity, can be either Drive, Folder, Zip, or Text
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Name of the entity
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Its overall file path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// If Text, the contents of a text file
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Overall size of the entity
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// List of "next" pointers to all available entities
        /// </summary>
        public List<Entity> Entities { get; set; }


        /// <summary>
        /// Previous pointer, to the parent entity
        /// </summary>
        public Entity Parent { get; set; }

        public Entity(string type, string name, string path)
        {
            if (type == "Drive")
            {
                Parent = null;
            }
            Type = type;
            Name = name;
            Path = path;
            Entities = new List<Entity>();

        }

        /// <summary>
        /// Iterate through the all entities and update the size based
        /// on the current entity, zip will be half the sum, while text 
        /// will be length of content
        /// </summary>
        public void UpdateSize()
        {
            int size = 0;
            foreach (var entity in Entities)
            {

                size += entity.Size;
            }
            if (this.Type == "Zip")
            {
                Size = size / 2;
            }
            else
            {
                Size = size;
            }
        }

        /// <summary>
        /// Update the path of the current entity
        /// </summary>
        public void UpdatePath()
        {
            this.Path = this.Parent.Path + "\\" + this.Name;
        }

      
    }
}
