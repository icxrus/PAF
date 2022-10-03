using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Armor Class", menuName = "Item/Armor")]
public class ArmorClass : ItemClass
{
    [Header("Armor")]
    //Data Specific to Armors
    public ArmorSlot armorSlot;
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
}
