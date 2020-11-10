using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
namespace Bear
{
    //SceneManager매서드
    //CreateScene,LoadeScene,LoadSceneAsync:비동기방식으로 씬로드, MergeScenes:소스씬을 다른 씬으로 통합, 
    //MoveGameObjectToScene:현 씬의 특정게임오브젝트를 다른 씬으로 이동, UnloadScene:현 씬의 게임 오브젝트 삭제

    public class Loading : MonoBehaviour
    {
        public Slider mProgressBar;
        public Text mLoadText;
        public Text mPercentNumber;

        void Start()
        {
            mProgressBar.value = 0f;
            mLoadText.text = "Loading...";
            mPercentNumber.text = "0%";
            StartCoroutine(LoadScene());
        }
        private void Update()
        {
            mPercentNumber.text = $"{Mathf.Round(mProgressBar.value * 100f)}%";
        }
        IEnumerator LoadScene()
        {
            yield return PercentCharge(0.5f);
            
            yield return Bear.GameData.ReadGameData(); //게임데이터를 부른다.

            yield return PercentCharge(1f);
            mLoadText.text = "Press Touch to Start?";

            yield return new WaitUntil(() => Input.anyKeyDown);
            
            Destroy(gameObject);
        }

        IEnumerator PercentCharge(float f)
        {
            while(mProgressBar.value < f)
            {
                mProgressBar.value = Mathf.MoveTowards(mProgressBar.value, f, Time.deltaTime);

                yield return null;
            }
        }
    }

}


