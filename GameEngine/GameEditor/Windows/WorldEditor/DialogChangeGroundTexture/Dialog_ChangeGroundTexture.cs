using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Graphics;
using SFML.Window;

using SfmlControls.Controls;
using SfmlControls.Window;
using GameEngine.Core.Textures;
using System.Diagnostics;
using SfmlControls.Events;

namespace GameEngine.GameEditor.Windows.WorldEditor.DialogChangeGroundTexture
{
    public class Dialog_ChangeGroundTexture : SfmlWindow
    {
        private const uint _W_WIDTH = 1000u;
        private const uint _W_HEIGHT = 500u;
        private const string _W_TITLE = "GameEditor - WorldEditor - DialogChangeGroundTexture";

        private TextureMainGroup? _mainGroupSelected = null;
        private TextureSubGroup? _subGroupSelected = null;
        private TextureDetails? _textureSelected = null;

        private SfmlDropdown _dropdownTextureMainGroups;

        private SfmlPanel _panelSubGroups;
        private SfmlPanel _panelTextures;

        private SfmlButton _btnOk;
        private SfmlButton _btnCancel;

        public Dialog_ChangeGroundTexture() : base(_W_WIDTH, _W_HEIGHT, _W_TITLE, Styles.Titlebar)
        {
            if (GameEditor.CurrentTileForEdit != null)
            {
                foreach (TextureMainGroup mainGroup in TextureManager.LibraryGround.MainGroups)
                {
                    foreach (TextureSubGroup subGroup in mainGroup.SubGroups)
                    {
                        foreach (TextureDetails texture in subGroup.Textures)
                        {
                            if (texture.Name == GameEditor.CurrentTileForEdit.Texture.Name)
                            {
                                _mainGroupSelected = mainGroup;
                                _subGroupSelected = subGroup;
                                _textureSelected = texture;

                                break;
                            }
                        }

                        if (_mainGroupSelected != null || _subGroupSelected != null || _textureSelected != null)
                            break;
                    }

                    if (_mainGroupSelected != null || _subGroupSelected != null || _textureSelected != null)
                        break;
                }
            }

            _dropdownTextureMainGroups = new SfmlDropdown(
                name: "dropdownTextureMainGroups",
                position: new Vector2f(10.0f, 10.0f),
                width: 200.0f
                );
            _dropdownTextureMainGroups.SelectionChanged += DropdownMainGroups_SelectionChanged;

            foreach (TextureMainGroup mainGroup in TextureManager.LibraryGround.MainGroups)
            {
                _dropdownTextureMainGroups.AddItem(
                    name: mainGroup.Name,
                    textStr: mainGroup.Name,
                    isSelected: _mainGroupSelected != null && _mainGroupSelected.Name == mainGroup.Name
                    );
            }

            this.Controls.Add(_dropdownTextureMainGroups);

            _panelSubGroups = new SfmlPanel(
                name: "panelSubGroups",
                position: new Vector2f(10.0f, 40.0f),
                size: new Vector2f(100.0f, 40.0f)
                );

            this.Controls.Add(_panelSubGroups);

            SetPanelSubGroups(_mainGroupSelected);

            _panelTextures = new SfmlPanel(
                name: "panelTextures",
                position: new Vector2f(10.0f, 80.0f),
                size: new Vector2f(100.0f, 100.0f)
                );

            this.Controls.Add(_panelTextures);

            SetPanelTextures(_subGroupSelected);

            ResizeWindowToContent();

            _btnCancel = new SfmlButton(
                name: "btnCancel",
                textStr: "Cancel",
                position: new Vector2f(10.0f, RenderWindow.Size.Y),
                size: new Vector2f((RenderWindow.Size.X - 30.0f) / 2.0f, 20.0f)
                );
            _btnCancel.MouseButtonReleased += BtnCancel_MouseButtonReleased;

            this.Controls.Add(_btnCancel);

            _btnOk = new SfmlButton(
               name: "btnOk",
               textStr: "OK",
               position: new Vector2f(_btnCancel.GlobalPosition.X + _btnCancel.Size.X + 10.0f, RenderWindow.Size.Y),
               size: new Vector2f((RenderWindow.Size.X - 30.0f) / 2.0f, 20.0f)
               );
            _btnOk.MouseButtonReleased += BtnOk_MouseButtonReleased;

            this.Controls.Add(_btnOk);

            ResizeWindowToContent();
        }

        public override void DrawOnFrame(ref RenderWindow renderWindow)
        {

        }

