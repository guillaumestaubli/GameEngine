using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Graphics;
using SFML.Window;

using SfmlControls.Controls;
using System.Diagnostics;

namespace SfmlControls.Window.PartialWindow
{
    public abstract class SfmlPartialWindow : IDisposable
    {
        private float _zoomInFactor = Config.ZOOM_IN_FACTOR;
        private float _zoomOutFactor = Config.ZOOM_OUT_FACTOR;
        private float _currentZoomFactor = 1.0f;
        private float _scrollOffset = Config.DEFAULT_SCROLL_OFFSET;
        private float _scrollOffsetMarginBottom = 10.0f;

        private bool _scrollableVertical = false;
        private bool _scrollableHorizontal = false;
        private bool _scrollableWithMouseWheel = false;
        private bool _scrollableWithKeyboardArrows = false;
        private bool _scrollableWithMouseDrag = false;
        private bool _scrollMouseDragRunning = false;
        private bool _zoomable = false;
        private bool _zoomableWithMouseWheel = false;
        private bool _ignoreMaxScrollOffsets = false;

        private Vector2f _viewGlobalPosition;
        private Vector2f _textureTotalSize;
        private Vector2f _scrollMouseDragLastPosition = new Vector2f(0.0f, 0.0f);
        private Vector2f _currentTotalScrollOffset = new Vector2f(0.0f, 0.0f);

        private Color _clearColor = Config.DEFAULT_CLEAR_COLOR;

        private List<SfmlControl> _controls = new List<SfmlControl>();
        private SfmlControl? _currentTextInputControl = null;
        private SfmlControl? _currentFocusControl = null;

        private RenderWindow _parentWindow;
        private View _view;
        private RenderTexture _renderTexture;
        private Sprite _renderSprite;

        #region GET_SET

        public float ZoomInFactor
        {
            get => _zoomInFactor;
            set => _zoomInFactor = value;
        }

        public float ZoomOutFactor
        {
            get => _zoomOutFactor;
            set => _zoomOutFactor = value;
        }

        public float CurrentZoomFactor
        {
            get => _currentZoomFactor;
        }

        public float ScrollOffset
        {
            get => _scrollOffset;
            set => _scrollOffset = value;
        }

        public bool ScrollableVertical
        {
            get => _scrollableVertical;
            set
            {
                _scrollableVertical = value;
            }
        }

        public bool ScrollableHorizontal
        {
            get => _scrollableHorizontal;
            set
            {
                _scrollableHorizontal = value;
            }
        }

        public bool ScrollableWithMouseWheel
        {
            get => _scrollableWithMouseWheel;
            set => _scrollableWithMouseWheel = value;
        }

        public bool ScrollableWithKeyboardArrows
        {
            get => _scrollableWithKeyboardArrows;
            set => _scrollableWithKeyboardArrows = value;
        }

        public bool ScrollableWithMouseDrag
        {
            get => _scrollableWithMouseDrag;
            set => _scrollableWithMouseDrag = value;
        }

        public bool ScrollMouseDragRunning
        {
            get => _scrollMouseDragRunning;
        }

        public bool IgnoreMaxScrollOffsets
        {
            get => _ignoreMaxScrollOffsets;
            set => _ignoreMaxScrollOffsets = value;
        }

        public bool Zoomable
        {
            get => _zoomable;
            set => _zoomable = value;
        }

        public bool ZoomableWithMouseWheel
        {
            get => _zoomableWithMouseWheel;
            set => _zoomableWithMouseWheel = value;
        }

        public Color ClearColor
        {
            get => _clearColor;
            set => _clearColor = value;
        }

        /// <summary>
        /// Returns a copy of the controls in the partial window
        /// </summary>
        public List<SfmlControl> Controls
        {
            get => _controls.ToList();
        }

        #endregion

