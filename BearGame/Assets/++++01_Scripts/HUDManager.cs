using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bear
{
    public class HUDManager        
    {
        Text mGoldText;

        public void Init()
        {
            mGoldText = GameObject.Find("GoldText").GetComponent<Text>();
        }


        public void Update()
        {
            mGoldText.text = Bear.LocalData.TotalGold.ToString();
        }
    }
}