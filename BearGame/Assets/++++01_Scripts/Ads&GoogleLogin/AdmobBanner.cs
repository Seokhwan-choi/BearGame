using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

//구글애드몹 라인배너
public class AdmobBanner : MonoBehaviour
{
    private BannerView banner;
    public Text LogText;
    private bool active = true;
    private void Start()
    {
        InitAd();
    }

    private void InitAd()
    {
        string id = Debug.isDebugBuild ? GoogleAdMobManager.GoogleAdmobID_Banner_Test : GoogleAdMobManager.GoogleAdmobID_Banner;

        banner = new BannerView(id, AdSize.SmartBanner, AdPosition.Bottom);

        //AdRequest request = new AdRequest.Builder().AddKeyword("유니티").AddTestDevice().Build();
        AdRequest request = new AdRequest.Builder().Build();

        banner.LoadAd(request);

        banner.Show();
    }

    public void ToggleAd()
    {
        active = !active;

        LogText.text = active.ToString();
        if (active)
        {
            banner.Show();
            LogText.text = "광고보기"+ active.ToString();
        }
        else
        {
            banner.Hide();
            LogText.text = "광고숨기기" + active.ToString();
        }
    }

    public void DestroyAd()
    {
        banner.Destroy();
    }
}
