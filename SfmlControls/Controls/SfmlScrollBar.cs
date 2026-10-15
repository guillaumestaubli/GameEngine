using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlControls.Events;
using SfmlControls.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlControls.Controls
{
    public class SfmlScrollBar : SfmlControl
    {
        protected ScrollAxis _scrollAxis;
        protected Color _scrollBarColor;
        protected Color _scrollBarContainerColor;

        protected float _scrollValue;
        protected Vector2f _scrollMinGlobalPosition;
        protected Vector2f _scrollMaxGlobalPosition;
        protected float _scrollCurrentLength;
        protected float _scrollTotalLength;
        protected FloatRect _scrollBarGlobalBounds;

        protected bool _isScrolling;
        protected Vector2f _mousePressedGlobalPosition;
        protected Vector2f _mouseReleasedGlobalPosition;

        #region GET_SET

        public ScrollAxis ScrollAxis
        {
            get { return _scrollAxis; }
        }

        public Color ScrollBarColor
        {
            get { return _scrollBarColor; }
            set
            {
                _scrollBarColor = value;
                GenerateRender();
            }
        }

        public Color ScrollBarContainerColor
        {
            get { return _scrollBarContainerColor; }
            set
            {
                _scrollBarContainerColor = value;
                GenerateRender();
            }
        }

        public float ScrollValue
        {
            get { return _scrollValue; }
        }

        public new SfmlControl? Parent
        {
            get { return _parent; }
            set
            {
                SfmlControl? oldParent = _parent;
                SfmlControl? newParent = value;

                UpdateComponentsGlobalPositions(oldParent, newParent);

                if (value == null && _parent != null)
                    if (_parent.Childrens.Contains(this))
                        _parent.Childrens.Remove(this);

                _parent = value;
                _globalPosition = SearchGlobalPosition(this);

                if (_parent != null)
                    if (!_parent.Childrens.Contains(this))
                        _parent.Childrens.Add(this);

                GenerateRender();
            }
        }

        #endregion

        public SfmlScrollBar(string name, ScrollAxis scrollAxis, Vector2f position, Vector2f size)
            : base(SfmlControlType.ScrollBar, name, position, size)
        {
            _scrollAxis = scrollAxis;
            _scrollBarColor = new Color(80, 80, 80);
            _scrollBarContainerColor = new Color(185, 185, 185);
            _scrollValue = 0.0f;

            Vector2f scrollBarSize;

            if (scrollAxis == ScrollAxis.Horizontal)
                scrollBarSize = new Vector2f(size.X / 10.0f, size.Y);
            else
                scrollBarSize = new Vector2f(size.X, size.Y / 10.0f);

            _scrollBarGlobalBounds = new FloatRect(position, scrollBarSize);
            _scrollMinGlobalPosition = position;
            _scrollCurrentLength = 0.0f;

            switch (scrollAxis)
            {
                case ScrollAxis.Horizontal:
                    _scrollMaxGlobalPosition = position + new Vector2f(size.X - _scrollBarGlobalBounds.Size.X, 0.0f);
                    _scrollTotalLength = _scrollMaxGlobalPosition.X - _scrollMinGlobalPosition.X;
                    _scrollValue = _scrollMinGlobalPosition.X - _scrollBarGlobalBounds.Position.X;
                    break;
                case ScrollAxis.Vertical:
                    _scrollMaxGlobalPosition = position + new Vector2f(0.0f, size.Y - _scrollBarGlobalBounds.Size.Y);
                    _scrollTotalLength = _scrollMaxGlobalPosition.Y - _scrollMinGlobalPosition.Y;
                    _scrollValue = _scrollMaxGlobalPosition.Y - _scrollMinGlobalPosition.Y;
                    break;
                default:
                    _scrollMaxGlobalPosition = position;
                    _scrollTotalLength = 0.0f;
                    _scrollValue = 0.0f;
                    break;
            }

            _mousePressedGlobalPosition = new Vector2f(0.0f, 0.0f);
            _mouseReleasedGlobalPosition = new Vector2f(0.0f, 0.0f);
            _isScrolling = false;

            GenerateRender();

            MouseButtonPressed += Scrollbar_MouseButtonPressed;
            MouseButtonReleased += Scrollbar_MouseButtonReleased;
        }

        #region EVENTS_DECLARATION

        public delegate void ScrollValueChangedHandler(SfmlScrollBar sender, ScrollValueChangedEventArgs e);
        public event ScrollValueChangedHandler ScrollValueChanged = delegate { };
        private void TriggerScrollValueChanged()
        {
            ScrollValueChangedEventArgs e = new ScrollValueChangedEventArgs(_name, _scrollAxis, _scrollValue);

            ScrollValueChanged?.Invoke(this, e);
        }

        #endregion

        #region EVENTS

        private void Scrollbar_MouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {
            Vector2f click = new Vector2f(e.X, e.Y);
            _isScrolling = false;

            if (_scrollBarGlobalBounds.Contains(click) && e.Button == Mouse.Button.Left)
            {
                _mousePressedGlobalPosition = new Vector2f(e.X, e.Y);
                _isScrolling = true;
            }
        }

        public void Scrollbar_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            Vector2f click = new Vector2f(e.X, e.Y);

            if (_isScrolling && e.Button == Mouse.Button.Left)
            {
                _mouseReleasedGlobalPosition = click;

                float diff = 0.0f;

                switch (_scrollAxis)
                {
                    case ScrollAxis.Horizontal:
                        diff = _mouseReleasedGlobalPosition.X - _mousePressedGlobalPosition.X;
                        break;
                    case ScrollAxis.Vertical:
                        diff = _mouseReleasedGlobalPosition.Y - _mousePressedGlobalPosition.Y;
                        break;
                }

                _scrollCurrentLength += diff;

                if (_scrollCurrentLength > _scrollTotalLength)
                    _scrollCurrentLength = _scrollTotalLength;
                else if (_scrollCurrentLength < 0.0f)
                    _scrollCurrentLength = 0.0f;

                _scrollValue = _scrollCurrentLength / _scrollTotalLength;

                if (_scrollValue > 1.0f)
                    _scrollValue = 1.0f;
                else if (_scrollValue < 0.0f)
                    _scrollValue = 0.0f;

                Vector2f boundsPosition = _scrollBarGlobalBounds.Position;
                Vector2f boundsSize = _scrollBarGlobalBounds.Size;

                switch (_scrollAxis)
                {
                    case ScrollAxis.Horizontal:
                        boundsPosition.X = _globalPosition.X + _scrollCurrentLength;
                        break;
                    case ScrollAxis.Vertical:
                        boundsPosition.Y = _globalPosition.Y + _scrollCurrentLength;
                        break;
                }

                _scrollBarGlobalBounds = new FloatRect(boundsPosition, boundsSize);

                _isScrolling = false;

                GenerateRender();

                TriggerScrollValueChanged();
            }
        }

        #endregion

        public override Cursor.CursorType GetCursorType(Vector2f cursorPosition)
        {
            if (Clickable)
            {
                switch (ScrollAxis)
                {
                    case ScrollAxis.Horizontal:
                        return Cursor.CursorType.SizeHorizontal;
                    case ScrollAxis.Vertical:
                        return Cursor.CursorType.SizeVertical;
                    default:
                        return Cursor.CursorType.Arrow;
                }
            }
            else
                return Cursor.CursorType.Arrow;
        }

        protected override void GenerateRender()
        {
            _drawables.Clear();

            VertexArray vertexArrayContainer = SfmlUtils.GetTriangleStripRectangle(_globalPosition, _size, _scrollBarContainerColor);
            VertexArray vertexArrayScrollBar = SfmlUtils.GetTriangleStripRectangle(_scrollBarGlobalBounds.Position, _scrollBarGlobalBounds.Size, _scrollBarColor);

            _drawables.Add(new SfmlControlDrawable(vertexArrayContainer));
            _drawables.Add(new SfmlControlDrawable(vertexArrayScrollBar));
        }

        public void ManualTriggerScroll(float scrollValue)
        {
            _scrollValue = scrollValue;

            if (_scrollValue > 1.0f)
                _scrollValue = 1.0f;
            else if (_scrollValue < 0.0f)
                _scrollValue = 0.0f;

            _scrollCurrentLength = _scrollTotalLength * _scrollValue;

            Vector2f boundsPosition = _scrollBarGlobalBounds.Position;
            Vector2f boundsSize = _scrollBarGlobalBounds.Size;

            switch (_scrollAxis)
            {
                case ScrollAxis.Horizontal:
                    boundsPosition.X = _globalPosition.X + _scrollCurrentLength;
                    break;
                case ScrollAxis.Vertical:
                    boundsPosition.Y = _globalPosition.Y + _scrollCurrentLength;
                    break;
            }

            _scrollBarGlobalBounds = new FloatRect(boundsPosition, boundsSize);

            GenerateRender();

            TriggerScrollValueChanged();
        }

        protected void UpdateComponentsGlobalPositions(SfmlControl? oldParent, SfmlControl? newParent)
        {
            Vector2f diff = new Vector2f(0.0f, 0.0f);

            if (oldParent == null && newParent == null)
            {
                // do nothing
                diff = new Vector2f(0.0f, 0.0f);
            }
            else if (oldParent == null && newParent != null)
            {
                diff = newParent.GlobalPosition;
            }
            else if (oldParent != null && newParent == null)
            {
                diff = -1.0f * oldParent.GlobalPosition;
            }
            else if (oldParent != null && newParent != null)
            {
                diff = newParent.GlobalPosition - oldParent.GlobalPosition;
            }

            Vector2f globalBoundsPosition = _scrollBarGlobalBounds.Position + diff;
            Vector2f globalBoundsSize = _scrollBarGlobalBounds.Size;
            _scrollBarGlobalBounds = new FloatRect(globalBoundsPosition, globalBoundsSize);

            _scrollMinGlobalPosition += diff;
            _scrollMaxGlobalPosition += diff;
        }
    }

    public enum ScrollAxis
    {
        Horizontal = 0,
        Vertical = 1
    }
}
