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
        static readonly string[] MenuName = new string[] { "Upgrade", "FishIndex", "Setting" };

        List<GameObject> mMenuList;
        List<GameObject> mTabButtonList;
        Dictionary<string, GameObject> mUpgradeButtonList;
        UpgradeManager mUpgradeManager;
        public void Init(UpgradeManager upgradeManager)
        {
            Close();

            mUpgradeManager = upgradeManager;
            mMenuList = new List<GameObject>();
            mTabButtonList = new List<GameObject>();
            mUpgradeButtonList = new Dictionary<string, GameObject>();

            var modal = Util.InstantiateUI("UI_Modal");

            InitMenuAndButton(modal);
            InitUpgradeData(); //업그레이드 데이터

            ShowMenu(0);
        }

        void OnCloseButton()
        {
            // 사운드
            Close();
        }

        void Close()
        {
            var modal = GameObject.Find("UI_Modal");
            if ( modal != null )
            {
                modal.Destroy();
            }
           
            mUpgradeButtonList?.Clear();
        }

        void InitMenuAndButton(GameObject modal)
        {
            // 닫기 버튼
            var closeButton = modal.Find("ButtonClose").Get<Button>();
            closeButton.onClick.AddListener(OnCloseButton);

            // 메뉴 & 탭 버튼
            for (int i = 0; i < MenuName.Length; ++i)
            {
                var menu = modal.Find($"{MenuName[i]}_Menu");
                mMenuList.Add(menu);

                var button = modal.Find($"{MenuName[i]}Button");
                int index = i;
                button.Get<Button>().onClick.AddListener(() => ShowMenu(index));
                mTabButtonList.Add(button);
            }
        }

        void ShowMenu(int index)
        {
            index = Mathf.Clamp(index, 0, MenuName.Length - 1);
                
            for(int i = 0; i < MenuName.Length; ++i)
            {
                if (index == i)
                {
                    mTabButtonList[i].GetComponent<Image>().color = Color.cyan;
                    mMenuList[i].SetActive(true);
                }
                else
                {
                    mTabButtonList[i].GetComponent<Image>().color = Color.gray;
                    mMenuList[i].SetActive(false);
                }
            }
        }

        void InitUpgradeData()
        {
            Transform parent = GameObject.Find("Upgrade").transform;
            //오브젝트 로드-데이터 넣기-버튼을 누를때 받을 수 있는 변수제작-캔버스/모달/아래 생성
            foreach (UpgradeData data in Bear.GameData.UpgradeData.Values)
            {
                //박스하나 프리팹을 가져와서 생성, 모달 아래에 붙여서 초기화
                var upgradeListBox = Util.Instantiate("Upgrade_List", parent);

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

                    button.GetComponent<Button>().onClick.RemoveAllListeners();
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


