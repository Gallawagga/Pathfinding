using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This was an awful implementation of a state machine, I tried to control both behavioural AI as well as switching conditions from within what amounts to 'if' and 'while
/// statements. This script is now obsolete, but it did help me realise:
/// Separate behavioural AI scripting from code responsible for switching between behaviours. 
/// While statements aren't the most reliable things in the world, they either loop infinitely or don't end exactly when you want them to.
/// Coroutines are not to be taken lightly, they must be respected as their powers (and temper-tantrums) are great indeed.
/// </summary>

namespace Pathfinding 
{
    public class EnemyStates : MonoBehaviour
    {
        [Header("Waypoints")]
        [SerializeField]
        private GameObject[] Waypoint;
        [SerializeField]
        private float minDistance = 0.5f;
        private int index = 0;

        [Header("State Behaviour Variables")]
        [SerializeField]
        private float chasePlayerDistance = 15f;
        [SerializeField]
        private float attackPlayerDistance = 15f;
        [SerializeField]
        private GameObject target;
        [SerializeField]
        private float speed = 10f;
        [SerializeField]
        private float health = 100f;
        [SerializeField]
        private float damage = 10f;
        [SerializeField]
        private float fleeThreshold = 25f;



        //creating new instance of Enemy class so I can access its variables. I don't need to redeclare/define the variables because they already exist in the Enemy class. 
        //Enemy enemy = new Enemy();


        public enum State
        {
            Patrol,
            Chase,
            Attack,
            Flee,
        }

        public State state;

        private void Start()
        {
            //begin the game in the patrol state.
            
            PatrolState(Waypoint[index].transform.position);

            //wtf is this?
            //enemy = this.GetComponent<Enemy>();
        }

        #region STATES

        private State PatrolState(Vector2 nextWaypoint)
        {
            state = State.Patrol;
            Debug.Log("Entering Patrol State");
            while (state == State.Patrol)
            {
                transform.position = Vector2.MoveTowards(transform.position, nextWaypoint, speed * Time.deltaTime);
                //IF player character is close enough go to chase state 
                if (Vector2.Distance(target.transform.position, transform.position) < chasePlayerDistance)
                {
                    ChaseState();
                    break;
                }
                float distance = Vector2.Distance(transform.position, Waypoint[index].transform.position);
                if (distance < minDistance)
                {
                    index++;
                }
                if (index >= Waypoint.Length)
                { 
                    index = 0;
                }

                //when we reach waypoint
                //go to next waypoint
                transform.position = Vector2.MoveTowards(transform.position, nextWaypoint, speed * Time.deltaTime);

            }
            NextState();
            return State.Patrol;
        }

        private State ChaseState()
        {
            state = State.Chase;

            while (Vector2.Distance(target.transform.position, this.transform.position) <= chasePlayerDistance)
            {
                //chase the player
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

                if (Vector2.Distance(target.transform.position, this.transform.position) <= attackPlayerDistance)
                {
                    //AttackState();
                    break;
                }

                //chase the player until within a certain distance, then start attacking. 
                return state;
            }
            PatrolState(Waypoint[index].transform.position);

            return state;
        }

        //private State AttackState()
        //{
           // state = State.Attack;
            
            //if(health <= fleeThreshold)
            //{
                //FleeState();
                //return;
           // }

            //StartCoroutine AttackPlayer();

            //If health is above 25%, keep attacking. If it isn't, enter the flee state.
            //Attack the player.
            //if enemy leaves attack area, revert to chase.
        //}

       // private State FleeState()
        //{
           // state = State.Flee;
            //same as patrol, but actively avoiding the player.
            //if health is above 50%, return to patrol. 
        //}

        //DO I NEED THIS CODE?

        private void NextState()
        {
            //work out the name of the method we want to run
            string methodName = state.ToString() + "State"; //if our current state is "walk" then this returns "walkState"
                                                            //give us a variable so we an run a method using its name
            System.Reflection.MethodInfo info =
                GetType().GetMethod(methodName,
                                    System.Reflection.BindingFlags.NonPublic |
                                    System.Reflection.BindingFlags.Instance);
            //Run our method
            StartCoroutine((IEnumerator)info.Invoke(this, null));
            //Using StartCoroutine() means we can leave and come back to the method that is running
            //All Coroutines must return IEnumerator
        }

        #endregion STATES

        //IEnumerator AttackPlayer()
        //{
            //deal 10 damage to player
            //wait 2 seconds
            //repeat
        //}

    }
}