using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SfmlControls.Events;
using SFML.Graphics;
using SfmlControls.Utils;

namespace SfmlControls.Controls
{
    public class SfmlDropdown : SfmlControl
    {
        protected uint _characterSize;
        protected bool _isInSelectMode;
        protected Color _color;
        protected Font _font;
        protected List<SfmlDropdownItem> _items;

        protected const float _INTERIOR_MARGIN = 2.0f;

        #region GET_SET

        public uint CharacterSize
        {
            get { return _characterSize; }
            set
            {
                _characterSize = value;
                GenerateRender();
            }
        }

        public bool IsInSelectMode
        {
            get { return _isInSelectMode; }
            set { _isInSelectMode = value; }
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

        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                GenerateRender();
            }
        }

        public List<SfmlDropdownItem> Items
        {
            get { return _items; }
        }

        #endregion

        public SfmlDropdown(string name, Vector2f position, float width)
            : base(SfmlControlType.Dropdown, name, position, new Vector2f(width, _INTERIOR_MARGIN + Config.DEFAULT_CHARACTER_SIZE + _INTERIOR_MARGIN))
        {
            _characterSize = Config.DEFAULT_CHARACTER_SIZE;
            _isInSelectMode = false;
            _color = Config.DEFAULT_TEXT_COLOR;
            _font = Config.DEFAULT_FONT;
            _items = new List<SfmlDropdownItem>();

            GenerateRender();

            MouseButtonReleased += SfmlDropdown_MouseButtonReleased;
        }

        #region EVENTS_DECLARATION

        public delegate void SelectionChangedHandler(SfmlDropdown sender, SelectionChangedEventArgs e);
        public event SelectionChangedHandler SelectionChanged = delegate { };
        private void TriggerSelectionChanged()
        {
            SfmlDropdownItem? selected = GetItemSelected();

            if (selected != null)
            {
                SelectionChangedEventArgs e = new SelectionChangedEventArgs(selected.Name);

                SelectionChanged?.Invoke(this, e);
            }
        }

        #endregion

        #region EVENTS

        private void SfmlDropdown_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (_isInSelectMode)
            {
                Vector2f click = new Vector2f(e.X, e.Y);

                SfmlDropdownItem? itemSelected = GetItemClicked(click);

                if (itemSelected != null)
                {
                    SelectItem(itemSelected.Name);

                    TriggerSelectionChanged();
                }
            }

            _isInSelectMode = !_isInSelectMode;

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

            GenerateRenderMainArea();
            GenerateRenderButton();

            if (_isInSelectMode)
                GenerateRenderSelectArea();

