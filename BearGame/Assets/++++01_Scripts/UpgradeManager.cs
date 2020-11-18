using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Bear
{
    public class UpgradeManager
    {
        // 업그레이드를 해준다. BearTouch or TouchPower LevelUp 될 수 록 도감 Open
        public void Upgrade(string id)
        {
            if ( Bear.GameData.UpgradeData.TryGetValue(id, out UpgradeData data) )
            {
                // Id를 찾아서 lvUp 시켜준다.
                if (Bear.LocalData.Upgrades.ContainsKey(id))
                {
                    var level = Bear.LocalData.Upgrades[id];

                    if (Bear.LocalData.TotalGold < data.requireGold * level)
                        Debug.Log("골드가 부족합니다.");
                    else
                    {
                        // 골드 소모
                        Bear.LocalData.TotalGold -= data.requireGold * level;

                        // lv 상승
                        Bear.LocalData.Upgrades[id]++;
                    }
                }
                else
                {
                    if (Bear.LocalData.TotalGold < data.requireGold)
                        Debug.Log("골드가 부족합니다.");
                    else
                    {
                        // 골드 소모
                        Bear.LocalData.TotalGold -= data.requireGold;

                        // lv 상승
                        Bear.LocalData.Upgrades.Add(id, 1);
                    }
                }

                Bear.LocalData.Save();

                CheckFishIndex();
            }
        }

        void CheckFishIndex()
        {
            if ( Bear.LocalData.Upgrades.ContainsKey("BearTouch") )
            {
                var level = Bear.LocalData.Upgrades["BearTouch"];

                var fishIndex = Bear.LocalData.FishIndex;

                foreach(var fishData in Bear.GameData.FishData.Values)
                {
                    // 조건을 만족하고, 도감에 들어있지 않다면 도감에 등록
                    if ( fishData.level <= level &&
                         fishIndex.ContainsKey(fishData.id) == false)
                    {
                        fishIndex.Add(fishData.id, fishData);
                    }
                }
            }
        }
    }
}

