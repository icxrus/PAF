using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon Class", menuName = "Item/Weapon")]
public class WeaponClass : ItemClass
{
    [Header("Weapon")]
    //Data specific to Weapons
    public WeaponType weaponType;
    public int itemLevel = 1;

    public enum WeaponType
    {
        GreatStaff,
        Daggers
    }

    public override ArmorClass GetArmor() { return null; }
    public override ConsumClass GetConsum() { return null; }
    public override ItemClass GetItem() { return this; }
    public override MiscClass GetMisc() {  return null; }
    public override WeaponClass GetWeapon() { return this; }

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
