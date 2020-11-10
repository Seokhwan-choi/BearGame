using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

namespace Bear
{
    static class Util
    {
        public const string RandCharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

        public static string RandString(int length, string charSet = RandCharSet)
        {
            var sb = new StringBuilder(length, length);

            RandString(sb, length, charSet);

            return sb.ToString();
        }

        public static void RandString(StringBuilder sb, int length, string charSet = RandCharSet)
        {
            for (int i = 0; i < length; i++)
            {
                sb.Append(SelectAny(charSet));
            }
        }

        public static char SelectAny(string str)
        {
            if (str == string.Empty)
                return (char)0;

            float seed = Time.time * 100f;

            UnityEngine.Random.InitState((int)seed);

            return str[UnityEngine.Random.Range(0, str.Length)];
        }

        public static T RandomEnum<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(UnityEngine.Random.Range(0,values.Length));
        }

        public static GameObject Instantiate(string prefabPath, Transform parent = null)
        {
            var obj = Resources.Load<GameObject>(String.Format("OBJ/{0}", prefabPath));

            if (obj == null)
            {
                return default(GameObject);
            }

            GameObject go = GameObject.Instantiate(obj, parent);
            
            return go;
        }

        public static GameObject Find(this GameObject go, string name, bool includeinactive = false)
        {
            if (go != null)
            {
                var tmList = go.GetComponentsInChildren<Transform>(includeinactive);
                foreach( var tm in tmList)
                {
                    if (tm.name == name)
                        return tm.gameObject;
                }
            }

            return null;
        }

        public static void Destroy(this GameObject go)
        {
            if ( go != null )
            {
                GameObject.Destroy(go);
            }
        }

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (comp == null)
            {
                comp = go.AddComponent<T>();
            }

            return comp;
        }

        public static T Get<T>(this GameObject go)
        {
            T comp = go.GetComponent<T>();
            
            return comp;
        }

        static public void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        public static bool Intersects(this GameObject go, GameObject other)
        {
            BoxCollider colliderA = go.GetComponent<BoxCollider>();
            BoxCollider colliderB = other.GetComponent<BoxCollider>();
            if ( colliderA != null && colliderB != null )
            {
                Bounds boundA = colliderA.bounds;
                Bounds boundB = colliderB.bounds;

                return boundA.Intersects(boundB);
            }

            return false;
        }

        public static bool Intersects2D(this GameObject go, GameObject other)
        {
            BoxCollider2D colliderA = go.GetComponent<BoxCollider2D>();
            BoxCollider2D colliderB = other.GetComponent<BoxCollider2D>();
            if (colliderA != null && colliderB != null)
            {
                Bounds boundA = colliderA.bounds;
                Bounds boundB = colliderB.bounds;

                return boundA.Intersects(boundB);
            }

            return false;
        }

        public static void ResetAnchor(this RectTransform rt)
        {
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
        }
    }
}