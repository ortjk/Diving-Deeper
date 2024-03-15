using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact();

    public delegate void Interacted();
    public event Interacted OnInteract;
    public event Interacted OnUninteract;
}
