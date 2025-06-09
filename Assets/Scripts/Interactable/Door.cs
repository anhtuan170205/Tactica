using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour
{
    [SerializeField] private bool isOpen = false;
    private GridPosition gridPosition;
    private Animator animator;
    private Action onInteractComplete;
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
        LevelGrid.Instance.SetDoorAtGridPosition(gridPosition, this);
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
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
            onInteractComplete();
        }
    }
    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;
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
