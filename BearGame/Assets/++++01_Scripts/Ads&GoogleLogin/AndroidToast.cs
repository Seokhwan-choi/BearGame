using UnityEngine;
using System.Collections;

namespace Bear
{
    //토스트용
    //https://iw90.tistory.com/70
    public class AndroidToast : MonoBehaviour
    {

        private AndroidJavaObject androidJavaObject;

        void Start()
        {
            // Unity 에서 Android 접근 하기 위해 생성자를 초기화
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        void OnGUI()
        {
            //Unity 에서 입력 받을 버튼 생성
            if (GUILayout.Button("toast Call", GUILayout.Width(200), GUILayout.Height(200)))
            {
                callToast();// Android 호출
            }
        }

        public void callToast()
        {
            androidJavaObject.Call("toastTest");
        }
    }
}