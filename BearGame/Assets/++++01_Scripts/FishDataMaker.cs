using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;

namespace Bear
{
    public class FishData
    {
        public int id;
        public int areaNumber;
        public string name;
        public int level;
        public string desc;
        public int gold;
        public FishType fishType;
    }

    public enum FishType
    {
        small,
        middle,
        large
    }

    static public class FishDataMaker
    {        
        public static void Make(string dataValue)
        {
            if (Bear.GameData.FishData == null)
                Bear.GameData.FishData = new Dictionary<int, FishData>();

            string[] lines = dataValue.Split("\n"[0]);

            for (int i = 1; i < lines.Length; ++i)      
            {
                string[] col = lines[i].Split(',');

                var fishData = new FishData();

                if (col.Length > 0) fishData.id = Int32.Parse(col[0]);
                if (col.Length > 1) fishData.areaNumber = Int32.Parse(col[1]);
                if (col.Length > 2) fishData.name = col[2];
                if (col.Length > 3) fishData.level = Int32.Parse(col[3]);
                if (col.Length > 4) fishData.gold = Int32.Parse(col[4]);
                if (col.Length > 5) fishData.desc = col[5];
                if (col.Length > 6) fishData.fishType = (FishType)Enum.Parse(typeof(FishType), col[6]);

                Bear.GameData.FishData.Add(fishData.id, fishData);
            }
        }
    }
}

