using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Textures
{
    public class TextureLibrary
    {
        protected string _name;
        protected string _path;

        protected List<TextureMainGroup> _mainGroups;

        #region GET_SET

        public string Name { get => _name; }
        public string Path { get => _path; }

        public List<TextureMainGroup> MainGroups { get => _mainGroups; }

        public int Count
        {
            get
            {
                int counter = 0;

                foreach(TextureMainGroup mainGroup in _mainGroups)
                {
                    foreach(TextureSubGroup subGroup in mainGroup.SubGroups)
                    {
                        counter += subGroup.Textures.Count;
                    }
                }

                return counter;
            }
        }

        #endregion

        public TextureLibrary(string path)
        {
            _name = path.Split(@"\").Last();
            _path = path;

            _mainGroups = new List<TextureMainGroup>();

            List<string> directories = Directory.GetDirectories(path).ToList();

            foreach(string directory in directories)
            {
                _mainGroups.Add(new TextureMainGroup(directory));
            }
        }

        public TextureDetails GetTexture(string name)
        {
            foreach(TextureMainGroup mainGroup in _mainGroups)
            {
                foreach(TextureSubGroup subGroup in mainGroup.SubGroups)
                {
                    foreach(TextureDetails texture in subGroup.Textures)
                    {
                        if(texture.Name == name)
                            return texture;
                    }
                }
            }

            throw new Exception($"Texture with name {name} not found");
        }

        public TextureSubGroup GetTextureSubGroup(string name)
        {
            foreach(TextureMainGroup mainGroup in _mainGroups)
            {
                foreach(TextureSubGroup subGroup in mainGroup.SubGroups)
                {
                    if (subGroup.Name == name)
                        return subGroup;
                }
            }

            throw new Exception($"TextureSubGroup with name {name} not found");
        }

        public TextureMainGroup GetTextureMainGroup(string name)
        {
            foreach(TextureMainGroup mainGroup in _mainGroups)
            {
                if (mainGroup.Name == name)
                    return mainGroup;
            }

            throw new Exception($"TextureMainGroup with name {name} not found");
        }

    }
}
