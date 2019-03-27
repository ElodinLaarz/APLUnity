using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item;
    public float itemLifetime = 3f;

    //public bool isPotion = false;

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = item.icon;

        //Despawn with a little bit of randomness
        itemLifetime += Random.Range(0f, 10f);
        Destroy(this.gameObject, itemLifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            Inventory.instance.Add(this);
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}
