﻿using UnityEngine.AI;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 8f;

    Transform target;
    NavMeshAgent agent;
    AudioSource groan;
    Animator animator;

    void Start()
    {
        target = PlayerManager.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        groan = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            PlaySound();
            animator.SetFloat("forward", 1.0f);
        }
        else
        {
            agent.SetDestination(transform.position);
            groan.Stop();
            animator.SetFloat("forward", 0.0f);
        }

        if(distance <= agent.stoppingDistance)
        {
            animator.SetFloat("forward", 0.0f);
            FaceTarget();
        }

        
    }
    void PlaySound()
    {
        if (!groan.isPlaying)
        {
            groan.Play();
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
