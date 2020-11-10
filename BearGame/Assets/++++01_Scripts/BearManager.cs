using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Bear
{
    public class BearManager
    {
        Button[] mButtonList;

        GameObject mPlayer;
        public GameObject Player => mPlayer;
        public BearManager()
        {
            mPlayer = Util.Instantiate("Player_Bear");
        }

        public void Init()
        {
            InitButton();
        }

        
        void InitButton()
        {
            GameObject content = GameObject.Find("Upgrade");

            //mButtonList = content.GetComponentsInChildren<Button>();
            

            //if (mButtonList.Length != Bear.GameData.UpgradeData.Count)
            //    return;

            //// 버튼에 업그레이드 등록
            //for (int i = 0; i < Bear.GameData.UpgradeData.Values.Count; ++i)
            //{
            //    var upgradeData = Bear.GameData.UpgradeData.Values.ToArray()[i];

            //    mButtonList[i].onClick.AddListener(() => Upgrade(upgradeData.id));
            //}
        }


        // 업그레이드를 해준다. BearTouch or TouchPower LevelUp 될 수 록 도감 Open
        const int UpgradeRequireGold = 100;
        public void Upgrade(string id)
        {
            if (Bear.LocalData.Upgrades == null)
                Bear.LocalData.Upgrades = new Dictionary<string, int>();

            // Id를 찾아서 lvUp 시켜준다.
            if ( Bear.LocalData.Upgrades.ContainsKey(id) )
            {
                var level = Bear.LocalData.Upgrades[id];

                if (Bear.LocalData.TotalGold < UpgradeRequireGold * level)
                    Debug.Log("골드가 부족합니다.");
                else
                {
                    // 골드 소모
                    Bear.LocalData.TotalGold -= UpgradeRequireGold * level;

                    // lv 상승
                    Bear.LocalData.Upgrades[id]++;
                }
            }
            else
            {
                if (Bear.LocalData.TotalGold < UpgradeRequireGold)
                    Debug.Log("골드가 부족합니다.");
                else
                {
                    // 골드 소모
                    Bear.LocalData.TotalGold -= UpgradeRequireGold;

                    // lv 상승
                    Bear.LocalData.Upgrades.Add(id, 1);
                }
            }

            Bear.LocalData.Save();

            CheckFishIndex();
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

