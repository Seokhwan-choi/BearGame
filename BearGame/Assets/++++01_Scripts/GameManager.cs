using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{ 
    public class GameManager : MonoBehaviour
    {
        BearManager mBearManager;
        FishManager mFishManager;
        TouchManager mTouchManager;

        ModalUI mModalUI;
        HUDManager mHUDManager;
        public BearManager BearManager => mBearManager;
        public FishManager FishManager => mFishManager;
        
        public void Init()
        {
            mBearManager = new BearManager();
            mBearManager.Init();

            mFishManager = new FishManager();
            mTouchManager = new TouchManager(this);

            mModalUI = new ModalUI();
            mModalUI.Init();

            mHUDManager = new HUDManager();
            mHUDManager.Init();
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;

            mTouchManager.Update(deltaTime);
            mHUDManager.Update();
        }
    }
}