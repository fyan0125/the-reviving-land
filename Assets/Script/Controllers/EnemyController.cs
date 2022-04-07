using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    Transform target;

    public GameObject[] items = new GameObject[3];
    
    
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet = false;
    public float walkPointRange;

    //Attacking
    public Vector3 attackPoint;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    float height;
    // public Transform bulletSpawnPoint;
    // public float bulletSpeed = 10;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake ()
    {
        // player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        target = player.transform;
        // TakeDamage(10);
    }

    private void Update() 
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange) Patroling();
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if(playerInSightRange && playerInAttackRange) AttackPlayer();
    }
    
    private void Patroling()
    {
        if(!walkPointSet) {
            SearchWalkPoint();
        }
        

        if(walkPointSet){
            agent.SetDestination(walkPoint);
            walkPointSet = false;
            StartCoroutine(waiter());
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkPoint reached
        if(distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
            
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(100);
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomY = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z + randomZ);
    
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.speed = 2;
        agent.destination = target.position;
        // Invoke(3);
    }

    private void AttackPlayer()
    {
        //make sure enemy doesn't move
        agent.SetDestination(transform.position);

        //transform.LookAt(player);
        FaceTarget();

        if(!alreadyAttacked){

            //Attack Code
            height = 0.3f;
            attackPoint = new Vector3(transform.position.x, transform.position.y + height, transform.position.z );
            Rigidbody rb = Instantiate(projectile, attackPoint, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 35f, ForceMode.Impulse);
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);
            // var bullet = Instantiate(projectile, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            // bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
            //

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Vector3 position = transform.position;
        if (health <= 0){
            Invoke(nameof(DestroyEnemy), 0.5f);
            foreach (GameObject item in items)
             {
                 if (item != null)
                 {
                     Instantiate(item, position, Quaternion.identity);
                 }
 
             }
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
