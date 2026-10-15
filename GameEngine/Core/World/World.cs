using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.System;

using GameEngine.Core.Entities;
using GameEngine.Core.Props;
using GameEngine.Core.Textures;
using GameEngine.Utils;
using GameEngine.Core.Saves;
using System.Diagnostics;

namespace GameEngine.Core.World
{
    public static class World
    {
        private static Vector2i _size;

        private static WorldTile[,] _tiles;

        private static PlayerEntity _playerEntity;
        private static List<NpcEntity> _npcEntites = new List<NpcEntity>();
        private static List<Prop> _props = new List<Prop>();

        private static int _npcEntityIdCounter = 0;
        private static int _propIdCounter = 0;

        #region GET_SET

        public static Vector2i Size { get => _size; }

        public static WorldTile[,] Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }

        public static Vector2f GlobalSize
        {
            get
            {
                return new Vector2f(Config.WORLD_SIZE_X * Config.WORLD_TILE_SIZE, Config.WORLD_SIZE_Y * Config.WORLD_TILE_SIZE);
            }
        }

        public static PlayerEntity PlayerEntity { get => _playerEntity; }

        public static List<NpcEntity> NpcEntities { get => _npcEntites; }
        public static List<Prop> Props { get => _props; }

        #endregion

        static World()
        {
            _size = new Vector2i(Config.WORLD_SIZE_X, Config.WORLD_SIZE_Y);

            _tiles = new WorldTile[Config.WORLD_SIZE_X, Config.WORLD_SIZE_Y];

            for (int x = 0; x < Config.WORLD_SIZE_X; ++x)
            {
                for (int y = 0; y < Config.WORLD_SIZE_Y; ++y)
                {
                    _tiles[x, y] = new WorldTile(new Vector2i(x, y), new Vector2f(Config.WORLD_TILE_SIZE, Config.WORLD_TILE_SIZE));
                }
            }

            _playerEntity = new PlayerEntity(
                name: "player_01",
                globalPosition: new Vector2f(250.0f, 250.0f),
                textureGroup: TextureManager.LibraryEntities.MainGroups.First(x => x.Name == "Male_Character").SubGroups.First(x => x.Name == "Sword"),
                size: new Vector2f(75.0f, 75.0f)
                );
        }

        public static void LoadSave(WorldSave save)
        {
            _size = new Vector2i(save.Size_X, save.Size_Y);
            _tiles = new WorldTile[_size.X, _size.Y];

            for (int x = 0; x < _size.X; ++x)
            {
                for (int y = 0; y < _size.Y; ++y)
                {
                    _tiles[x, y] = new WorldTile(save.Tiles[x, y]);
                }
            }

            if (save.PlayerEntity == null)
                throw new Exception("PlayerEntity does not exists in save file");

            _playerEntity = new PlayerEntity(save.PlayerEntity);

            foreach (NpcEntitySave npc in save.NpcEntities)
            {
                _npcEntites.Add(new NpcEntity(npc));
            }

            foreach (PropSave prop in save.Props)
            {
                _props.Add(new Prop(prop));
            }
        }

        public static WorldTile? GetTileClicked(Vector2f click)
        {
            for (int x = 0; x < Config.WORLD_SIZE_X; ++x)
            {
                for (int y = 0; y < Config.WORLD_SIZE_Y; ++y)
                {
                    if (_tiles[x, y].GlobalBounds.Contains(click))
                        return _tiles[x, y];
                }
            }

            return null;
        }

        public static Prop? GetPropClicked(Vector2f f_click)
        {
            Vector2i click = (Vector2i)f_click;

            List<Prop> propsGlobal = _props.Where(x => x.GlobalBounds.Contains(click)).ToList();

            List<Prop> props = new List<Prop>();

            Vector2i nullVector = new Vector2i(0, 0);

            foreach(Prop prop in propsGlobal)
            {
                Vector2i p_left = prop.SpriteBoundPoints.FirstOrDefault(p => p.X < click.X && p.Y == click.Y);
                Vector2i p_right = prop.SpriteBoundPoints.FirstOrDefault(p => p.X > click.X && p.Y == click.Y);
                Vector2i p_top = prop.SpriteBoundPoints.FirstOrDefault(p => p.X == click.X && p.Y < click.Y);
                Vector2i p_bottom = prop.SpriteBoundPoints.FirstOrDefault(p => p.X == click.X && p.Y > click.Y);

                if(p_left != nullVector && p_right != nullVector && p_top != nullVector && p_bottom != nullVector)
                {
                    props.Add(prop);
                }
            }

            return props.FirstOrDefault();
        }

        public static void AddNpcEntity(NpcEntity npc)
        {
            ++_npcEntityIdCounter;

            if (npc.Name.Length == 0)
                npc.Name = $"entity_{_npcEntityIdCounter}";

            _npcEntites.Add(npc);
        }

        public static void AddProp(Prop prop)
        {
            ++_propIdCounter;

            if (prop.Name.Length == 0)
                prop.Name = $"prop_{_propIdCounter}";

            _props.Add(prop);
        }

        public static void MovePlayer(Vector2f move)
        {
            // move the player to check if intersections with props
            PlayerEntity.Move(move);

            List<Prop> propsIntersected = new List<Prop>();

            // check if prop sprite bounds contains a point of player sprite bounds
            foreach (Prop prop in _props)
            {
                foreach (Vector2i point in PlayerEntity.CurrentSpriteGlobalBoundsPoints)
                {
                    if (prop.SpriteBoundPoints.Contains(point))
                    {
                        propsIntersected.Add(prop);
                        break;
                    }
                }
            }

            if(propsIntersected.Count > 0)
            {
                // cancel the movement to avoid intersection with props
                PlayerEntity.Move(move * -1.0f);
            }
        }

        public static bool RenameProp(Prop target, string newName)
        {
            int target_index = _props.IndexOf(target);

            if (target_index == -1)
                return false;

            bool alreadyExists = _props.FirstOrDefault(x => x.Name == newName) != null;

            if (alreadyExists)
                return false;

            _props[target_index].Name = newName;

            return true;
        }

        public static bool MoveProp(Prop target, Vector2f newGlobalPosition)
        {
            int target_index = _props.IndexOf(target);

            if (target_index == -1)
                return false;

            _props[target_index].SetGlobalPosition(newGlobalPosition);

            return true;
        }

        public static bool ResizeProp(Prop target, Vector2f newSize)
        {
            int target_index = _props.IndexOf(target);

            if (target_index == -1)
                return false;

            _props[target_index].SetSize(newSize);

            return true;
        }

        public static bool DeleteProp(Prop target)
        {
            _props = _props.Where(x => x.Name != target.Name).ToList();

            return true;
        }
    }
}
