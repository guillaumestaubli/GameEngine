using GameEngine.Core.Saves;
using GameEngine.Core.Textures;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Entities
{
    public class NpcEntity : Entity
    {
        public NpcEntity(string name, Vector2f globalPosition, TextureSubGroup textureGroup, Vector2f size)
            : base(name, globalPosition, textureGroup, size)
        {

        }

        public NpcEntity(NpcEntitySave save)
            :base(
                 name: save.Name,
                 globalPosition: new Vector2f(save.GlobalPosition_X, save.GlobalPosition_Y),
                 textureGroup: TextureManager.LibraryEntities.GetTextureSubGroup(save.TextureSubGroup),
                 size: new Vector2f(save.Size_X, save.Size_Y)
                 )
        {
            this.ChangeDirection(save.CurrentDirection);
            this.ChangeAction(save.CurrentAction);
        }
    }
}
