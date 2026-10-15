using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Textures
{
    public class TextureMainGroup
    {
        protected string _name;
        protected string _path;

        protected List<TextureSubGroup> _subGroups;

        #region GET_SET

        public string Name { get => _name; }
        public string Path { get => _path; }

        public List<TextureSubGroup> SubGroups { get => _subGroups; }

        #endregion

        public TextureMainGroup(string path)
        {
            _name = path.Split(@"\").Last();
            _path = path;

            _subGroups = new List<TextureSubGroup>();

            List<string> directories = Directory.GetDirectories(path).ToList();

            foreach (string directory in directories)
            {
                _subGroups.Add(new TextureSubGroup(directory));
            }

        }
    }
}
