using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    public WalkingSound walkingFX;


    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float crouchSpeed = 3f;
    public float crawlSpeed = 3f;
    public float climbSpeed = 3f;

    [Header("")]

    public float defaultHeight = 2f;
    public float defaultRadius = 0.5f;
    public float crouchHeight = 1f;
    public float crouchRadius { get { return defaultRadius; } }
    public float crawlHeight = 0.3f;
    public float crawlRadius = 0.3f;

    [Header("")]

    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 90f;


    [Header("")]

    public float ladderClimbDistance = 1f;

    public enum MovementState
    {
        Walk,
        Run,
        Crouch,
        Crawlspace,
        Fall,
        Ladder,
    }

    public MovementState movementState = MovementState.Fall;

    private Vector3 lateralMovement = Vector3.zero;

    private float rotationX = 0;
    private CharacterController characterController;
    private Detection Detection;

    private Ladder ladder;

    private RaycastHit groundHit;
    public bool isGround { get; private set; }

    private bool rotateToLadder;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Detection = GetComponent<Detection>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (movementState == MovementState.Ladder && !Detection.InLadderRange(ladder))
        {
            SetMovementState(MovementState.Fall);
        }

        isGround = Physics.Raycast(transform.position, Vector3.down, out groundHit, GroundCastDistance());

        if (isGround)
        {
            if (movementState == MovementState.Fall)
            {
                SetMovementState(MovementState.Walk);
            }

            if (Input.GetKeyDown(InputMappings.Crouch))
            {
                ToggleCrouch();
            }

            if (Input.GetKeyDown(InputMappings.Run))
            {
                SetMovementState(MovementState.Run);
            }

            if (Input.GetKeyUp(InputMappings.Run))
            {
                SetMovementState(MovementState.Walk);
            }
        }
        else if (movementState != MovementState.Ladder && movementState != MovementState.Fall)
        {
            SetMovementState(MovementState.Fall);
        }

        switch (movementState)
        {
            case MovementState.Fall: Fall(); break;
            case MovementState.Ladder: ClimbLadder(); break;
            default: DoGroundMovement(); break;
        }

        //Debug.Log(movementState);

        AimCamera();

    }

    private float GroundCastDistance()
    {
        float height;

        switch (movementState)
        {
            case MovementState.Crouch: height = Mathf.Max(crouchRadius, crouchHeight / 2); break;
            case MovementState.Crawlspace: height = Mathf.Max(crawlRadius, crawlHeight / 2); break;
            default: height = Mathf.Max(defaultRadius, defaultHeight / 2); break;
        }

        return height * transform.lossyScale.y + characterController.skinWidth + characterController.stepOffset + 0.01f;
    }

    public void SetMovementState(MovementState setTo)
    {
        SetHeight(defaultHeight);
        SetRadius(defaultRadius);

        switch(setTo)
        {
            case MovementState.Crouch:
                SetHeight(crouchHeight);

                break;

            case MovementState.Crawlspace:
                movementState = MovementState.Crawlspace;

                SetHeight(crawlHeight);
                SetRadius(crawlRadius);

                break;

            case MovementState.Walk:
  
                break;

            case MovementState.Run:
                
                break;
        }

        movementState = setTo;
    }

    public bool ToggleLadderMovement(Ladder _ladder)
    {
        SetMovementState (
            movementState == MovementState.Ladder ?
            MovementState.Walk :
            MovementState.Ladder);

        if (movementState == MovementState.Ladder)
        {
            ladder = _ladder;
            rotateToLadder = true;

            return true;
        }

        return false;
    }

    private void ToggleCrouch()
    {
        SetMovementState (
        movementState == MovementState.Crouch ?
        MovementState.Walk :
        MovementState.Crouch);
    }

    public void EnableCrawlSpaceMovement()
    {
        SetMovementState(MovementState.Crawlspace);
    }

    public void DisableCrawlSpaceMovement()
    {
        SetMovementState(MovementState.Walk);
    }

    private void DoGroundMovement()
    {
        characterController.Move(GroundMoveVector());
    }
    
    private void Fall()
    {
        characterController.Move(Vector3.up * -gravity * Time.deltaTime);
    }

    private void ClimbLadder()
    {
        Transform ladderT = ladder.transform;

        float forwardOffset = Vector3.Dot(transform.position - (ladderT.position + ladderT.forward * ladderClimbDistance), ladderT.forward);
        float horizontalDistance = Vector3.Dot(transform.position - ladderT.position, ladderT.right);
        float climbingSpeed = Input.GetAxis("Vertical") * climbSpeed;

        Vector3 movement = Vector3.up * climbingSpeed;
        movement += ladderT.forward * -forwardOffset * 5f;
        movement += ladderT.right * -horizontalDistance * 5f;

        characterController.Move(movement * Time.deltaTime);

        if (rotateToLadder)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-ladderT.forward), Time.deltaTime * 5f);

            if (Vector3.Dot(transform.forward, -ladderT.forward) > 0.99f)
            {
                rotateToLadder = false;
                Debug.Log("Rotated");
            }
        }
        else
        {
            AimCamera();
        }
    }

    private Vector3 GroundMoveVector()
    { 
        float verticalSpeed = 0f;

        if(Input.GetButton("Jump"))
        {
            verticalSpeed = jumpPower;
        }
        else if ((transform.position - groundHit.point).y > defaultHeight / 2 * transform.lossyScale.y)
        {
            verticalSpeed = -gravity;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        lateralMovement = right * Input.GetAxis("Horizontal") + forward * Input.GetAxis("Vertical");

        switch (movementState)
        {
            case MovementState.Run: lateralMovement *= runSpeed; break;
            case MovementState.Crouch: lateralMovement *= crouchSpeed; break;
            case MovementState.Crawlspace: lateralMovement *= crawlSpeed; break;
            default: lateralMovement *= walkSpeed; break;
        }

        return (lateralMovement + Vector3.up * verticalSpeed) * Time.deltaTime;
    }

    private void AimCamera()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    public void SetHeight(float setTo)
    {
        characterController.height = setTo;
        GetComponent<CapsuleCollider>().height = setTo;
    }

    public void SetRadius(float setTo)
    {
        characterController.radius = setTo;
        GetComponent<CapsuleCollider>().radius = setTo;
    }
}
