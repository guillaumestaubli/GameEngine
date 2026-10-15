using GameEngine.Core.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace GameEngine.Core.Saves
{
    public static class SavesManager
    {
        public static void SaveCurrentState()
        {
            WorldSave worldSave = new WorldSave(
                worldSize: World.World.Size,
                tiles: World.World.Tiles,
                player: World.World.PlayerEntity,
                npcs: World.World.NpcEntities,
                props: World.World.Props
                );

            string json = JsonConvert.SerializeObject(worldSave, Formatting.Indented);

            DateTime now = DateTime.Now;

            string path = $"{Config.SAVE_PATH}\\World_Save_{now.Year}-{now.Month}-{now.Day}_{now.Hour}_{now.Minute}_{now.Second}.json";

            StreamWriter stream = new StreamWriter(path);

            stream.Write(json);

            stream.Flush();
            stream.Close();

            Console.WriteLine($"> SavesManager - SaveCurrentState - Saved successfully at :");
            Console.WriteLine($"    > {path}");
        }

        public static WorldSave? LoadLastSaveFile()
        {
            List<string> saveFiles = Directory.GetFiles(Config.SAVE_PATH).ToList();

            if (saveFiles.Count == 0)
                return null;

            string latestFile = string.Empty;
            DateTime latestTime = DateTime.MinValue;

            foreach (string file in saveFiles)
            {
                DateTime fileCreation = File.GetCreationTime(file);

                if (fileCreation > latestTime)
                {
                    latestFile = file;
                    latestTime = fileCreation;
                }
            }

            string json = string.Empty;

            StreamReader stream = new StreamReader(latestFile);

            json = stream.ReadToEnd();

            stream.Close();

            WorldSave? save = JsonConvert.DeserializeObject<WorldSave>(json);

            if (save == null)
                throw new Exception($"Save file {latestFile} can't be deserialized to WorldSave");
            else
                Console.WriteLine($"> Latest save file found : {latestFile}");

            return save;
        }
    }
}
