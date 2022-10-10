using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private GameObject itemCursor;
    [SerializeField] private GameObject inventoryHolder;

    [SerializeField] private GameObject slotsHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;

    [SerializeField] private SlotClass[] startingItems;
    private SlotClass[] inventoryList;
    private GameObject[] slots;

    private PlayerInput input;
    private InputAction leftClick;
    private InputAction rightClick;
    private InputAction look;
    private InputAction useItem;
    private InputAction drop;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;

    [SerializeField] private bool isMovingItem;

    private void Start()
    {
        //Using new input system, these are required for references to input
        input = GetComponent<PlayerInput>();
        leftClick = input.actions["Attack"];
        rightClick = input.actions["Block"];
        look = input.actions["Position"];
        useItem = input.actions["Use"];
        drop = input.actions["InventoryDrop"];


        slots = new GameObject[slotsHolder.transform.childCount]; //Get slot amount
        inventoryList = new SlotClass[slots.Length];

        //Initialise inventory and add starting items
        for (int i = 0; i < inventoryList.Length; i++)
        {
            inventoryList[i] = new SlotClass();
        }
        for (int i = 0; i < startingItems.Length; i++)
        {
            inventoryList[i] = startingItems[i];
        }

        //Initialising slots
        for (int i = 0; i < slotsHolder.transform.childCount; i++) //set up slots
        {
            slots[i] = slotsHolder.transform.GetChild(i).gameObject;
        }
        RefreshUI(); //refres UI to match changes

        //Testing functionality works
        AddItem(itemToAdd, 1);
        RemoveItem(itemToRemove, false);
    }

    private void Update()
    {
        itemCursor.SetActive(isMovingItem); //If we are moving an item, set the holder active
        itemCursor.transform.position = look.ReadValue<Vector2>(); //Make the picked up item icon follow the cursor movement
        if (isMovingItem)//Checking if we are holding an item
        {
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon; //Set icon
            if (movingSlot.GetItem().isStackable) //Check if we can stack the item
            {
                itemCursor.GetComponentInChildren<TMP_Text>().text = movingSlot.GetQuantity() + ""; //Input quantity to the cursor following image
            } else
            {
                itemCursor.GetComponentInChildren<TMP_Text>().text = ""; //Otherwise we don't need the text, item is not stackable.
            } 
            
        }

        if (leftClick.WasPressedThisFrame()) //left click pick up whole stack
        {
            
            if (isMovingItem) //Don't start movement if we aren't moving anything, just drop it in the slot or the first available if outside inventory
            {
                EndItemMove();
            }  //end movement
            else
                BeginItemMove(); //pick up full amount of items in the slot
        }
        else if (rightClick.WasPressedThisFrame()) //right click halves stack
        {
            if (isMovingItem) //Don't start movement if we aren't moving anything, just drop one of it in the slot or the first available if outside inventory
            {
                EndItemMove_Single();
            }  //end movement
            else
                BeginItemMove_Half(); //If we don't have anything pick up half of the item in the slot
        }

        if (inventoryHolder.activeSelf) //If our inventory is active
        {
            if (drop.WasPressedThisFrame()) //And drop keybind was pressed
            {
                SlotClass tmp = new(GetClosestSlot());
                if (tmp != null) //Only remove it if there is an item
                {
                    RemoveItem(tmp.GetItem(), true); //true to remove the whole stack

                }
            }
        }
    }

    #region Inventory Utility
    /// <summary>
    /// Refreshes slots and their content
    /// </summary>
    public void RefreshUI() //Refreshing images
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try //Watch out for empty slots
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = inventoryList[i].GetItem().itemIcon;
                if (inventoryList[i].GetItem().isStackable)//Show quantity only if we can have multiple in one place.
                    slots[i].transform.GetChild(1).GetComponent<TMP_Text>().text = inventoryList[i].GetQuantity() + "";
                else
                    slots[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            } catch //Empty ones should be empty
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;

            }
        }
        
    }

    /// <summary>
    /// Add an item to the inventory list
    /// </summary>
    /// <param name="item">ItemClass item to add</param>
    /// <param name="quantity">Item's quantity</param>
    /// <returns></returns>
    public bool AddItem(ItemClass item, int quantity) //Adds an item to the list
    {
        //check if inventory already contains item
        SlotClass slot = ContainsItem(item);
        if (slot != null && slot.GetItem().isStackable) //If slot contains an item and it is stackable add the quantity.
        {
            slot.AddQuantity(quantity);
        }
        else
        {
            for (int i = 0; i < inventoryList.Length; i++)
            {
                if (inventoryList[i].GetItem() == null) //If there is nothing in the slot put item there.
                {
                    inventoryList[i] = new SlotClass(item, quantity);
                    break;
                }
            }

        }
        RefreshUI();
        return true;
    }

    /// <summary>
    /// Removes an item from the inventory list
    /// </summary>
    /// <param name="item">ItemClass</param>
    /// <param name="removeAllItems">True to delete every quantity of that item in that slot.</param>
    /// <returns></returns>
    public bool RemoveItem(ItemClass item, bool removeAllItems)
    {
        SlotClass temp = ContainsItem(item);
        if (temp != null) //Continue if there is an item to remove
        {
            if (temp.GetQuantity() > 1 && !removeAllItems)
                temp.AddQuantity(-1);
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < inventoryList.Length; i++)
                {
                    if (inventoryList[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }

                inventoryList[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }
        
        RefreshUI();
        return true;
    }
    
    /// <summary>
    /// Checks if that item exists in the list, if it does returns that item otherwise returns null
    /// </summary>
    /// <param name="item">Comparable item</param>
    /// <returns></returns>
    public SlotClass ContainsItem(ItemClass item) //Checking if slot has the same item, return that item, else it's not the same one
    {
        for (int i = 0; i < inventoryList.Length; i++)
        {
            if (inventoryList[i].GetItem() == item)
            {
                return inventoryList[i];
            }
        }
    
        return null;
    }
    #endregion

    #region Moving Items
    /// <summary>
    /// Beginning Item Movement
    /// </summary>
    /// <returns>True if successful, false if there is no item in that slot</returns>
    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot(); //Find the closest slot (clicked slot)

        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false; // We have nothing to move!
        }

        movingSlot = new SlotClass(originalSlot); //Set the held slot to be what we picked up
        originalSlot.Clear(); //We took the whole thing so nothing should be left behind
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    /// <summary>
    /// Beginning of item move with half a stack
    /// </summary>
    /// <returns>True if success, false if no item to move</returns>
    private bool BeginItemMove_Half()
    {
        originalSlot = GetClosestSlot(); //Find the closest slot (clicked slot)

        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false; // We have nothing to move!
        }

        movingSlot = new SlotClass(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity() / 2f)); //Pick up only half the stack, rounded up so we don't lose anything
        originalSlot.AddQuantity(-Mathf.CeilToInt(originalSlot.GetQuantity() / 2f)); //Remove the quantity from the original slot as we have it in our hand
        if (originalSlot.GetQuantity() == 0) // if there is nothing we should delete the item so we can't duplicate it
        {
            originalSlot.Clear();
        }
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    /// <summary>
    /// Ending the item movement and placing it into a slot, either the clicked one or the first available one
    /// </summary>
    /// <returns>True if successful, false if unsuccessful</returns>
    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null) //If the clicked slot has nothing in it we can just add the item to it
        {
            AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else //otherwise we should check if it is the same item or a different item than the picked up one
        {

            if (originalSlot.GetItem() != null) //Is there something there?
            {
                if (originalSlot.GetItem() == movingSlot.GetItem()) //Same item, can it stack?
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.Clear();
                    }
                    else // if not stackable, unsuccessful
                        return false;
                }
                else //swap items if not same
                {
                    tempSlot = new SlotClass(originalSlot); //a = b
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity()); //b = c
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity()); //c = a
                    RefreshUI();
                    return true;
                }
            }
            else //Place item normally
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }
        isMovingItem = false;
        RefreshUI();
        return true;
    }

    /// <summary>
    /// Finalizing the movement of half a stack
    /// </summary>
    /// <returns>True if successful, false if not</returns>
    private bool EndItemMove_Single()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            return false; // We have nothing to move!
        }
        if (originalSlot.GetItem() != null && originalSlot.GetItem() != movingSlot.GetItem()) //Avoid deleting other objects by checking there is an item here and it's not the same as the hand item
            return false;

        movingSlot.AddQuantity(-1);//Remove from hand
        if (originalSlot.GetItem() != null && originalSlot.GetItem() == movingSlot.GetItem()) // if slot has something in it and it matches the held item add one
        {
            originalSlot.AddQuantity(1);
        }
        else
        {
            originalSlot.AddItem(movingSlot.GetItem(), 1); //Otherwise we add it to the slot
        }

        if (movingSlot.GetQuantity() < 1) // If we put everything down, we shouldn't have anything else left!
        {
            isMovingItem = false;
            movingSlot.Clear();
        }

        RefreshUI();
        return true;
    }

    /// <summary>
    /// Used for finding the nearest clicked slot in the inventory
    /// </summary>
    /// <returns>A slot if there is one there, otherwise null.</returns>
    private SlotClass GetClosestSlot() //Finding the clicked slot
    {

        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, look.ReadValue<Vector2>()) <= 50)
            {
                return inventoryList[i];
            }
        }

        return null;
    }
    #endregion
}
