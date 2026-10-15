using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.IO;
using GameEngine.Core.Textures;
using GameEngine.Core.Saves;
using GameEngine.Core.World;
using SFML.Graphics;

namespace GameEngine
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"  ________                      ___________              .__               ");
            Console.WriteLine(@" /  _____/_____    _____   ____ \_   _____/ ____    ____ |__| ____   ____  ");
            Console.WriteLine(@"/   \  ___\__  \  /     \_/ __ \ |    __)_ /    \  / ___\|  |/    \_/ __ \ ");
            Console.WriteLine(@"\    \_\  \/ __ \|  Y Y  \  ___/ |        \   |  \/ /_/  >  |   |  \  ___/ ");
            Console.WriteLine(@" \______  (____  /__|_|  /\___  >_______  /___|  /\___  /|__|___|  /\___  >");
            Console.WriteLine(@"        \/     \/      \/     \/        \/     \//_____/         \/     \/ ");
            Console.WriteLine(@"___________________________________________________________________________");
            Console.WriteLine(@"");
            Console.WriteLine(@"");

            WorldSave? lastSave = SavesManager.LoadLastSaveFile();

            if (lastSave != null)
            {
                Console.WriteLine($"> Loading save file ...");
                World.LoadSave(lastSave);
                Console.WriteLine($"> Save file loaded.");
            }
            else
            {
                Console.WriteLine($"> No save file found, creating a new world");
            }

            GameEngine.GameEditor.GameEditor.Run();
        }
    }
}
