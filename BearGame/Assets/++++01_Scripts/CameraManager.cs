using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    class CameraManager : MonoBehaviour
    {
        Transform mCameraTm;

        Vector3 mDefaultPos;
        Quaternion mDefaultRot;

        Vector3 mTargetPos;
        float mSpeed;

        Vector3 mOrgPos;
        Vector3 mRightward;
        Vector3 mUpward;

        float mDuration;
        float mElapsed;
        float mPowerX;
        float mPowerY;
        float mDelay;
        float mVibrate;

        bool mIsShake;

        public bool IsShake { get { return mIsShake && mElapsed < mDuration; } }

        public void Init()
        {
            mCameraTm = GameObject.Find("Main_Camera").transform;

            mDefaultPos = mCameraTm.localPosition;
            mDefaultRot = mCameraTm.localRotation;

            mIsShake = false;
        }

        public void MoveTo(Vector3 target, float speed = 0.5f)
        {
            mTargetPos = target;
            mSpeed = speed;
        }

        private void FixedUpdate()
        {
            if (mIsShake)
            {
                ShakeUpdate();
            }
        }

        void ShakeUpdate()
        {
            mDelay -= Time.deltaTime;
            if (mDelay > 0)
                return;

            mElapsed += Time.deltaTime;

            float dx = Mathf.Cos(mElapsed * mVibrate * 2.5f + 13) +
                       Mathf.Sin(mElapsed * mVibrate * 4.7f - 4) * 0.5f;
            float dy = Mathf.Cos(mElapsed * mVibrate * 3.0f + 19) +
                       Mathf.Sin(mElapsed * mVibrate * 5.1f + 9) * 0.5f;

            float amp = Mathf.Pow(1 - mElapsed / mDuration, 2.0f);

            Vector3 pos = mOrgPos + amp * dx * mPowerX * mRightward + amp * dy * mPowerY * mUpward;

            if (mCameraTm != null)
            {
                mCameraTm.position = pos;

                if (mElapsed >= mDuration)
                {
                    mCameraTm.position = mOrgPos;
                    mElapsed = mDuration = 0;
                    mIsShake = false;
                }
            }
        }

        public void Refresh()
        {
            mCameraTm.localPosition = mDefaultPos;
            mCameraTm.localRotation = mDefaultRot;
        }

        public void Shake(float power, float duration, float vibrate, float delay = 0f)
        {
            Shake(power, power, duration, vibrate, delay);
        }

        public void Shake(float powerX, float powerY, float duration, float vibrate, float delay)
        {
            // 이미 실행중이면 아래 값들은 그냥 그대로 둔다.
            if (IsShake == false)
            {
                mOrgPos = mCameraTm.position;
                mUpward = mCameraTm.up;
                mRightward = mCameraTm.right;
                mDelay = delay;
                mIsShake = true;
            }

            mDuration = duration;
            mElapsed = 0;
            mPowerX = powerX;
            mPowerY = powerY;
            mVibrate = vibrate;
        }
    }
}

