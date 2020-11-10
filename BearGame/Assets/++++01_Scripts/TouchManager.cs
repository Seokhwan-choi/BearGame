using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bear
{
    public class TouchManager
    {
        const float TouchDelay = 0.2f;
        const float Fever_TouchDelay = 0.1f;

        BearManager mBearManager;
        FishManager mFishManager;

        float mTouchDelay;
        bool mIsLeft;

        

        public TouchManager(GameManager gameManager)
        {
            mBearManager = gameManager.BearManager;
            mFishManager = gameManager.FishManager;

            mTouchDelay = TouchDelay;
            
        }

        public void Update(float deltaTime)
        {
            mTouchDelay -= deltaTime;
            
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                if ( Input.GetMouseButtonDown(0) )
                {
                    if (mTouchDelay <= 0)
                    {
                        TouchBear(mousePos);
                    }
                }

                TouchFish(mousePos);
            }
        }

        void TouchFish(Vector2 mousePos)
        {
            var fishList = mFishManager.FishList;
            for (int i = 0; i < fishList.Count; ++i)
            {
                // 물고기 획득
                if (fishList[i].GetComponent<BoxCollider2D>().OverlapPoint(mousePos) )
                {
                    if (mFishManager.RemoveFish(i))
                    {
                        Bear.LocalData.TotalGold += 100;
                        //mGoldText.text = Bear.LocalData.TotalGold.ToString();
                        Bear.LocalData.Save();
                        --i;
                    }
                }
            }
        }

        void TouchBear(Vector2 mousePos)
        {
            if (mBearManager.Player.GetComponent<BoxCollider2D>().OverlapPoint(mousePos))
            {
                mIsLeft = !mIsLeft;

                int touchPower = 0;
                if (Bear.LocalData.Upgrades != null && 
                    Bear.LocalData.Upgrades.ContainsKey("TouchPower"))
                {
                    touchPower = Bear.LocalData.Upgrades["TouchPower"];
                }

                for(int i = 0; i < touchPower + 1; ++i)
                {
                    // 물고기 생성
                    mFishManager.CreateFish(1, 1, mIsLeft,FishType.large);
                }

                mTouchDelay = TouchDelay;

                // To Do : 피버게이지 획득
            }
        }
        
    }
}