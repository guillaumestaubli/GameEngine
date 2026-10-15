using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

using SfmlControls;
using SfmlControls.Controls;
using SfmlControls.Window;
using SfmlControls.Window.PartialWindow;
using GameEngine.GameEditor.Windows.WorldEditor.PartialWindows;
using GameEngine.Core.Entities;
using GameEngine.Core.Textures;
using GameEngine.Core.Saves;
using GameEngine.Core.World;

namespace GameEngine.GameEditor.Windows.WorldEditor
{
    public class WorldEditor : SfmlWindow
    {
        private bool _controlPlayer = false;

        private bool _showTiles = true;
        private bool _showTilesBorders = true;

        private bool _showProps = true;
        private bool _showPropsOrigin = false;
        private bool _showPropsGlobalBounds = false;
        private bool _showPropsSpriteBounds = false;

        private bool _showPlayer = true;
        private bool _showPlayerOrigin = false;
        private bool _showPlayerGlobalBounds = false;
        private bool _showPlayerSpriteBounds = false;

        private bool _showEntities = true;
        private bool _showEntitiesOrigin = false;
        private bool _showEntitiesGlobalBounds = false;
        private bool _showEntitiesSpriteBounds = false;

        private bool _showSettingsToolbar = false;
        private bool _showInspectTileToolbar = true;
        private bool _showDrawGroundToolbar = false;
        private bool _showAddPropToolbar = false;
        private bool _showAddEntityToolbar = false;
        private bool _showInspectPropToolbar = false;

        #region GET_SET

        public bool ShowTiles { get => _showTiles; set => _showTiles = value; }
        public bool ShowTilesBorders { get => _showTilesBorders; set => _showTilesBorders = value; }

        public bool ShowProps { get => _showProps; set => _showProps = value; }
        public bool ShowPropsOrigin { get => _showPropsOrigin; set => _showPropsOrigin = value; }
        public bool ShowPropsGlobalBounds { get => _showPropsGlobalBounds; set => _showPropsGlobalBounds = value; }
        public bool ShowPropsSpriteBounds { get => _showPropsSpriteBounds; set => _showPropsSpriteBounds = value; }

        public bool ShowPlayer { get => _showPlayer; set => _showPlayer = value; }
        public bool ShowPlayerOrigin { get => _showPlayerOrigin; set => _showPlayerOrigin = value; }
        public bool ShowPlayerGlobalBounds { get => _showPlayerGlobalBounds; set => _showPlayerGlobalBounds = value; }
        public bool ShowPlayerSpriteBounds { get => _showPlayerSpriteBounds; set => _showPlayerSpriteBounds = value; }

        public bool ShowEntities { get => _showEntities; set => _showEntities = value; }
        public bool ShowEntitiesOrigin { get => _showEntitiesOrigin; set => _showEntitiesOrigin = value; }
        public bool ShowEntitiesGlobalBounds { get => _showEntitiesGlobalBounds; set => _showEntitiesGlobalBounds = value; }
        public bool ShowEntitiesSpriteBounds { get => _showEntitiesSpriteBounds; set => _showEntitiesSpriteBounds = value; }

        public bool ShowInspectTileToolbar { get => _showInspectTileToolbar; }
        public bool ShowDrawGroundToolbar { get => _showDrawGroundToolbar; }
        public bool ShowAddPropToolbar { get => _showAddPropToolbar; }
        public bool ShowAddEntityToolbar { get => _showAddEntityToolbar; }
        public bool ShowInspectPropToolbar { get => _showInspectPropToolbar; }

        #endregion

        #region CONTROLS_DECLARATION

        private SfmlPanel _panelToolbarTop;
        private SfmlButton _btnSettings;
        private SfmlButton _btnInspectTileMode;
        private SfmlButton _btnDrawGroundMode;
        private SfmlButton _btnAddPropMode;
        private SfmlButton _btnAddEntityMode;
        private SfmlButton _btnControlPlayer;
        private SfmlButton _btnInspectPropMode;
        private SfmlButton _btnSave;

        private SfmlPartialWindowContainer _containerRenderArea;
        private SfmlPartialWindowContainer _containerToolbarRight;

