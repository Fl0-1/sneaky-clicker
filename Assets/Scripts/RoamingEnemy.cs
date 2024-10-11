using UnityEngine;
using UnityEngine.AI;

public class RoamingEnemy : MonoBehaviour
{
    [SerializeField] private float roamRadius = 5f;
    [SerializeField] private float moveDistance = 1f; // Distance to move on each beat
    [SerializeField] private float tempoChangeRadius = 3f; // Radius within which the tempo changes
    [SerializeField] private float newTempo = 0.5f; // New tempo when close to the player
    public Transform player;

    private NavMeshAgent agent;
    private Vector3 roamPosition;
    private bool isMoving = true;
    private float originalTempo;
    private bool isTempoChanged = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on RoamingEnemy!");
            enabled = false;
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player reference not set for RoamingEnemy!");
            enabled = false;
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBeat += Move;
            originalTempo = GameManager.Instance.GetBeatInterval();
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
            enabled = false;
            return;
        }

        SetNewRoamPosition();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBeat -= Move;
            ResetTempo();
        }
    }

    private void Update()
    {
        CheckTempoChange();
    }

    private void SetNewRoamPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas);
        roamPosition = hit.position;
    }

    private void Move()
    {
        if (!isMoving || transform == null) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 newPosition = transform.position + directionToPlayer * moveDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(newPosition, out hit, moveDistance, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }

        // Rotate the enemy to face the movement direction
        if (directionToPlayer != Vector3.zero)
        {
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
        }

        // Check if we've reached the current roam position
        if (Vector3.Distance(transform.position, roamPosition) < 0.01f)
        {
            SetNewRoamPosition();
        }
    }

    private void CheckTempoChange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= tempoChangeRadius && !isTempoChanged)
        {
            ChangeTempo(newTempo);
            isTempoChanged = true;
        }
        else if (distanceToPlayer > tempoChangeRadius && isTempoChanged)
        {
            ResetTempo();
            isTempoChanged = false;
        }
    }

    private void ChangeTempo(float newTempo)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetBeatInterval(newTempo);
        }
    }

    private void ResetTempo()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetBeatInterval(originalTempo);
        }
    }
}
