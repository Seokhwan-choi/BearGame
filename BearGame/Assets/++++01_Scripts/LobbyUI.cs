using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bear
{
    public class LobbyUI
    {
        public void Init(UpgradeManager upgradeManager)
        {
            // 메뉴 버튼 눌렀을 때
            var menuButton = GameObject.Find("ButtonMenu").Get<Button>();
            menuButton.onClick.AddListener(() => ShowModalUI(upgradeManager));
        }

        void ShowModalUI(UpgradeManager upgradeManager)
        {
            var modalUI = new ModalUI();
            modalUI.Init(upgradeManager);
        }
    }
}

