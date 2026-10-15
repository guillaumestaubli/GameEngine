using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SfmlControls.Controls;

using GameEngine.Core.Textures;
using GameEngine.Core.Saves;
using GameEngine.Utils;

using SFML.Graphics;
using SFML.System;

namespace GameEngine.Core.World
{
    public class WorldTile
    {
        protected Vector2i _worldIndex;
        protected Vector2f _globalPosition;
        protected Vector2f _size;
        protected TextureDetails _texture;
        protected RenderStates _renderStates;
        protected WorldTileDrawable _drawable;

        #region GET_SET

        public Vector2i WorldIndex
        {
            get { return _worldIndex; }
        }

        public Vector2f GlobalPosition
        {
            get { return _globalPosition; }
        }

        public Vector2f Size
        {
            get { return _size; }
        }

        public TextureDetails Texture
        {
            get => _texture;
            set
            {
                _texture = value;
                _renderStates = new RenderStates(_texture.Texture);
                _renderStates.BlendMode = BlendMode.None;

                GenerateRender();
            }
        }

        public RenderStates RenderStates
        {
            get { return _renderStates; }
        }

        public FloatRect GlobalBounds
        {
            get
            {
                return new FloatRect(_globalPosition, _size);
            }
        }

        public WorldTileDrawable Drawable
        {
            get => _drawable;
        }

        #endregion

        public WorldTile(Vector2i worldIndex, Vector2f size)
        {
            _worldIndex = worldIndex;
            _globalPosition = new Vector2f(worldIndex.X * size.X, worldIndex.Y * size.Y);
            _size = size;
            _texture = TextureManager.Texture_DefaultGround;
            _renderStates = new RenderStates(_texture.Texture);
            _renderStates.BlendMode = BlendMode.None;

            _drawable = new WorldTileDrawable();

            GenerateRender();
        }

        public WorldTile(TileSave tile)
        {
            _worldIndex = new Vector2i(tile.WorldIndex_X, tile.WorldIndex_Y);
            _globalPosition = new Vector2f(tile.GlobalPosition_X, tile.GlobalPosition_Y);
            _size = new Vector2f(tile.Size_X, tile.Size_Y);

            TextureDetails? texture = TextureManager.LibraryGround.GetTexture(tile.GroundTextureName);

            _texture = texture == null ? TextureManager.Texture_NotFound : texture;
            _renderStates = new RenderStates(_texture.Texture);
            _renderStates.BlendMode = BlendMode.None;

            _drawable = new WorldTileDrawable();

            GenerateRender();
        }

        private void GenerateRender()
        {
            VertexArray background = SfmlUtils.GetTriangleStripRectangle(_globalPosition, _size, (Vector2f)_texture.Texture.Size, Color.White);
            VertexArray borders = SfmlUtils.GetLineStripRectangle(_globalPosition, _size, Color.Black);

            _drawable = new WorldTileDrawable(background, _renderStates, borders);

        }
    }

    public struct WorldTileDrawable
    {
        public readonly VertexArray Background;
        public readonly RenderStates BackgroundRenderStates;
        public readonly VertexArray Borders;

        public WorldTileDrawable(VertexArray background, RenderStates backgroundRenderStates, VertexArray borders)
        {
            Background = background;
            BackgroundRenderStates = backgroundRenderStates;
            Borders = borders;
        }
    }


}
