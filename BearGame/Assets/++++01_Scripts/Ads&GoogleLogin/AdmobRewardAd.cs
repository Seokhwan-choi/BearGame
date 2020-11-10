using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System;
public class AdmobRewardAd : MonoBehaviour
{
    private RewardedAd rewardAd;
    public Text LogText;

    private void Start()
    {
        InitAd();
        //Invoke("Show", 10f);    //10초 후 show 실행
    }

    void InitAd()
    {
        string id = Debug.isDebugBuild ? GoogleAdMobManager.GoogleAdmobID_Reward_Test : GoogleAdMobManager.GoogleAdmobID_Reward;

        rewardAd = new RewardedAd(id);

        Handle(rewardAd);
        Load();
    }


    private void Handle(RewardedAd rewardAd)
    {
        rewardAd.OnAdLoaded += HandleOnAdLoaded;
        rewardAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        rewardAd.OnAdFailedToShow += HandleOnAdFailedToShow;
        rewardAd.OnAdOpening += HandleOnAdOpening;
        rewardAd.OnAdClosed += HandleOnAdClosed;
        rewardAd.OnUserEarnedReward += HandleOnUserEarnedReward;
    }
    private void Load()
    {
        AdRequest request = new AdRequest.Builder().Build();
        rewardAd.LoadAd(request);
    }

    public RewardedAd ReloadAd()
    {
        string id = Debug.isDebugBuild ? GoogleAdMobManager.GoogleAdmobID_Reward_Test : GoogleAdMobManager.GoogleAdmobID_Reward;
        RewardedAd videoAd = new RewardedAd(id);
        Handle(videoAd);
        AdRequest request = new AdRequest.Builder().Build();
        //테스트 기기가 여러 개일 경우 .AddTestDevice(디바이스 아이디)를 계속 추가해주시면 됩니다.
        videoAd.LoadAd(request);
        return videoAd;
    }

    public void Show()
    {
        LogText.text = "코루틴 진입";
        StartCoroutine("ShowRewardAd");
        LogText.text = "코루틴 빠져나옴";
    }

    private IEnumerator ShowRewardAd()
    {
        while (!rewardAd.IsLoaded())
        {
            LogText.text = "뿅";
            yield return null;
        }
        rewardAd.Show();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {

    }

    public void HandleOnAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        LogText.text = "로드실패";
    }

    public void HandleOnAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        LogText.text = "보기실패";
    }

    public void HandleOnAdOpening(object sender, EventArgs args)
    {

    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        this.rewardAd = ReloadAd();
        LogText.text = "닫음";
    }

    public void HandleOnUserEarnedReward(object sender, EventArgs args)
    {
        //이곳에 광고 시청에 대한 보상을 지급하는 내용을 넣어주시면 됩니다.
        LogText.text = "보상 골드를 지급합니다.";
    }


}