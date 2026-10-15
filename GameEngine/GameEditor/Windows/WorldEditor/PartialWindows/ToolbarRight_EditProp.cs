using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.System;
using SFML.Window;

using SfmlControls.Controls;
using SfmlControls.Events;
using SfmlControls.Window.PartialWindow;

using GameEngine.Core.Textures;
using GameEngine.Core.World;
using GameEngine.Core.Props;
using System.Globalization;

namespace GameEngine.GameEditor.Windows.WorldEditor.PartialWindows
{
    public class ToolbarRight_EditProp : SfmlPartialWindow
    {
        private const float SIZE_X = WorldEditorConfig.TB_RIGHT_WIDTH;
        private const float SIZE_Y = WorldEditorConfig.TB_RIGHT_HEIGHT;

        private const float POS_X = WorldEditorConfig.RA_WIDTH;
        private const float POS_Y = WorldEditorConfig.TB_TOP_HEIGHT;

        private SfmlLabel _lblTitle;

        private SfmlLabel _lblName;
        private SfmlLabel _lblPosition;
        private SfmlLabel _lblSize;
        private SfmlLabel _lblTexture;

        private SfmlTextbox _txtName;
        private SfmlTextbox _txtPositionX;
        private SfmlTextbox _txtPositionY;
        private SfmlTextbox _txtSizeX;
        private SfmlTextbox _txtSizeY;

        private SfmlImage _imgTexture;

        private SfmlButton _btnDelete;

        public ToolbarRight_EditProp(RenderWindow parentWindow) : base(new Vector2f(POS_X, POS_Y), new Vector2f(SIZE_X, SIZE_Y), parentWindow)
        {
            #region CONTROLS

            Vector2f lbl_pos = new Vector2f(5.0f, 5.0f);
            Vector2f lbl_inter = new Vector2f(0.0f, 30.0f);

            _lblTitle = new SfmlLabel("lblTitle", "Prop Properties :", lbl_pos);
            _lblTitle.Styles = Text.Styles.Bold | Text.Styles.Underlined;
            this.AddControl(_lblTitle);

            lbl_pos += lbl_inter;

            _lblName = new SfmlLabel("lblName", "Name :", lbl_pos);
            this.AddControl(_lblName);

            lbl_pos += lbl_inter;

            _lblPosition = new SfmlLabel("lblPosition", "Position (X - Y) :", lbl_pos);
            this.AddControl(_lblPosition);

            lbl_pos += lbl_inter;

            _lblSize = new SfmlLabel("lblSize", "Size (X - Y) :", lbl_pos);
            this.AddControl(_lblSize);

            lbl_pos += lbl_inter;

            _lblTexture = new SfmlLabel("lblTexture", "Prop Texture :", lbl_pos);
            this.AddControl(_lblTexture);

            Vector2f txt_size = new Vector2f(180.0f, 20.0f);
            Vector2f txt_pos = new Vector2f(100.0f, 33.0f);
            Vector2f txt_inter = new Vector2f(0.0f, 30.0f);

            _txtName = new SfmlTextbox("txtName", txt_pos, txt_size, string.Empty);
            _txtName.TextChanged += TxtName_TextChanged;
            this.AddControl(_txtName);

            txt_pos += txt_inter;

            _txtPositionX = new SfmlTextbox("txtPositionX", txt_pos, new Vector2f(85.0f, 20.0f), string.Empty);
            _txtPositionX.TextChanged += TxtPositionX_TextChanged;
            this.AddControl(_txtPositionX);

            _txtPositionY = new SfmlTextbox("txtPositionY", txt_pos + new Vector2f(95.0f, 0.0f), new Vector2f(85.0f, 20.0f), string.Empty);
            _txtPositionY.TextChanged += TxtPositionY_TextChanged;
            this.AddControl(_txtPositionY);

            txt_pos += txt_inter;

            _txtSizeX = new SfmlTextbox("txtSizeX", txt_pos, new Vector2f(85.0f, 20.0f), string.Empty);
            _txtSizeX.TextChanged += TxtSizeX_TextChanged;
            this.AddControl(_txtSizeX);

            _txtSizeY = new SfmlTextbox("txtSizeY", txt_pos + new Vector2f(95.0f, 0.0f), new Vector2f(85.0f, 20.0f), string.Empty);
            _txtSizeY.TextChanged += TxtSizeY_TextChanged;
            this.AddControl(_txtSizeY);

            txt_pos += txt_inter;

            _imgTexture = new SfmlImage("imgTexture", txt_pos, new Vector2f(100.0f, 100.0f), TextureManager.Texture_NotFound.Image);
            _imgTexture.HasBorder = true;
            this.AddControl(_imgTexture);





            _btnDelete = new SfmlButton("btnDelete", "Delete Prop", new Vector2f(10.0f, SIZE_Y - 30.0f), new Vector2f(150.0f, 20.0f));
            _btnDelete.MouseButtonReleased += BtnDelete_MouseButtonReleased;
            this.AddControl(_btnDelete);

            #endregion
        }

