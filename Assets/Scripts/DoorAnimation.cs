using UnityEngine;
using UnityEngine.Events;

public class DoorAnimation : MonoBehaviour
{
    public Animator doorAnimator;
    public Sensor doorSensor;

    public void OpenDoor()
    {
        doorAnimator.SetBool("isOpen", true);
    }

    public void CloseDoor()
    {
        doorAnimator.SetBool("isOpen", false);
    }

    public void CalibrateSensorRange()
    {
        doorSensor.range = Vector3.Distance(doorSensor.transform.position, transform.position) + 1.8f;
    }
}
