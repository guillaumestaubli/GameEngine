using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.System;
using SFML.Window;

using SfmlControls.Controls;

namespace SfmlControls.Window
{
    public abstract class SfmlWindow : IDisposable
    {
        private uint _windowWidth;
        private uint _windowHeight;
        private string _windowText;
        private Styles _windowStyles;

        private long _frameDuration = Config.DEFAULT_FRAME_DURATION;
        private Color _clearColor = Config.DEFAULT_CLEAR_COLOR;
        private Stopwatch _clock = new Stopwatch();
        private Stopwatch _elapsed = new Stopwatch();
        private Stopwatch _frameTimer = new Stopwatch();
        private Cursor.CursorType _cursorType = Cursor.CursorType.Arrow;

        private RenderWindow _renderWindow;

        private List<SfmlControl> _controls = new List<SfmlControl>();
        private SfmlControl? _currentFocusControl = null;
        private SfmlControl? _currentTextInputControl = null;

        private Text _fpsCounterLabel;
        private bool _showFpsCounter = false;

        #region GET_SET

        public long FrameDuration
        {
            get { return _frameDuration; }
            set { _frameDuration = value; }
        }

        public Color ClearColor
        {
            get { return _clearColor; }
            set { _clearColor = value; }
        }

        public Stopwatch Clock
        {
            get { return _clock; }
        }

        public Cursor.CursorType CursorType
        {
            get { return _cursorType; }
            set { _cursorType = value; }
        }

        public RenderWindow RenderWindow
        {
            get { return _renderWindow; }
        }

        public List<SfmlControl> Controls
        {
            get { return _controls; }
            set { _controls = value; }
        }

        public bool ShowFpsCounter { get => _showFpsCounter; set => _showFpsCounter = value; }

        public SfmlControl? CurrentFocusControl { get => _currentFocusControl; }

        #endregion

        public SfmlWindow(uint width, uint height, string text, Styles styles)
        {
            _windowWidth = width;
            _windowHeight = height;
            _windowText = text;
            _windowStyles = styles;

            VideoMode videoMode = new VideoMode(_windowWidth, _windowHeight);

            _renderWindow = new RenderWindow(videoMode, _windowText, _windowStyles);
            _renderWindow.SetVerticalSyncEnabled(true);

            _renderWindow.Closed += _renderWindow_Closed;
            _renderWindow.Resized += _renderWindow_Resized;
            _renderWindow.LostFocus += _renderWindow_LostFocus;
            _renderWindow.GainedFocus += _renderWindow_GainedFocus;
            _renderWindow.TextEntered += _renderWindow_TextEntered;
            _renderWindow.KeyPressed += _renderWindow_KeyPressed;
            _renderWindow.KeyReleased += _renderWindow_KeyReleased;
            _renderWindow.MouseWheelScrolled += _renderWindow_MouseWheelScrolled;
            _renderWindow.MouseButtonPressed += _renderWindow_MouseButtonPressed;
            _renderWindow.MouseButtonReleased += _renderWindow_MouseButtonReleased;
            _renderWindow.MouseMoved += _renderWindow_MouseMoved;
            _renderWindow.MouseEntered += _renderWindow_MouseEntered;
            _renderWindow.MouseLeft += _renderWindow_MouseLeft;

            _fpsCounterLabel = new Text("FPS : XXX", Config.DEFAULT_FONT);
            _fpsCounterLabel.Origin = new Vector2f(0.0f, 0.0f);
            _fpsCounterLabel.Position = new Vector2f(5.0f, 5.0f);
            _fpsCounterLabel.FillColor = Color.Red;
        }

        public void Dispose()
        {
            foreach (SfmlControl control in _controls)
            {
                control.Dispose();
            }

            _renderWindow.Dispose();
        }


        #region ABSTRACT

        /// <summary>
        /// Called each frame after the controls are drawn (and before RenderWindow.Display() to allow the user to draw manually stuff on the screen
        /// </summary>
        /// <param name="renderWindow"></param>
        public abstract void DrawOnFrame(ref RenderWindow renderWindow);

        /// <summary>
        /// Called each frame before anything is drawn on the window
        /// </summary>
        /// <param name="renderWindow"></param>
        public abstract void ExecuteBeforeDraw(ref RenderWindow renderWindow, long elapsed_ms);

        #endregion

        public void Run()
        {
            _clock.Start();
            _frameTimer.Start();
            _elapsed.Start();

            while (_renderWindow.IsOpen)
            {
                _renderWindow.DispatchEvents();

                if (_clock.ElapsedMilliseconds >= _frameDuration)
                {
                    _renderWindow.Clear(_clearColor);

                    ExecuteBeforeDraw(ref _renderWindow, _elapsed.ElapsedMilliseconds);

                    _elapsed.Restart();

                    DrawFrame();

                    DrawOnFrame(ref _renderWindow);

                    _renderWindow.SetMouseCursor(new Cursor(_cursorType));

                    if (_showFpsCounter)
                    {
                        _fpsCounterLabel = new Text($"FPS : {1000 / _frameTimer.ElapsedMilliseconds}", Config.DEFAULT_FONT, 18u);
                        _fpsCounterLabel.Origin = new Vector2f(0.0f, 0.0f);
                        _fpsCounterLabel.Position = new Vector2f(5.0f, 5.0f);
                        _fpsCounterLabel.FillColor = Color.Red;

                        _renderWindow.Draw(_fpsCounterLabel);

                        _fpsCounterLabel.Dispose();
                    }

                    _frameTimer.Restart();

                    _renderWindow.Display();

                    _clock.Restart();
                }
            }

            _renderWindow.Dispose();
        }

