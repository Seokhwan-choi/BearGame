using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class FishManager
    {
        List<GameObject> mFishList;

        public List<GameObject> FishList => mFishList;
        public FishManager()
        {
            mFishList = new List<GameObject>();
        }

        public GameObject CreateFish(int areaNumber, int lv, bool isLeft, FishType type)
        {
            // lv를 확인해서 
            // 확률적으로 물고기가 나온다.
            var typeDice = UnityEngine.Random.Range(0, 3);
            string fishSize = (typeDice == 0 ? "Fish_S" : (typeDice == 1 ? "Fish_M" : "Fish_L"));
            
            var fish = Bear.ObjectPool.Acquire(fishSize);
            if ( fish != null )
            {
                mFishList.Add(fish);

                // 생성된 물고기는 하늘로 날아간다.
                var rbd = fish.GetComponent<Rigidbody2D>();
                var directionX = isLeft ? -1 : 1;
                var highForce = Random.Range(5, 15);

                rbd.AddForce(new Vector2(directionX, highForce), ForceMode2D.Impulse);

                //id로 스프라이트 변경
                var sprite = fish.GetComponent<Fish>();
                sprite.Init(UnityEngine.Random.Range(0, 10));

            }

            return fish;
        }

        public bool RemoveFish(int index)
        {
            if (mFishList.Count > index)
            {
                var go = mFishList[index];
                if (go != null)
                {
                    go.transform.position = Vector3.zero;

                    mFishList.RemoveAt(index);

                    Bear.ObjectPool.Release(go);

                    return true;
                }
            }

            return false;
        }
    }
}

