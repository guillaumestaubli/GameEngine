using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlControls.Controls
{
    public abstract class SfmlControl : IDisposable
    {
        protected SfmlControlType _type;
        protected bool _clickable;
        protected bool _isDisplayed;
        protected bool _isTextInput;
        protected string _name;
        protected Vector2f _globalPosition;
        protected Vector2f _localPosition;
        protected Vector2f _size;
        protected List<SfmlControlDrawable> _drawables;
        protected SfmlControl? _parent;
        protected List<SfmlControl> _childrens;

        #region GET_SET

        public SfmlControlType Type
        {
            get { return _type; }
        }

        public bool Clickable
        {
            get { return _clickable; }
            set { _clickable = value; }
        }

        public bool IsDisplayed
        {
            get { return _isDisplayed; }
            set { _isDisplayed = value; }
        }

        public bool IsTextInput
        {
            get { return _isTextInput; }
        }

        public string Name
        {
            get { return _name; }
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

        public Vector2f LocalPosition
        {
            get { return _localPosition; }
            set
            {
                _localPosition = value;
                _globalPosition = SearchGlobalPosition(this);
                GenerateRender();
            }
        }

        public Vector2f Size
        {
            get { return _size; }
            set
            {
                _size = value;
                GenerateRender();
            }
        }

        public FloatRect GlobalBounds
        {
            get { return new FloatRect(_globalPosition, _size); }
        }

        public FloatRect LocalBounds
        {
            get { return new FloatRect(_localPosition, _size); }
        }

        public List<SfmlControlDrawable> Drawables
        {
            get { return _drawables; }
        }

        public SfmlControl? Parent
        {
            get { return _parent; }
        }

        public List<SfmlControl> Childrens
        {
            get { return _childrens; }
        }

        #endregion

        public SfmlControl(SfmlControlType type, string name, Vector2f position, Vector2f size)
        {
            _type = type;
            _clickable = true;
            _isDisplayed = true;
            _isTextInput = false;
            _name = name;
            _globalPosition = position;
            _localPosition = position;
            _size = size;
            _drawables = new List<SfmlControlDrawable>();
            _parent = null;
            _childrens = new List<SfmlControl>();
        }

        public void Dispose()
        {
            foreach (SfmlControlDrawable drawable in _drawables)
            {
                if (drawable.Drawable is VertexArray)
                {
                    if (drawable.HasRenderState)
                    {
                        drawable.RenderState.Texture.Dispose();
                    }

                    ((VertexArray)drawable.Drawable).Dispose();
                }
                else if(drawable.Drawable is Image)
                {
                    ((Image)drawable.Drawable).Dispose();
                }
            }

            foreach(SfmlControl child in _childrens)
            {
                child.Dispose();
            }
        }

        #region ABSTRACT

        protected abstract void GenerateRender();

        public abstract Cursor.CursorType GetCursorType(Vector2f cursorPosition);

        #endregion

        #region EVENTS_DECLARATION

        public delegate void ResizedHandler(object? sender, SizeEventArgs e);
        public event ResizedHandler Resized = delegate { };
        public void TriggerResized(object? sender, SizeEventArgs e)
        {
            Resized?.Invoke(this, e);
        }

        public delegate void LostFocusHandler(object? sender, EventArgs e);
        public event LostFocusHandler LostFocus = delegate { };
        public void TriggerLostFocus(object? sender, EventArgs e)
        {
            LostFocus?.Invoke(this, e);
        }

        public delegate void GainedFocusHandler(object? sender, EventArgs e);
        public event GainedFocusHandler GainedFocus = delegate { };
        public void TriggerGainedFocus(object? sender, EventArgs e)
        {
            GainedFocus?.Invoke(this, e);
        }

        public delegate void TextEnteredHandler(object? sender, TextEventArgs e);
        public event TextEnteredHandler TextEntered = delegate { };
        public void TriggerTextEntered(object? sender, TextEventArgs e)
        {
            TextEntered?.Invoke(this, e);
        }

        public delegate void KeyPressedHandler(object? sender, KeyEventArgs e);
        public event KeyPressedHandler KeyPressed = delegate { };
        public void TriggerKeyPressed(object? sender, KeyEventArgs e)
        {
            KeyPressed?.Invoke(this, e);
        }

        public delegate void KeyReleasedHandler(object? sender, KeyEventArgs e);
        public event KeyReleasedHandler KeyReleased = delegate { };
        public void TriggerKeyReleased(object? sender, KeyEventArgs e)
        {
            KeyReleased?.Invoke(this, e);
        }

        public delegate void MouseWheelScrolledHandler(object? sender, MouseWheelScrollEventArgs e);
        public event MouseWheelScrolledHandler MouseWheelScrolled = delegate { };
        public void TriggerMouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
        {
            MouseWheelScrolled?.Invoke(this, e);
        }

        public delegate void MouseButtonPressedHandler(object? sender, MouseButtonEventArgs e);
        public event MouseButtonPressedHandler MouseButtonPressed = delegate { };
        public void TriggerMouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {
            MouseButtonPressed?.Invoke(this, e);
        }

        public delegate void MouseButtonReleasedHandler(object? sender, MouseButtonEventArgs e);
        public event MouseButtonReleasedHandler MouseButtonReleased = delegate { };
        public void TriggerMouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            MouseButtonReleased?.Invoke(this, e);
        }

        public delegate void MouseMovedHandler(object? sender, MouseMoveEventArgs e);
        public event MouseMovedHandler MouseMoved = delegate { };
        public void TriggerMouseMoved(object? sender, MouseMoveEventArgs e)
        {
            MouseMoved?.Invoke(this, e);
        }

        public delegate void MouseEnteredHandler(object? sender, EventArgs e);
        public event MouseEnteredHandler MouseEntered = delegate { };
        public void TriggerMouseEntered(object? sender, EventArgs e)
        {
            MouseEntered?.Invoke(this, e);
        }

        public delegate void MouseLeftHandler(object? sender, EventArgs e);
        public event MouseLeftHandler MouseLeft = delegate { };
        public void TriggerMouseLeft(object? sender, EventArgs e)
        {
            MouseLeft?.Invoke(this, e);
        }

        #endregion

        #region UTILS

        public override string ToString()
        {
            string type = _type.ToString();
            string name = _name;
            string globalPos = $"({_globalPosition.X};{_globalPosition.Y})";
            string localPos = $"({_localPosition.X};{_localPosition.Y})";
            string size = $"({_size.X};{_size.Y})";

            return $"SfmlControl - type : {type} - name : {name} - globalPosition : {globalPos} - localPosition : {localPos} - size : {size}";
        }

        protected Vector2f SearchGlobalPosition(SfmlControl current)
        {
            if (current._parent == null)
            {
                return current._localPosition;
            }
            else
            {
                return current._localPosition + SearchGlobalPosition(current._parent);
            }
        }

        private void RemoveChildrenByNameIfExists(string name)
        {
            _childrens = _childrens.Where(x => x._name != name).ToList();
        }

        public void SetParent(SfmlControl? parent)
        {
            if (parent == null)
            {
                if (_parent != null)
                {
                    _parent.RemoveChildrenByNameIfExists(this._name);
                }

                _parent = null;
            }
            else
            {
                if (_parent != null)
                {
                    _parent.RemoveChildrenByNameIfExists(this._name);
                }

                this._globalPosition = this._localPosition;

                _parent = parent;

                this._globalPosition += _parent.GlobalPosition;
            }

            GenerateRender();
        }

        public void AddChildren(SfmlControl children)
        {
            if (children._parent != null)
            {
                children._parent.RemoveChildrenByNameIfExists(children.Name);
                children._parent = null;

                children._globalPosition = children._localPosition;
            }

            children._parent = this;

            children._globalPosition = children._localPosition + this._globalPosition;

            children.GenerateRender();

            _childrens.Add(children);

            GenerateRender();
        }

        public void ClearChildrens()
        {
            foreach (SfmlControl children in _childrens)
            {
                children._parent = null;
                children._globalPosition = children._localPosition;
            }

            _childrens = new List<SfmlControl>();
        }

        #endregion
    }

    public enum SfmlControlType
    {
        Button,
        Label,
        Panel,
        RadioButton,
        Dropdown,
        ScrollBar,
        Image,
        Textbox,
        PartialWindowContainer,
        Checkbox
    }

    public struct SfmlControlDrawable
    {
        public readonly Drawable Drawable;
        public readonly bool HasRenderState;
        public readonly RenderStates RenderState;

        public SfmlControlDrawable(Drawable drawable, bool hasRenderState, RenderStates renderState)
        {
            Drawable = drawable;
            HasRenderState = hasRenderState;
            RenderState = renderState;
        }

        public SfmlControlDrawable(Drawable drawable)
        {
            Drawable = drawable;
            HasRenderState = false;
        }
    }
}