        public SfmlPartialWindow(Vector2f globalPosition, Vector2f size, RenderWindow parentWindow)
        {
            _viewGlobalPosition = globalPosition;
            _textureTotalSize = size;
            _parentWindow = parentWindow;

            _view = new View(size / 2.0f, size);
            _renderTexture = new RenderTexture((uint)size.X, (uint)size.Y);
            _renderTexture.Clear(_clearColor);
            _renderTexture.SetView(_view);
            _renderTexture.Display();

            _renderSprite = new Sprite(_renderTexture.Texture);
            _renderSprite.Origin = new Vector2f(0.0f, 0.0f);
            _renderSprite.Position = _viewGlobalPosition;

            Resized += PartialWindow_Resized;
            LostFocus += PartialWindow_LostFocus;
            GainedFocus += PartialWindow_GainedFocus;
            TextEntered += PartialWindow_TextEntered;
            KeyPressed += PartialWindow_KeyPressed;
            KeyReleased += PartialWindow_KeyReleased;
            MouseWheelScrolled += PartialWindow_MouseWheelScrolled;
            MouseButtonPressed += PartialWindow_MouseButtonPressed;
            MouseButtonReleased += PartialWindow_MouseButtonReleased;
            MouseMoved += PartialWindow_MouseMoved;
            MouseEntered += PartialWindow_MouseEntered;
            MouseLeft += PartialWindow_MouseLeft;
        }

        public void Dispose()
        {
            foreach(SfmlControl control in _controls)
            {
                control.Dispose();
            }

            _view.Dispose();
            _renderTexture.Dispose();
            _renderSprite.Dispose();
        }

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

        #region EVENTS

        private void PartialWindow_Resized(object? sender, SizeEventArgs e)
        {

        }

        private void PartialWindow_LostFocus(object? sender, EventArgs e)
        {
            if (_currentFocusControl != null)
            {
                _currentFocusControl.TriggerLostFocus(this, e);
                _currentFocusControl = null;
            }

            if (_scrollMouseDragRunning)
            {
                EndScrollMouseDrag(_scrollMouseDragLastPosition);
            }
        }

        private void PartialWindow_GainedFocus(object? sender, EventArgs e)
        {
            Vector2f mouseLocalPosition = ConvertGlobalPositionToLocalPosition((Vector2f)Mouse.GetPosition(_parentWindow));

            SfmlControl? controlHovered = GetControl(mouseLocalPosition);

            if (controlHovered != null)
            {
                _currentFocusControl = controlHovered;
                _currentFocusControl.TriggerGainedFocus(this, e);
            }
        }

        private void PartialWindow_TextEntered(object? sender, TextEventArgs e)
        {
            if (_currentTextInputControl != null)
            {
                _currentTextInputControl.TriggerTextEntered(this, e);
            }
        }

        private void PartialWindow_KeyPressed(object? sender, KeyEventArgs e)
        {
            if(_currentFocusControl != null && _currentTextInputControl != null)
            {
                if(_currentFocusControl.Name == _currentTextInputControl.Name)
                {
                    _currentTextInputControl.TriggerKeyPressed(this, e);
                }
                else
                {
                    _currentFocusControl.TriggerKeyPressed(this, e);
                    _currentTextInputControl.TriggerKeyPressed(this, e);
                }
            }
            else
            {
                if (_currentFocusControl != null)
                    _currentFocusControl.TriggerKeyPressed(this, e);

                if (_currentTextInputControl != null)
                    _currentTextInputControl.TriggerKeyPressed(this, e);
            }

            
        }

        private void PartialWindow_KeyReleased(object? sender, KeyEventArgs e)
        {

        }

        private void PartialWindow_MouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
        {
            Vector2f localMousePosition = ConvertGlobalPositionToLocalPosition(new Vector2f(e.X, e.Y));

            SfmlControl? controlScrolled = GetControl(localMousePosition);

            if (controlScrolled != null)
            {
                MouseWheelScrollEvent ev = new MouseWheelScrollEvent();
                ev.Wheel = e.Wheel;
                ev.Delta = e.Delta;
                ev.X = (int)localMousePosition.X;
                ev.Y = (int)localMousePosition.Y;

                MouseWheelScrollEventArgs ev_args = new MouseWheelScrollEventArgs(ev);

                controlScrolled.TriggerMouseWheelScrolled(this, ev_args);
            }

            if (_zoomable && _zoomableWithMouseWheel)
            {
                if (e.Delta > 0)
                {
                    ZoomIn();
                }
                else if (e.Delta < 0)
                {
                    ZoomOut();
                }
            }

            if (_scrollableVertical && _scrollableWithMouseWheel)
            {
                if (e.Delta < 0)
                {
                    Move(new Vector2f(0.0f, -1.0f * ScrollOffset));
                }
                else if (e.Delta > 0)
                {
                    Move(new Vector2f(0.0f, ScrollOffset));
                }
            }
        }