        #region OVERRIDE
        public override void DrawOnFrame(ref RenderTexture renderTexture)
        {

        }

        public override Cursor.CursorType GetCursorType(Vector2f globalPosition)
        {
            Vector2f localPosition = ConvertGlobalPositionToLocalPosition(globalPosition);

            SfmlControl? control = GetControl(localPosition);

            if (control == null)
                return Cursor.CursorType.Arrow;

            return control.GetCursorType(localPosition);
        }

        #endregion

        #region EVENTS

        private void TxtName_TextChanged(object? sender, TextChangedEventsArgs e)
        {
            if (GameEditor.CurrentPropForEdit == null)
                return;

            Prop? prop = World.Props.FirstOrDefault(x => x.Name == GameEditor.CurrentPropForEdit.Name);

            if (prop == null)
                return;

            World.RenameProp(prop, e.TextStr);
        }

        private void TxtPositionX_TextChanged(object? sender, TextChangedEventsArgs e)
        {
            if (GameEditor.CurrentPropForEdit == null)
                return;

            float pos_x = 0.0f;

            if(!float.TryParse(e.TextStr, CultureInfo.InvariantCulture.NumberFormat, out pos_x))
            {
                Console.WriteLine($"> {nameof(ToolbarRight_EditProp)} - {nameof(TxtPositionX_TextChanged)} - Error - Cannot parse {e.TextStr} to float.");
                return;
            }

            Vector2f newPos = GameEditor.CurrentPropForEdit.GlobalPosition;
            newPos.X = pos_x;

            World.MoveProp(GameEditor.CurrentPropForEdit, newPos);

            GameEditor.CurrentPropForEdit = World.Props.First(x => x.Name == GameEditor.CurrentPropForEdit.Name);
        }

        private void TxtPositionY_TextChanged(object? sender, TextChangedEventsArgs e)
        {
            if (GameEditor.CurrentPropForEdit == null)
                return;

            float pos_y = 0.0f;

            if (!float.TryParse(e.TextStr, CultureInfo.InvariantCulture.NumberFormat, out pos_y))
            {
                Console.WriteLine($"> {nameof(ToolbarRight_EditProp)} - {nameof(TxtPositionY_TextChanged)} - Error - Cannot parse {e.TextStr} to float.");
                return;
            }

            Vector2f newPos = GameEditor.CurrentPropForEdit.GlobalPosition;
            newPos.Y = pos_y;

            World.MoveProp(GameEditor.CurrentPropForEdit, newPos);

            GameEditor.CurrentPropForEdit = World.Props.First(x => x.Name == GameEditor.CurrentPropForEdit.Name);
        }

        private void TxtSizeX_TextChanged(object? sender, TextChangedEventsArgs e)
        {
            if (GameEditor.CurrentPropForEdit == null)
                return;

            float size_x = 0.0f;

            if(!float.TryParse(e.TextStr, CultureInfo.InvariantCulture.NumberFormat, out size_x))
            {
                Console.WriteLine($"> {nameof(ToolbarRight_EditProp)} - {nameof(TxtSizeX_TextChanged)} - Error - Cannot parse {e.TextStr} to float.");
                return;
            }

            float ratio = GameEditor.CurrentPropForEdit.Texture.Texture.Size.X / size_x;

            Vector2f newSize = new Vector2f(0.0f, 0.0f);
            newSize.X = size_x;
            newSize.Y = GameEditor.CurrentPropForEdit.Texture.Texture.Size.Y / ratio;

            World.ResizeProp(GameEditor.CurrentPropForEdit, newSize);

            GameEditor.CurrentPropForEdit = World.Props.First(x => x.Name == GameEditor.CurrentPropForEdit.Name);

            _txtSizeY.SetTextStr($"{GameEditor.CurrentPropForEdit.Size.Y}");
        }

