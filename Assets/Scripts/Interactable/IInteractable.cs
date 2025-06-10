using UnityEngine;
using System;

public interface IInteractable
{
    public void Interact(Action onInteractionComplete);
}
