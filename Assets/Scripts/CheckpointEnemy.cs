using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CheckpointEnemy : MonoBehaviour
{
    public enum EndBehavior
    {
        Loop,
        Reverse,
        Stop
    }

    [SerializeField] private EndBehavior endBehavior = EndBehavior.Loop;
    [SerializeField] private float moveDistance = 1f; // Distance to move on each beat

    private List<Transform> checkpoints;
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

        GetCheckpoints();

        if (checkpoints.Count == 0)
        {
            Debug.LogError("No checkpoints found for CheckpointEnemy!");
        }
        else
        {
            targetPosition = checkpoints[currentCheckpointIndex].position;
        }
    }

    private void GetCheckpoints()
    {
        Transform checkpointsParent = transform.Find("Checkpoints");
        if (checkpointsParent != null)
        {
            // Prevent the Checkpoints object from being treated as a checkpoint
            checkpoints = checkpointsParent.GetComponentsInChildren<Transform>()
                .Where(t => t != checkpointsParent)
                .ToList();
        }
        else
        {
            Debug.LogError("Checkpoints child object not found!");
            checkpoints = new List<Transform>();
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

        // Move the enemy GameObject towards the target position
        transform.GetChild(0).position = Vector3.MoveTowards(transform.GetChild(0).position, targetPosition, moveDistance);

        // Check if we've reached the current checkpoint
        if (Vector3.Distance(transform.GetChild(0).position, checkpoints[currentCheckpointIndex].position) < 0.01f)
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