using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlControls.Events;
using SfmlControls.Utils;
using SfmlControls.Window;
using SfmlControls.Window.PartialWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SfmlControls.Controls
{
    public class SfmlTextbox : SfmlControl
    {
        protected string _textStr;
        protected uint _characterSize;
        protected Color _textColor;
        protected Color _backgroundColor;
        protected Color _borderColor;
        protected Text.Styles _styles;
        protected Font _font;
        protected bool _isInInputMode;
        protected bool _isDisabled;
        protected int _editIndex;

        protected const float _INTERIOR_MARGIN = 5.0f;
        protected const char _EDITING_CHAR = '|';

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

        public bool IsInInputMode
        {
            get { return _isInInputMode; }
            set
            {
                _isInInputMode = value;
                GenerateRender();
            }
        }

        public bool IsDisabled
        {
            get { return _isDisabled; }
            set
            {
                _isDisabled = value;
                GenerateRender();
            }
        }

        #endregion

        public SfmlTextbox(string name, Vector2f position, Vector2f size, string textStr = "")
            : base(SfmlControlType.Textbox, name, position, size)
        {
            _textStr = textStr;
            _characterSize = Config.DEFAULT_CHARACTER_SIZE;
            _textColor = Config.DEFAULT_TEXT_COLOR;
            _backgroundColor = Config.DEFAULT_BACKGROUND_COLOR_TEXTBOX;
            _borderColor = Config.DEFAULT_BORDER_COLOR;
            _styles = Text.Styles.Regular;
            _font = Config.DEFAULT_FONT;
            _isInInputMode = false;
            _isDisabled = false;
            _isTextInput = true;
            _editIndex = textStr.Length;

            GenerateRender();

            MouseButtonReleased += Textbox_MouseButtonReleased;
            TextEntered += Textbox_TextEntered;
            KeyPressed += Textbox_KeyPressed;
        }

        #region EVENTS_DECLARATION

        public delegate void TextChangedHandler(object? sender, TextChangedEventsArgs e);
        public event TextChangedHandler TextChanged = delegate { };
        public void TriggerTextChanged()
        {
            TextChangedEventsArgs args = new TextChangedEventsArgs(this._textStr);

            TextChanged?.Invoke(this, args);
        }

        #endregion

        #region EVENTS

        public void Textbox_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (_isDisabled)
                return;

            _isInInputMode = true;

            GenerateRender();
        }

        private void Textbox_KeyPressed(object? sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Left:
                    {
                        _editIndex -= 1;

                        if (_editIndex < 0)
                            _editIndex = 0;

                        GenerateRender();

                        break;
                    }
                case Keyboard.Key.Right:
                    {
                        _editIndex += 1;

                        if (_editIndex > _textStr.Length)
                            _editIndex = _textStr.Length;

                        GenerateRender();

                        break;
                    }
                case Keyboard.Key.Backspace:
                    {
                        string textStr = "";

                        for (int i = 0; i < _textStr.Length; ++i)
                        {
                            if (_editIndex - 1 != i)
                            {
                                textStr += _textStr[i];
                            }
                        }

                        _textStr = textStr;
                        _editIndex -= 1;

                        GenerateRender();

                        break;
                    }
                case Keyboard.Key.Delete:
                    {
                        string textStr = "";

                        for (int i = 0; i < _textStr.Length; ++i)
                        {
                            if(_editIndex != i)
                            {
                                textStr += _textStr[i];
                            }
                        }

                        _textStr = textStr;

                        GenerateRender();

                        break;
                    }
                case Keyboard.Key.Enter:
                    {
                        TriggerTextChanged();
                        break;
                    }
            }
        }


        public void Textbox_TextEntered(object? sender, TextEventArgs e)
        {
            if (_isDisabled)
                return;

            if (_isInInputMode)
            {
                switch (e.Unicode)
                {
                    case "\b":
                        break;
                    case "\r":
                        break;
                    default:

                        if (_editIndex == _textStr.Length)
                        {
                            _textStr += e.Unicode;
                            _editIndex += 1;
                        }
                        else
                        {
                            string textStr = "";

                            for (int i = 0; i < _textStr.Length; ++i)
                            {
                                if (_editIndex == i)
                                {
                                    textStr += e.Unicode;
                                }

                                textStr += _textStr[i];
                            }

                            _textStr = textStr;

                            _editIndex += 1;
                        }



                        break;
                }
            }

            GenerateRender();
        }

        #endregion

        public override Cursor.CursorType GetCursorType(Vector2f cursorPosition)
        {
            if (Clickable)
            {
                if (_isDisabled)
                {
                    return Cursor.CursorType.Arrow;
                }
                else
                {
                    return Cursor.CursorType.Text;
                }
            }
            else
            {
                return Cursor.CursorType.Arrow;
            }
        }

        protected override void GenerateRender()
        {
            _drawables.Clear();

            VertexArray vertexArrayBackground = SfmlUtils.GetTriangleStripRectangle(
                position: _globalPosition,
                size: _size,
                color: _isInInputMode ? Color.White : _backgroundColor);

            VertexArray vertexArrayBorder = SfmlUtils.GetLineStripRectangle(_globalPosition, _size, _borderColor);

            string textStr = "";

            if (!_isInInputMode)
                textStr = _textStr;
            else
            {
                for (int i = 0; i < _textStr.Length; ++i)
                {
                    if (i == _editIndex)
                        textStr += _EDITING_CHAR;

                    textStr += _textStr[i];
                }

                if (_editIndex == _textStr.Length)
                    textStr += _EDITING_CHAR;
            }

            Text text = new Text(textStr, _font, _characterSize);
            text.FillColor = _textColor;
            text.Style = _styles;

            // center the text vertically on the inside of the background
            Vector2f origin = new Vector2f(0.0f, 0.0f);
            origin.Y = (text.GetGlobalBounds().Size / 2.0f + text.GetLocalBounds().Position).Y;

            Vector2f position = new Vector2f(_INTERIOR_MARGIN, 0.0f);
            position.X += vertexArrayBackground.Bounds.Position.X;
            position.Y = (vertexArrayBackground.Bounds.Position + vertexArrayBackground.Bounds.Size / 2.0f).Y;

            text.Origin = origin;
            text.Position = position;

            _drawables.Add(new SfmlControlDrawable(vertexArrayBackground));
            _drawables.Add(new SfmlControlDrawable(vertexArrayBorder));
            _drawables.Add(new SfmlControlDrawable(text));
        }

        public void DisableInputMode()
        {
            _isInInputMode = false;
            GenerateRender();
        }

        public void SetTextStr(string textStr)
        {
            _textStr = textStr;

            _editIndex = _textStr.Length;

            GenerateRender();
        }
    }
}