        private void PartialWindow_MouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {
            Vector2f localMousePosition = ConvertGlobalPositionToLocalPosition(new Vector2f(e.X, e.Y));

            SfmlControl? controlPressed = GetControl(localMousePosition);

            if (controlPressed != null)
            {
                MouseButtonEvent ev = new MouseButtonEvent();
                ev.Button = e.Button;
                ev.X = (int)localMousePosition.X;
                ev.Y = (int)localMousePosition.Y;

                MouseButtonEventArgs ev_args = new MouseButtonEventArgs(ev);

                controlPressed.TriggerMouseButtonPressed(this, ev_args);
            }

            if (
                (_scrollableHorizontal || _scrollableVertical) &&
                _scrollableWithMouseDrag &&
                _scrollMouseDragRunning == false &&
                e.Button == Mouse.Button.Left
                )
            {
                StartScrollMouseDrag(new Vector2f(e.X, e.Y));
            }
        }

        private void PartialWindow_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            Vector2f localMousePosition = ConvertGlobalPositionToLocalPosition(new Vector2f(e.X, e.Y));

            SfmlControl? controlReleased = GetControl(localMousePosition);

            if (controlReleased != null)
            {
                MouseButtonEvent ev = new MouseButtonEvent();
                ev.Button = e.Button;
                ev.X = (int)localMousePosition.X;
                ev.Y = (int)localMousePosition.Y;

                MouseButtonEventArgs ev_args = new MouseButtonEventArgs(ev);

                controlReleased.TriggerMouseButtonReleased(this, ev_args);

                if(controlReleased is SfmlTextbox)
                {
                    if (_currentTextInputControl != null)
                    {
                        if(_currentTextInputControl.Name == controlReleased.Name)
                        {
                            ((SfmlTextbox)_currentTextInputControl).IsInInputMode = false;
                            _currentTextInputControl = null;
                        }
                        else
                        {
                            ((SfmlTextbox)_currentTextInputControl).IsInInputMode = false;
                            _currentTextInputControl = controlReleased;
                        }
                    }
                    else
                    {
                        _currentTextInputControl = controlReleased;
                    }
                }
            }

            if (_scrollMouseDragRunning && e.Button == Mouse.Button.Left)
            {
                EndScrollMouseDrag(new Vector2f(e.X, e.Y));
            }
        }

        private void PartialWindow_MouseMoved(object? sender, MouseMoveEventArgs e)
        {
            Vector2f mouseLocalPosition = ConvertGlobalPositionToLocalPosition((Vector2f)Mouse.GetPosition(_parentWindow));

            SfmlControl? controlHovered = GetControl(mouseLocalPosition);

            if (controlHovered != null)
            {
                if (_currentFocusControl != null)
                {
                    if (_currentFocusControl.Name != controlHovered.Name)
                    {
                        _currentFocusControl.TriggerLostFocus(this, e);
                        _currentFocusControl = controlHovered;
                        _currentFocusControl.TriggerGainedFocus(this, e);
                    }
                }
                else
                {
                    _currentFocusControl = controlHovered;
                    _currentFocusControl.TriggerGainedFocus(this, e);
                }
            }

            if (_scrollMouseDragRunning)
            {
                ContinueScrollMouseDrag(new Vector2f(e.X, e.Y));
            }
        }

        private void PartialWindow_MouseEntered(object? sender, EventArgs e)
        {

        }

        private void PartialWindow_MouseLeft(object? sender, EventArgs e)
        {

        }

        #endregion

        #region ABSTRACT

        /// <summary>
        /// Called each time DrawFrame() is called to allow to manually draw stuff on the partialWindow
        /// </summary>
        public abstract void DrawOnFrame(ref RenderTexture renderTexture);

        public abstract Cursor.CursorType GetCursorType(Vector2f globalPosition);

        #endregion

