using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System;

//구글애드몹 전면광고
public class AdmobScreenAd : MonoBehaviour
{
    private InterstitialAd ImageAd;
    private InterstitialAd VideoAd;
    //private InterstitialAd ScreenAd;

    public Text LogText;

    private void Start()
    {
        InitAd();
        //Invoke("Show", 10f);    //10초 후 show 실행
    }

    void InitAd()
    {
        string imageid = Debug.isDebugBuild ? GoogleAdMobManager.GoogleAdmobID_Screen_Image_Test : GoogleAdMobManager.GoogleAdmobID_Screen_Image;
        string videoid = Debug.isDebugBuild ? GoogleAdMobManager.GoogleAdmobID_Screen_Video_Test : GoogleAdMobManager.GoogleAdmobID_Screen_Video;

        ImageAd = new InterstitialAd(imageid);
        VideoAd = new InterstitialAd(videoid);
        
        AdRequest request = new AdRequest.Builder().Build();

        //로드후 바로 광고보기(show)를 실행하면 로딩되는 시간때문에 안될 수 있음. 그래서 코루틴으로 따로 빼서
        ImageAd.LoadAd(request);
        VideoAd.LoadAd(request);


        //ImageAd.OnAdClosed += (sender, e) => LogText.text = "일반전면광고 완료. 보상";
        //ImageAd.OnAdLoaded += (sender, e) => LogText.text = "일반전면광고 로드";
        //
        //VideoAd.OnAdClosed += (sender, e) => LogText.text = "비디오전면광고 완료.보상";
        //VideoAd.OnAdLoaded += (sender, e) => LogText.text = "비디오전면광고 로드";

        Handle(ImageAd);
        Handle(VideoAd);
    }

    public void ShowImage()
    {
        StartCoroutine("ShowImagenAd");
    }

    private IEnumerator ShowImagenAd()
    {
        while(!ImageAd.IsLoaded())
        {
            yield return null;
        }

        ImageAd.Show();
    }


    public void ShowVideo()
    {
        StartCoroutine("ShowVideoAd");
    }

    private IEnumerator ShowVideoAd()
    {
        while (!VideoAd.IsLoaded())
        {
            yield return null;
        }

        VideoAd.Show();
    }





    //===============

    private void Handle(InterstitialAd Ad)
    {
        //Ad.OnAdLoaded += HandleOnAdLoaded;
        //Ad.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        //Ad.OnAdFailedToShow += HandleOnAdFailedToShow;
        //Ad.OnAdOpening += HandleOnAdOpening;
        Ad.OnAdClosed += HandleOnAdClosed;
        //Ad.OnUserEarnedReward += HandleOnUserEarnedReward;
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        InitAd();
    }
}
