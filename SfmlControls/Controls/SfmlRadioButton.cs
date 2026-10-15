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
    public class SfmlRadioButton : SfmlControl
    {
        protected uint _characterSize;
        protected Font _font;
        protected Color _textColor;
        protected List<SfmlRadioButtonItem> _items;

        protected const float _ITEMS_VERTICAL_SPACING = 8.0f;

        #region GET_SET

        public uint CharacterSize
        {
            get { return _characterSize; }
            set
            {
                _characterSize = value;

                foreach (SfmlRadioButtonItem item in _items)
                    item.CharacterSize = _characterSize;

                GenerateRender();
            }
        }

        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;

                foreach (SfmlRadioButtonItem item in _items)
                    item.Font = _font;

                GenerateRender();
            }
        }

        public Color TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;

                foreach (SfmlRadioButtonItem item in _items)
                    item.Color = _textColor;

                GenerateRender();
            }
        }

        public List<SfmlRadioButtonItem> Items
        {
            get { return _items; }
        }

        #endregion

        public SfmlRadioButton(string name, Vector2f position, Vector2f size)
            : base(SfmlControlType.RadioButton, name, position, size)
        {
            _characterSize = Config.DEFAULT_CHARACTER_SIZE;
            _font = Config.DEFAULT_FONT;
            _textColor = Config.DEFAULT_TEXT_COLOR;
            _items = new List<SfmlRadioButtonItem>();

            GenerateRender();

            MouseButtonReleased += RadioButton_MouseButtonReleased;
        }

        #region EVENTS_DECLARATION

        public delegate void SelectionChangedHandler(SfmlRadioButton sender, SelectionChangedEventArgs e);
        public event SelectionChangedHandler SelectionChanged = delegate { };
        private void TriggerSelectionChanged()
        {
            SfmlRadioButtonItem? selected = GetItemChecked();

            if(selected != null)
            {
                SelectionChangedEventArgs e = new SelectionChangedEventArgs(selected.Name);

                SelectionChanged?.Invoke(this, e);
            }
        }

        #endregion

        #region EVENTS

        private void RadioButton_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            Vector2f click = new Vector2f(e.X, e.Y);

            SfmlRadioButtonItem? itemChecked = GetItemClicked(click);

            if (itemChecked != null)
            {
                CheckItem(itemChecked.Name);

                TriggerSelectionChanged();
            }

            GenerateRender();
        }

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

            if (_items != null && _items.Count > 0)
            {
                foreach (SfmlRadioButtonItem item in _items)
                {
                    foreach(Drawable drawable in item.Drawables)
                    {
                        _drawables.Add(new SfmlControlDrawable(drawable));
                    }
                }

                SfmlRadioButtonItem lastItem = _items.OrderBy(x => x.GlobalPosition.Y).Last();

                _size.Y = lastItem.GlobalPosition.Y - _globalPosition.Y + lastItem.Size.Y;
            }
        }

        public void AddItem(string name, string textStr, bool isChecked = false)
        {
            Vector2f globalPosition = _globalPosition;

            for (int i = 0; i < _items.Count; ++i)
                globalPosition.Y += _items[i].Size.Y + _ITEMS_VERTICAL_SPACING;

            _items.Add(new SfmlRadioButtonItem(name, textStr, isChecked, _characterSize, _font, _textColor, globalPosition));

            GenerateRender();
        }

        public void CheckItem(string name)
        {
            SfmlRadioButtonItem? item = _items.FirstOrDefault(x => x.Name == name);

            if (item != null)
                CheckItem(item);
        }

        public void CheckItem(SfmlRadioButtonItem item)
        {
            for (int i = 0; i < _items.Count; ++i)
                _items[i].IsChecked = _items[i].Name == item.Name;
        }

        private SfmlRadioButtonItem? GetItemClicked(Vector2f click)
        {
            foreach (SfmlRadioButtonItem item in _items)
                if (item.GlobalBounds.Contains(click))
                    return item;

            return null;
        }

        public SfmlRadioButtonItem? GetItemChecked()
        {
            foreach (SfmlRadioButtonItem item in _items)
                if (item.IsChecked)
                    return item;

            return null;
        }
    }

    public class SfmlRadioButtonItem
    {
        protected string _name;
        protected string _textStr;
        protected bool _isChecked;
        protected uint _characterSize;
        protected Font _font;
        protected Color _color;
        protected Vector2f _globalPosition;
        protected Vector2f _size;
        protected List<Drawable> _drawables;

        protected const float _HORIZONTAL_SPACING = 10.0f;
        protected const float _CHECKED_BOX_INSIDE_SPACING = 2.0f;

        #region GET_SET

        public string Name
        {
            get { return _name; }
        }

        public string TextStr
        {
            get { return _textStr; }
            set
            {
                _textStr = value;
                GenerateRender();
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
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

        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                GenerateRender();
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                GenerateRender();
            }
        }

        public Vector2f GlobalPosition
        {
            get { return _globalPosition; }
            set
            {
                _globalPosition = value;
                GenerateRender();
            }
        }

        public Vector2f Size
        {
            get { return _size; }
        }

        public FloatRect GlobalBounds
        {
            get { return new FloatRect(_globalPosition, _size); }
        }

        public List<Drawable> Drawables
        {
            get { return _drawables; }
        }

        #endregion

        public SfmlRadioButtonItem(string name, string textStr, bool isChecked, uint characterSize, Font font, Color color, Vector2f globalPosition)
        {
            _name = name;
            _textStr = textStr;
            _isChecked = isChecked;
            _characterSize = characterSize;
            _font = font;
            _color = color;
            _globalPosition = globalPosition;
            _size = new Vector2f(characterSize, characterSize);
            _drawables = new List<Drawable>();

            GenerateRender();
        }

        public void GenerateRender()
        {
            _drawables.Clear();

            Vector2f origin = _globalPosition;

            VertexArray vertexArraySquareUnchecked = SfmlUtils.GetLineStripRectangle(origin, new Vector2f(_characterSize, _characterSize), _color);

            _drawables.Add(vertexArraySquareUnchecked);

            if (_isChecked)
            {
                origin += new Vector2f(_CHECKED_BOX_INSIDE_SPACING, _CHECKED_BOX_INSIDE_SPACING);

                float checkedSize = _characterSize - _CHECKED_BOX_INSIDE_SPACING - _CHECKED_BOX_INSIDE_SPACING;

                VertexArray vertexArraySquareChecked = SfmlUtils.GetTriangleStripRectangle(origin, new Vector2f(checkedSize, checkedSize), _color);

                _drawables.Add(vertexArraySquareChecked);
            }

            Text label = new Text(_textStr, _font, _characterSize);
            label.Position = origin + new Vector2f(_characterSize + _HORIZONTAL_SPACING, 0.0f);
            label.FillColor = _color;

            _drawables.Add(label);

            float size_x = _characterSize + _HORIZONTAL_SPACING + label.GetLocalBounds().Width;
            float size_y = _characterSize;

            _size = new Vector2f(size_x, size_y);
        }
    }
}
