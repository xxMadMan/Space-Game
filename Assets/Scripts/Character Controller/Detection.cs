using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    private CharacterMovement CharacterMovement;

    private void Awake()
    {
        CharacterMovement = GetComponent<CharacterMovement>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<CrawlSpace>())
        {
            CharacterMovement.EnableCrawlSpaceMovement();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<CrawlSpace>())
        {
            CharacterMovement.DisableCrawlSpaceMovement();
        }
    }

    public bool InLadderRange(Ladder ladder)
    {
        Bounds bounds = ladder.GetComponentInChildren<Collider>().bounds;

        return Mathf.Abs((transform.position - bounds.center).y) < bounds.extents.y;
    }
}
