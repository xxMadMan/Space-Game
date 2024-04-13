using UnityEngine;
using UnityEngine.Events;

public class Sensor : MonoBehaviour
{
    public GameObject target;
    
    public float range = 4f;

    private bool inRange = false;

    public UnityEvent m_onInRange;

    public UnityEvent m_onOutOfRange;

    private void Update()
    {
        if (!inRange && SenseTarget())
        {
            inRange = true;
            m_onInRange.Invoke();
        }
        if (inRange && !SenseTarget())
        {
            inRange = false;
            m_onOutOfRange.Invoke();
        }
    }

    public bool SenseTarget()
    {
        return Vector3.Distance(transform.position, target.transform.position) < range;
    }
}
