using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    [Header("Inventory")]
    public GameObject inventoryGUI;
    //public Transform itemsParent;

    Inventory inventory;

    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;

        slots = inventoryGUI.GetComponentsInChildren<InventorySlot>();
        inventory.onItemChangedCallback += UpdateUI;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Inventory") && inventoryGUI.GetComponent<Animator>() != null)
        {
            bool isOpen = inventoryGUI.GetComponent<Animator>().GetBool("isOpen");
            if (isOpen)
            {
                inventoryGUI.GetComponent<Animator>().SetBool("isOpen", false);
            }
            else
            {
                inventoryGUI.GetComponent<Animator>().SetBool("isOpen", true);
            }
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
