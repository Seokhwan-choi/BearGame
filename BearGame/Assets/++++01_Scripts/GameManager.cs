using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{ 
    public class GameManager : MonoBehaviour
    {
        FishManager mFishManager;
        TouchManager mTouchManager;
        UpgradeManager mUpgradeManager;

        GameObject mPlayer;
        LobbyUI mLobbyUI;
        HUDManager mHUDManager;
        
        public GameObject Player => mPlayer;
        public FishManager FishManager => mFishManager;
        
        public void Init()
        {
            mPlayer = Util.Instantiate("Player_Bear");

            mFishManager = new FishManager();
            mTouchManager = new TouchManager(this);
            mUpgradeManager = new UpgradeManager();

            mLobbyUI = new LobbyUI();
            mLobbyUI.Init(mUpgradeManager);

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