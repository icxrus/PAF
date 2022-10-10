using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class EnableMenu : MonoBehaviour
{
    private PlayerInput input;
    private InputAction openInventory;
    private InputAction pauseInput;

    [SerializeField] private GameObject inventoryView;
    [SerializeField] private GameObject pauseMenuView;
    [SerializeField] private GameObject settingsMenuView;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject player;
    [SerializeField] private bool isMenuOpen;

    public CinemachineFreeLook freeLook;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        openInventory = input.actions["Inventory"];
        pauseInput = input.actions["Pause"];
        inventoryView.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (openInventory.WasPerformedThisFrame() && !isMenuOpen) //Open inventory if nothing is already open
        {
            //set up view correctly
            inventoryView.SetActive(true);
            buttons.SetActive(true);
            //Lock player and camera
            player.GetComponent<PlayerController>().locked = true;
            Lock();
            Cursor.lockState = CursorLockMode.None;
            isMenuOpen = true;

            //Set up buttons correctly
            buttons.transform.GetChild(0).GetComponent<Button>().interactable = true;
            buttons.transform.GetChild(1).GetComponent<Button>().interactable = true;
            buttons.transform.GetChild(2).GetComponent<Button>().interactable = false;
        }
        else if (pauseInput.WasPerformedThisFrame() && !isMenuOpen) //open pause menu if nothing is open
        {
            //set up view correctly
            pauseMenuView.SetActive(true);
            buttons.SetActive(true);
            //Lock player and camera
            player.GetComponent<PlayerController>().locked = true;
            Lock();
            Cursor.lockState = CursorLockMode.None;
            isMenuOpen = true;

            //Set up buttons correctly
            buttons.transform.GetChild(0).GetComponent<Button>().interactable = false;
            buttons.transform.GetChild(1).GetComponent<Button>().interactable = true;
            buttons.transform.GetChild(2).GetComponent<Button>().interactable = true;
        }
        else if (isMenuOpen && openInventory.WasPerformedThisFrame() || pauseInput.WasPerformedThisFrame() ) //Close it if it is open
        {
            if (inventoryView.activeSelf == true || pauseMenuView.activeSelf == true || settingsMenuView.activeSelf == true) //Check that some menu tab is open
            {
                player.GetComponent<PlayerController>().locked = false; // Free player
                //Hide all panels
                inventoryView.SetActive(false);
                pauseMenuView.SetActive(false);
                settingsMenuView.SetActive(false);
                buttons.SetActive(false); //Hide buttons
                Unlock(); //free camera
                Cursor.lockState = CursorLockMode.Locked;
                isMenuOpen = false;

                //Reset buttons
                buttons.transform.GetChild(0).GetComponent<Button>().interactable = true;
                buttons.transform.GetChild(1).GetComponent<Button>().interactable = true;
                buttons.transform.GetChild(2).GetComponent<Button>().interactable = true;
            }
        }
        
    }

    //Lock the camera and unlock it so it won't move when moving in the inventory
    private void Lock() => freeLook.enabled = false;
    private void Unlock() => freeLook.enabled = true;

    /// <summary>
    /// Switches menu view to Inventory
    /// </summary>
    public void SwitchToInventory()
    {
        //activate correct menu Panel
        inventoryView.SetActive(true);
        pauseMenuView.SetActive(false);
        settingsMenuView.SetActive(false);

        //Set up buttons correctly
        buttons.transform.GetChild(0).GetComponent<Button>().interactable = true;
        buttons.transform.GetChild(1).GetComponent<Button>().interactable = true;
        buttons.transform.GetChild(2).GetComponent<Button>().interactable = false;

        //Lock player and camera
        player.GetComponent<PlayerController>().locked = true;
        Lock();
        Cursor.lockState = CursorLockMode.None;
        isMenuOpen = true;
    }
    /// <summary>
    /// Switches Menu view to pause menu
    /// </summary>
    public void SwitchToPauseMenu()
    {
        //activate correct menu Panel
        inventoryView.SetActive(false);
        pauseMenuView.SetActive(true);
        settingsMenuView.SetActive(false);

        //Set up buttons correctly
        buttons.transform.GetChild(0).GetComponent<Button>().interactable = false;
        buttons.transform.GetChild(1).GetComponent<Button>().interactable = true;
        buttons.transform.GetChild(2).GetComponent<Button>().interactable = true;

        //Lock player and camera
        player.GetComponent<PlayerController>().locked = true;
        Lock();
        Cursor.lockState = CursorLockMode.None;
        isMenuOpen = true;
    }
    /// <summary>
    /// Switches view to settings menu
    /// </summary>
    public void SwitchToSettings()
    {
        //activate correct menu Panel
        inventoryView.SetActive(false);
        pauseMenuView.SetActive(false);
        settingsMenuView.SetActive(true);

        //Set up buttons correctly
        buttons.transform.GetChild(0).GetComponent<Button>().interactable = true;
        buttons.transform.GetChild(1).GetComponent<Button>().interactable = false;
        buttons.transform.GetChild(2).GetComponent<Button>().interactable = true;

        //Lock player and camera
        player.GetComponent<PlayerController>().locked = true;
        Lock();
        Cursor.lockState = CursorLockMode.None;
        isMenuOpen = true;
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Returns to main menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        //Return to main menu scene
    }
}
