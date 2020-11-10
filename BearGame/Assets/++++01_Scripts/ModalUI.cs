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

        Color EnabledColor = Color.cyan;
        Color DisabledColor = Color.gray;

        public void Init()
        {
            modal = Resources.Load<GameObject>("OBJ/UI_Modal");

            mOpenButton = GameObject.Find("ButtonMenu").Get<Button>();
            mOpenButton.onClick.AddListener(CreateUI);
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
            DataReadUpgrade(); 
        }

        void DestroyUI()
        {
            mModalUI.Destroy();
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



        public void DataReadUpgrade()
        {
            //오브젝트 로드-데이터 넣기-버튼을 누를때 받을 수 있는 변수제작-캔버스/모달/아래 생성
            List<string> upgradeList = new List<string>(Bear.GameData.UpgradeData.Keys);

            foreach (string index in upgradeList)
            {
                //박스하나 프리팹을 가져와서 생성, 모달 아래에 붙여서 초기화
                var upgradeListBox = GameObject.Instantiate(Resources.Load<GameObject>("OBJ/Upgrade_List"));
                Transform canvas = GameObject.Find("Upgrade").transform;
                upgradeListBox.transform.SetParent(canvas);
                upgradeListBox.transform.localScale = Vector3.one;
                upgradeListBox.transform.localPosition = new Vector3(0, 0, 0);


                //데이터 접근하여 하나씩 받아옴
                var id = Bear.GameData.UpgradeData[index].id;
                var name = Bear.GameData.UpgradeData[index].name;
                var desc = Bear.GameData.UpgradeData[index].desc;
                var level = Bear.GameData.UpgradeData[index].levelUpValue;
                var count = Bear.LocalData.Upgrades.ContainsKey(id)?Bear.LocalData.Upgrades[id]:0;
                var gold = count<=0? Bear.GameData.UpgradeData[index].requireGold : Bear.GameData.UpgradeData[index].requireGold*count;


                var text = upgradeListBox.Find("Text").GetComponent<Text>();
                text.text = id + "\r\n" + name + "\r\n" + desc;

                var button = upgradeListBox.Find("Button");
                var buttonText = button.Find("Text").GetComponent<Text>();
                buttonText.text = level + "\r\n" + gold;
                button.GetComponent<Button>().onClick.AddListener(delegate { Method_upgradeButton(id); });
            }
        }

        void DataReadFishIndex()
        {
            List<int> upgradeList = new List<int>(Bear.GameData.FishData.Keys);

            foreach(int index in upgradeList)
            {

            }
        }



        //로컬데이터
        //서버데이터(초기값,전체기준값)- 업그레이드: UpgradeData / 

        void Method_upgradeButton(string id)
        {
            //if(Bear.LocalData.TotalGold>=뿌엏?
            Bear.GameData.UpgradeData[id].levelUpValue++;
            Bear.GameData.UpgradeData[id].requireGold+=100;
            //버튼에서 누를 경우 
            //숫자,골드, 곰 수치 변경

            Debug.Log("업글완료");
        }


    }
}


