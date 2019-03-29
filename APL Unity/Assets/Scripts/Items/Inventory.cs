using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public delegate void OnItemChangedCallback();
    public OnItemChangedCallback onItemChangedCallback;


    public int maxPotions = 5;
    private int curHpPot = 0;
    private int curManaPot = 0;

    public int maxInventory = 24;
    public List<Item> items = new List<Item>();
    public Item currentWeapon;

    GameManager gm;

    #region Singleton
    // The Singleton pattern appears frequently enough, I've placed a collapsable region around it, to clear space for readability.

    public static Inventory instance = null;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one inventory in scene!");
            return;
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    private void Start()
    {
        gm = GameManager.instance;
    }

    public void Add(ItemObject itemObj)
    {
        // Names are hard... Within the itemObject is the information of the object which is in a scriptable object of name 'item.'
        Item item = itemObj.item;

        if (!item.isDefaultItem && items.Count < maxInventory)
        {
            items.Add(item);
            onItemChangedCallback();
            itemObj.Despawn();
        }
        else
        {
            if (item.name == "Health Potion")
            {
                if (curHpPot < maxPotions)
                {
                    curHpPot++;
                    itemObj.Despawn();
                    onItemChangedCallback();
                    return;
                }
                else
                {
                    Debug.Log("You're already carrying max Health Potions!");
                }
            }
            else if (item.name == "Mana Potion")
            {
                if (curManaPot < maxPotions)
                {
                    curManaPot++;
                    Destroy(itemObj);
                    onItemChangedCallback();
                    return;
                }
                else
                {
                    Debug.Log("You're already carrying max Mana Potions!");
                }
            }
            else if (itemObj.tag == "Weapon")
            {
                equipWeapon(item);
                gm.RefreshItem(currentWeapon);
                onItemChangedCallback();
                itemObj.Despawn();

            } else if (itemObj.tag == "Armor")
            {
                equipArmor(item);
                onItemChangedCallback();
                itemObj.Despawn();
            }
            else
            {
                Debug.LogError("Item incorrectly labeled as Default Item.");
            }
        }
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        onItemChangedCallback();
    }

    public Vector2 CurPots()
    {
        return new Vector2(curHpPot, curManaPot);
    }

    public void equipArmor(Item item)
    {
        // To be implemented...
    }

    #region Weapon
    public void equipWeapon(Item newWeapon)
    {
        currentWeapon = newWeapon;

        // Why is this in two places?! IDK!! I'll fix it...
        gm.playerWeapon = currentWeapon;
        onItemChangedCallback();
    }

    public void removeWeapon()
    {
        //Add the item to inventory if possible
        currentWeapon = null;

        // I know this is going to give me problems... I'll worry about adding it after I've considered null exceptions.
        // onItemChangedCallback();
    }

    public Item CurWeapon()
    {
        return currentWeapon;
    }
    #endregion

}
