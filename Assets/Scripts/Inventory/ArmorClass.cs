using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Armor Class", menuName = "Item/Armor")]
public class ArmorClass : ItemClass
{
    [Header("Armor")]
    //Data Specific to Armors
    public ArmorSlot armorSlot;
    public int itemLevel = 1;
    public enum ArmorSlot
    {
        Head,
        Body,
        Bottom
    }
    public override ArmorClass GetArmor() { return this; }
    public override ConsumClass GetConsum() { return null; }
    public override ItemClass GetItem() { return this; }
    public override MiscClass GetMisc() { return null; }
    public override WeaponClass GetWeapon() { return null; }

    public override void SetValueInCoins(int setValue)
    {
        valueInCoins *= (itemLevel * 7);
    }

    public void SetLevel(int level)
    {
        itemLevel += level;
        SetValueInCoins(1);
    }
    public int GetLevel()
    {
        return itemLevel;
    }

    public void RandomizeLevel()
    {
        //Get player level and randomize +- 2 levels
    }
}
