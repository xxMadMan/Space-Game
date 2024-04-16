using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    private CharacterMovement CharacterMovement;

    private bool inCrawlSpace;

    private void Awake()
    {
        CharacterMovement = GetComponent<CharacterMovement>();
    }
    
    private void Update()
    {
        Collider[] colliders = Physics.OverlapCapsule(
            transform.position - transform.up * (CharacterMovement.defaultHeight / 2),
            transform.position + transform.up * (CharacterMovement.defaultHeight / 2),
            CharacterMovement.defaultRadius,
            Physics.AllLayers,
            QueryTriggerInteraction.Collide
        );

        inCrawlSpace = IsCrawlSpace(colliders);

        if (inCrawlSpace && CharacterMovement.movementState != CharacterMovement.MovementState.Crawlspace)
        {
            CharacterMovement.EnableCrawlSpaceMovement();
        }
        else if (!inCrawlSpace && CharacterMovement.movementState == CharacterMovement.MovementState.Crawlspace)
        {
            CharacterMovement.DisableCrawlSpaceMovement();
        }
    }

    private bool IsCrawlSpace(Collider [] colliders)
    {
        foreach (Collider col in colliders)
        {
            if (col.isTrigger && col.GetComponentInParent<CrawlSpace>())
            {
                return true;
            }
        }

        return false;
    }

    public bool InLadderRange(Ladder ladder)
    {
        Bounds bounds = ladder.GetComponentInChildren<Collider>().bounds;

        return Mathf.Abs((transform.position - bounds.center).y) < bounds.extents.y;
    }
}
