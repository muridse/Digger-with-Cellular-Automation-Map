using System.Collections.Generic;
using System.Windows.Forms;
using Digger.Cellular_automaton;

namespace Digger
{
    public static class Game
    {
        private const string mapWithPlayerTerrain = @"
TTT T
TTP T
T T T
TT TT";

        private const string mapWithPlayerTerrainSackGold = @"
PTTGTT TS
TST  TSTT
TTTTTTSTT
T TSTS TT
T TTTG ST
TSTSTT TT";

        private const string mapWithPlayerTerrainSackGoldMonster = @"
PTTGTT TST
TST  TSTTM
TTT TTSTTT
T TSTS TTT
T TTTGMSTS
T TMT M TS
TSTSTTMTTT
S TTST  TG
 TGST MTTT
 T  TMTTTT";

        public static ICreature[,] Map;
        public static int Scores;
        public static bool IsOver;

        public static Keys KeyPressed;
        public static int MapWidth => Map.GetLength(0);
        public static int MapHeight => Map.GetLength(1);

        public static GenerationSettings GenerationSettings = new GenerationSettings 
        { 
            Rows = 31,
            Columns = 60,
            LiveChanse = 49,
            GenerationCount = 30,
            LiveLimit = new Dictionary<int, int>() { { 4, 8 } },
            BornLimit = new Dictionary<int, int>() { { 5, 8 } }
        };
        public static SpawnRateSettings SpawnRateSettings = new SpawnRateSettings
        {
            GoldChance = 69,
            MonsterChance = 88
        };
        public static void CreateMap()
        {
            Map = CreatureMapCreator.CreateMap(new MapGenerator(GenerationSettings, SpawnRateSettings).GetMap());
        }
    }
}