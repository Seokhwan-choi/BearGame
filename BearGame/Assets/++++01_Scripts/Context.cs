using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    //항상 실행될 클래스
    public class Context : MonoBehaviour
    {        
        private void Awake()
        {
            Application.targetFrameRate = 60;
            Application.backgroundLoadingPriority = UnityEngine.ThreadPriority.Normal;
            QualitySettings.vSyncCount = 0; //수직동기화 Off...비디오카드 불라불라

            Bear.Init(gameObject);
        }
    }


    class Bear
    {
        static public ObjectPool ObjectPool;
        static public CameraManager CameraManager;
        static public PreLoad Atlas;
        static public LocalData LocalData;
        static public GameData GameData;
        static public GameManager GameManager;
        static public SoundManager SoundManager;
        
        static public void Init(GameObject go)
        {
            //CameraManager = go.AddComponent<CameraManager>();
            //CameraManager.Init();

            GameData = go.AddComponent<GameData>();

            ObjectPool = go.AddComponent<ObjectPool>();
            ObjectPool.Init();

            Atlas = new PreLoad();
            Atlas.Init();

            LocalDataSerializer.Delete();
            LocalData = LocalDataSerializer.Load();
            LocalData.Init();

            Transform canvas = GameObject.Find("Canvas").transform;
            Util.Instantiate("Loading", canvas);

            Bear.GameManager = go.AddComponent<GameManager>();
            Bear.GameManager.Init();

            //var obj = GameObject.Find("SoundManager");            
            //SoundManager = obj.AddComponent<SoundManager>();
            //SoundManager.Init();
        }

    }
}

