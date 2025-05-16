using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage = "Press [E] to interact"; 

    public abstract void Interact();
}
