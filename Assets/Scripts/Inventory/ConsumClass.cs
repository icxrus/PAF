using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Consumable Class", menuName = "Item/Consumable")]
public class ConsumClass : ItemClass
{
    [Header("Consumable")]
    //Data Specific to Consumables
    public float recoveryAmount;

    public override ArmorClass GetArmor() { return null; }
    public override ConsumClass GetConsum() { return this; }
    public override ItemClass GetItem() { return this; }
    public override MiscClass GetMisc() { return null; }
    public override WeaponClass GetWeapon() { return null; }
}
