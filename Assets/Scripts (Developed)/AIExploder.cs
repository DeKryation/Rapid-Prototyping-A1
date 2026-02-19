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
    public float chaseSpeed = 7f;
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

    // Start is called before the first frame update
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
        animator = GetComponentInChildren<Animator>();
        wanderPoint = RandomMovePoint();
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
        // Don't update if exploded
        if (hasExploded)
            return;

        if (isAware)
        {
            agent.SetDestination(fpsc.transform.position);

            // Check if exploder zombie is close enough to explode
            if (isExploder)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, fpsc.transform.position);
                if (distanceToPlayer <= explosionRange)
                {
                    Explode();
                    return;
                }
            }
            else
            {
                // Regular zombie attack
                if (agent.remainingDistance < 1.0f)
                {
                    animator.SetTrigger("Attack");
                }
            }

            animator.SetBool("Aware", true);
            agent.speed = chaseSpeed;

            //renderer.material.color = Color.red;
        }
        else
        {
            SearchForPlayer();
            Wander();
            animator.SetBool("Aware", false);
            agent.speed = WanderSpeed;
            //renderer.material.color = Color.blue;
        }
    }

    public void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;

        float dist = Vector3.Distance(transform.position, fpsc.transform.position);

        if (dist <= explosionRange)
        {
            fpsc.GetComponent<MobileHealthController>().playerHealth -= explosionDamage;
        }

        // Damage nearby zombies
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue; // skip self

            // Check each zombie type
            hit.GetComponent<AIScript>()?.TakeDamage(explosionDamage);
            hit.GetComponent<AITankScript>()?.TakeDamage(explosionDamage);
            hit.GetComponent<AIExploder>()?.TakeDamage(explosionDamage); // chain explosions
        }
        Destroy(gameObject);
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

    void Death()
    {
        // If it's an exploder, explode on death
        if (isExploder && !hasExploded)
        {
            Explode();
        }
        else
        {
            animator.SetBool("Death", true);
            chaseSpeed = 0f;
            WanderSpeed = 0f;
            Destroy(gameObject, 3f);
        }
    }
}