using GameEngine.Core.Entities;
using GameEngine.Core.Props;
using GameEngine.Core.World;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Saves
{
    public class WorldSave
    {
        public int Size_X { get; set; } = -1;
        public int Size_Y { get; set; } = -1;

        public TileSave[,] Tiles { get; set; } = new TileSave[0, 0];

        public PlayerEntitySave? PlayerEntity { get; set; } = null;

        public List<NpcEntitySave> NpcEntities { get; set; } = new List<NpcEntitySave>();

        public List<PropSave> Props { get; set; } = new List<PropSave>();

        public WorldSave()
        {

        }

        public WorldSave(Vector2i worldSize, WorldTile[,] tiles, PlayerEntity player, List<NpcEntity> npcs, List<Prop> props)
        {
            Size_X = worldSize.X;
            Size_Y = worldSize.Y;

            Tiles = new TileSave[Size_X, Size_Y];

            for (int x = 0; x < Size_X; ++x)
            {
                for (int y = 0; y < Size_Y; ++y)
                {
                    Tiles[x, y] = new TileSave(tiles[x, y]);
                }
            }

            PlayerEntity = new PlayerEntitySave(player);

            NpcEntities = new List<NpcEntitySave>();

            for (int i = 0; i < npcs.Count; ++i)
            {
                NpcEntities.Add(new NpcEntitySave(npcs[i]));
            }

            Props = new List<PropSave>();

            for (int i = 0; i < props.Count; ++i)
            {
                Props.Add(new PropSave(props[i]));
            }
        }
    }

    public class TileSave
    {
        public int WorldIndex_X { get; set; } = -1;
        public int WorldIndex_Y { get; set; } = -1;

        public float GlobalPosition_X { get; set; } = -1.0f;
        public float GlobalPosition_Y { get; set; } = -1.0f;

        public float Size_X { get; set; } = -1.0f;
        public float Size_Y { get; set; } = -1.0f;

        public string GroundTextureName { get; set; } = string.Empty;

        public TileSave()
        {

        }

        public TileSave(WorldTile worldTile)
        {
            WorldIndex_X = worldTile.WorldIndex.X;
            WorldIndex_Y = worldTile.WorldIndex.Y;

            GlobalPosition_X = worldTile.GlobalPosition.X;
            GlobalPosition_Y = worldTile.GlobalPosition.Y;

            Size_X = worldTile.Size.X;
            Size_Y = worldTile.Size.Y;

            GroundTextureName = worldTile.Texture.Name;
        }
    }

    public class PlayerEntitySave
    {
        public string Name { get; set; } = string.Empty;

        public float Size_X { get; set; } = -1.0f;
        public float Size_Y { get; set; } = -1.0f;

        public float Origin_X { get; set; } = -1.0f;
        public float Origin_Y { get; set; } = -1.0f;

        public float GlobalPosition_X { get; set; } = -1.0f;
        public float GlobalPosition_Y { get; set; } = -1.0f;

        public Directions CurrentDirection { get; set; } = Directions.South;
        public Actions CurrentAction { get; set; } = Actions.Idle;

        public string TextureSubGroup { get; set; } = string.Empty;

        public PlayerEntitySave()
        {

        }

        public PlayerEntitySave(PlayerEntity player)
        {
            Name = player.Name;

            Size_X = player.Size.X;
            Size_Y = player.Size.Y;

            Origin_X = player.Origin.X;
            Origin_Y = player.Origin.Y;

            GlobalPosition_X = player.GlobalPosition.X;
            GlobalPosition_Y = player.GlobalPosition.Y;

            CurrentDirection = player.CurrentDirection;
            CurrentAction = player.CurrentAction;

            TextureSubGroup = player.TextureSubGroup.Name;
        }
    }

    public class NpcEntitySave
    {
        public string Name { get; set; } = string.Empty;

        public float Size_X { get; set; } = -1.0f;
        public float Size_Y { get; set; } = -1.0f;

        public float Origin_X { get; set; } = -1.0f;
        public float Origin_Y { get; set; } = -1.0f;

        public float GlobalPosition_X { get; set; } = -1.0f;
        public float GlobalPosition_Y { get; set; } = -1.0f;

        public Directions CurrentDirection { get; set; } = Directions.South;
        public Actions CurrentAction { get; set; } = Actions.Idle;

        public string TextureSubGroup { get; set; } = string.Empty;

        public NpcEntitySave()
        {

        }

        public NpcEntitySave(NpcEntity npcEntity)
        {
            Name = npcEntity.Name;

            Size_X = npcEntity.Size.X;
            Size_Y = npcEntity.Size.Y;

            Origin_X = npcEntity.Origin.X; 
            Origin_Y = npcEntity.Origin.Y;

            GlobalPosition_X = npcEntity.GlobalPosition.X;
            GlobalPosition_Y = npcEntity.GlobalPosition.Y;

            CurrentDirection = npcEntity.CurrentDirection;
            CurrentAction = npcEntity.CurrentAction;

            TextureSubGroup = npcEntity.TextureSubGroup.Name;
        }
    }

    public class PropSave
    {
        public string Name { get; set; } = string.Empty;

        public float Size_X { get; set; } = -1.0f;
        public float Size_Y { get; set; } = -1.0f;

        public float Origin_X { get; set; } = -1.0f;
        public float Origin_Y { get; set; } = -1.0f;

        public float GlobalPosition_X { get; set; } = -1.0f;
        public float GlobalPosition_Y { get; set; } = -1.0f;

        public string TextureName { get; set; } = string.Empty;

        public PropSave()
        {

        }

        public PropSave(Prop prop)
        {
            Name = prop.Name;

            Size_X = prop.Size.X;
            Size_Y = prop.Size.Y;

            Origin_X = prop.Origin.X;
            Origin_Y = prop.Origin.Y;

            GlobalPosition_X = prop.GlobalPosition.X;
            GlobalPosition_Y = prop.GlobalPosition.Y;

            TextureName = prop.Texture.Name;
        }
    }
}