        public override void ExecuteBeforeDraw(ref RenderWindow renderWindow, long elapsed_ms)
        {
            
        }

        #region EVENTS

        private void DropdownMainGroups_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
                return;

            if (sender is not SfmlDropdown)
                return;

            TextureMainGroup? mainGroup = TextureManager.LibraryGround.MainGroups.FirstOrDefault(x => x.Name == e.ItemName);

            if (mainGroup != null)
            {
                if (_mainGroupSelected == null)
                {
                    _mainGroupSelected = mainGroup;
                    SetPanelSubGroups(_mainGroupSelected);

                    _subGroupSelected = _mainGroupSelected.SubGroups.FirstOrDefault();
                    SetPanelTextures(_subGroupSelected);
                }
                else if (mainGroup.Name != _mainGroupSelected.Name)
                {
                    _mainGroupSelected = mainGroup;
                    SetPanelSubGroups(_mainGroupSelected);

                    _subGroupSelected = _mainGroupSelected.SubGroups.FirstOrDefault();
                    SetPanelTextures(_subGroupSelected);
                }
            }

            ResizeWindowToContent();
        }

        private void BtnSubGroup_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (sender == null)
                return;

            if (sender is not SfmlButton)
                return;

            SfmlButton btnSubGroup = (SfmlButton)sender;

            if (_mainGroupSelected == null)
                return;

            TextureSubGroup? subGroup = _mainGroupSelected.SubGroups.FirstOrDefault(x => x.Name == btnSubGroup.Name);

            if (subGroup != null)
            {
                if (_subGroupSelected == null)
                {
                    _subGroupSelected = subGroup;
                    SetPanelTextures(_subGroupSelected);
                }
                else if (subGroup.Name != _subGroupSelected.Name)
                {
                    _subGroupSelected = subGroup;
                    SetPanelTextures(_subGroupSelected);
                }
            }

            UpdateSubGroupButtonsBackgroundColor();

