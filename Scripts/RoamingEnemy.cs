using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RoamingEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;                 // Drag your Player here
    public Transform playerStartPoint;       // Drag PlayerStart here
    public Transform[] enemySpawnPoints;     // Drag all EnemySpawn points here

    [Header("Wander Settings")]
    public float wanderRadius = 20f;
    public float wanderSpeed = 2f;
    public float waypointTolerance = 0.5f;
    public float idleTimeAtPoint = 1f;

    [Header("Chase Settings")]
    [Tooltip("Distance at which the enemy will start chasing you (no line of sight).")]
    public float chaseRange = 8f;           // how close you must be for him to chase
    public float chaseSpeed = 4f;
    public float catchDistance = 1.5f;      // how close he must get to catch you

    [Header("SFX")]
    public AudioClip screamSound;           // Scream when chase starts

    [Header("Anti-Stuck Settings")]
    public float stuckPositionThreshold = 0.05f;
    public float stuckTimeThreshold = 2f;

    // NEW: reference to the death screen UI
    public DeathScreen deathScreen;         // Drag DeathScreenManager here

    private NavMeshAgent agent;
    private AudioSource audioSource;

    private Vector3 enemyStartPosition;
    private Vector3 currentWanderTarget;
    private bool hasWanderTarget = false;
    private float idleTimer = 0f;

    private bool isChasing = false;
    private Vector3 lastPosition;
    private float stuckTimer = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f; // 3D sound
        }
    }

    void Start()
    {
        enemyStartPosition = transform.position;
        lastPosition = transform.position;

        // Initial random spawn
        RespawnEnemyRandom();

        PickNewWanderTarget();
    }

    void Update()
    {
        if (player == null) return;

        HandleStuckDetection();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // SIMPLE LOGIC: chase if within range, otherwise wander
        if (distanceToPlayer <= chaseRange)
        {
            if (!isChasing)
            {
                StartChase();
            }

            HandleChasing(distanceToPlayer);
        }
        else
        {
            if (isChasing)
            {
                StopChase();
            }

            HandleWandering();
        }
    }

    // ================== WANDERING ==================

    void HandleWandering()
    {
        agent.speed = wanderSpeed;

        if (!hasWanderTarget)
        {
            PickNewWanderTarget();
        }

        if (!agent.pathPending && agent.remainingDistance <= waypointTolerance)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTimeAtPoint)
            {
                idleTimer = 0f;
                PickNewWanderTarget();
            }
        }
    }

    void PickNewWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        Vector3 candidate = enemyStartPosition + new Vector3(randomCircle.x, 0f, randomCircle.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(candidate, out hit, 2f, NavMesh.AllAreas))
        {
            currentWanderTarget = hit.position;
            agent.SetDestination(currentWanderTarget);
            hasWanderTarget = true;
        }
    }

    // ================== CHASING ==================

    void HandleChasing(float distanceToPlayer)
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        if (distanceToPlayer <= catchDistance)
        {
            ResetPlayerAndEnemy();
        }
    }

    void StartChase()
    {
        isChasing = true;

        // scream once
        if (screamSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(screamSound);
        }

        // switch to chase music
        if (HorrorMusicManager.Instance != null)
        {
            HorrorMusicManager.Instance.PlayChaseMusic();
        }
    }

    void StopChase()
    {
        isChasing = false;
        hasWanderTarget = false; // so wander picks a new target next time

        // back to ambient
        if (HorrorMusicManager.Instance != null)
        {
            HorrorMusicManager.Instance.StopChaseMusic();
        }
    }

    // ================== RESET / RESPAWN ==================

    void ResetPlayerAndEnemy()
    {
        // Reset player position
        if (player != null && playerStartPoint != null)
        {
            var controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                player.position = playerStartPoint.position;
                controller.enabled = true;
            }
            else
            {
                player.position = playerStartPoint.position;
            }
        }

        // Respawn enemy
        RespawnEnemyRandom();

        // End chase state + music
        StopChase();

        // NEW: show the "You Died" screen AFTER everything else
        if (deathScreen != null)
        {
            deathScreen.Show();
        }
    }

    void RespawnEnemyRandom()
    {
        Vector3 spawnPos = enemyStartPosition;

        if (enemySpawnPoints != null && enemySpawnPoints.Length > 0)
        {
            int idx = Random.Range(0, enemySpawnPoints.Length);
            spawnPos = enemySpawnPoints[idx].position;
        }

        if (agent != null)
        {
            agent.Warp(spawnPos);
        }
        else
        {
            transform.position = spawnPos;
        }

        enemyStartPosition = spawnPos;
        hasWanderTarget = false;
        idleTimer = 0f;
    }

    // ================== ANTI-STUCK ==================

    void HandleStuckDetection()
    {
        float movedSqr = (transform.position - lastPosition).sqrMagnitude;

        if (movedSqr < stuckPositionThreshold * stuckPositionThreshold)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= stuckTimeThreshold)
            {
                if (isChasing)
                {
                    if (player != null)
                    {
                        agent.SetDestination(player.position);
                    }
                }
                else
                {
                    hasWanderTarget = false;
                    PickNewWanderTarget();
                }

                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPosition = transform.position;
    }

    // ================== COLLISION BACKUP ==================

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ResetPlayerAndEnemy();
        }
    }
}