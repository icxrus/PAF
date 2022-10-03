using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnableInventory : MonoBehaviour
{
    private PlayerInput input;
    private InputAction openInventory;
    [SerializeField] private GameObject inventoryView;
    [SerializeField] private GameObject player;
    [SerializeField] private bool waitInput;
    public CinemachineFreeLook freeLook;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        openInventory = input.actions["Inventory"];
        inventoryView.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (openInventory.WasPerformedThisFrame() && !waitInput)
        {
            inventoryView.SetActive(true);
            player.GetComponent<PlayerController>().locked = true;
            Lock();
            Cursor.lockState = CursorLockMode.None;
            waitInput = true;
        }
        else if (openInventory.WasPerformedThisFrame() && waitInput)
        {
            if (inventoryView.activeSelf == true)
            {
                player.GetComponent<PlayerController>().locked = false;
                inventoryView.SetActive(false);
                Unlock();
                Cursor.lockState = CursorLockMode.Locked;
                waitInput = false;
            }
        }
    }

    private void Lock() => freeLook.enabled = false;
    private void Unlock() => freeLook.enabled = true;
}
