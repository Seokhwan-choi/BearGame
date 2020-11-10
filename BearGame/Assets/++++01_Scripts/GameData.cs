using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Bear
{
    public class GameData : MonoBehaviour
    {
        const string FishDataLink = "https://docs.google.com/spreadsheets/d/1h6IVNmEM8tHsVnUE1J8gf2LiGzcF-NBW3wLbOgG-a6Y/export?format=csv";
        const string UpgradeDataLink = "https://docs.google.com/spreadsheets/d/1h6IVNmEM8tHsVnUE1J8gf2LiGzcF-NBW3wLbOgG-a6Y/export?format=csv&gid=1257219393";
        
        public Dictionary<int, FishData> FishData;
        public Dictionary<string, UpgradeData> UpgradeData;

        public bool Finish { get; set; }

        GameData()
        {
            Finish = false;
        }

        public IEnumerator ReadGameData()
        {
            //물고기도감
            UnityWebRequest www = UnityWebRequest.Get(FishDataLink);
            yield return www.SendWebRequest();

            //업그레이드 정보
            string data = www.downloadHandler.text;
            FishDataMaker.Make(data);
            yield return ReadUpgradeData();
        }
        
        
        IEnumerator ReadUpgradeData()
        {
            UnityWebRequest www = UnityWebRequest.Get(UpgradeDataLink);
            yield return www.SendWebRequest();

            string data = www.downloadHandler.text;
            UpgradeDataMaker.Make(data);
            Finish = true;
        }
    }
}

