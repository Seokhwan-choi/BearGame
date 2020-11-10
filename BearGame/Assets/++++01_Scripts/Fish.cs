using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear 
{
    public class Fish : MonoBehaviour
    {
        public void Init(int id)
        {
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sprite = Bear.Atlas.GetSprite($"fish{id}");
            }
        }

        private void Update()
        {
            Vector3 worldpos = Camera.main.WorldToViewportPoint(this.transform.position);
            Rigidbody2D rbd = GetComponent<Rigidbody2D>();

            if (worldpos.x < 0f)
            {
                rbd.AddForce(new Vector2(1, 1), ForceMode2D.Impulse);
            }
                
            if (worldpos.y < 0f)
            {
                worldpos.y = 0f;
            }
                
            if (worldpos.x > 1f)
            {
                rbd.AddForce(new Vector2(-1, 1), ForceMode2D.Impulse);
            }

            this.transform.position = Camera.main.ViewportToWorldPoint(worldpos);
        }
    }
}

