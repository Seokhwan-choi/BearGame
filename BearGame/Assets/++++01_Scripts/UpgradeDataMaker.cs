using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Bear
{
    public class UpgradeData
    {
        public string id;
        public string name;
        public string desc;
        public int levelUpValue;
        public int requireGold;
    }

    public static class UpgradeDataMaker
    {
        public static void Make(string dataValue)
        {
            if (Bear.GameData.UpgradeData == null)
                Bear.GameData.UpgradeData = new Dictionary<string, UpgradeData>();

            string[] lines = dataValue.Split("\n"[0]);

            for (int i = 1; i < lines.Length; ++i)
            {
                string[] col = lines[i].Split(',');

                var upgradeData = new UpgradeData();
                if (col.Length > 0) upgradeData.id = col[0];
                if (col.Length > 1) upgradeData.name = col[1];
                if (col.Length > 2) upgradeData.desc = col[2];
                if (col.Length > 3) upgradeData.levelUpValue = Int32.Parse(col[3]);
                if (col.Length > 4) upgradeData.requireGold = Int32.Parse(col[4]);

                Bear.GameData.UpgradeData.Add(upgradeData.id, upgradeData);
            }
        }

        static UpgradeData AddUpgradeData(string id)
        {
            var upgradeData = new UpgradeData() { id = id };

            Bear.GameData.UpgradeData.Add(id, upgradeData);

            return upgradeData;
        }


        

    }
}


