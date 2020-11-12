using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bear
{
    public class TouchManager
    {
        const float TouchDelay = 0.2f;
        const float Fever_TouchDelay = 0.1f;
        const float Fever_Time = 10f;
        const float Fever_Gauge = 2;

        BearManager mBearManager;
        FishManager mFishManager;

        float mTouchDelay;

        float mFeverTime;
        bool mIsFeverMode;
        int mFeverGauge;

        public TouchManager(GameManager gameManager)
        {
            mBearManager = gameManager.BearManager;
            mFishManager = gameManager.FishManager;

            mTouchDelay = TouchDelay;
        }

        public void Update(float deltaTime)
        {
            mTouchDelay -= deltaTime;
            mFeverTime -= deltaTime;
            
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

            if (mFeverTime <= 0)
            {
                mIsFeverMode = false;
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
                    bool isGoldFish = fishList[i].IsGoldFish;
                    if (mFishManager.RemoveFish(i--))
                    {
                        Bear.LocalData.TotalGold += 100 * (isGoldFish ? 2 : 1);
                        Bear.LocalData.Save();
                    }
                }
            }
        }

        void TouchBear(Vector2 mousePos)
        {
            if (mBearManager.Player.GetComponent<BoxCollider2D>().OverlapPoint(mousePos))
            {
                bool isLeft = Random.Range(0, 2) == 0 ? true : false;

                // 한번에 생성할 물고기 양
                int touchPower = 0;
                if (Bear.LocalData.Upgrades != null && 
                    Bear.LocalData.Upgrades.ContainsKey("TouchPower"))
                {
                    touchPower = Bear.LocalData.Upgrades["TouchPower"];
                }

                // 피버 게이지 증가
                if (mIsFeverMode)
                {
                    for(int i = 0; i < touchPower + 1; ++i)
                    {
                        mFishManager.CreateGoldFish(1, isLeft, FishType.large);
                    }

                    mTouchDelay = Fever_TouchDelay;
                }
                else
                {
                    mFeverGauge++;
                    if (mFeverGauge >= Fever_Gauge)
                    {
                        mFeverGauge = 0;
                        mIsFeverMode = true;
                        mFeverTime = Fever_Time;
                    }

                    // 물고기 생성
                    for (int i = 0; i < touchPower + 1; ++i)
                    {
                        mFishManager.CreateFish(1, 1, isLeft, FishType.large);
                    }

                    mTouchDelay = TouchDelay;
                }
            }
        }
        
    }
}