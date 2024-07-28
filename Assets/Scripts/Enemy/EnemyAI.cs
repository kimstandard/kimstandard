using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float loseInterestRange = 5f; // 플레이어 추적을 중지하는 범위

    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked)
        {
            if (distanceToTarget > loseInterestRange)
            {
                isProvoked = false;
                StopChasingTarget();
            }
            else
            {
                EngageTarget();
            }
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
    }

    private void EngageTarget()
    {
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void ChaseTarget()
    {
        //GetComponent<Animator>().SetBool("attack", false);
        //GetComponent<Animator>().SetTrigger("move");
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        //GetComponent<Animator>().SetBool("attack", true);
    }

    private void StopChasingTarget()
    {
        //GetComponent<Animator>().SetBool("attack", false);
        //GetComponent<Animator>().SetTrigger("idle");
        navMeshAgent.ResetPath(); // 적이 이동을 멈추도록 설정
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, loseInterestRange);
    }
}
