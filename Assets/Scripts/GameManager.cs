using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float beatInterval = 1f;
    private float beatTimer = 0f;

    public delegate void BeatAction();
    public event BeatAction OnBeat;

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
        }
    }

    public float GetBeatInterval()
    {
        return beatInterval;
    }
}