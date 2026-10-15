using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.System;

using GameEngine.Core.Textures;
using GameEngine.Core.World;
using GameEngine.Core.Props;
using GameEngine.Core.Entities;
using GameEngine.GameEditor.Windows.WorldEditor;
using GameEngine.Utils;

namespace GameEngine.GameEditor
{
    public static class GameEditor
    {
        private static WorldEditor _worldEditorWindow;

        private static WorldTile? _currentTileForEdit = null;
        private static VertexArray? _currentTileForEdit_Outline = null;

        private static Prop? _currentPropForEdit = null;

        private static TextureDetails? _currentTextureToDrawOnTile = null;
        private static TextureDetails? _currentPropTextureToAddOnWorld = null;
        private static TextureSubGroup? _currentEntityTexturesToAddOnWorld = null;

        #region GET_SET
        public static WorldTile? CurrentTileForEdit
        {
            get => _currentTileForEdit;
        }

        public static VertexArray? CurrentTileForEdit_Outline
        {
            get => _currentTileForEdit_Outline;
        }

        public static Prop? CurrentPropForEdit { get => _currentPropForEdit; set => _currentPropForEdit = value; }

        #endregion

        static GameEditor()
        {
            _worldEditorWindow = new WorldEditor();
        }

        public static void Run()
        {
            _worldEditorWindow.Run();
        }

        public static void CurrentTileForEdit_Set(WorldTile tile)
        {
            _currentTileForEdit = tile;
            _currentTileForEdit_Outline = SfmlUtils.GetLineStripRectangle(tile.GlobalPosition, tile.Size, Color.Red);
        }

        public static void CurrentTileForEdit_Reset()
        {
            _currentTileForEdit = null;
            _currentTileForEdit_Outline = null;
        }

        public static void CurrentTileForEdit_EditTexture(TextureDetails textureDetails)
        {
            if (_currentTileForEdit == null)
                return;

            World.Tiles[_currentTileForEdit.WorldIndex.X, _currentTileForEdit.WorldIndex.Y].Texture = textureDetails;
        }

        public static void CurrentTextureToDrawOnTile_Set(TextureDetails texture)
        {
            _currentTextureToDrawOnTile = texture;
        }

        public static void CurrentTextureToDrawOnTile_Reset()
        {
            _currentTextureToDrawOnTile = null;
        }

        public static void CurrentTextureToDrawOnTile_ApplyToTile(WorldTile tile)
        {
            if (_currentTextureToDrawOnTile == null)
                return;

            World.Tiles[tile.WorldIndex.X, tile.WorldIndex.Y].Texture = _currentTextureToDrawOnTile;
        }

        public static void CurrentPropToAddOnWorld_Set(TextureDetails texture)
        {
            _currentPropTextureToAddOnWorld = texture;
        }

        public static void CurrentPropToAddOnWorld_Reset()
        {
            _currentPropTextureToAddOnWorld = null;
        }

        public static void CurrentPropToAddOnWorld_ApplyToWorld(Vector2f worldPosition)
        {
            if (_currentPropTextureToAddOnWorld == null)
                return;

            Prop prop = new Prop(
                name: "",
                globalPosition: worldPosition,
                texture: _currentPropTextureToAddOnWorld
                );

            World.AddProp(prop);
        }

        public static void CurrentEntityToAddOnWorld_Set(TextureSubGroup subGroup)
        {
            _currentEntityTexturesToAddOnWorld = subGroup;
        }

        public static void CurrentEntityToAddOnWorld_Reset()
        {
            _currentEntityTexturesToAddOnWorld = null;
        }

        public static void CurrentEntityToAddOnWorld_ApplyToWorld(Vector2f worldPosition)
        {
            if (_currentEntityTexturesToAddOnWorld == null)
                return;
        }
    }
}
