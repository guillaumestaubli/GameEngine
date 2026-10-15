using GameEngine.Core.Props;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Textures
{
    public class TextureDetails
    {
        protected string _name;
        protected string _path;

        protected Texture _texture;
        protected Image _image;

        protected List<Vector2i> _pointsSpriteBorder = new List<Vector2i>();

        #region GET_SET

        public string Name { get => _name; }
        public string Path { get => _path; }

        public Texture Texture { get => _texture; }
        public Image Image { get => _image; }

        public List<Vector2i> PointsSpriteBorder
        {
            get => _pointsSpriteBorder;
        }

        #endregion

        public TextureDetails(string path)
        {
            _name = path.Split(@"\").Last().Split('.').First();
            _path = path;

            _image = new Image(path);

            _texture = new Texture(_image);
            _texture.Smooth = true;
            _texture.Repeated = true;

            if (path.Contains("Prop"))
                _pointsSpriteBorder = TextureManager.GenerateSpriteAbsoluteBounds(this);
        }
    }
}