        public void AddControl(SfmlControl control)
        {
            if (GetControl(control.Name) != null)
                throw new Exception($"A control with the name \"{control.Name}\" already exists in the controls list");

            _controls.Add(control);

            SfmlControl ctrl_max_x = _controls.OrderBy(x => x.GlobalPosition.X).Last();
            SfmlControl ctrl_max_y = _controls.OrderBy(x => x.GlobalPosition.Y).Last();

            float max_x = ctrl_max_x.GlobalPosition.X + ctrl_max_x.Size.X;
            float max_y = ctrl_max_y.GlobalPosition.Y + ctrl_max_y.Size.Y;

            _textureTotalSize = new Vector2f(max_x, max_y);
        }

        public void RemoveControl(SfmlControl control)
        {
            _controls = _controls.Where(x => x.Name != control.Name).ToList();

            if (_controls.Count <= 0)
            {
                _textureTotalSize = _view.Size;
            }
            else
            {
                SfmlControl ctrl_max_x = _controls.OrderBy(x => x.GlobalPosition.X).Last();
                SfmlControl ctrl_max_y = _controls.OrderBy(x => x.GlobalPosition.Y).Last();

                float max_x = ctrl_max_x.GlobalPosition.X + ctrl_max_x.Size.X;
                float max_y = ctrl_max_y.GlobalPosition.Y + ctrl_max_y.Size.Y;

                _textureTotalSize = new Vector2f(max_x, max_y);
            }
        }


        public SfmlControl? GetControl(string name)
        {
            foreach (SfmlControl current in _controls)
            {
                SfmlControl? control = SearchControl(name, current);

                if (control != null)
                    return control;
            }

            return null;
        }

        public SfmlControl? GetControl(Vector2f globalPosition)
        {
            foreach (SfmlControl current in _controls)
            {
                SfmlControl? control = SearchControl(globalPosition, current);

                if (control != null)
                    return control;
            }

            return null;
        }

        public SfmlControl? SearchControl(string name, SfmlControl current)
        {
            // look first on childrens to find the lowest level control clicked
            if (current.Childrens != null && current.Childrens.Count > 0)
                foreach (SfmlControl child in current.Childrens)
                {
                    SfmlControl? control = SearchControl(name, child);

                    if (control != null)
                        return control;
                }

            // then look is the current control has been clicked is no children has been clicked
            if (current.Clickable && current.IsDisplayed && current.Name == name)
                return current;

            return null;
        }

        public SfmlControl? SearchControl(Vector2f globalPosition, SfmlControl current)
        {
            // look first on childrens to find the lowest level control clicked
            if (current.Childrens != null && current.Childrens.Count > 0)
                foreach (SfmlControl child in current.Childrens)
                {
                    SfmlControl? control = SearchControl(globalPosition, child);

                    if (control != null)
                        return control;
                }

            // then look is the current control has been clicked is no children has been clicked
            if (current.Clickable && current.IsDisplayed && current.GlobalBounds.Contains(globalPosition))
                return current;

            return null;
        }

        public Vector2f ConvertGlobalPositionToLocalPosition(Vector2f globalPosition)
        {
            Vector2f localPosition = globalPosition - _viewGlobalPosition;

            localPosition = _renderTexture.MapPixelToCoords((Vector2i)localPosition);

            return localPosition;
        }

        public void DrawFrame()
        {
            _renderTexture.Clear(_clearColor);
            _renderTexture.SetView(_view);

            foreach (SfmlControl control in _controls)
            {
                if (control is SfmlPartialWindowContainer)
                {
                    SfmlPartialWindowContainer container = (SfmlPartialWindowContainer)control;

                    Sprite sprite = container.PartialWindow.GetRenderSprite();

                    _renderTexture.Draw(sprite);
                }

                DrawControl(control);
            }

            DrawOnFrame(ref _renderTexture);
        }

        private void DrawControl(SfmlControl current)
        {
            foreach (SfmlControl control in current.Childrens)
            {
                DrawControl(control);
            }

            foreach (SfmlControlDrawable controlDrawable in current.Drawables)
            {
                if (controlDrawable.HasRenderState)
                {
                    _renderTexture.Draw(controlDrawable.Drawable, controlDrawable.RenderState);
                }
                else
                {
                    _renderTexture.Draw(controlDrawable.Drawable);
                }
            }
        }

