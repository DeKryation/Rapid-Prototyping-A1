using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.AI;

public class AIScript : MonoBehaviour
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
    public Transform hitVFXPrefab;

    public GameObject m_RightFist;

    public float health = 50f;
   

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
        //Insert zombie punch sfx

    }

    public void ZombieEnd()
    {
        m_RightFist.GetComponent<Collider>().enabled = false;
    }
    public void Update()
    {

        if (isAware)
        {
            agent.SetDestination(fpsc.transform.position);
            if(agent.remainingDistance < 1.0f)
            {
                animator.SetTrigger("Attack");
            }
            animator.SetBool("Aware", true);
            agent.speed = chaseSpeed;
            
            //renderer.material.color = Color.red;
        } else
        {
            SearchForPlayer();
            Wander();
            animator.SetBool("Aware", false);
            agent.speed = WanderSpeed;
            //renderer.material.color = Color.blue;
        }

        
    }

    public void SearchForPlayer()
    {
        if(Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(fpsc.transform.position)) < fov / 2f)
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
        } else
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
        //insert hit sfx

        if (hitVFXPrefab != null)
        {
            Transform vfx = Instantiate(hitVFXPrefab, transform.position, transform.rotation);
            vfx.SetParent(transform); // Attach VFX to the zombie so it moves with it
            Destroy(vfx.gameObject, 2f);

        }
        if (health <= 0f)
        {
            Death();
        }
    }
    void Death ()
    {
        //Insert death sfx
        animator.SetBool("Death", true);
        chaseSpeed = 0f;
        WanderSpeed = 0f;
        Destroy(gameObject, 3f);
        
    }


}
