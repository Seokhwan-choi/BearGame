using Like;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bear
{
    public class ModalUI
    {
        GameObject modal;
        GameObject mModalUI;
        Button mOpenButton;
        Button mCloseButton;
        RectTransform mRectTransform;

        GameObject mUpgrade;
        GameObject mFishIndex;
        GameObject mSetting;
        GameObject mUpgradeTabButton;
        GameObject mFishIndexTabButton;
        GameObject mSettingTabButton;

        UpgradeManager mUpgradeManager;
        Dictionary<string, GameObject> mUpgradeButtonList;

        Color EnabledColor = Color.cyan;
        Color DisabledColor = Color.gray;

        public void Init(UpgradeManager upgradeManager)
        {
            mUpgradeManager = upgradeManager;
            modal = Resources.Load<GameObject>("OBJ/UI_Modal");

            mOpenButton = GameObject.Find("ButtonMenu").Get<Button>();
            mOpenButton.onClick.AddListener(CreateUI);

            mUpgradeButtonList = new Dictionary<string, GameObject>();
        }

        public void OnClick() 
        {
            CreateUI();//생성
            
            ShowUpgrade(); //최초 업글창오픈으로 초기화
            ChangButtonColor();  //서브탭버튼 맞춰서 초기화
        }


        void CreateUI()
        {
            Transform canvas = GameObject.Find("Canvas").transform;

            mModalUI = GameObject.Instantiate(modal);
            mModalUI.transform.SetParent(canvas);
            mModalUI.transform.localScale = Vector3.one;
            mModalUI.transform.localPosition = new Vector3(0, 0, 0);

            mRectTransform = mModalUI.GetComponent<RectTransform>();
            mRectTransform.offsetMin = new Vector2(0, 0);
            mRectTransform.offsetMax = new Vector2(0, 0);

            //닫기 버튼
            mCloseButton = GameObject.Find("ButtonClose").Get<Button>();

            //서브탭버튼
            mUpgradeTabButton = GameObject.Find("UpgradeButton");
            mFishIndexTabButton = GameObject.Find("FishIndexButton");
            mSettingTabButton = GameObject.Find("SettingButton");
            
            //서브탭 내 메뉴
            mUpgrade = GameObject.Find("Menu1");
            mFishIndex = GameObject.Find("Menu2");
            mSetting = GameObject.Find("Menu3");
            
            //버튼 이벤트
            mCloseButton.onClick.AddListener(DestroyUI);
            mUpgradeTabButton.Get<Button>().onClick.AddListener(ShowUpgrade);
            mFishIndexTabButton.Get<Button>().onClick.AddListener(ShowFishIndex);
            mSettingTabButton.Get<Button>().onClick.AddListener(ShowSetting);

            //업그레이드 데이터
            ReadUpradeData(); 
        }

        void DestroyUI()
        {
            mModalUI.Destroy();
            mUpgradeButtonList.Clear();
        }


        public void ShowUpgrade()
        {
            mUpgrade.SetActive(true);
            mFishIndex.SetActive(false);
            mSetting.SetActive(false);
            
            ChangButtonColor();            
        }

        public void ShowFishIndex()
        {
            mUpgrade.SetActive(false);
            mFishIndex.SetActive(true);
            mSetting.SetActive(false);

            ChangButtonColor();
        }

        public void ShowSetting()
        {
            mUpgrade.SetActive(false);
            mFishIndex.SetActive(false);
            mSetting.SetActive(true);

            ChangButtonColor();
        }

        public void ChangButtonColor()
        {
            //탭 색상 변경
            mUpgradeTabButton.GetComponent<Image>().color = mUpgrade.activeSelf ? EnabledColor : DisabledColor;
            mFishIndexTabButton.GetComponent<Image>().color = mFishIndex.activeSelf ? EnabledColor : DisabledColor;
            mSettingTabButton.GetComponent<Image>().color = mSetting.activeSelf ? EnabledColor : DisabledColor;
        }

        public void UpgradeList()
        {

            
        }

        public void ReadUpradeData()
        {
            Transform canvas = GameObject.Find("Upgrade").transform;
            //오브젝트 로드-데이터 넣기-버튼을 누를때 받을 수 있는 변수제작-캔버스/모달/아래 생성
            foreach (UpgradeData data in Bear.GameData.UpgradeData.Values)
            {
                //박스하나 프리팹을 가져와서 생성, 모달 아래에 붙여서 초기화
                var upgradeListBox = Util.Instantiate("Upgrade_List");
                upgradeListBox.transform.SetParent(canvas);
                upgradeListBox.transform.localScale = Vector3.one;
                upgradeListBox.transform.localPosition = new Vector3(0, 0, 0);

                mUpgradeButtonList.Add(data.id, upgradeListBox);

                RefreshUpgradeList(data.id);
            }
        }

        void OnUpgradeButton(string id)
        {
            // Upgrade 수치 반영
            mUpgradeManager.Upgrade(id);

            // 보이는 UI 갱신
            RefreshUpgradeList(id);

            Debug.Log("업글완료");
        }

        void RefreshUpgradeList(string id)
        {
            if (Bear.GameData.UpgradeData.TryGetValue(id, out UpgradeData data) )
            {
                if (mUpgradeButtonList.TryGetValue(id, out GameObject upgradeListBox))
                {
                    //데이터 접근하여 하나씩 받아옴
                    int level = GetUpgradeLevel(data.id);
                    int gold = GetUpgradeRequireGold(data, level);

                    Text text = upgradeListBox.Find("Text").GetComponent<Text>();
                    text.text = data.id + "\r\n" + data.name + "\r\n" + data.desc;

                    GameObject button = upgradeListBox.Find("Button");
                    Text buttonText = button.Find("Text").GetComponent<Text>();
                    buttonText.text = level + "\r\n" + gold;

                    button.GetComponent<Button>().onClick.AddListener(() => OnUpgradeButton(data.id));
                }
            }
        }

        int GetUpgradeLevel(string id)
        {
            if (Bear.LocalData.Upgrades.ContainsKey(id))
            {
                return Bear.LocalData.Upgrades[id];
            }
            else
            {
                return 0;
            }
        }

        int GetUpgradeRequireGold(UpgradeData data, int level)
        {
            if (level <= 0)
            {
                return data.requireGold;
            }
            else
            {
                return data.requireGold * level;
            }
        }
    }
}


