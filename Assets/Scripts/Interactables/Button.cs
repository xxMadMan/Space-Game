using UnityEngine;
using UnityEngine.Events;

public class Button : Interactable
{
    public UnityEvent m_onPress;

    protected override void OnInteract()
    {
        m_onPress.Invoke();
    }
}
