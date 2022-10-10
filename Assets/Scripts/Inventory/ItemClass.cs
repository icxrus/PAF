using System.Collections;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item")] //Common data for every item inheriting from ItemClass
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable = true;
    public int valueInCoins = 1;

    public abstract ItemClass GetItem();
    public abstract WeaponClass GetWeapon();
    public abstract ArmorClass GetArmor();
    public abstract ConsumClass GetConsum();
    public abstract MiscClass GetMisc();

    public virtual void SetValueInCoins(int setValue)
    {
        valueInCoins = setValue;
    }
    public virtual int GetValueInCoins()
    {
        return valueInCoins;
    }
    
}
