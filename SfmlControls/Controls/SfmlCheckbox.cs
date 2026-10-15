using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlControls.Events;
using SfmlControls.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlControls.Controls
{
    public class SfmlCheckbox : SfmlControl
    {
        protected string _textStr;
        protected bool _isChecked;
        protected uint _characterSize;
        protected Font _font;
        protected Color _color;

        protected const float _HORIZONTAL_SPACING = 10.0f;

        #region GET_SET

        public string TextStr
        {
            get => _textStr;
            set
            {
                _textStr = value;
                GenerateRender();
            }
        }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                GenerateRender();
            }
        }

        public uint CharacterSize
        {
            get => _characterSize;
            set
            {
                _characterSize = value;
                GenerateRender();
            }
        }

        public Font Font
        {
            get => _font;
            set
            {
                _font = value;
                GenerateRender();
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                GenerateRender();
            }
        }

        #endregion


        public SfmlCheckbox(string name, string textStr, Vector2f position, bool isChecked = false)
            : base(SfmlControlType.Checkbox, name, position, new Vector2f(Config.DEFAULT_CHARACTER_SIZE, Config.DEFAULT_CHARACTER_SIZE))
        {
            _textStr = textStr;
            _isChecked = isChecked;
            _characterSize = Config.DEFAULT_CHARACTER_SIZE;
            _font = Config.DEFAULT_FONT;
            _color = Config.DEFAULT_TEXT_COLOR;

            this.Clickable = true;

            GenerateRender();

            MouseButtonReleased += Checkbox_MouseButtonReleased;
        }

        #region EVENTS_DECLARATION

        public delegate void CheckedChangedHandler(SfmlCheckbox sender, CheckedChangedEventsArgs e);
        public event CheckedChangedHandler CheckedChanged = delegate { };
        private void TriggerCheckedChanged()
        {
            CheckedChanged?.Invoke(this, new CheckedChangedEventsArgs(this._isChecked));
        }

        #endregion

        #region EVENTS

        private void Checkbox_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (sender == null)
                return;

            if (sender is not SfmlCheckbox)
                return;

            FloatRect clickableBounds = new FloatRect(_globalPosition, new Vector2f(_characterSize, _characterSize));
            Vector2f click = new Vector2f(e.X, e.Y);

            if (clickableBounds.Contains(click))
            {
                _isChecked = !_isChecked;
                TriggerCheckedChanged();

                GenerateRender();
            }
        }

        #endregion

        public override Cursor.CursorType GetCursorType(Vector2f cursorPosition)
        {
            FloatRect clickableBounds = new FloatRect(_globalPosition, new Vector2f(_characterSize, _characterSize));

            if (clickableBounds.Contains(cursorPosition) && Clickable)
            {
                return Cursor.CursorType.Hand;
            }

            return Cursor.CursorType.Arrow;
        }

        protected override void GenerateRender()
        {
            _drawables.Clear();

            Vector2f origin = _globalPosition;

            VertexArray vertexArraySquareUnchecked = SfmlUtils.GetLineStripRectangle(origin, new Vector2f(_characterSize, _characterSize), _color);

            _drawables.Add(new SfmlControlDrawable(vertexArraySquareUnchecked));

            if (_isChecked)
            {
                VertexArray vertexArrayChecked = new VertexArray(PrimitiveType.Lines);

                Vertex v_0 = new Vertex(origin, _color);
                Vertex v_1 = new Vertex(origin + new Vector2f(_characterSize, _characterSize), _color);
                Vertex v_2 = new Vertex(origin + new Vector2f(_characterSize, 0.0f), _color);
                Vertex v_3 = new Vertex(origin + new Vector2f(0.0f, _characterSize), _color);

                vertexArrayChecked.Append(v_0);
                vertexArrayChecked.Append(v_1);
                vertexArrayChecked.Append(v_2);
                vertexArrayChecked.Append(v_3);

                _drawables.Add(new SfmlControlDrawable(vertexArrayChecked));
            }

            Text label = new Text(_textStr, _font, _characterSize);
            label.Position = origin + new Vector2f(_characterSize + _HORIZONTAL_SPACING, 0.0f);
            label.FillColor = _color;

            _drawables.Add(new SfmlControlDrawable(label));

            float size_x = _characterSize + _HORIZONTAL_SPACING + label.GetLocalBounds().Width;
            float size_y = _characterSize;

            _size = new Vector2f(size_x, size_y);
        }
    }
}
