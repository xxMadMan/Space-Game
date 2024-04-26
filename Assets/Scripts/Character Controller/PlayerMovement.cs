using System;
using UnityEngine;

public class PlayerMovement 
{
    private Player Player;
    private PlayerDetection PlayerDetection;
    private CharacterController CharacterController;

    private Transform transform;

    public enum MovementState
    {
        Walk,
        Run,
        Crouch,
        Crawlspace,
        Fall,
        Ladder,
    }

    public MovementState movementState { get; private set; } = MovementState.Fall;

    private Vector3 lateralMovement = Vector3.zero;
    private float rotationX = 0;

    private Ladder ladder;
    private bool rotateToLadder;

    public void Initialize(Player _Player, PlayerDetection _PlayerDetection, CharacterController _CharacterController)
    {
        Player = _Player;
        PlayerDetection = _PlayerDetection;
        CharacterController = _CharacterController;

        transform = Player.transform;
    }

    public void HandleMovement()
    {
        if (movementState == MovementState.Ladder && !PlayerDetection.InLadderRange(ladder))
        {
            SetMovementState(MovementState.Fall);
        }

        if (PlayerDetection.isGround)
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

    public void SetMovementState(MovementState setTo)
    {
        if (movementState == MovementState.Crawlspace &&
            PlayerDetection.inCrawlSpace)
        {
            return;
        }

        Player.SetHeight(Player.defaultHeight);
        Player.SetRadius(Player.defaultRadius);

        switch(setTo)
        {
            case MovementState.Crouch:
                Player.SetHeight(Player.crouchHeight);

                break;

            case MovementState.Crawlspace:
                movementState = MovementState.Crawlspace;

                Player.SetHeight(Player.crawlHeight);
                Player.SetRadius(Player.crawlRadius);

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
        CharacterController.Move(GroundMoveVector());
    }
    
    private void Fall()
    {
        CharacterController.Move(Vector3.up * -Player.gravity * Time.deltaTime);
    }

    private void ClimbLadder()
    {
        Transform ladderT = ladder.transform;

        float forwardOffset = Vector3.Dot(
            transform.position - (ladderT.position + ladderT.forward * Player.ladderClimbDistance),
            ladderT.forward);

        float horizontalDistance = Vector3.Dot(
            transform.position - ladderT.position,
            ladderT.right);
            
        float climbingSpeed = Input.GetAxis("Vertical") * Player.climbSpeed;

        Vector3 movement = Vector3.up * climbingSpeed;
        movement += ladderT.forward * -forwardOffset * 5f;
        movement += ladderT.right * -horizontalDistance * 5f;

        CharacterController.Move(movement * Time.deltaTime);

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

/*
        if(Input.GetButton("Jump"))
        {
            verticalSpeed = jumpPower;
        }
        else
*/
        {
            verticalSpeed = PlayerDetection.groundClampDistance * Player.heightSpeed;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        lateralMovement = right * Input.GetAxis("Horizontal") + forward * Input.GetAxis("Vertical");

        switch (movementState)
        {
            case MovementState.Run: lateralMovement *= Player.runSpeed; break;
            case MovementState.Crouch: lateralMovement *= Player.crouchSpeed; break;
            case MovementState.Crawlspace: lateralMovement *= Player.crawlSpeed; break;
            default: lateralMovement *= Player.walkSpeed; break;
        }

        return (lateralMovement + Vector3.up * verticalSpeed) * Time.deltaTime;
    }

    private void AimCamera()
    {
        rotationX += -Input.GetAxis("Mouse Y") * Player.lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -Player.lookXLimit, Player.lookXLimit);
        Player.playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * Player.lookSpeed, 0);
    }
}
