using UnityEngine;
using UnityEngine.Events;

public class DoorAnimation : Button
{
    public Animator doorAnimator;
    public Sensor doorSensor;
    
    protected override void OnInteract()
    {
        OpenDoor();

        doorSensor.range = Vector3.Distance(doorSensor.transform.position, transform.position) + 1.8f;
        
        Debug.Log("door");
    }

    public void OpenDoor()
    {
        doorAnimator.SetBool("isOpen", true);
    }

    public void CloseDoor()
    {
        doorAnimator.SetBool("isOpen", false);
    }
}