        private void DrawFrame()
        {
            if (_renderWindow == null)
                return;

            foreach (SfmlControl control in _controls)
            {
                if (control is SfmlPartialWindowContainer)
                {
                    SfmlPartialWindowContainer container = (SfmlPartialWindowContainer)control;

                    container.PartialWindow.DrawFrame();

                    _renderWindow.Draw(container.PartialWindow.GetRenderSprite());
                }

                DrawControl(control);
            }
        }

        private void DrawControl(SfmlControl current)
        {
            if (_renderWindow == null)
                return;

            foreach (SfmlControlDrawable controlDrawable in current.Drawables)
            {
                if (controlDrawable.HasRenderState)
                {
                    _renderWindow.Draw(controlDrawable.Drawable, controlDrawable.RenderState);
                }
                else
                {
                    _renderWindow.Draw(controlDrawable.Drawable);
                }
            }

            foreach (SfmlControl control in current.Childrens)
            {
                DrawControl(control);
            }
        }

        #region RENDER_WINDOW_EVENTS

        private void _renderWindow_Closed(object? sender, EventArgs e)
        {
            if (_renderWindow != null)
            {
                _renderWindow.Close();
            }
        }

        private void _renderWindow_Resized(object? sender, SizeEventArgs e)
        {

        }

        private void _renderWindow_LostFocus(object? sender, EventArgs e)
        {

        }

        private void _renderWindow_GainedFocus(object? sender, EventArgs e)
        {

        }

        private void _renderWindow_TextEntered(object? sender, TextEventArgs e)
        {
            if (_currentTextInputControl != null)
            {
                _currentTextInputControl.TriggerTextEntered(this, e);
            }

            if(_currentFocusControl != null)
            {
                _currentFocusControl.TriggerTextEntered(this, e);
            }
        }

        private void _renderWindow_KeyPressed(object? sender, KeyEventArgs e)
        {
            if (_currentTextInputControl != null)
            {
                _currentTextInputControl.TriggerKeyPressed(this, e);
            }

            if (_currentFocusControl != null)
            {
                _currentFocusControl.TriggerKeyPressed(this, e);
            }
        }

        private void _renderWindow_KeyReleased(object? sender, KeyEventArgs e)
        {
            if (_currentTextInputControl != null)
            {
                _currentTextInputControl.TriggerKeyReleased(this, e);
            }

            if (_currentFocusControl != null)
            {
                _currentFocusControl.TriggerKeyReleased(this, e);
            }
        }

        private void _renderWindow_MouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
        {
            SfmlControl? controlScrolled = GetControl(new Vector2f(e.X, e.Y));

            if (controlScrolled != null)
            {
                controlScrolled.TriggerMouseWheelScrolled(this, e);
            }
        }

        private void _renderWindow_MouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {
            SfmlControl? controlPressed = GetControl(new Vector2f(e.X, e.Y));

            if (controlPressed != null && controlPressed.Clickable == true)
            {
                controlPressed.TriggerMouseButtonPressed(this, e);
            }
        }

        private void _renderWindow_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            SfmlControl? controlReleased = GetControl(new Vector2f(e.X, e.Y));

            if (controlReleased != null && controlReleased.Clickable == true)
            {
                controlReleased.TriggerMouseButtonReleased(this, e);
            }
        }

        private void _renderWindow_MouseMoved(object? sender, MouseMoveEventArgs e)
        {
            SfmlControl? controlHovered = GetControl(new Vector2f(e.X, e.Y));

            if (controlHovered != null)
            {
                controlHovered.TriggerMouseMoved(controlHovered, e);

                if (_currentFocusControl != null)
                {
                    if (_currentFocusControl.Name != controlHovered.Name)
                    {
                        _currentFocusControl.TriggerLostFocus(this, e);
                        controlHovered.TriggerGainedFocus(this, e);

                        _currentFocusControl = controlHovered;
                    }
                }
                else
                {
                    _currentFocusControl = controlHovered;
                    _currentFocusControl.TriggerGainedFocus(this, e);
                }

                _cursorType = controlHovered.GetCursorType(new Vector2f(e.X, e.Y));
            }
            else
            {
                _cursorType = Cursor.CursorType.Arrow;

                if (_currentFocusControl != null)
                {
                    _currentFocusControl.TriggerLostFocus(this, e);
                    _currentFocusControl = null;
                }
            }
        }

        private void _renderWindow_MouseEntered(object? sender, EventArgs e)
        {

        }

        private void _renderWindow_MouseLeft(object? sender, EventArgs e)
        {

        }

        #endregion

        #region UTILS

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

        public void RemoveControl(string name)
        {
            SfmlControl? control = _controls.FirstOrDefault(x => x.Name == name);

            if (control == null)
                return;

            RemoveControl(control);
        }

        public void RemoveControl(SfmlControl control)
        {
            _controls = _controls.Where(x => x.Name != control.Name).ToList();
        }

        #endregion
    }
}