        private RenderArea _renderArea;
        private ToolbarRight_Settings _toolbarRight_Settings;
        private ToolbarRight_EditTile _toolbarRight_EditTile;
        private ToolbarRight_DrawWorldGround _toolbarRight_DrawWorldGround;
        private ToolbarRight_AddProp _toolbarRight_AddProp;
        private ToolbarRight_AddEntity _toolbarRight_AddEntity;
        private ToolbarRight_EditProp _toolbarRight_EditProp;

        #endregion

        public WorldEditor()
            : base(WorldEditorConfig._W_WIDTH, WorldEditorConfig._W_HEIGHT, WorldEditorConfig._W_TITLE, WorldEditorConfig._W_STYLES)
        {
            this.ShowFpsCounter = true;

            _renderArea = new RenderArea(this);
            _toolbarRight_Settings = new ToolbarRight_Settings(this);
            _toolbarRight_EditTile = new ToolbarRight_EditTile(RenderWindow);
            _toolbarRight_DrawWorldGround = new ToolbarRight_DrawWorldGround(RenderWindow);
            _toolbarRight_AddProp = new ToolbarRight_AddProp(RenderWindow);
            _toolbarRight_AddEntity = new ToolbarRight_AddEntity(RenderWindow);
            _toolbarRight_EditProp = new ToolbarRight_EditProp(RenderWindow);

            #region CONTROLS_TOOLBAR_TOP

            _panelToolbarTop = new SfmlPanel(
                name: "panelToolbarTop",
                position: new Vector2f(0.0f, 0.0f),
                size: new Vector2f(1200.0f, 50.0f)
                );

            this.Controls.Add(_panelToolbarTop);

            _btnSettings = new SfmlButton(
                name: "btnSettings",
                textStr: "Settings",
                position: new Vector2f(10.0f, 10.0f),
                size: new Vector2f(100.0f, 30.0f)
                );
            _btnSettings.BackgroundColor = _showSettingsToolbar ? Config.DefaultColors.LightGray : Config.DefaultColors.White;
            _btnSettings.MouseButtonReleased += BtnSettingsMode_MouseButtonReleased;

            this.Controls.Add(_btnSettings);

            _btnInspectTileMode = new SfmlButton(
                name: "btnInspectTileMode",
                textStr: "Inspect Tile",
                position: new Vector2f(120.0f, 10.0f),
                size: new Vector2f(100.0f, 30.0f)
                );
            _btnInspectTileMode.BackgroundColor = _showInspectTileToolbar ? Config.DefaultColors.LightGray : Config.DefaultColors.White;
            _btnInspectTileMode.MouseButtonReleased += BtnInspectTileMode_MouseButtonReleased;

            this.Controls.Add(_btnInspectTileMode);

            _btnDrawGroundMode = new SfmlButton(
                name: "btnDrawGroundMode",
                textStr: "Draw Ground",
                position: new Vector2f(230.0f, 10.0f),
                size: new Vector2f(100.0f, 30.0f)
                );
            _btnDrawGroundMode.BackgroundColor = _showDrawGroundToolbar ? Config.DefaultColors.LightGray : Config.DefaultColors.White;
            _btnDrawGroundMode.MouseButtonReleased += BtnDrawGroundMode_MouseButtonReleased;

            this.Controls.Add(_btnDrawGroundMode);

            _btnAddPropMode = new SfmlButton(
                name: "btnAddPropMode",
                textStr: "Add Prop",
                position: new Vector2f(340.0f, 10.0f),
                size: new Vector2f(100.0f, 30.0f)
                );
            _btnAddPropMode.BackgroundColor = _showAddPropToolbar ? Config.DefaultColors.LightGray : Config.DefaultColors.White;
            _btnAddPropMode.MouseButtonReleased += BtnAddPropMode_MouseButtonReleased;

            this.Controls.Add(_btnAddPropMode);

            _btnAddEntityMode = new SfmlButton(
                name: "btnAddEntityMode",
                textStr: "Add Entity",
                position: new Vector2f(450.0f, 10.0f),
                size: new Vector2f(100.0f, 30.0f)
                );
            _btnAddEntityMode.BackgroundColor = _showAddEntityToolbar ? Config.DefaultColors.LightGray : Config.DefaultColors.White;
            _btnAddEntityMode.MouseButtonReleased += BtnAddEntityMode_MouseButtonReleased;
            _btnAddEntityMode.Clickable = false;

            this.Controls.Add(_btnAddEntityMode);

            _btnControlPlayer = new SfmlButton(
                name: "btnControlPlayer",
                textStr: "Control Player",
                position: new Vector2f(560.0f, 10.0f),
                size: new Vector2f(100.0f, 30.0f)
                );
            _btnControlPlayer.BackgroundColor = Config.DefaultColors.White;
            _btnControlPlayer.MouseButtonReleased += (sender, e) =>
            {
                _controlPlayer = !_controlPlayer;
                _btnControlPlayer.BackgroundColor = _controlPlayer ? Config.DefaultColors.LightGray : Config.DefaultColors.White;
            };

            this.Controls.Add(_btnControlPlayer);

            _btnInspectPropMode = new SfmlButton(
                name: "btnInspectProp",
                textStr: "Inspect Prop",
                position: new Vector2f(670.0f, 10.0f),
                size: new Vector2f(100.0f, 30.0f)
                );
            _btnInspectPropMode.BackgroundColor = _showInspectPropToolbar ? Config.DefaultColors.LightGray : Config.DefaultColors.White;
            _btnInspectPropMode.MouseButtonReleased += BtnInspectPropMode_MouseButtonReleased;

            this.Controls.Add(_btnInspectPropMode);



            _btnSave = new SfmlButton(
                name: "btnSave",
                textStr: "Save",
                position: new Vector2f(WorldEditorConfig._W_WIDTH - 100.0f - 10.0f, 10.0f),
                size: new Vector2f(100.0f, 30.0f)
                );
            _btnSave.MouseButtonReleased += BtnSave_MouseButtonReleased;

            this.Controls.Add(_btnSave);

            #endregion

            #region CONTROLS_RENDER_AREA

            _containerRenderArea = new SfmlPartialWindowContainer(
                name: "containerRenderArea",
                position: new Vector2f(0.0f, WorldEditorConfig.TB_TOP_HEIGHT),
                size: new Vector2f(WorldEditorConfig.RA_WIDTH, WorldEditorConfig.RA_HEIGHT),
                partialWindow: _renderArea
                );

            this.Controls.Add(_containerRenderArea);

            #endregion

            #region CONTROLS_TOOLBAR_RIGHT

            _containerToolbarRight = new SfmlPartialWindowContainer(
                name: "containerToolbarRight",
                position: new Vector2f(WorldEditorConfig.RA_WIDTH, WorldEditorConfig.TB_TOP_HEIGHT),
                size: new Vector2f(WorldEditorConfig.TB_RIGHT_WIDTH, WorldEditorConfig.TB_RIGHT_HEIGHT),
                partialWindow: _toolbarRight_EditTile
                );

            this.Controls.Add(_containerToolbarRight);

            #endregion
        }