        private void TxtSizeY_TextChanged(object? semder,  TextChangedEventsArgs e)
        {
            if (GameEditor.CurrentPropForEdit == null)
                return;

            float size_y = 0.0f;

            if (!float.TryParse(e.TextStr, CultureInfo.InvariantCulture.NumberFormat, out size_y))
            {
                Console.WriteLine($"> {nameof(ToolbarRight_EditProp)} - {nameof(TxtSizeY_TextChanged)} - Error - Cannot parse {e.TextStr} to float.");
                return;
            }

            float ratio = GameEditor.CurrentPropForEdit.Texture.Texture.Size.Y / size_y;

            Vector2f newSize = new Vector2f(0.0f, 0.0f);
            newSize.X = GameEditor.CurrentPropForEdit.Texture.Texture.Size.X / ratio;
            newSize.Y = size_y;

            World.ResizeProp(GameEditor.CurrentPropForEdit, newSize);

            GameEditor.CurrentPropForEdit = World.Props.First(x => x.Name == GameEditor.CurrentPropForEdit.Name);

            _txtSizeX.SetTextStr($"{GameEditor.CurrentPropForEdit.Size.X}");
        }

        private void TxtSize_TextChanged(object? sender, TextChangedEventsArgs e)
        {
            if (GameEditor.CurrentPropForEdit == null)
                return;

            Prop? prop = World.Props.FirstOrDefault(x => x.Name == GameEditor.CurrentPropForEdit.Name);

            if (prop == null)
                return;

            string txtSize = e.TextStr;

            string[] xy_str_size = txtSize.Split(";");

            if (xy_str_size.Length != 2)
                return;

            string x_str_size = xy_str_size[0];
            string y_str_size = xy_str_size[1];

            float size_x = 0.0f;
            float size_y = 0.0f;

            if (!float.TryParse(x_str_size, CultureInfo.InvariantCulture.NumberFormat, out size_x))
            {
                Console.WriteLine($"> {nameof(ToolbarRight_EditProp)} - {nameof(TxtSize_TextChanged)} - Error - Cannot parse {x_str_size} to float.");

                return;
            }

            if (!float.TryParse(y_str_size, CultureInfo.InvariantCulture.NumberFormat, out size_y))
            {
                Console.WriteLine($"> {nameof(ToolbarRight_EditProp)} - {nameof(TxtSize_TextChanged)} - Error - Cannot parse {y_str_size} to float.");

                return;
            }

            Vector2f newSize = new Vector2f(size_x, size_y);

            World.ResizeProp(prop, newSize);

            prop = World.Props.First(x => x.Name == prop.Name);

            GameEditor.CurrentPropForEdit = prop;
        }

        private void BtnDelete_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (GameEditor.CurrentPropForEdit == null)
                return;

            World.DeleteProp(GameEditor.CurrentPropForEdit);

            GameEditor.CurrentPropForEdit = null;

            UpdateControlsDisplayValues();
        }

        #endregion

        public void UpdateControlsDisplayValues()
        {
            if (GameEditor.CurrentPropForEdit == null)
            {
                _txtName.TextStr = string.Empty;
                _txtPositionX.TextStr = string.Empty;
                _txtPositionY.TextStr = string.Empty;
                _txtSizeX.TextStr = string.Empty;
                _txtSizeY.TextStr = string.Empty;
                _imgTexture.Image = TextureManager.Texture_NotFound.Image;
                _btnDelete.Clickable = false;
            }
            else
            {
                _txtName.SetTextStr($"{GameEditor.CurrentPropForEdit.Name}");
                _txtPositionX.SetTextStr($"{GameEditor.CurrentPropForEdit.GlobalPosition.X}");
                _txtPositionY.SetTextStr($"{GameEditor.CurrentPropForEdit.GlobalPosition.Y}");
                _txtSizeX.SetTextStr($"{GameEditor.CurrentPropForEdit.Size.X}");
                _txtSizeY.SetTextStr($"{GameEditor.CurrentPropForEdit.Size.Y}");
                _imgTexture.Image = GameEditor.CurrentPropForEdit.Texture.Image;
                _btnDelete.Clickable = true;
            }
        }
    }
}
