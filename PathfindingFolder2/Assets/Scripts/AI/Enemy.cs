using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pathfinding
{
    public class Enemy : Creature
    {
        [Header("Waypoints")]
        [SerializeField]
        private GameObject[] Waypoint;
        [SerializeField]
        private float minDistance = 0.5f;
        private int index = 0;

        [Header("State Behaviour Variables")]
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private float chasePlayerDistance = 15f;
        [SerializeField]
        private float attackPlayerDistance = 7.5f;
        [SerializeField]
        private GameObject target;
        [SerializeField]
        private float fleeThreshold = 25f;
        [SerializeField]
        private float attackRate = 1f;
        private float attackPause;
        private float maxHealth;



        private Player targetPlayer; //necessary?



        protected override void Start()
        {
            base.Start(); // 'base' calls the same function from the parent class
            maxHealth = health; //setting maxHealth to be whatever the NPC starts the game with. Will do for current context.
            targetPlayer = target.GetComponent<Player>();
            healthBar.SetMaxHealth(maxHealth);
        }

        void MoveAI(Vector2 targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        #region MEASUREMENTS 
        //This region holds all code relating to measuring variables as prerequisites for other behaviours/states. 

        public bool IsPlayerChaseable()
        {
            //this is a more efficient check of a boolean outcome, simply returning the outcome of the following check:
            return (Vector2.Distance(target.transform.position, transform.position) <= chasePlayerDistance);
            //ORIGINALLY IT WAS THIS, same same but different. 
            // if (Vector2.Distance(target.transform.position, transform.position) <= chasePlayerDistance)
            {  // return true; 
            }
            // else
            {// return false;
            }
        }
        public bool IsPlayerAttackable() //again, streamlined boolean statement checking whether the player is within a certain distance to the AI.
        {
            return (Vector2.Distance(target.transform.position, transform.position) <= attackPlayerDistance);
        }
        public bool TimeToBail()//check the percentage of current health vs maxHealth
        {
            return health / maxHealth <= 0.25f;
        }

        public bool HealedYet()//check if health is above 50% of max.
        {
            return health / maxHealth >= 0.5f;
        } 

        #endregion MEASUREMENTS

        #region STATES
        public void Patrol()
        {
            //distance defined as the distance between the game object and the next waypoint. 
            float distance = Vector2.Distance(transform.position, Waypoint[index].transform.position);

            if (distance < minDistance)
            {
                //once patrolling AI makes it to waypoint, target next waypoint as destination. 
                index++;
            }
            if (index >= Waypoint.Length)
            {
                //reset waypoint array back to 0;
                index = 0;
            }

            MoveAI(Waypoint[index].transform.position);

        }

        public void Chase()
        {
            MoveAI(target.transform.position);
        }

        public void Attack()
        {
            //move at half speed and keep chasing player. 
            MoveAI(target.transform.position);

            if (Time.time > attackPause)
            {
                //deliver one attack every attackRate amount of time. 
                //animator.SetBool("IsAttacking", true);
                animator.Play("BobFire");

                targetPlayer.ReceiveDamage(damage);

                attackPause = Time.time + attackRate;
                //animator.Play("BobIdle");

                //animator.SetBool("IsAttacking", false);
            }


        }

        public void Flee() //stays away from player, keeps patrolling. 
        {
            Vector2 fleeDirection = transform.position - target.transform.position; // enemy's position - Bob's position apparently gives back a direction; Cartesian coordinates are a doozy.
            //The flee direction will be the opposite direction to whatever direction the player is in. Cool!

            if (Vector2.Distance(target.transform.position, transform.position) <= chasePlayerDistance) //if player is close enough to normally chase, flee instead. Otherwise patrol. 
            {
                MoveAI(fleeDirection);
            }
            else
            {
                Patrol(); 
            }
        }
        #endregion STATES

    }
}