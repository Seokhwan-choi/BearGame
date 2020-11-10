using UnityEngine.U2D;
using UnityEngine;

namespace Bear
{
    class PreLoad
    {
        SpriteAtlas mAtlas;

        public void Init()
        {
            mAtlas = Resources.Load<SpriteAtlas>("Atlas/Fish");
        }

        public Sprite GetSprite(string name)
        {
            return mAtlas.GetSprite(name);
        }
    }
}

