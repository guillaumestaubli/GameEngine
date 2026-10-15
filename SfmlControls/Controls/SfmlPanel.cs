using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlControls.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlControls.Controls
{
    public class SfmlPanel : SfmlControl
    {
        protected Color _backgroundColor;
        protected Color _borderColor;
        protected bool _showBorder;

        #region GET_SET

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

        public bool ShowBorder
        {
            get { return _showBorder; }
            set
            {
                _showBorder = value;
                GenerateRender();
            }
        }

        #endregion

        public SfmlPanel(string name, Vector2f position, Vector2f size)
            : base(SfmlControlType.Panel, name, position, size)
        {
            _backgroundColor = Config.DEFAULT_BACKGROUND_COLOR;
            _borderColor = Config.DEFAULT_BORDER_COLOR;
            _showBorder = true;

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

            VertexArray vertexArrayBorder = SfmlUtils.GetLineStripRectangle(_globalPosition, _size, _borderColor);
            VertexArray vertexArrayBackground = SfmlUtils.GetTriangleStripRectangle(_globalPosition, _size, _backgroundColor);

            _drawables.Add(new SfmlControlDrawable(vertexArrayBackground));

            if (_showBorder)
                _drawables.Add(new SfmlControlDrawable(vertexArrayBorder));

        }
    }
}
