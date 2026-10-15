
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Graphics;
using SFML.Window;

using SfmlControls.Controls;
using SfmlControls.Window.PartialWindow;
using SfmlControls.Utils;

namespace SfmlControls.Controls
{
    public class SfmlPartialWindowContainer : SfmlControl
    {
        SfmlPartialWindow _partialWindow;

        Color _backgroundColor;
        Color _borderColor;

        bool _hasBorder;

        #region GET_SET

        public SfmlPartialWindow PartialWindow
        {
            get { return _partialWindow; }
            set { _partialWindow = value; }
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

        public bool HasBorders
        {
            get { return _hasBorder; }
            set
            {
                _hasBorder = value;
                GenerateRender();
            }
        }

        #endregion

        public SfmlPartialWindowContainer(string name, Vector2f position, Vector2f size, SfmlPartialWindow partialWindow)
            : base(SfmlControlType.PartialWindowContainer, name, position, size)
        {
            _partialWindow = partialWindow;

            _backgroundColor = Config.DEFAULT_BACKGROUND_COLOR;
            _borderColor = Config.DEFAULT_BORDER_COLOR;

            _hasBorder = true;

            GenerateRender();

            Resized += PartialWindowContainer_Resized;
            LostFocus += PartialWindowContainer_LostFocus;
            GainedFocus += PartialWindowContainer_GainedFocus;
            TextEntered += PartialWindowContainer_TextEntered;
            KeyPressed += PartialWindowContainer_KeyPressed;
            KeyReleased += PartialWindowContainer_KeyReleased;
            MouseWheelScrolled += PartialWindowContainer_MouseWheelScrolled;
            MouseButtonPressed += PartialWindowContainer_MouseButtonPressed;
            MouseButtonReleased += PartialWindowContainer_MouseButtonReleased;
            MouseMoved += PartialWindowContainer_MouseMoved;
            MouseEntered += PartialWindowContainer_MouseEntered;
            MouseLeft += PartialWindowContainer_MouseLeft;
        }

        #region EVENTS

        private void PartialWindowContainer_Resized(object? sender, SizeEventArgs e)
        {
            _partialWindow.TriggerResized(this, e);
        }

        private void PartialWindowContainer_LostFocus(object? sender, EventArgs e)
        {
            _partialWindow.TriggerLostFocus(this, e);
        }

        private void PartialWindowContainer_GainedFocus(object? sender, EventArgs e)
        {
            _partialWindow.TriggerGainedFocus(this, e);
        }

        private void PartialWindowContainer_TextEntered(object? sender, TextEventArgs e)
        {
            _partialWindow.TriggerTextEntered(this, e);
        }

        private void PartialWindowContainer_KeyPressed(object? sender, KeyEventArgs e)
        {
            _partialWindow.TriggerKeyPressed(this, e);
        }

        private void PartialWindowContainer_KeyReleased(object? sender, KeyEventArgs e)
        {
            _partialWindow.TriggerKeyReleased(this, e);
        }

        private void PartialWindowContainer_MouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
        {
            _partialWindow.TriggerMouseWheelScrolled(this, e);
        }

        private void PartialWindowContainer_MouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {
            _partialWindow.TriggerMouseButtonPressed(this, e);
        }

        private void PartialWindowContainer_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            _partialWindow.TriggerMouseButtonReleased(this, e);
        }

        private void PartialWindowContainer_MouseMoved(object? sender, MouseMoveEventArgs e)
        {
            _partialWindow.TriggerMouseMoved(this, e);
        }

        private void PartialWindowContainer_MouseEntered(object? sender, EventArgs e)
        {
            _partialWindow.TriggerMouseEntered(this, e);
        }

        private void PartialWindowContainer_MouseLeft(object? sender, EventArgs e)
        {
            _partialWindow.TriggerMouseLeft(this, e);
        }

        #endregion

        public override Cursor.CursorType GetCursorType(Vector2f cursorPosition)
        {
            return _partialWindow.GetCursorType(cursorPosition);
        }

        protected override void GenerateRender()
        {
            _drawables.Clear();

            if (_hasBorder)
            {
                VertexArray borders = SfmlUtils.GetLineStripRectangle(_globalPosition, _size, _borderColor);

                _drawables.Add(new SfmlControlDrawable(borders));
            }
        }
    }
}
