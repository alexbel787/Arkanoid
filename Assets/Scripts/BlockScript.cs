using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public int health;
    public Sprite[] crackedSpites;
    private SpriteRenderer SR;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }
    public void ChangeSprite()
    {
        SR.sprite = crackedSpites[health - 1];
    }

    public IEnumerator DestroyBlockCoroutine()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime * 2;
            transform.localScale = new Vector3(t, t, t);
            yield return null;
        }
        Destroy(gameObject);
    }


}
