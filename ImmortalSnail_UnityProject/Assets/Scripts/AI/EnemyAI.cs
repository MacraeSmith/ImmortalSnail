using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask Ground, Player;
    public Animator anim;
    public int health = 3;
    public GameObject energyDrop;
    public int dropQuantity = 1;
    

    public GameObject walkingEnergyDrop;
    public GameObject Stinger;
    public AudioSource deathSound;
    public AudioSource damageSound;
    public bool canShoot = false;

    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed = 30f;
    private float timeToFire;
    public float fireRate = 10f;
    public AudioSource fireSound;
    

    private bool dead = false;





    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 200f;

    //Attacking
    public float timeBetweenAttacks = 2f;
    bool alreadyAttacked;

    //States
    public float sightRange = 20f, attackRange = 3f;
    private bool playerInSightRange, playerInAttackRange;
    public bool randomize = true;

    private void Awake()
    {
       

        if (randomize == true) //RANDOMIZES NUMBERS
        {
            
            if (canShoot == false)
            {
                health = Random.Range(3, 8);
                walkPointRange = Random.Range(10f, 30f);
                sightRange = Random.Range(20f, 50f);
                dropQuantity = Random.Range(0, 4);
            }
            else
            {
                health = Random.Range(2, 5);
                walkPointRange = Random.Range(10f, 50f);
                sightRange = Random.Range(50f, 150f);
                attackRange = 30f;
                dropQuantity = Random.Range(1, 4);


            }


        }


        
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        energyDrop.SetActive(false);
        

        //projectile.SetActive(false);
    }
    private void Update()
    {

        if(dead != true)
        {
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);

            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && playerInAttackRange && !canShoot) AttackPlayer();
            if (playerInSightRange && playerInAttackRange && canShoot) ShootPlayer();

        }

       

    }


    private void Patroling()//ENEMY WALKS AROUND 
    {
        this.anim.SetBool("Walk Forward Fast", false);
        this.anim.SetBool("Walk Forward Slow", true);
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        

        //walkPoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {

            //walkpointDrop();
            walkPointSet = false;
            int num = Random.Range(1, 10);
            if ( num%2 == 0)
            {
                walkpointDrop();

            }

            

        }
        agent.speed = 1f;

        
    }

    private void SearchWalkPoint() //ENEMY FINDS WHERE TO WALK TO
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, Ground))
        {
            walkPointSet = true;

        }

    }
    private void ChasePlayer() //ENEMY SEES PLAYER AND MOVES TOWARDS HIM. SPEED INCREASES
    {
        this.anim.SetBool("Walk Forward Slow", false);
        this.anim.SetBool("Walk Forward Fast", true);
        agent.SetDestination(player.position);
       

        agent.speed = 5f;
    }
    private void AttackPlayer() //ENEMY ATTACKS PLAYER
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < (attackRange- 1))
        {
            //this.anim.SetBool("Walk Backward Fast", true);
            //this.anim.SetBool("Walk Forward Fast", false);
            //agent.speed = 10f;


            Vector3 dirToPlayer = transform.position - player.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            agent.SetDestination(newPos);
        }
        else
        {
            //agent.speed = 5f;
            //this.anim.SetBool("Walk Backward Fast", false);
            //this.anim.SetBool("Walk Forward Fast", true);


            agent.SetDestination(transform.position); // need to add run away from player

        }
        //Make enemy stop moving
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            int num = Random.Range(1, 3);
            if (num == 1)
            {
                Stinger.SetActive(true);
                this.anim.SetTrigger("Tail Stab Attack");
            }
            else if (num == 2)
            {
                Stinger.SetActive(true);
                this.anim.SetTrigger("Tail Flick Attack");
            }
            
            //Attack Code here
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ShootPlayer() //ENEMY SPAWNS PROJECTILE
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < (attackRange - 5))
        {
            this.anim.SetBool("Walk Backward Fast", true);
            this.anim.SetBool("Walk Forward Fast", false);
            agent.speed = 7f;


            Vector3 dirToPlayer = transform.position - player.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            agent.SetDestination(newPos);
        }
        else
        {
            agent.speed = 5f;
            this.anim.SetBool("Walk Backward Fast", false);
            this.anim.SetBool("Walk Forward Fast", true);


            agent.SetDestination(transform.position); // need to add run away from player

        }
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            this.anim.SetTrigger("Front Legs Attack");
            InstantiateProjectile();

            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        Stinger.SetActive(false);
    }

    private void TakeDamage() //ENEMY LOSES HEALTH
    {

        //Debug.Log("Enemy Hit");
        if (dead == false)
        {
            this.anim.SetTrigger("Take Damage");
            damageSound.Play();

        }

        health --;

        if (health <= 0 && dead == false)
        {
            Death();
        }
    }  

    private void DestroyEnemy() //ENEMY DISAPPEARS WHEN DEAD
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision co)
    {
        if (co.gameObject.tag == "Projectile")
        {
            TakeDamage();
        }
    }

    void Death() //ENEMY DIES, STOPS MOVING, PLAYS ANIMATION, AND DISSAPPEARS
    {
        dead = true;

        this.anim.SetTrigger("Die");
        agent.speed = 0f;
        deathSound.Play();

        
        Invoke(nameof(DestroyEnemy), 2f);
        deathDrop();
       

    }
    
    private void deathDrop() //ENEMY DROPS ENERGY ORBS WHEN IT DIES
    {
        while (dropQuantity >= 0)
        {
            GameObject duplicate = Instantiate(this.energyDrop);


            energyDrop.SetActive(true);
            energyDrop.transform.SetParent(null);
            
            duplicate.transform.Translate(Random.Range(-3, 3), 0, Random.Range(-5, 5));


            dropQuantity -= 1;
        }
        
    }
    
    private void walkpointDrop() 
    {
        
        GameObject walkingdrop = Instantiate(this.walkingEnergyDrop);


        walkingEnergyDrop.SetActive(true);
        walkingEnergyDrop.transform.SetParent(null);
        walkingdrop.transform.Translate(Random.Range(-3, 3), 0, Random.Range(-3, 3));



    }
    private void InstantiateProjectile()
    {
        var ProjectileBullet = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;


    }
}