            if(_items != null && _items.Count > 0 && _isInSelectMode)
            {
                SfmlDropdownItem lastItem = _items.OrderBy(x => x.GlobalPosition.Y).Last();
                _size.Y = lastItem.GlobalPosition.Y - _globalPosition.Y + lastItem.Size.Y;
            }
            else
            {
                _size.Y = _characterSize + _INTERIOR_MARGIN + _INTERIOR_MARGIN;
            }
        }

        private void GenerateRenderMainArea()
        {
            Vector2f position = _globalPosition;

            Vector2f size = new Vector2f(
                _size.X - _characterSize - _INTERIOR_MARGIN - _INTERIOR_MARGIN,
                _INTERIOR_MARGIN + _characterSize + _INTERIOR_MARGIN
                );

            VertexArray vertexArray = SfmlUtils.GetLineStripRectangle(position, size, _color);

            _drawables.Add(new SfmlControlDrawable(vertexArray));

            SfmlDropdownItem? item = GetItemSelected();

            if (item != null)
            {
                Text text = new Text(item.TextStr, _font, _characterSize);
                text.FillColor = _color;
                text.Style = Text.Styles.Bold;
                text.Position = position + new Vector2f(_INTERIOR_MARGIN, _INTERIOR_MARGIN);

                _drawables.Add(new SfmlControlDrawable(text));
            }
        }

        private void GenerateRenderButton()
        {
            Vector2f position = _globalPosition;
            position.X += _size.X - _characterSize - _INTERIOR_MARGIN - _INTERIOR_MARGIN;

            Vector2f size = new Vector2f(
                _INTERIOR_MARGIN + _characterSize + _INTERIOR_MARGIN,
                _INTERIOR_MARGIN + _characterSize + _INTERIOR_MARGIN
                );

            VertexArray vertexArray = SfmlUtils.GetLineStripRectangle(position, size, _color);

            _drawables.Add(new SfmlControlDrawable(vertexArray));

            if (_isInSelectMode)
                GenerateRenderButtonArrowUp(position, size);
            else
                GenerateRenderButtonArrowDown(position, size);
        }

        private void GenerateRenderButtonArrowUp(Vector2f position, Vector2f size)
        {
            position.X += _INTERIOR_MARGIN;
            position.Y += _INTERIOR_MARGIN;

            size.X -= 2 * _INTERIOR_MARGIN;
            size.Y -= 2 * _INTERIOR_MARGIN;

            Vertex[] vertices = new Vertex[3]
            {
                new Vertex()
                {
                    Position = position + new Vector2f(0.0f, size.Y),
                    Color = _color,
                },
                new Vertex()
                {
                    Position = position + new Vector2f(size.X / 2.0f, 0.0f),
                    Color = _color,
                },
                new Vertex()
                {
                    Position = position + size,
                    Color = _color,
                }
            };

            VertexArray vertexArray = new VertexArray(PrimitiveType.LineStrip);

            for (int i = 0; i < vertices.Length; ++i)
                vertexArray.Append(vertices[i]);

            _drawables.Add(new SfmlControlDrawable(vertexArray));
        }

        private void GenerateRenderButtonArrowDown(Vector2f position, Vector2f size)
        {
            position.X += _INTERIOR_MARGIN;
            position.Y += _INTERIOR_MARGIN;

            size.X -= 2 * _INTERIOR_MARGIN;
            size.Y -= 2 * _INTERIOR_MARGIN;

            Vertex[] vertices = new Vertex[3]
            {
                new Vertex()
                {
                    Position = position,
                    Color = _color,
                },
                new Vertex()
                {
                    Position = position + new Vector2f(size.X / 2.0f, size.Y),
                    Color = _color,
                },
                new Vertex()
                {
                    Position = position + new Vector2f(size.X, 0.0f),
                    Color = _color,
                }
            };

            VertexArray vertexArray = new VertexArray(PrimitiveType.LineStrip);

            for (int i = 0; i < vertices.Length; ++i)
                vertexArray.Append(vertices[i]);

            _drawables.Add(new SfmlControlDrawable(vertexArray));
        }

        private void GenerateRenderSelectArea()
        {
            foreach (SfmlDropdownItem item in _items)
            {
                foreach(Drawable drawable in item.Drawables)
                {
                    _drawables.Add(new SfmlControlDrawable(drawable));
                }
            }
        }

        public void AddItem(string name, string textStr, bool isSelected = false)
        {
            if (isSelected)
                for (int i = 0; i < _items.Count; ++i)
                    _items[i].IsSelected = false;

            Vector2f globalPosition = _globalPosition;
            globalPosition.Y += _size.Y;

            for (int i = 0; i < _items.Count; ++i)
                globalPosition.Y += _items[i].Size.Y;

            _items.Add(new SfmlDropdownItem(name, textStr, isSelected, _characterSize, _size.X, globalPosition));

            GenerateRender();
        }

        public SfmlDropdownItem? GetItemClicked(Vector2f click)
        {
            foreach (SfmlDropdownItem item in _items)
                if (item.GlobalBounds.Contains(click))
                    return item;

            return null;
        }

        public void SelectItem(string name)
        {
            SfmlDropdownItem? item = _items.FirstOrDefault(x => x.Name == name);

            if (item == null)
                return;

            SelectItem(item);
        }

        public void SelectItem(SfmlDropdownItem item)
        {
            for (int i = 0; i < _items.Count; ++i)
                _items[i].IsSelected = _items[i].Name == item.Name;
        }

        public SfmlDropdownItem? GetItemSelected()
        {
            if (_items != null && _items.Count > 0)
                foreach (SfmlDropdownItem item in _items)
                    if (item.IsSelected)
                        return item;

            return null;
        }

    }

    public class SfmlDropdownItem
    {
        protected string _name;
        protected string _textStr;
        protected bool _isSelected;
        protected uint _characterSize;
        protected Color _textColor;
        protected Font _font;
        protected Vector2f _globalPosition;
        protected Vector2f _size;
        protected List<Drawable> _drawables;

        protected const float _INTERIOR_MARGIN = 2.0f;

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

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
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

        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                GenerateRender();
            }
        }

        public Vector2f GlobalPosition
        {
            get { return _globalPosition; }
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

        public SfmlDropdownItem(string name, string textStr, bool isSelected, uint characterSize, float width, Vector2f globalPosition)
        {
            _name = name;
            _textStr = textStr;
            _isSelected = isSelected;
            _characterSize = characterSize;
            _textColor = Config.DEFAULT_TEXT_COLOR;
            _font = Config.DEFAULT_FONT;
            _globalPosition = globalPosition;
            _size = new Vector2f(width, _INTERIOR_MARGIN + characterSize + _INTERIOR_MARGIN);
            _drawables = new List<Drawable>();

            GenerateRender();
        }

        public void GenerateRender()
        {
            _drawables.Clear();

            VertexArray vertexArray = SfmlUtils.GetLineStripRectangle(_globalPosition, _size, _textColor);

            _drawables.Add(vertexArray);

            Text text = new Text(_textStr, _font, _characterSize);
            text.Position = _globalPosition + new Vector2f(_INTERIOR_MARGIN, _INTERIOR_MARGIN);
            text.FillColor = _textColor;
            text.Style = _isSelected ? Text.Styles.Bold : Text.Styles.Regular;

            _drawables.Add(text);
        }
    }
}
