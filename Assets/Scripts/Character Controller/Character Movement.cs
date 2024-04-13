using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float climbSpeed = 3f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 90f;
    public float defaultHeight = 2f;
    public float defaultRadius = 0.5f;
    public float crouchHeight = 1f;
    public float crawlHeight = 0.01f;
    public float crawlRadius = 0.01f;
    public float crouchSpeed = 3f;
    public float crawlSpeed = 3f;

    [Header("")]

    public float ladderClimbDistance = 1f;

    public enum MovementState
    {
        Ground,
        Crawlspace,
        Ladder,
    }

    private enum GroundMovementState
    {
        Crouch,
        Walk,
        Run
    }

    public MovementState movementState = MovementState.Ground;
    private GroundMovementState groundMovementState = GroundMovementState.Walk;

    private Vector3 lateralMovement = Vector3.zero;
    private float verticalSpeed = 0f;

    private float rotationX = 0;
    private CharacterController characterController;

    private Transform ladder;

    private bool rotateToLadder;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        ResetCharacterController();

        switch(movementState)
        {
            case MovementState.Ground: GroundMovement(); break;
            case MovementState.Ladder: LadderMovement(); break;
            case MovementState.Crawlspace: CrawlSpaceMovement(); break;
            default: characterController.Move(Vector3.zero); break;
        }
    }

    private void ResetCharacterController()
    {
        characterController.height = defaultHeight;
        characterController.radius = defaultRadius;
    }

    private void GroundMovement()
    { 
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        lateralMovement = right * Input.GetAxis("Horizontal") + forward * Input.GetAxis("Vertical");

        groundMovementState = SelectGroundMovementState();

        if (groundMovementState == GroundMovementState.Crouch) characterController.height = crouchHeight;

        switch (groundMovementState)
        {
            case GroundMovementState.Crouch: lateralMovement *= crouchSpeed; break;
            case GroundMovementState.Walk: lateralMovement *= walkSpeed; break;
            case GroundMovementState.Run: lateralMovement *= runSpeed; break;
        }

        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            verticalSpeed = jumpPower;
        }

        if (!characterController.isGrounded)
        {
            verticalSpeed -= gravity * Time.deltaTime;
        }

        characterController.Move((lateralMovement + Vector3.up * verticalSpeed) * Time.deltaTime);

        AimCamera();
    }

    private GroundMovementState SelectGroundMovementState()
    {
        if (Input.GetKey(InputMappings.Run))
        {
            return GroundMovementState.Run;
        }
        else if (Input.GetKey(InputMappings.Crouch))
        {
            return GroundMovementState.Crouch;
        }
        else
        {
            return GroundMovementState.Walk;
        }
    }

    public bool ToggleLadderMovement(Transform _ladder)
    {
        movementState = (
            movementState == MovementState.Ladder ?
            MovementState.Ground :
            MovementState.Ladder);

        if (movementState == MovementState.Ladder)
        {
            ladder = _ladder;
            rotateToLadder = true;

            return true;
        }

        return false;
    }

    public void EnableCrawlSpaceMovement()
    {
        movementState = MovementState.Crawlspace;
    }

    public void DisableCrawlSpaceMovement()
    {
        movementState = MovementState.Ground;
    }

    private void LadderMovement()
    {
        float forwardOffset = Vector3.Dot(transform.position - (ladder.position + ladder.forward * ladderClimbDistance), ladder.forward);
        float horizontalDistance = Vector3.Dot(transform.position - ladder.position, ladder.right);
        float climbingSpeed = Input.GetAxis("Vertical") * climbSpeed;

        Vector3 movement = Vector3.up * climbingSpeed;
        movement += ladder.forward * -forwardOffset * 5f;
        movement += ladder.right * -horizontalDistance * 5f;

        characterController.Move(movement * Time.deltaTime);

        if (rotateToLadder)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-ladder.forward), Time.deltaTime * 5f);

            if (Vector3.Dot(transform.forward, -ladder.forward) > 0.99f)
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

    private void CrawlSpaceMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        lateralMovement = right * Input.GetAxis("Horizontal") + forward * Input.GetAxis("Vertical");

        groundMovementState = SelectGroundMovementState();

        characterController.height = crawlHeight;
        characterController.radius = crawlRadius;

        lateralMovement *= crawlSpeed;

        characterController.Move((lateralMovement + Vector3.up * verticalSpeed) * Time.deltaTime);

        AimCamera();
    }

    private void AimCamera()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
