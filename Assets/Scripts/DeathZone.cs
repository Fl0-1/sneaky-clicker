using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Sprite jumpscareSprite;
    [SerializeField] private AudioClip jumpscareSound;
    [SerializeField] private float jumpscareDuration = 2f;

    private AudioSource audioSource;
    private Image jumpscareImage;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = false;

        // Create a new UI Image for the jumpscare
        CreateJumpscareImage();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            StartCoroutine(Jumpscare(collider.gameObject));
        }
    }

    private void CreateJumpscareImage()
    {
        GameObject canvasObject = new GameObject("JumpscareCanvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject imageObject = new GameObject("JumpscareImage");
        imageObject.transform.SetParent(canvasObject.transform, false);

        jumpscareImage = imageObject.AddComponent<Image>();
        jumpscareImage.sprite = jumpscareSprite;
        jumpscareImage.rectTransform.anchorMin = Vector2.zero;
        jumpscareImage.rectTransform.anchorMax = Vector2.one;
        jumpscareImage.rectTransform.sizeDelta = Vector2.zero;
        jumpscareImage.enabled = false;
    }

    private IEnumerator Jumpscare(GameObject player)
    {
        // Show jumpscare image
        if (jumpscareImage != null)
        {
            jumpscareImage.enabled = true;
        }

        // Play jumpscare sound
        if (jumpscareSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpscareSound);
        }

        // Destroy the player
        Destroy(player);

        // Wait for the jumpscare duration
        yield return new WaitForSeconds(jumpscareDuration);

        // Hide jumpscare image
        if (jumpscareImage != null)
        {
            jumpscareImage.enabled = false;
        }

        // Call GameOver method from GameManager
        GameManager.Instance.GameOver();
    }
}
