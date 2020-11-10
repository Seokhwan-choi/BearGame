using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
public class UnityADSManager : MonoBehaviour
{
    const string CanSkipAds = "video";
    const string RewardAds = "rewardedVideo";
    const string UnityADSId = "3819273";
    const bool TestMode = true;

    public Text LogText;

    private void Awake()
    {
        Advertisement.Initialize(UnityADSId, TestMode);
    }

    public void ShowCanSkipAds()
    {
        //스킵가능 광고
        if (Advertisement.IsReady())
        {
            Advertisement.Show(CanSkipAds);
        }
        else
        {
            LogText.text = "스킵가능 광고 : 유니티광고가 준비되지 않았습니다.";
        }
    }

    public void ShowRewardedAds()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show(RewardAds, new ShowOptions() { resultCallback = AdsResultHandler});
        }
        else
        {
            LogText.text = "보상형 광고 : 유니티광고가 준비되지 않았습니다.";
        }
    }

    void AdsResultHandler(ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Finished:
                LogText.text = "광고보기 성공";
                break;
            case ShowResult.Skipped:
                LogText.text = "광고보기 스킵";
                break;
            case ShowResult.Failed:
                LogText.text = "광고보기 실패";
                break;
        }
    }
    
    //public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    //{
    //    switch (showResult)
    //    {
    //        case ShowResult.Finished:
    //            Debug.Log("광고보기 성공");
    //            break;
    //        case ShowResult.Skipped:
    //            Debug.Log("광고보기 스킵");
    //            break;
    //        case ShowResult.Failed:
    //            Debug.Log("광고보기 실패");
    //            break;
    //    }
    //}

    //public void OnUnityAdsDidError(string message)
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void OnUnityAdsDidStart(string placementId)
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void OnUnityAdsReady(string placementId)
    //{
    //    throw new System.NotImplementedException();
    //}

}
