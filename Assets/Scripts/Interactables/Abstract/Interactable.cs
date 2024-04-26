using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour
{
    protected Player player;

    public void InteractObject(Player _player)
    {
        player = _player;
        OnInteract();
    }

    protected abstract void OnInteract();

    public void EnableOutline()
    {
        var outlineOBJ = GetComponent<Outline>();

        outlineOBJ.enabled = true;
        outlineOBJ.OutlineTimer(0.18f);
    }
}
