using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class FishManager
    {
        List<Fish> mFishList;

        public List<Fish> FishList => mFishList;
        public FishManager()
        {
            mFishList = new List<Fish>();
        }

        public Fish CreateFish(int areaNumber, int lv, bool isLeft, FishType type)
        {
            // lv를 확인해서 
            // 확률적으로 물고기가 나온다.
            var typeDice = UnityEngine.Random.Range(0, 3);
            string fishName = (typeDice == 0 ? "Fish_S" : (typeDice == 1 ? "Fish_M" : "Fish_L"));
            
            var fishObj = Bear.ObjectPool.Acquire(fishName);
            if (fishObj != null )
            {
                var fish = fishObj.GetOrAddComponent<Fish>();
                fish.Init(UnityEngine.Random.Range(1, 10), false);

                mFishList.Add(fish);

                FishAddForce(fish, isLeft);

                return fish;
            }

            return null;
        }

        public Fish CreateGoldFish(int lv, bool isLeft, FishType type)
        {
            var fishObj = Bear.ObjectPool.Acquire("Fish_L");
            if (fishObj != null)
            {
                var fish = fishObj.GetOrAddComponent<Fish>();
                fish.Init(9, true);

                mFishList.Add(fish);

                FishAddForce(fish, isLeft);

                return fish;
            }

            return null;
        }

        public bool RemoveFish(int index)
        {
            if (mFishList.Count > index)
            {
                var fish = mFishList[index];
                if (fish != null)
                {
                    var go = fish.gameObject;
                    go.transform.position = Vector3.zero;

                    mFishList.RemoveAt(index);

                    Bear.ObjectPool.Release(go);

                    return true;
                }
            }

            return false;
        }

        void FishAddForce(Fish fish, bool isLeft)
        {
            // 생성된 물고기는 하늘로 날아간다.
            var rbd = fish.GetComponent<Rigidbody2D>();
            var directionX = isLeft ? -1 : 1;
            var highForce = Random.Range(5, 15);

            rbd.AddForce(new Vector2(directionX, highForce), ForceMode2D.Impulse);
        }
    }
}

