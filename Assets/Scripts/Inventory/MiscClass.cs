using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Misc Class", menuName = "Item/Misc")]
public class MiscClass : ItemClass
{
    //Data specific to misc

    public override ArmorClass GetArmor() { return null; }
    public override ConsumClass GetConsum() { return null; }
    public override ItemClass GetItem() { return this; }
    public override MiscClass GetMisc() { return this; }
    public override WeaponClass GetWeapon() { return null; }
}