        #region OVERRIDE

        public override void DrawOnFrame(ref RenderWindow renderWindow)
        {

        }

        public override void ExecuteBeforeDraw(ref RenderWindow renderWindow, long elapsed_ms)
        {
            if (base.CurrentFocusControl == null)
                return;

            if (base.CurrentFocusControl.Name != _containerRenderArea.Name)
                return;

            ProcessKeyboardInputs_Player(elapsed_ms);
        }

        #endregion

        #region CONTROLS_EVENTS

        private void BtnSave_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            SavesManager.SaveCurrentState();
        }

        private void BtnSettingsMode_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (_showSettingsToolbar)
                return;

            _showSettingsToolbar = true;
            _showInspectTileToolbar = false;
            _showDrawGroundToolbar = false;
            _showAddPropToolbar = false;
            _showInspectPropToolbar = false;

            _btnSettings.BackgroundColor = Config.DefaultColors.LightGray;
            _btnInspectTileMode.BackgroundColor = Config.DefaultColors.White;
            _btnDrawGroundMode.BackgroundColor = Config.DefaultColors.White;
            _btnAddPropMode.BackgroundColor = Config.DefaultColors.White;
            _btnInspectPropMode.BackgroundColor = Config.DefaultColors.White;

            _toolbarRight_EditTile.Dispose();
            _toolbarRight_DrawWorldGround.Dispose();
            _toolbarRight_AddProp.Dispose();

            _containerToolbarRight.PartialWindow = _toolbarRight_Settings;

