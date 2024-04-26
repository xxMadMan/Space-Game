using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovementState = PlayerMovement.MovementState;

public class PlayerDetection
{
    private Player Player;
    private PlayerMovement PlayerMovement;

    private Transform transform;

    public RaycastHit groundHit { get; private set; }
    public bool isGround { get; private set; }
    public float groundClampDistance
    {
        get
        {
            if (groundHit.transform == null)
            {
                return 0f;
            }
            
            return
                groundHit.point.y -
                (transform.position.y - (Mathf.Max(Player.radius, Player.height) / 2f + Player.skinWidth));
        }
    }

    public bool inCrawlSpace { get; private set; }

    public void Initialize(Player _Player, PlayerMovement _PlayerMovement)
    {
        Player = _Player;
        PlayerMovement = _PlayerMovement;

        transform = Player.transform;
    }
    
    public void HandleDetection()
    {
        isGround = CastGround(out RaycastHit hit);
        groundHit = hit;

        inCrawlSpace = IsCrawlSpace();

        if (PlayerMovement.movementState != MovementState.Crawlspace && inCrawlSpace)
        {
            PlayerMovement.EnableCrawlSpaceMovement();
        }
        else if (PlayerMovement.movementState == MovementState.Crawlspace && !inCrawlSpace)
        {
            PlayerMovement.DisableCrawlSpaceMovement();
        }
    }

    private bool IsCrawlSpace()
    {
        Collider[] colliders = Physics.OverlapCapsule(
            transform.position - transform.up * (Player.defaultHeight / 2),
            transform.position + transform.up * (Player.defaultHeight / 2),
            CrawlSpaceDetectionRadius(),
            Physics.AllLayers,
            QueryTriggerInteraction.Collide
        );

        foreach (Collider col in colliders)
        {
            if (col.GetComponent<CrawlSpace>())
            {
                col.isTrigger = true;
                return true;
            }
        }

        return false;
    }

    private float CrawlSpaceDetectionRadius()
    {
        float radiusScaled = Player.defaultRadius * Mathf.Max(Player.transform.lossyScale.x, Player.transform.lossyScale.z);
        float multiplier = 2f; // inCrawlSpace ? 2f : 2.5f;

        return (radiusScaled + 0.5f) * multiplier + Player.skinWidth;
    }

    public bool InLadderRange(Ladder ladder)
    {
        Bounds bounds = ladder.GetComponentInChildren<Collider>().bounds;

        return Mathf.Abs((transform.position - bounds.center).y) < bounds.extents.y;
    }

    public bool CastGround(out RaycastHit hit)
    {
        float radius = GroundCastRadius();
        float distance = GroundCastDistance();

        bool cast = Physics.SphereCast(
            transform.position + Vector3.up * radius, radius, Vector3.down, out hit, distance);

        return cast;
    }

    private float GroundCastRadius()
    {
        return Player.defaultRadius * Mathf.Max(Player.transform.lossyScale.x, Player.transform.lossyScale.z);
    }

    private float GroundCastDistance()
    {
        return
            Mathf.Max(Player.defaultRadius, Player.defaultHeight) * Player.transform.lossyScale.y / 2 +
            Player.skinWidth +
            0.01f;
    }
}
