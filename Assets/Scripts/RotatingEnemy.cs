using UnityEngine;

public class RotatingEnemy : MonoBehaviour
{
    [SerializeField] private float rotationAngle = 45f;
    [SerializeField] private float rotationInterval = 1f;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= rotationInterval)
        {
            transform.Rotate(Vector3.forward, rotationAngle);
            timer = 0f;
        }
    }
}