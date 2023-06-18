using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger.Cellular_automaton
{
    internal class MapGenerator
    {
        private readonly GenerationSettings _generationSettings;
        private readonly SpawnRateSettings _spawnRateSettings;

        public MapGenerator(GenerationSettings generationSettings, SpawnRateSettings spawnRateSettings) 
        {
            _generationSettings = generationSettings;
            _spawnRateSettings = spawnRateSettings;
        }
        private int[,] InitializeRandom() 
        {
            Random rnd = new Random();
            int[,] generation = new int[_generationSettings.Rows,_generationSettings.Columns];
            for (int i = 0; i < _generationSettings.Rows; i++)
            {
                for (int j = 0; j < _generationSettings.Columns; j++)
                {
                    if (rnd.Next(0, 100) > _generationSettings.LiveChanse)
                        generation[i,j] = 1;
                    else
                        generation[i, j] = 0;
                }
            }
            return generation;
        }
        private int[,] ProcessLiveNeighbours(int[,] generation) 
        {
            for (int i = 1; i < generation.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < generation.GetLength(1) - 1; j++)
                {
                    var count = 0;
                    if (generation[i-1, j-1] == 1) count++;
                    if (generation[i - 1, j] == 1) count++;
                    if (generation[i - 1, j + 1] == 1) count++;
                    if (generation[i, j - 1] == 1) count++;
                    if (generation[i, j + 1] == 1) count++;
                    if (generation[i + 1, j - 1] == 1) count++;
                    if (generation[i + 1, j] == 1) count++;
                    if (generation[i + 1, j + 1] == 1) count++;

                    if (generation[i, j] == 1)
                    {
                        if(
                        count >= _generationSettings.LiveLimit.Keys.First() &&
                        count <= _generationSettings.LiveLimit.Values.First())
                            generation[i, j] = 1;
                        else generation[i, j] = 0;
                    }
                    else 
                    {
                        if(
                        count >= _generationSettings.BornLimit.Keys.First() &&
                        count <= _generationSettings.BornLimit.Values.First())
                            generation[i, j] = 1;
                        else generation[i, j] = 0;
                    }
                }
            }
            return generation;
        }
        private int[,] RepeatGeneration(int[,] generation) 
        {
            for (int i = 0; i < _generationSettings.GenerationCount; i++)
            {
                generation = ProcessLiveNeighbours(generation);
            }
            return generation;
        }
        private int[,] FillGeneration(int[,] generation) 
        {
            Random rnd = new Random();
            for (int i = 2; i < generation.GetLength(0) - 2; i++)
            {
                for (int j = 2; j < generation.GetLength(1) - 2; j++)
                {
                    if (generation[i, j] == 1) 
                    {
                        var count = 0;
                        if (generation[i - 1, j - 1] == 1) count++;
                        if (generation[i - 1, j] == 1) count++;
                        if (generation[i - 1, j + 1] == 1) count++;
                        if (generation[i, j - 1] == 1) count++;
                        if (generation[i, j + 1] == 1) count++;
                        if (generation[i + 1, j - 1] == 1) count++;
                        if (generation[i + 1, j] == 1) count++;
                        if (generation[i + 1, j + 1] == 1) count++;

                        if (generation[i - 2, j - 2] == 1) count++;
                        if (generation[i - 2, j - 1] == 1) count++;
                        if (generation[i - 2, j] == 1) count++;
                        if (generation[i - 2, j + 1] == 1) count++;
                        if (generation[i - 2, j + 2] == 1) count++;

                        if (generation[i - 1, j - 2] == 1) count++;
                        if (generation[i, j - 2] == 1) count++;
                        if (generation[i + 1, j - 2] == 1) count++;

                        if (generation[i - 1, j + 2] == 1) count++;
                        if (generation[i, j + 2] == 1) count++;
                        if (generation[i + 1, j + 2] == 1) count++;

                        if (generation[i + 2, j - 2] == 1) count++;
                        if (generation[i + 2, j - 1] == 1) count++;
                        if (generation[i + 2, j] == 1) count++;
                        if (generation[i + 2, j + 1] == 1) count++;
                        if (generation[i + 2, j + 2] == 1) count++;

                        if (count == 24) 
                        {
                            if (rnd.Next(0, 100) > _spawnRateSettings.GoldChance) 
                            {
                                generation[i, j] = 2;
                                if (rnd.Next(0, 100) > _spawnRateSettings.SackChance) generation[i - 1, j] = 4;
                            }
                            else 
                            {
                                if (rnd.Next(0, 100) > _spawnRateSettings.MonsterChance) generation[i, j] = 3;
                            }
                        }
                    }
                }
            }
            return generation;
        }
        private string ConvertToMap(int[,] generation) 
        {
            StringBuilder map = new StringBuilder();
            for (int i = 0; i < generation.GetLength(0); i++)
            {
                for (int j = 0; j < generation.GetLength(1); j++)
                {
                    switch (generation[i, j])
                    {
                        case 0:
                            map.Append(' ');
                            break;
                        case 1:
                            map.Append('T');
                            break;
                        case 2:
                            map.Append('G');
                            break;
                        case 3:
                            map.Append('M');
                            break;
                        case 4:
                            map.Append('S');
                            break;
                        default:
                            throw new Exception($"wrong code for map: {generation[i, j]}");
                    }
                }
                map.Append('\n');
            }
            map[0] = 'P';
            return map.ToString();
        }
        private string Generate() 
        {
            var generation = InitializeRandom();
            generation = RepeatGeneration(generation);
            generation = FillGeneration(generation);
            return ConvertToMap(generation);
        }
        public string GetMap() 
        {
            return Generate();
        }
    }
}