            ResizeWindowToContent();
        }

        private void ImgTexture_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (sender == null)
                return;

            if (sender is not SfmlImage)
                return;

            SfmlImage imgTexture = (SfmlImage)sender;

            TextureDetails? texture = TextureManager.LibraryGround.GetTexture(imgTexture.Name);

            if (texture == null)
                return;

            _textureSelected = texture;

            SetPanelTextures(_subGroupSelected);
        }

        private void BtnCancel_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            this.RenderWindow.Close();
        }

        private void BtnOk_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (GameEditor.CurrentTileForEdit != null && _textureSelected != null)
            {
                GameEditor.CurrentTileForEdit_EditTexture(_textureSelected);
            }

            this.RenderWindow.Close();
        }

        #endregion

        private void SetPanelSubGroups(TextureMainGroup? mainGroup)
        {
            _panelSubGroups.ClearChildrens();

            if (mainGroup != null)
            {
                Vector2f btn_pos = new Vector2f(10.0f, 10.0f);
                Vector2f btn_size = new Vector2f(100.0f, 20.0f);

                Vector2f btn_add = new Vector2f(110.0f, 0.0f);

                foreach (TextureSubGroup subGroup in mainGroup.SubGroups)
                {
                    SfmlButton btnSubGroup = new SfmlButton(
                        name: subGroup.Name,
                        textStr: subGroup.Name,
                        position: btn_pos,
                        size: btn_size
                        );
                    btnSubGroup.BackgroundColor = _subGroupSelected != null && _subGroupSelected.Name == subGroup.Name ? Config.DefaultColors.LightGray : Config.DefaultColors.White;
                    btnSubGroup.MouseButtonReleased += BtnSubGroup_MouseButtonReleased;

                    _panelSubGroups.AddChildren(btnSubGroup);

                    btn_pos += btn_add;
                }

                SfmlControl? btnMax_x = _panelSubGroups.Childrens.OrderBy(x => x.LocalPosition.X).LastOrDefault();
                SfmlControl? btnMax_y = _panelSubGroups.Childrens.OrderBy(x => x.LocalPosition.Y).LastOrDefault();

                if (btnMax_x != null && btnMax_y != null)
                {
                    Vector2f panelSize = new Vector2f(
                        x: btnMax_x.LocalPosition.X + btnMax_x.Size.X + 10.0f,
                        y: btnMax_y.LocalPosition.Y + btnMax_y.Size.Y + 10.0f
                        );

                    _panelSubGroups.Size = panelSize;
                }
            }

            UpdateDialogControls();
        }

        private void SetPanelTextures(TextureSubGroup? subGroup)
        {
            _panelTextures.ClearChildrens();

            if (subGroup != null)
            {
                Vector2f img_pos = new Vector2f(10.0f, 10.0f);
                Vector2f img_size = new Vector2f(100.0f, 100.0f);

                float img_add_x = img_size.X + 10.0f;
                float img_add_y = img_size.Y + 10.0f;

                int counter = 0;

                foreach (TextureDetails texture in subGroup.Textures)
                {
                    SfmlImage imgTexture = new SfmlImage(
                        name: texture.Name,
                        position: img_pos,
                        size: img_size,
                        image: texture.Image
                        );
                    imgTexture.HasBorder = true;
                    imgTexture.BorderColor = Color.Black;
                    imgTexture.Clickable = true;
                    imgTexture.MouseButtonReleased += ImgTexture_MouseButtonReleased;

                    _panelTextures.AddChildren(imgTexture);

                    if (_textureSelected != null && _textureSelected.Name == texture.Name)
                        imgTexture.BorderColor = Color.Red;

                    if (counter < 2)
                    {
                        img_pos.X += img_add_x;
                        ++counter;
                    }
                    else
                    {
                        img_pos.X = 10.0f;
                        img_pos.Y += img_add_y;
                        counter = 0;
                    }
                }

                SfmlControl? img_max_x = _panelTextures.Childrens.OrderBy(x => x.LocalPosition.X).LastOrDefault();
                SfmlControl? img_max_y = _panelTextures.Childrens.OrderBy(x => x.LocalPosition.Y).LastOrDefault();

                if (img_max_x != null && img_max_y != null)
                {
                    Vector2f panelSize = new Vector2f(
                        x: img_max_x.LocalPosition.X + img_max_x.Size.X + 10.0f,
                        y: img_max_y.LocalPosition.Y + img_max_y.Size.Y + 10.0f
                    );

                    _panelTextures.Size = panelSize;
                }
            }

            UpdateDialogControls();
        }

        private void UpdateSubGroupButtonsBackgroundColor()
        {
            if (_subGroupSelected == null)
            {
                foreach (SfmlButton btn in _panelSubGroups.Childrens.Where(x => x is SfmlButton))
                {
                    btn.BackgroundColor = Config.DefaultColors.White;
                }
            }
            else
            {
                foreach (SfmlButton btn in _panelSubGroups.Childrens.Where(x => x is SfmlButton))
                {
                    if (btn.Name == _subGroupSelected.Name)
                    {
                        btn.BackgroundColor = Config.DefaultColors.LightGray;
                    }
                    else
                    {
                        btn.BackgroundColor = Config.DefaultColors.White;
                    }
                }
            }

        }

        private void ResizeWindowToContent()
        {
            SfmlControl ctrl_max_x = this.Controls.OrderBy(x => x.GlobalPosition.X + x.Size.X).Last();
            SfmlControl ctrl_max_y = this.Controls.OrderBy(x => x.GlobalPosition.Y + x.Size.Y).Last();

            float max_x = ctrl_max_x.GlobalPosition.X + ctrl_max_x.Size.X;
            float max_y = ctrl_max_y.GlobalPosition.Y + ctrl_max_y.Size.Y;

            Vector2f newSize = new Vector2f(max_x + 10.0f, max_y + 10.0f);

            this.RenderWindow.Size = (Vector2u)newSize;

            View view = new View(newSize / 2.0f, newSize);

            this.RenderWindow.SetView(view);
        }

        private void UpdateDialogControls()
        {
            if (_btnCancel == null || _btnOk == null)
                return;

            this.RemoveControl(_btnCancel);
            this.RemoveControl(_btnOk);

            ResizeWindowToContent();

            _btnCancel.GlobalPosition = new Vector2f(10.0f, RenderWindow.Size.Y);
            _btnCancel.Size = new Vector2f((RenderWindow.Size.X - 30.0f) / 2.0f, 20.0f);

            _btnOk.GlobalPosition = new Vector2f(_btnCancel.GlobalPosition.X + _btnCancel.Size.X + 10.0f, RenderWindow.Size.Y);
            _btnOk.Size = new Vector2f((RenderWindow.Size.X - 30.0f) / 2.0f, 20.0f);

            this.Controls.Add(_btnCancel);
            this.Controls.Add(_btnOk);

            ResizeWindowToContent();
        }
    }
}
