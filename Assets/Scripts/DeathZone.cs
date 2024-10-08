using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Destroy(collider.gameObject);
            Debug.Log("Player destroyed by DeathZone!");
        }
    }
}
