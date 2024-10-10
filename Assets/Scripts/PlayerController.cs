using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float normalMoveSpeed = 5f;
    [SerializeField] private float sneakyMoveSpeed = 2.5f;
    private Vector3 targetPosition;
    private bool isSneaky = false;

    public bool IsSneaky { get { return isSneaky; } }

    private void Update()
    {
        isSneaky = Input.GetButton("Fire2");

        SetTargetPosition();
        MovePlayer();
    }

    void SetTargetPosition()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = transform.position.z;
    }

    void MovePlayer()
    {
        float currentMoveSpeed = isSneaky ? sneakyMoveSpeed : normalMoveSpeed;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentMoveSpeed * Time.deltaTime);
    }
}