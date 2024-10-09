using UnityEngine;
using System.Collections.Generic;

public class CheckpointEnemy : MonoBehaviour
{
    public enum EndBehavior
    {
        Loop,
        Reverse,
        Stop
    }

    [SerializeField] private List<Transform> checkpoints;
    [SerializeField] private EndBehavior endBehavior = EndBehavior.Loop;
    [SerializeField] private float moveDistance = 1f; // Distance to move on each beat

    private int currentCheckpointIndex = 0;
    private bool movingForward = true;
    private Vector3 targetPosition;
    private bool isMoving = true;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBeat += Move;
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }

        if (checkpoints.Count == 0)
        {
            Debug.LogError("No checkpoints assigned to CheckpointEnemy!");
        }
        else
        {
            targetPosition = checkpoints[currentCheckpointIndex].position;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBeat -= Move;
        }
    }

    private void Move()
    {
        if (checkpoints.Count == 0 || !isMoving) return;

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveDistance);

        // Check if we've reached the current checkpoint
        if (Vector3.Distance(transform.position, checkpoints[currentCheckpointIndex].position) < 0.01f)
        {
            // Move to the next checkpoint
            if (movingForward)
            {
                currentCheckpointIndex++;
                if (currentCheckpointIndex >= checkpoints.Count)
                {
                    HandleEndBehavior();
                }
            }
            else
            {
                currentCheckpointIndex--;
                if (currentCheckpointIndex < 0)
                {
                    HandleEndBehavior();
                }
            }

            // Set the new target position if still moving
            if (isMoving)
            {
                targetPosition = checkpoints[currentCheckpointIndex].position;
            }
        }
    }

    private void HandleEndBehavior()
    {
        switch (endBehavior)
        {
            case EndBehavior.Loop:
                if (movingForward)
                {
                    currentCheckpointIndex = 0;
                }
                else
                {
                    currentCheckpointIndex = checkpoints.Count - 1;
                }
                break;

            case EndBehavior.Reverse:
                movingForward = !movingForward;
                currentCheckpointIndex = movingForward ? 1 : checkpoints.Count - 2;
                break;

            case EndBehavior.Stop:
                isMoving = false;
                currentCheckpointIndex = movingForward ? checkpoints.Count - 1 : 0;
                break;
        }
    }
}