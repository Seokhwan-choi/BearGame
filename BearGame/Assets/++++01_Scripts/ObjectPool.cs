using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Bear
{
    class ObjectPool : MonoBehaviour
    {
        public class PoolEntry
        {
            public GameObject Prefab;
            public Stack<GameObject> Items = new Stack<GameObject>();
        }

        // 오브젝트 풀
        public Dictionary<string, PoolEntry> mPathToPool = new Dictionary<string, PoolEntry>();
        public Dictionary<GameObject, PoolEntry> mObjToPool = new Dictionary<GameObject, PoolEntry>();
        public Dictionary<GameObject, PoolEntry> mActiveObjects = new Dictionary<GameObject, PoolEntry>();

        // 하이라키 상에서 임시 엄마
        Transform mUIPool;

        int mUniqueId = 100;

        public void Init()
        {
            var uiPool = GameObject.Find("Canvas/UIPool");
            if (uiPool != null )
            {
                mUIPool = uiPool.transform;
            }            
        }

        public GameObject Acquire(string prefabPath, Transform parent = null)
        {
            mPathToPool.TryGetValue(prefabPath, out PoolEntry pool);
            if ( pool == null )
            {
                var prefabObj = Resources.Load("OBJ/" + prefabPath) as GameObject;
                if (prefabObj == null) 
                    return null;

                pool = new PoolEntry() { Prefab = prefabObj };

                mPathToPool.Add(prefabPath, pool);
            }

            return Acquire(pool, parent);
        }


        public GameObject Acquire(PoolEntry pool, Transform parent = null)
        {
            Debug.Assert(pool != null);

            GameObject go;

            if ( pool.Items.Count > 0)
            {
                go = pool.Items.Pop();
            }
            else
            {
                go = Instantiate(pool.Prefab, parent);
                go.name = $"{pool.Prefab.name}_{mUniqueId++}";
            }

            if (parent == null)
            {
                go.transform.SetParent(mUIPool, false);
            }
            else
            {
                go.transform.SetParent(parent, false);
            }

            go.SetActive(true);

            mActiveObjects[go] = pool;

            return go;
        }

        public void Release(GameObject go, Transform parent = null)
        {
            mActiveObjects.TryGetValue(go, out PoolEntry pool);
            if ( pool != null )
            {
                // 반납               
                if (parent == null)
                {
                    go.transform.SetParent(mUIPool, false);
                }
                else
                {
                    go.transform.SetParent(parent, false);
                }
                // 비활성화
                go.SetActive(false);

                pool.Items.Push(go);
                mActiveObjects.Remove(go);
            }
            else
            {
                Debug.LogError($"GameObjectPool : Try to releasing not activated object ({go.name})");
            }
        }

        public void DestroyAllObjects()
        {
            var pools = mPathToPool.Values.Concat(mObjToPool.Values);

            foreach( PoolEntry pool in pools)
            {
                foreach( var go in pool.Items )
                {
                    Destroy(go);
                }

                pool.Items.Clear();
            }

            foreach( var item in mActiveObjects )
            {
                Destroy(item.Key);
            }

            mPathToPool.Clear();
            mObjToPool.Clear();
            mActiveObjects.Clear();
        }
    }
}


