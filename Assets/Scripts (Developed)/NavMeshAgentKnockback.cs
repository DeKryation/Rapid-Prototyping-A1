using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
public class NavMeshAgentKnockback : MonoBehaviour
{
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        StartCoroutine(Knockback(direction, force));
    }

    IEnumerator Knockback(Vector3 direction, float force)
    {
        //insert knockback sfx here
        if (agent == null) yield break;

        agent.enabled = false;

        Vector3 knockbackDir = new Vector3(direction.x, 0f, direction.z).normalized;
        float elapsed = 0f;
        float duration = 0.2f;

        while (elapsed < duration)
        {
            transform.position += knockbackDir * force * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        agent.enabled = true;
    }
}