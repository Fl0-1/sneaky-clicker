using UnityEngine;

public class WinZone : MonoBehaviour
{
    private GameManager gameManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Win();
        }
    }
}