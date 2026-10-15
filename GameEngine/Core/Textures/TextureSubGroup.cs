using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Textures
{
    public class TextureSubGroup
    {
        protected string _name;
        protected string _path;

        protected List<TextureDetails> _textures;

        #region GET_SET

        public string Name { get => _name; }
        public string Path { get => _path; }

        public List<TextureDetails> Textures { get => _textures; }

        #endregion

        public TextureSubGroup(string path)
        {
            _name = path.Split(@"\").Last();
            _path = path;

            _textures = new List<TextureDetails>();

            List<string> files = Directory.GetFiles(path).Where(x => x.Contains(".png")).ToList();

            foreach (string file in files)
            {
                _textures.Add(new TextureDetails(file));
            }
        }
    }
}
