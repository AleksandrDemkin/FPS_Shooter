using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerMask, groundMask;
    [SerializeField] private float health;
    [SerializeField] private float damage;
    
    //Patrolling
    private Vector3 _walkPoint;
    [SerializeField] private bool walkPointSet;
    [SerializeField] private float walkPointRange;
    
    //Attacking
    [SerializeField] private float timeBetweenAttacks;
    private bool _attacked;
    [SerializeField] private GameObject bullet;
    
    //States
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check sight or attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrol();
        }
        
        if (playerInSightRange && !playerInAttackRange)
        {
            Chase();
        }
        
        if (playerInSightRange && playerInAttackRange)
        {
            Attack();
        }
    }

    private void Patrol()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            navMeshAgent.SetDestination(_walkPoint);
        }

        var distanceToWalkPoint = transform.position - _walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    
    private void Chase()
    {
        navMeshAgent.SetDestination(player.position);
    }
    
    private void Attack()
    {
        navMeshAgent.SetDestination(player.position);
        transform.LookAt(player);

        if (!_attacked)
        {
            //Attacking
            var rb = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            //
            _attacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _attacked = false;
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sightRange);
    }

    private void SearchWalkPoint()
    {
        //calculate random point in range
        var randomZ = Random.Range(-walkPointRange, walkPointRange);
        var randomX = Random.Range(-walkPointRange, walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 5f, groundMask))
        {
            walkPointSet = true;
        }
    }
}
