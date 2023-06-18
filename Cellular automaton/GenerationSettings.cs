using System.Collections.Generic;

namespace Digger.Cellular_automaton
{
    public class GenerationSettings
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int LiveChanse { get; set; }
        public int GenerationCount { get; set; }
        public Dictionary<int, int> LiveLimit { get; set; }
        public Dictionary<int, int> BornLimit { get; set; }

    }
}
