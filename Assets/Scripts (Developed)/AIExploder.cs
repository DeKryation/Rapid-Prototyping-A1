using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.AI;

public class AIExploder : MonoBehaviour
{
    public FirstPersonController fpsc;
    public float fov = 120f;
    public float viewDistance = 10f;
    public float wanderingRadius = 7f;
    public float WanderSpeed = 4f;
    public float chaseSpeed = 6f;
    private bool isAware = false;
    private Vector3 wanderPoint;
    private NavMeshAgent agent;
    private Renderer renderer;
    private Animator animator;
    public GameObject m_RightFist;
    public float health = 50f;

    // Exploder zombie variables
    public bool isExploder = false;
    public float explosionRange = 2.5f;
    public float explosionDamage = 40f;
    private bool hasExploded = false;
    public MobileHealthController mhc;

    // Start is called before the first frame update
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
        animator = GetComponentInChildren<Animator>();
        wanderPoint = RandomMovePoint();

        mhc = FindObjectOfType<MobileHealthController>();
        if (mhc == null)
        {
            Debug.LogError("[AIExploder] MobileHealthController not found in scene! Player damage will not work.");
        }
    }

    public void ZombiePunchStart()
    {
        m_RightFist.GetComponent<Collider>().enabled = true;
    }

    public void ZombieEnd()
    {
        m_RightFist.GetComponent<Collider>().enabled = false;
    }

    public void Update()
    {
        if (hasExploded) return;
        if (agent == null || !agent.isOnNavMesh || !agent.enabled) return;

        if (isAware)
        {
            agent.SetDestination(fpsc.transform.position);

            // Regular zombie attack behaviour, no proximity explosion check here
            if (agent.remainingDistance < 1.0f)
            {
                animator.SetTrigger("Attack");
            }

            animator.SetBool("Aware", true);
            agent.speed = chaseSpeed;
        }
        else
        {
            SearchForPlayer();
            Wander();
            animator.SetBool("Aware", false);
            agent.speed = WanderSpeed;
        }
    }

    public void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;

        float dist = Vector3.Distance(transform.position, fpsc.transform.position);
        Debug.Log($"[AIExploder] {gameObject.name} EXPLODED! | Radius: {explosionRange}m | Damage: {explosionDamage}");

        if (dist <= explosionRange)
        {
            if (mhc != null)
            {
                mhc.playerHealth -= explosionDamage;
                Debug.Log($"[AIExploder] --> Damaged PLAYER | Distance: {dist:F2}m | Damage dealt: {explosionDamage}");
            }
            else
            {
                Debug.LogError("[AIExploder] MobileHealthController reference is missing!");
            }
        }

        // Damage nearby zombies
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue; // skip self

            AIScript ai = hit.GetComponent<AIScript>();
            AITankScript tank = hit.GetComponent<AITankScript>();
            AIExploder exploder = hit.GetComponent<AIExploder>();

            if (ai != null)
            {
                ai.TakeDamage(explosionDamage);
                Debug.Log($"[AIExploder] --> Damaged AIScript: {hit.gameObject.name} | Damage: {explosionDamage} | Distance: {Vector3.Distance(transform.position, hit.transform.position):F2}m");
            }
            if (tank != null)
            {
                tank.TakeDamage(explosionDamage);
                Debug.Log($"[AIExploder] --> Damaged AITankScript: {hit.gameObject.name} | Damage: {explosionDamage} | Distance: {Vector3.Distance(transform.position, hit.transform.position):F2}m");
            }
            if (exploder != null)
            {
                exploder.TakeDamage(explosionDamage);
                Debug.Log($"[AIExploder] --> Chain Explode triggered on: {hit.gameObject.name} | Damage: {explosionDamage} | Distance: {Vector3.Distance(transform.position, hit.transform.position):F2}m");
            }
            if (ai == null && tank == null && exploder == null)
            {
                Debug.Log($"[AIExploder] --> Hit non-enemy object: {hit.gameObject.name} (ignored)");
            }
        }
        // Play death animation then destroy
        animator.SetBool("Death", true);
        agent.enabled = false;
        chaseSpeed = 0f;
        WanderSpeed = 0f;
        Destroy(gameObject, 3f); // Gives time for death animation to play
    }
    public void SearchForPlayer()
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(fpsc.transform.position)) < fov / 2f)
        {
            if (Vector3.Distance(fpsc.transform.position, transform.position) < viewDistance)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, fpsc.transform.position, out hit, -1))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        OnAware();
                    }
                }
            }
        }
    }

    public void OnAware()
    {
        isAware = true;
    }

    public void Wander()
    {
        if (agent == null || !agent.isOnNavMesh || !agent.enabled) return;

        if (Vector3.Distance(transform.position, wanderPoint) < 2f)
        {
            wanderPoint = RandomMovePoint();
        }
        else
        {
            agent.SetDestination(wanderPoint);
        }
    }

    public Vector3 RandomMovePoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderingRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, wanderingRadius, -1);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Death();
        }
    }

    public void Death()
    {
        // Explode on death regardless of isExploder check (it's AIExploder after all)
        if (!hasExploded)
        {
            Debug.Log($"[AIExploder] {gameObject.name} died and is EXPLODING!");
            Explode();
        }
    }
}