            GameEditor.CurrentTextureToDrawOnTile_Reset();
            GameEditor.CurrentTileForEdit_Reset();
            GameEditor.CurrentPropToAddOnWorld_Reset();
            GameEditor.CurrentPropForEdit = null;
        }

        private void BtnInspectTileMode_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (_showInspectTileToolbar)
                return;

            _showSettingsToolbar = false;
            _showInspectTileToolbar = true;
            _showDrawGroundToolbar = false;
            _showAddPropToolbar = false;
            _showInspectPropToolbar = false;

            _btnSettings.BackgroundColor = Config.DefaultColors.White;
            _btnInspectTileMode.BackgroundColor = Config.DefaultColors.LightGray;
            _btnDrawGroundMode.BackgroundColor = Config.DefaultColors.White;
            _btnAddPropMode.BackgroundColor = Config.DefaultColors.White;
            _btnInspectPropMode.BackgroundColor = Config.DefaultColors.White;

            _toolbarRight_EditTile.Dispose();
            _toolbarRight_DrawWorldGround.Dispose();
            _toolbarRight_AddProp.Dispose();

            _toolbarRight_EditTile = new ToolbarRight_EditTile(this.RenderWindow);
            _containerToolbarRight.PartialWindow = _toolbarRight_EditTile;

            GameEditor.CurrentTextureToDrawOnTile_Reset();
            GameEditor.CurrentTileForEdit_Reset();
            GameEditor.CurrentPropToAddOnWorld_Reset();
            GameEditor.CurrentPropForEdit = null;
        }

        private void BtnDrawGroundMode_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (_showDrawGroundToolbar)
                return;

            _showSettingsToolbar = false;
            _showInspectTileToolbar = false;
            _showDrawGroundToolbar = true;
            _showAddPropToolbar = false;
            _showInspectPropToolbar = false;

            _btnSettings.BackgroundColor = Config.DefaultColors.White;
            _btnInspectTileMode.BackgroundColor = Config.DefaultColors.White;
            _btnDrawGroundMode.BackgroundColor = Config.DefaultColors.LightGray;
            _btnAddPropMode.BackgroundColor = Config.DefaultColors.White;
            _btnInspectPropMode.BackgroundColor = Config.DefaultColors.White;

            _toolbarRight_EditTile.Dispose();
            _toolbarRight_DrawWorldGround.Dispose();
            _toolbarRight_AddProp.Dispose();

            _toolbarRight_DrawWorldGround = new ToolbarRight_DrawWorldGround(this.RenderWindow);
            _containerToolbarRight.PartialWindow = _toolbarRight_DrawWorldGround;

            GameEditor.CurrentTextureToDrawOnTile_Reset();
            GameEditor.CurrentTileForEdit_Reset();
            GameEditor.CurrentPropToAddOnWorld_Reset();
            GameEditor.CurrentPropForEdit = null;
        }

        private void BtnAddPropMode_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (_showAddPropToolbar)
                return;

            _showSettingsToolbar = false;
            _showInspectTileToolbar = false;
            _showDrawGroundToolbar = false;
            _showAddPropToolbar = true;
            _showInspectPropToolbar = false;

            _btnSettings.BackgroundColor = Config.DefaultColors.White;
            _btnInspectTileMode.BackgroundColor = Config.DefaultColors.White;
            _btnDrawGroundMode.BackgroundColor = Config.DefaultColors.White;
            _btnAddPropMode.BackgroundColor = Config.DefaultColors.LightGray;
            _btnInspectPropMode.BackgroundColor = Config.DefaultColors.White;

            _toolbarRight_EditTile.Dispose();
            _toolbarRight_DrawWorldGround.Dispose();
            _toolbarRight_AddProp.Dispose();

            _toolbarRight_AddProp = new ToolbarRight_AddProp(this.RenderWindow);
            _containerToolbarRight.PartialWindow = _toolbarRight_AddProp;

            GameEditor.CurrentTextureToDrawOnTile_Reset();
            GameEditor.CurrentTileForEdit_Reset();
            GameEditor.CurrentPropToAddOnWorld_Reset();
            GameEditor.CurrentPropForEdit = null;
        }

        private void BtnAddEntityMode_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {

        }

        private void BtnInspectPropMode_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (_showInspectPropToolbar)
                return;

            _showSettingsToolbar = false;
            _showInspectTileToolbar = false;
            _showDrawGroundToolbar = false;
            _showAddPropToolbar = false;
            _showInspectPropToolbar = true;

            _btnSettings.BackgroundColor = Config.DefaultColors.White;
            _btnInspectTileMode.BackgroundColor = Config.DefaultColors.White;
            _btnDrawGroundMode.BackgroundColor = Config.DefaultColors.White;
            _btnAddPropMode.BackgroundColor = Config.DefaultColors.White;
            _btnInspectPropMode.BackgroundColor = Config.DefaultColors.LightGray;

            _toolbarRight_EditTile.Dispose();
            _toolbarRight_DrawWorldGround.Dispose();
            _toolbarRight_AddProp.Dispose();

            _toolbarRight_EditProp = new ToolbarRight_EditProp(this.RenderWindow);
            _containerToolbarRight.PartialWindow = _toolbarRight_EditProp;

            GameEditor.CurrentTextureToDrawOnTile_Reset();
            GameEditor.CurrentTileForEdit_Reset();
            GameEditor.CurrentPropToAddOnWorld_Reset();
            GameEditor.CurrentPropForEdit = null;
        }

        #endregion

        

        public void Update_ToolbarRight_EditTile()
        {
            _toolbarRight_EditTile.UpdateControlsDisplayValues();
        }

        public void Update_ToolbarRight_EditProp()
        {
            _toolbarRight_EditProp.UpdateControlsDisplayValues();
        }

        private void ProcessKeyboardInputs_Player(long elapsed_ms)
        {
            if (_controlPlayer == false)
                return;

            float move_ratio = (float)elapsed_ms / 10.0f;

            bool player_moving = false;
            bool sprint_enabled = false;
            Vector2f player_move = new Vector2f(0.0f, 0.0f);
            Directions player_direction = Directions.South;
            Actions player_action = Actions.Idle;

            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                sprint_enabled = true;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                player_moving = true;
                player_move += new Vector2f(0.0f, -1.0f);
                player_direction = Directions.North;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                player_moving = true;
                player_move += new Vector2f(0.0f, 1.0f);
                player_direction = Directions.South;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                player_moving = true;
                player_move += new Vector2f(-1.0f, 0.0f);
                player_direction = Directions.West;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                player_moving = true;
                player_move += new Vector2f(1.0f, 0.0f);
                player_direction = Directions.East;
            }

            if (player_moving)
            {
                player_action = Actions.Walk;

                float xy_sum = Math.Abs(player_move.X) + Math.Abs(player_move.Y);

                if (xy_sum > 1.0f)
                {
                    float x = (float)Math.Sqrt(2.0) / 2.0f;
                    float y = (float)Math.Sqrt(2.0) / 2.0f;

                    if (player_move.X < 0.0f)
                        x *= -1.0f;

                    if (player_move.Y < 0.0f)
                        y *= -1.0f;

                    player_move = new Vector2f(x, y);
                }

                if (sprint_enabled)
                {
                    player_action = Actions.Run;
                    player_move *= 1.5f;
                }

                player_move *= move_ratio;

                World.PlayerEntity.ChangeDirection(player_direction);
                World.PlayerEntity.ChangeAction(player_action);

                World.MovePlayer(player_move);

                _renderArea.CenterViewOnPlayer(World.PlayerEntity);
            }
            else
            {
                World.PlayerEntity.ChangeAction(player_action);
            }
        }
    }

    public static class WorldEditorConfig
    {
        public const string _W_TITLE = "GameEngine - GameEditor - WorldEditor";
        public const uint _W_WIDTH = 1200;
        public const uint _W_HEIGHT = 800;
        public const Styles _W_STYLES = Styles.Close;

        public const float TB_TOP_WIDTH = _W_WIDTH;
        public const float TB_TOP_HEIGHT = 50.0f;
        public const float TB_RIGHT_WIDTH = 300.0f;
        public const float TB_RIGHT_HEIGHT = _W_HEIGHT - TB_TOP_HEIGHT;
        public const float RA_WIDTH = _W_WIDTH - TB_RIGHT_WIDTH;
        public const float RA_HEIGHT = _W_HEIGHT - TB_TOP_HEIGHT;

        public static readonly Color _BTN_CURSOR_MODE_ACTTIVE_COLOR = Config.DefaultColors.LightGray;
        public static readonly Color _BTN_CURSOR_MODE_INACTIVE_COLOR = Config.DefaultColors.White;
    }
}
