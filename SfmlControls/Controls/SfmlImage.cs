using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlControls.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlControls.Controls
{
    public class SfmlImage : SfmlControl
    {
        protected Vector2u _imageSize;
        protected Texture _texture;
        protected bool _hasBorder;
        protected Color _borderColor;

        #region GET_SET

        public Texture Texture
        {
            get { return _texture; }
        }

        public bool HasBorder
        {
            get { return _hasBorder; }
            set
            {
                _hasBorder = value;
                GenerateRender();
            }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                GenerateRender();
            }
        }

        public Image Image
        {
            set
            {
                _imageSize = value.Size;

                _texture = new Texture(value);
                _texture.Smooth = true;
                _texture.Repeated = false;

                GenerateRender();
            }
        }

        #endregion

        public SfmlImage(string name, Vector2f position, Vector2f size, Image image)
            : base(SfmlControlType.Image, name, position, size)
        {
            _imageSize = image.Size;

            _texture = new Texture(image);
            _texture.Smooth = true;
            _texture.Repeated = false;

            _hasBorder = false;
            _borderColor = Config.DEFAULT_BORDER_COLOR;

            _clickable = false;

            GenerateRender();
        }

        public SfmlImage(string name, Vector2f position, Vector2f size, Texture texture, Vector2u imageSize)
            : base(SfmlControlType.Image, name, position, size)
        {
            _imageSize = imageSize;

            _texture = texture;

            _hasBorder = false;
            _borderColor = Config.DEFAULT_BORDER_COLOR;

            _clickable = false;

            GenerateRender();
        }

        #region EVENTS

        #endregion

        public override Cursor.CursorType GetCursorType(Vector2f cursorPosition)
        {
            if (Clickable)
                return Cursor.CursorType.Hand;
            else
                return Cursor.CursorType.Arrow;
        }

        protected override void GenerateRender()
        {
            _drawables.Clear();

            VertexArray vertexArrayImage = SfmlUtils.GetTriangleStripRectangle(_globalPosition, _size, (Vector2f)_imageSize, Color.White);

            RenderStates renderStates = new RenderStates(_texture);
            renderStates.BlendMode = BlendMode.None;

            _drawables.Add(new SfmlControlDrawable(vertexArrayImage, true, renderStates));

            if (_hasBorder)
            {
                VertexArray vertexArrayBorder = SfmlUtils.GetLineStripRectangle(_globalPosition, _size, _borderColor);

                _drawables.Add(new SfmlControlDrawable(vertexArrayBorder));
            }
        }
    }
}
