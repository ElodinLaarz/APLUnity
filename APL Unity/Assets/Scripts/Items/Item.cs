using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    // Overwrite the Name Property
    new public string name = "";
    public Sprite icon = null;

    public bool isDefaultItem = false;
    [Header("Weapon Stats")]
    #region WeaponStats
    public int damage = 0;
    public float attackSpeed = 0f;
    public float secondaryCooldown = 0f;
    public int numSecondaryProj = 0;

    #endregion

    [Header("Armor Stats")]
    #region ArmorStats
    public int armor = 0;
    public int speedBonus = 0;
    #endregion

}
