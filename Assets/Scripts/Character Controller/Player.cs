using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Movement")]

    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 3f;
    public float crawlSpeed = 3f;
    public float climbSpeed = 6f;
    public float heightSpeed = 2.5f;
    //public float jumpPower = 7f;
    public float gravity = 10f;

    [Header("Camera")]

    public Camera playerCamera;

    public float lookSpeed = 2f;
    public float lookXLimit = 80f;

    [Header("Height & Radius")]

    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crawlHeight { get; private set; } = 0.01f;
    public float defaultRadius = 0.5f;
    public float crouchRadius { get { return defaultRadius; } }
    public float crawlRadius { get; private set; } = 0.01f;

    [Header("Detection")]

    public float pickupDistance = 5f;
    public float ladderClimbDistance = 1.5f;

    [Header("Interaction")]

    public float throwForce = 10f;
    public Transform holdPos;

    private CharacterController CharacterController;
    private PlayerDetection Detection;
    private PickUpController PickUpController;
    private PlayerInteraction Interaction;
    private PlayerMovement Movement;

    public float radius { get { return CharacterController.radius * transform.lossyScale.y; } }
    public float height { get { return CharacterController.height * transform.lossyScale.y; } }
    public float skinWidth { get { return CharacterController.skinWidth; } }
    public float stepOffset { get { return CharacterController.stepOffset; } }
    public Vector3 velocity { get { return CharacterController.velocity; } }

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        Detection = new PlayerDetection();
        PickUpController = new PickUpController();
        Interaction = new PlayerInteraction();
        Movement = new PlayerMovement();

        Detection.Initialize(this, Movement);
        PickUpController.Initialize(this);
        Interaction.Initialize(this, PickUpController, Movement);
        Movement.Initialize(this, Detection, CharacterController);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (Sensor s in FindObjectsOfType<Sensor>())
        {
            if (s.target == null)
            {
                s.target = gameObject;
            }
        }
    }

    void Update()
    {
        Detection.HandleDetection();
        PickUpController.HandlePickups();
        Interaction.HandleInteractions();
        Movement.HandleMovement();
    }

    public void SetHeight(float setTo)
    {
        CharacterController.height = setTo;
    }

    public void SetRadius(float setTo)
    {
        CharacterController.radius = setTo;
    }
}
