using UnityEngine;

public class RotatingEnemy : MonoBehaviour
{
    [SerializeField] private float rotationAngle = 90f;

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
        transform.Rotate(Vector3.forward, rotationAngle);
    }
}