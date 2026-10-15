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
    public class SfmlButton : SfmlControl
    {
        protected string _textStr;
        protected uint _characterSize;
        protected Color _backgroundColor;
        protected Color _borderColor;
        protected Color _textColor;
        protected Font _font;
        protected Text.Styles _textStyles;

        #region GET_SET

        public string TextStr
        {
            get { return _textStr; }
            set
            {
                _textStr = value;
                GenerateRender();
            }
        }

        public uint CharacterSize
        {
            get { return _characterSize; }
            set
            {
                _characterSize = value;
                GenerateRender();
            }
        }

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
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

        public Color TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
                GenerateRender();
            }
        }

        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                GenerateRender();
            }
        }

        public Text.Styles TextStyles
        {
            get { return _textStyles; }
            set
            {
                _textStyles = value;
                GenerateRender();
            }
        }

        #endregion

        public SfmlButton(string name, string textStr, Vector2f position, Vector2f size)
            : base(SfmlControlType.Button, name, position, size)
        {
            _textStr = textStr;
            _characterSize = Config.DEFAULT_CHARACTER_SIZE;
            _backgroundColor = Config.DEFAULT_BACKGROUND_COLOR;
            _borderColor = Config.DEFAULT_BORDER_COLOR;
            _textColor = Config.DEFAULT_TEXT_COLOR;
            _font = Config.DEFAULT_FONT;
            _textStyles = Text.Styles.Regular;

            GenerateRender();
        }

        #region EVENTS


        #endregion

        protected override void GenerateRender()
        {
            _drawables.Clear();

            VertexArray vertexArrayBackground = SfmlUtils.GetTriangleStripRectangle(_globalPosition, _size, _backgroundColor);
            VertexArray vertexArrayBorder = SfmlUtils.GetLineStripRectangle(_globalPosition, _size, _borderColor);

            Text text = new Text(_textStr, _font, _characterSize);
            text.FillColor = _textColor;
            text.Style = _textStyles;

            // center the text on the inside of the button
            text.Origin = text.GetGlobalBounds().Size / 2.0f + text.GetLocalBounds().Position;
            text.Position = vertexArrayBackground.Bounds.Position + vertexArrayBackground.Bounds.Size / 2.0f;

            // remove non integer values to avoid blurry effect when drawn on screen
            text.Origin = (Vector2f)((Vector2i)text.Origin);
            text.Position = (Vector2f)((Vector2i)text.Position);

            _drawables.Add(new SfmlControlDrawable(vertexArrayBackground));
            _drawables.Add(new SfmlControlDrawable(vertexArrayBorder));
            _drawables.Add(new SfmlControlDrawable(text));
        }

        public override Cursor.CursorType GetCursorType(Vector2f cursorPosition)
        {
            if (Clickable)
                return Cursor.CursorType.Hand;
            else
                return Cursor.CursorType.Arrow;
        }
    }
}
