using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Snail_AI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask Ground, Player;
    public Animator anim;

    public Vector3 walkPoint;
    bool walkPointSet;

    private float attackRange = 3f;
    private bool playerInAttackRange, playerInSightRange;

    public float timeBetweenAttacks = .5f;
    bool alreadyAttacked;

    public float walkPointRange = 200f;



    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        //agent.SetDestination(player.position);
    }

    // Update is called once per frame
    private void Update()
    {

        
        transform.LookAt(player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);
       
       
        
        if(!playerInAttackRange)
        {
            ChasePlayer();

        }

        else if (playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void ChasePlayer() //ENEMY SEES PLAYER AND MOVES TOWARDS HIM. SPEED INCREASES
    {
       this.anim.SetBool("isMoving", true);
       agent.SetDestination(player.position);


        agent.speed = .5f;
    }

    private void AttackPlayer() //ENEMY ATTACKS PLAYER
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < (attackRange - 1))
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
    
}
