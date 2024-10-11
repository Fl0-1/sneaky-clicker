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
    [SerializeField] private GameObject detectionArea;
    [SerializeField] private Sprite enemyTopSprite;
    [SerializeField] private Sprite enemyBottomSprite;
    [SerializeField] private Sprite enemyRightSprite;

    private List<Transform> checkpoints;
    private int currentCheckpointIndex = 0;
    private bool movingForward = true;
    private Vector3 targetPosition;
    private bool isMoving = true;
    private Transform enemyTransform;
    private SpriteRenderer spriteRenderer;

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

        enemyTransform = transform.GetChild(0);
        if (enemyTransform == null)
        {
            Debug.LogError("Enemy child object not found!");
        }

        if (detectionArea == null)
        {
            detectionArea = enemyTransform.Find("Detection Area")?.gameObject;
            if (detectionArea == null)
            {
                Debug.LogError("Detection Area not found!");
            }
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found!");
        }
    }

    private void GetCheckpoints()
    {
        Transform checkpointsParent = transform.Find("Checkpoints");
        if (checkpointsParent != null)
        {
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
        if (checkpoints.Count == 0 || !isMoving || enemyTransform == null) return;

        // Calculate movement direction
        Vector3 direction = (targetPosition - enemyTransform.position).normalized;

        // Move the enemy GameObject towards the target position
        enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, targetPosition, moveDistance);

        // Rotate the Detection Area to face the movement direction
        if (direction != Vector3.zero && detectionArea != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Add 90 degrees to the angle to align the bottom of the sprite with the movement direction
            detectionArea.transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
            UpdateSprite(angle + 90f);
        }

        // Check if we've reached the current checkpoint
        if (Vector3.Distance(enemyTransform.position, checkpoints[currentCheckpointIndex].position) < 0.01f)
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

    private void UpdateSprite(float angle)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("No sprite renderer");
            return;
        }

        // Normalize the angle to be between 0 and 360
        angle = (angle + 360) % 360;

        if (angle > 315 || angle <= 45) // Facing right
        {
            spriteRenderer.sprite = enemyRightSprite;
            spriteRenderer.flipX = false;
        }
        else if (angle > 45 && angle <= 135) // Facing down
        {
            spriteRenderer.sprite = enemyBottomSprite;
            spriteRenderer.flipX = false;
        }
        else if (angle > 135 && angle <= 225) // Facing left
        {
            spriteRenderer.sprite = enemyRightSprite;
            spriteRenderer.flipX = true;
        }
        else // Facing up
        {
            spriteRenderer.sprite = enemyTopSprite;
            spriteRenderer.flipX = false;
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
