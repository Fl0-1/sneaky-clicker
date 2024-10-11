using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float beatInterval = 1f;
    private float beatTimer = 0f;
    [SerializeField] private AudioClip beatSound;

    public delegate void BeatAction();
    public event BeatAction OnBeat;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        beatTimer += Time.deltaTime;

        if (beatTimer >= beatInterval)
        {
            beatTimer = 0f;
            OnBeat?.Invoke();
            BeatSound();
        }
    }

    public float GetBeatInterval()
    {
        return beatInterval;
    }

    public void SetBeatInterval(float newInterval)
    {
        beatInterval = newInterval;
        beatTimer = 0f; // Reset the timer to keep the beat consistent
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BeatSound()
    {
        if (beatSound != null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.loop = false;
            audioSource.PlayOneShot(beatSound);
        }
    }
}
