using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private GameObject itemCursor;

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

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;

    [SerializeField] private bool isMovingItem;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        leftClick = input.actions["Attack"];
        rightClick = input.actions["Block"];
        look = input.actions["Position"];
        useItem = input.actions["Use"];


        slots = new GameObject[slotsHolder.transform.childCount]; //Get slot amount
        inventoryList = new SlotClass[slots.Length];


        for (int i = 0; i < inventoryList.Length; i++)
        {
            inventoryList[i] = new SlotClass();
        }
        for (int i = 0; i < startingItems.Length; i++)
        {
            inventoryList[i] = startingItems[i];
        }

        for (int i = 0; i < slotsHolder.transform.childCount; i++) //set up slots
        {
            slots[i] = slotsHolder.transform.GetChild(i).gameObject;
        }
        RefreshUI(); //refres UI to match changes

        AddItem(itemToAdd, 1);
        RemoveItem(itemToRemove);
    }

    private void Update()
    {
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = look.ReadValue<Vector2>();
        if (isMovingItem)
        {
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
            if (movingSlot.GetItem().isStackable)
            {
                itemCursor.GetComponentInChildren<TMP_Text>().text = movingSlot.GetQuantity() + "";
            } else
            {
                itemCursor.GetComponentInChildren<TMP_Text>().text = "";
            } 
            
        }

        if (leftClick.WasPressedThisFrame()) //left click
        {
            
            if (isMovingItem)
            {
                EndItemMove();
            }  //end movement
            else
                BeginItemMove();
        }
        else if (rightClick.WasPressedThisFrame()) //right click
        {
            if (isMovingItem)
            {
                EndItemMove_Single();
            }  //end movement
            else
                BeginItemMove_Half();
        }
    }

    #region Inventory Utility
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


    public bool AddItem(ItemClass item, int quantity) //Adds an item to the list
    {
        //check if inventory already contains item
        

        SlotClass slot = ContainsItem(item);
        if (slot != null && slot.GetItem().isStackable)
        {
            slot.AddQuantity(quantity);
        }
        else
        {
            for (int i = 0; i < inventoryList.Length; i++)
            {
                if (inventoryList[i].GetItem() == null)
                {
                    inventoryList[i] = new SlotClass(item, quantity);
                    break;
                }
            }

        }
        RefreshUI();
        return true;
    }

    public bool RemoveItem(ItemClass item) //Removes an item from the list
    {
        SlotClass temp = ContainsItem(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
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
    
    public SlotClass ContainsItem(ItemClass item)
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
    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot(); //Find the closest slot (clicked slot)

        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false; // We have nothing to move!
        }

        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();
        return true;
    }
    private bool BeginItemMove_Half()
    {
        originalSlot = GetClosestSlot(); //Find the closest slot (clicked slot)

        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false; // We have nothing to move!
        }

        movingSlot = new SlotClass(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity() / 2f)); //Pick up only half the stack, rounded up so we don't lose anything
        originalSlot.AddQuantity(-Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));
        if (originalSlot.GetQuantity() == 0)
        {
            originalSlot.Clear();
        }
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {

            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem() == movingSlot.GetItem()) //Same item, can it stack?
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.Clear();
                    }
                    else // if not, unsuccessful
                        return false;
                }
                else //swap
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

        if (movingSlot.GetQuantity() < 1)
        {
            isMovingItem = false;
            movingSlot.Clear();
        }
        

        RefreshUI();
        return true;
    }

    private SlotClass GetClosestSlot()
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