        public Sprite GetRenderSprite()
        {
            _renderSprite.Dispose();
            _renderSprite = new Sprite(_renderTexture.Texture);
            _renderSprite.Origin = new Vector2f(0.0f, 0.0f);
            _renderSprite.Position = _viewGlobalPosition;

            return _renderSprite;
        }

        public void ZoomIn()
        {
            _view.Zoom(_zoomInFactor);

            _currentZoomFactor *= _zoomInFactor;
        }

        public void ZoomOut()
        {
            _view.Zoom(_zoomOutFactor);

            _currentZoomFactor *= _zoomOutFactor;
        }

        public void Move(Vector2f offset)
        {
            offset *= -1.0f;
            offset *= _currentZoomFactor;

            _currentTotalScrollOffset += offset;

            if (_ignoreMaxScrollOffsets == false)
            {
                // check if scroll outside of the texture total size
                float max_offset_x = _textureTotalSize.X - _view.Size.X;
                float max_offset_y = _textureTotalSize.Y - _view.Size.Y;

                float delta_x = _currentTotalScrollOffset.X - max_offset_x;
                float delta_y = _currentTotalScrollOffset.Y - max_offset_y;

                if (_currentTotalScrollOffset.X < 0.0f)
                {
                    offset.X -= _currentTotalScrollOffset.X;
                    _currentTotalScrollOffset.X -= _currentTotalScrollOffset.X;
                }
                else if (max_offset_x > 0.0f && delta_x > 0)
                {
                    offset.X -= delta_x;
                    _currentTotalScrollOffset.X -= delta_x;

                    offset.X += _scrollOffsetMarginBottom;
                    _currentTotalScrollOffset.X += _scrollOffsetMarginBottom;
                }

                if (_currentTotalScrollOffset.Y < 0.0f)
                {
                    offset.Y -= _currentTotalScrollOffset.Y;
                    _currentTotalScrollOffset.Y -= _currentTotalScrollOffset.Y;
                }
                else if (max_offset_y > 0.0f && delta_y > 0)
                {
                    offset.Y -= delta_y;
                    _currentTotalScrollOffset.Y -= delta_y;

                    offset.Y += _scrollOffsetMarginBottom;
                    _currentTotalScrollOffset.Y += _scrollOffsetMarginBottom;
                }
            }

            _view.Move(offset);
        }

        public void SetViewCenter(Vector2f center)
        {
            this._view.Center = center;
        }

        private void StartScrollMouseDrag(Vector2f startPosition)
        {
            _scrollMouseDragRunning = true;
            _scrollMouseDragLastPosition = startPosition;
        }

        private void ContinueScrollMouseDrag(Vector2f currentPosition)
        {
            if (_scrollMouseDragRunning == false)
                return;

            Vector2f offset = new Vector2f(0.0f, 0.0f);

            if (_scrollableVertical && _scrollableHorizontal)
            {
                offset = currentPosition - _scrollMouseDragLastPosition;
            }
            else if (_scrollableHorizontal)
            {
                offset.X = currentPosition.X - _scrollMouseDragLastPosition.X;
            }
            else if (_scrollableVertical)
            {
                offset.Y = currentPosition.Y - _scrollMouseDragLastPosition.Y;
            }

            Move(offset);

            _scrollMouseDragLastPosition = currentPosition;
        }

        private void EndScrollMouseDrag(Vector2f endPosition)
        {
            if (_scrollMouseDragRunning == false)
                return;

            Vector2f offset = new Vector2f(0.0f, 0.0f);

            if (_scrollableVertical && _scrollableHorizontal)
            {
                offset = endPosition - _scrollMouseDragLastPosition;
            }
            else if (_scrollableHorizontal)
            {
                offset.X = endPosition.X - _scrollMouseDragLastPosition.X;
            }
            else if (_scrollableVertical)
            {
                offset.Y = endPosition.Y - _scrollMouseDragLastPosition.Y;
            }

            Move(offset);

            _scrollMouseDragLastPosition = new Vector2f(0.0f, 0.0f);
            _scrollMouseDragRunning = false;
        }
    }
}
