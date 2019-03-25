using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName = "";
    public Sprite itemImage;

    public float itemLifetime = 3f;

    //public bool isPotion = false;

    GameManager gameManager;

    void Start()
    {
        //Despawn with a little bit of randomness
        itemLifetime += Random.Range(0f,10f);
        Debug.Log(itemLifetime);
        Destroy(this.gameObject, itemLifetime);
        gameManager = GameManager.instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (itemName == "Health Potion")
        {
            if (gameManager.GiveHPPot() == true)
            {
                Destroy(gameObject);
            }
            else {
                Debug.Log("You're already carrying max potions!");
            }
        }
        else if(itemName == "Mana Potion")
        {
            gameManager.GiveManaPot();
        }
        else
        {
            gameManager.GiveItem(this);
            Destroy(gameObject);
        }


    }

}
