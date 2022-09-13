using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Snail_AI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;

    
    public LayerMask Ground, Player;
    public Transform player;
    public GameObject snailSpawn;

    private Vector3 walkPoint;
    bool walkPointSet;
    public float patrolRange = 10f;

    public float sightRange = 20f, attackRange = 3f;

    private bool playerInAttackRange, playerInSightRange;

    public float timeBetweenAttacks = .5f;
    bool alreadyAttacked;

    [Range(1f,4f)]
    public int snailLevel = 1;
    [Range(5f,300f)]
    public float snailUpgradeSpeed = 5f;





    void Start()
    {
        StartCoroutine(SnailUpgrade());
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        //agent.SetDestination(player.position);
    }

    // Update is called once per frame
    private void Update()
    {
        SnailStats();



        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);
       
       
        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        else if(playerInSightRange &&!playerInAttackRange)
        {
            ChasePlayer();

        }

        else if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void ChasePlayer() //ENEMY SEES PLAYER AND MOVES TOWARDS HIM. SPEED INCREASES
    {
       this.anim.SetBool("isMoving", true);
        transform.LookAt(player);

        agent.SetDestination(player.position);


    }

    private void AttackPlayer() //ENEMY ATTACKS PLAYER
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < (attackRange - .5f))
        {
            //this.anim.SetBool("Walk Backward Fast", true);
            //this.anim.SetBool("Walk Forward Fast", false);



            Vector3 dirToPlayer = transform.position - player.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            agent.SetDestination(newPos);
        }
        else
        {

            //this.anim.SetBool("Walk Backward Fast", false);
            //this.anim.SetBool("Walk Forward Fast", true);\
            
            this.anim.SetTrigger("Attack");

            


            agent.SetDestination(transform.position); // need to add run away from player

        }
        //Make enemy stop moving
        transform.LookAt(player);

        if (!alreadyAttacked)
        {


            //Attack Code here
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        //Stinger.SetActive(false);
    }

    private void SnailStats()
    {
        if(snailLevel == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
            agent.speed = .5f;
            sightRange = 10f;
            attackRange = 1f;
        }
        else if(snailLevel == 2)
        {
            transform.localScale = new Vector3(2, 2, 2);
            agent.speed = 1f;
            sightRange = 20f;
            attackRange = 3f;

        }
        else if(snailLevel == 3)
        {
            transform.localScale = new Vector3(3, 3, 3);

            agent.speed = 2f;
            sightRange = 50f;
            attackRange = 3f;
        }
        else if(snailLevel == 4)
        {
            transform.localScale = new Vector3(4, 4, 4);

            agent.speed = 5f;
            sightRange = 1000f;
            attackRange = 3f;
        }
        else if(snailLevel == 5)
        {
            agent.speed = 0f;
            GameObject spawn = Instantiate(this.snailSpawn);
            snailSpawn.SetActive(true);
            snailSpawn.transform.SetParent(null);
            snailLevel = 6;

        }
        else
        {
            transform.localScale = new Vector3(4, 4, 4);

            agent.speed = 5f;
            sightRange = 1000f;
            attackRange = 3f;
        }
    }
    private void Patroling()//ENEMY WALKS AROUND 
    {
        
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            //agent.speed = 2f;

        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude <= 1f)
        {
            walkPointSet = false;
            //agent.speed = .5f;
            
        }
       


        
    }

    private void SearchWalkPoint() //ENEMY FINDS WHERE TO WALK TO
    {
        float randomZ = Random.Range(-patrolRange, patrolRange);
        float randomX = Random.Range(-patrolRange, patrolRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, Ground))
        {

                walkPointSet = true;

        }

       

    }
    IEnumerator SnailUpgrade()
    {
        while (snailLevel <= 4)
        {
            yield return new WaitForSeconds(snailUpgradeSpeed);
            snailLevel++;
            
        }



    }
    
    
        


    
}
