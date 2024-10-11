using UnityEngine;

public class RotatingEnemy : MonoBehaviour
{
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private GameObject detectionArea;
    [SerializeField] private Sprite enemyTopSprite;
    [SerializeField] private Sprite enemyBottomSprite;
    [SerializeField] private Sprite enemyRightSprite;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBeat += Rotate;
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }

        if (detectionArea == null)
        {
            detectionArea = transform.Find("Detection Area")?.gameObject;
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

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBeat -= Rotate;
        }
    }

    private void Rotate()
    {
        if (detectionArea != null)
        {
            detectionArea.transform.Rotate(Vector3.forward, rotationAngle);
            UpdateSprite();
        }
        else
        {
            Debug.LogError("Detection Area is null!");
        }
    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null || detectionArea == null) return;

        float angle = detectionArea.transform.eulerAngles.z;

        if (angle >= 315 || angle < 45) // Facing right
        {
            spriteRenderer.sprite = enemyRightSprite;
            spriteRenderer.flipX = false;
        }
        else if (angle >= 45 && angle < 135) // Facing up
        {
            spriteRenderer.sprite = enemyTopSprite;
            spriteRenderer.flipX = false;
        }
        else if (angle >= 135 && angle < 225) // Facing left
        {
            spriteRenderer.sprite = enemyRightSprite;
            spriteRenderer.flipX = true;
        }
        else // Facing down
        {
            spriteRenderer.sprite = enemyBottomSprite;
            spriteRenderer.flipX = false;
        }
    }
}
