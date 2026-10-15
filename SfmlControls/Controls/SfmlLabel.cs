using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SfmlControls.Controls
{
    public class SfmlLabel : SfmlControl
    {
        protected string _textStr;
        protected uint _characterSize;
        protected Color _textColor;
        protected Text.Styles _styles;
        protected Font _font;

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

        public Color TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
                GenerateRender();
            }
        }

        public Text.Styles Styles
        {
            get { return _styles; }
            set
            {
                _styles = value;
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

        #endregion

        public SfmlLabel(string name, string textStr, Vector2f position)
            : base(SfmlControlType.Label, name, position, new Vector2f(0.0f, Config.DEFAULT_CHARACTER_SIZE))
        {
            _textStr = textStr;
            _characterSize = Config.DEFAULT_CHARACTER_SIZE;
            _textColor = Config.DEFAULT_TEXT_COLOR;
            _styles = Text.Styles.Regular;
            _font = Config.DEFAULT_FONT;

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

            Text text = new Text(_textStr, _font, _characterSize);
            text.Position = _globalPosition;
            text.FillColor = _textColor;
            text.Style = _styles;

            _drawables.Add(new SfmlControlDrawable(text));

            _size = text.GetGlobalBounds().Size;
        }

    }
}
