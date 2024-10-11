using UnityEngine;

public class WinZone : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FinfObjectOfType<GameManager>();;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.Win();
        }
    }
}