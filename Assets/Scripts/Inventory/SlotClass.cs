using System.Collections;
using UnityEngine;

[System.Serializable]
public class SlotClass
{
    [SerializeField] private ItemClass item;
    [SerializeField] private int quantity;

    //Empty constructor
    public SlotClass()
    {
        item = null;
        quantity = 0;
    }

    //Constructor that takes in a slotclass item
    public SlotClass(SlotClass slot)
    {
        item = slot.item;
        quantity = slot.quantity;
    }

    //constructor that takes in an item class and it's quantity 
    public SlotClass ( ItemClass _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;
    }

    /// <summary>
    /// Returns the ItemClass item that SlotClass holds
    /// </summary>
    /// <returns></returns>
    public ItemClass GetItem() { return item; }
    /// <summary>
    /// Returns the quantity that the slot holds in itself
    /// </summary>
    /// <returns></returns>
    public int GetQuantity() { return quantity; }
    /// <summary>
    /// Adds given quantity to the slot overall quantity
    /// </summary>
    /// <param name="_quantity">Give a quantity to add</param>
    public void AddQuantity(int _quantity) { quantity += _quantity; }
    /// <summary>
    /// Adds an item to the slot
    /// </summary>
    /// <param name="_item">ItemClass</param>
    /// <param name="_quantity">Int</param>
    public void AddItem(ItemClass _item, int _quantity)
    {
        this.item = _item;
        this.quantity = _quantity;
    }
    /// <summary>
    /// Empties out the slot
    /// </summary>
    public void Clear()
    {
        this.item = null;
        this.quantity = 0;
    }
}
