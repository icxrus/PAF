using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    //animation controllers
    [SerializeField]
    private RuntimeAnimatorController[] animatorControllers;
    private RuntimeAnimatorController baseController;
    private RuntimeAnimatorController runController;
    private RuntimeAnimatorController crouchController;

    //General required connections
    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField]
    private bool groundedPlayer;
    private PlayerInput playerInput;
    private Transform cameraTransform;
    private Animator animator;

    //Variables for statemachine
    [SerializeField]
    bool runHoldDown = false;
    [SerializeField]
    bool crouchHoldDown = false;
    [SerializeField]
    private bool CanRun = true;
    [SerializeField]
    private bool CanCrouch = true;
    public bool locked = false;
    [SerializeField]
    private bool autoRunning = false;

    //Movement vectors
    [SerializeField]
    public float playerSpeed = 5.0f;
    [SerializeField]
    private float walkSpeed = 5.0f;
    [SerializeField]
    private float runSpeed = 15.0f;
    [SerializeField]
    private float crouchSpeed = 3.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 5f;
    [SerializeField]
    private float animationSmoothTime = 0.15f;
    [SerializeField]
    private float animationPlayTransition = 0.15F;

    //Input
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction crouchAction;
    private InputAction autoRunAction;
    private InputAction attackAction;

    //animations
    int speedXAnimationParameterID;
    int speedYAnimationParameterID;
    int jumpAnimation;

    [SerializeField]
    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;

    public ParticleObjectData projectile;
    public float distance;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        runAction = playerInput.actions["Run"];
        crouchAction = playerInput.actions["Crouch"];
        autoRunAction = playerInput.actions["AutoRun"];
        attackAction = playerInput.actions["Attack"];

        animator = GetComponent<Animator>();
        speedXAnimationParameterID = Animator.StringToHash("MoveX");
        speedYAnimationParameterID = Animator.StringToHash("MoveZ");
        jumpAnimation = Animator.StringToHash("JumpStart");

        baseController = animatorControllers[0];
        runController = animatorControllers[1];
        crouchController = animatorControllers[2];
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Move to game controller when pause and saving implemented

        animator.runtimeAnimatorController = baseController;
        groundedPlayer = true;
    }


    void Update()
    {
        if (!locked)
        {
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            GroundCheck();

            runAction.performed += _ => runHoldDown = true; //Checking for holding down sprint button
            runAction.canceled += _ => runHoldDown = false;

            crouchAction.performed += _ => crouchHoldDown = true; //Checking for holding down crouch button
            crouchAction.canceled += _ => crouchHoldDown = false;

            //Lock run and crouch depending on current action
            if (CanRun)
            {
                if (runHoldDown)
                {
                    animator.runtimeAnimatorController = runController;
                    if (attackAction.triggered)
                    {
                        StartAttack();
                    }
                    else
                    {
                        playerSpeed = runSpeed;
                        CanCrouch = false;
                    }

                }
                else if (!runHoldDown)
                {
                    if (!crouchHoldDown)
                    {
                        animator.runtimeAnimatorController = baseController;
                    }

                    if (attackAction.triggered)
                    {
                        StartAttack();
                    }
                    else
                    {
                        playerSpeed = walkSpeed;
                        CanCrouch = true;
                    }
                }
            }
            if (CanCrouch)
            {
                if (crouchHoldDown)
                {
                    animator.runtimeAnimatorController = crouchController;
                    if (attackAction.triggered)
                    {
                        StartAttack();
                    }
                    else
                    {
                        playerSpeed = crouchSpeed;
                        CanRun = false;
                    }
                }
                else if (!crouchHoldDown)
                {
                    if (!runHoldDown)
                    {
                        animator.runtimeAnimatorController = baseController;
                    }

                    if (attackAction.triggered)
                    {
                        StartAttack();
                    }
                    else
                    {
                        playerSpeed = walkSpeed;
                        CanRun = true;
                    }
                }
            }

            Vector2 input = moveAction.ReadValue<Vector2>();
            currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime);
            Vector3 _lastInput = input;
            Vector3 move = new();
            if (autoRunAction.WasPressedThisFrame() || autoRunning) //Check if we should start autorunning or if autorun is already on
            {
                if (!_lastInput.Equals(Vector2.zero)) //If we are running check for move buttons to be pressed to stop running
                {
                    autoRunning = false;
                    playerSpeed = walkSpeed;
                }
                else //Otherwise we keep running
                {
                    autoRunning = true;
                }
                playerSpeed = runSpeed;
                currentAnimationBlendVector.x = 0;
                currentAnimationBlendVector.y = 1;
                move = new(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);
                move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
                move.y = 0f;
                controller.Move(playerSpeed * Time.deltaTime * move);
                animator.runtimeAnimatorController = runController;
                animator.SetFloat(speedXAnimationParameterID, currentAnimationBlendVector.x);
                animator.SetFloat(speedYAnimationParameterID, currentAnimationBlendVector.y);
            }
            else if (!autoRunAction.WasPressedThisFrame() && !autoRunning) //If we are not autorunning, we should use input to move character
            {
                move = new(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);
                move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
                move.y = 0f;

                controller.Move(playerSpeed * Time.deltaTime * move);

                animator.SetFloat(speedXAnimationParameterID, currentAnimationBlendVector.x);
                animator.SetFloat(speedYAnimationParameterID, currentAnimationBlendVector.y);
            }

            // Changes the height position of the player (jump)
            if (jumpAction.triggered && groundedPlayer)
            {
                move.y = 0f;
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                if (!runHoldDown)
                {
                    animator.Play(jumpAnimation);
                }
                else
                {
                    animator.Play(jumpAnimation);
                }
            }

            //Jump Control
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

            //groundedPlayer = controller.isGrounded;
            if (groundedPlayer && !jumpAction.triggered)
            {
                controller.Move(-Vector3.up * distance);

            }

            if (groundedPlayer)
            {
                animator.SetBool("InAir", false);
            }
            else
            {
                animator.SetBool("InAir", true);
            }

            // Rotate towards camera direction when moving
            if (_lastInput.sqrMagnitude == 0) return;
            float targetAngle = cameraTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetFloat(speedXAnimationParameterID, 0);
            animator.SetFloat(speedYAnimationParameterID, 0);
        }
            
    }

    private void StartAttack() //Basic attack control
    {
        animator.runtimeAnimatorController = baseController;
        animator.Play("Greatstaff Attack");
        var ray = Camera.main.ScreenPointToRay(new Vector2(Screen.height / 2, Screen.width / 2));
        RaycastHit hitPoint;
        Vector3 spawnPosition;
        if (Physics.Raycast(ray, out hitPoint, 100f)) // check hit
        {
            Debug.DrawRay(ray.direction, hitPoint.transform.position);
            if(hitPoint.transform.CompareTag("Enemy"))
            {
                // do damge to enemy
            }
        }
        spawnPosition = ray.direction + cameraTransform.position;
        Instantiate(projectile.prefab, spawnPosition, Quaternion.Euler(cameraTransform.eulerAngles.x, 0, cameraTransform.eulerAngles.z));
        

    }

    void GroundCheck()
    {
        RaycastHit hit;
        
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            groundedPlayer = true;
        }
        else
        {
            groundedPlayer = false;
        }
    }

} //Base of code by Samyam's tutorials on YouTube - Extra functionality self-made
