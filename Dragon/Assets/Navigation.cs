using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{   

    public float wanderRadius = 50;
    public float wanderTimer = 5;

    private Transform target;
    public NavMeshAgent agent;
    public Animator animator;
    private float timer;
        
    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (animator != null)
        {
            animator.SetInteger("condition", 1);
            animator.SetBool("running", true);
        }

        if (timer >= wanderTimer)
        {
            var position = Random.insideUnitSphere * wanderRadius;
            position = new Vector3(position.x, 0, position.z);
            agent.SetDestination(position);

            timer = 0;
            if (animator != null)
            {
                animator.SetInteger("condition", 1);
                animator.SetBool("running", true);
            }
        }
    }

}
