using GameEngine.Core.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Textures
{
    public static class EntityTextureConfig
    {
        public static readonly List<EntityTextureInfos> EntityTextureInfos = new List<EntityTextureInfos>();

        static EntityTextureConfig()
        {
            StreamReader streamReader = new StreamReader(@"EntityTexturesConfig.json");

            string jsonContent = streamReader.ReadToEnd();

            streamReader.Close();

            List<EntityTextureInfos>? deserializedConfig = JsonConvert.DeserializeObject<List<EntityTextureInfos>>(jsonContent);

            if (deserializedConfig != null)
            {
                EntityTextureInfos = deserializedConfig;
                Console.WriteLine($"> EntityTextureConfig - {EntityTextureInfos.Count} EntityTextureInfos loaded.");
            }
            else
            {
                Console.WriteLine($"> ERROR - EntityTextureConfig : No EntityTextureInfos loaded.");
            }
        }
    }

    public class EntityTextureInfos
    {
        public string MainGroup { get; set; } = string.Empty;
        public string SubGroup {  get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int NbColumns { get; set; } = 0;
        public int NbLines { get; set; } = 0;
        public Actions Action { get; set; } = 0;

        public List<EntityTextureLineDescription> TextureLinesDescription { get; set; } = new List<EntityTextureLineDescription>();

    }

    public class EntityTextureLineDescription
    {
        public Directions Direction { get; set; } = 0;
        public int LineIndex { get; set; } = 0;
        public int NbSprites { get; set; } = 0;
        public long AnimationInterval { get; set; } = 170;
    }
}
