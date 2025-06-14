using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpen;
    private GridPosition gridPosition;
    private Animator animator;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;
    private int openString = Animator.StringToHash("IsOpen"); 
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        isOpen = false;
        Close();
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isActive = false;
            onInteractionComplete();
        }
    }
    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = .5f;

        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }

    }

    private void Close()
    {
        isOpen = false;
        animator.SetBool(openString, isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }

    private void Open()
    {
        isOpen = !isOpen;
        animator.SetBool(openString, isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, isOpen);
    }
}
