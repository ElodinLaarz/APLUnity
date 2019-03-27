using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI tmpgItemName;
    public Button removeButton;

    Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        tmpgItemName.text = item.name;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        tmpgItemName.text = "";
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }

    public void UseItem()
    {
        // Need to Implement
    }

}
