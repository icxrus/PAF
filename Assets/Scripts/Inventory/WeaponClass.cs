using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon Class", menuName = "Item/Weapon")]
public class WeaponClass : ItemClass
{
    [Header("Weapon")]
    //Data specific to Weapons
    public WeaponType weaponType;

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
}
