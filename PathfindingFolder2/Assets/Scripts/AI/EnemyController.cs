using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pathfinding
{

    public class EnemyController : MonoBehaviour
    {
        Enemy enemy;
        bool isSlow = false;

        public enum EnemyBehaviourState
        {
            Patrol,
            Chase,
            Attack,
            Flee,
        }

        private EnemyBehaviourState currentState;

        public EnemyBehaviourState GetCurrentState()
        {
            return currentState;
        }

        public void SetCurrentState(EnemyBehaviourState newState)
        {
            currentState = newState;
        }


        // Start is called before the first frame update
        void Start()
        {
            //use gameObject to reference whatever object the script is attached to. 
            enemy = gameObject.GetComponent<Enemy>();
            SetCurrentState(EnemyBehaviourState.Patrol);

        }


        void Update()
        {
            
            switch (GetCurrentState()) //Switching between the state machine's states
            {

                case EnemyBehaviourState.Patrol:
                    enemy.Patrol();

                    if (enemy.IsPlayerChaseable())   //if the player is within chase distance, switch to chase.
                    {
                        SetCurrentState(EnemyBehaviourState.Chase); // a setter for the current state.
                        GetCurrentState();                          // a getter for the current state. 
                    }
                    break;

                case EnemyBehaviourState.Chase:
                    enemy.Chase();

                    if (enemy.IsPlayerAttackable())//if the player is within attack distance, switch to attack.
                    {
                        SetCurrentState(EnemyBehaviourState.Attack);
                        GetCurrentState();
                    }
                    
                    if (!enemy.IsPlayerChaseable())//if the player leaves chase distance, return to patrol. 
                    {
                        SetCurrentState(EnemyBehaviourState.Patrol);
                        GetCurrentState();
                    }
                    break;  


                case EnemyBehaviourState.Attack:
                    enemy.Attack();
                    //halve enemy speed while in attacking state
                    if (!isSlow) //this will run ONCE, even though it is under the update function. Thank you booleans. 
                    {
                        isSlow = true;
                        enemy.speed = enemy.speed * 0.5f;
                    }
                   
                    if (!enemy.IsPlayerAttackable())//if the player leaves attack distance, return to chase.
                    {
                        //resume normal speed
                        isSlow = false;
                        enemy.speed = enemy.speed * 2f;
                        //change states
                        SetCurrentState(EnemyBehaviourState.Chase);
                        GetCurrentState();
                    } 
                        
                    if (enemy.TimeToBail())    //if health drops below 25%, switch to flee.
                    {
                        //resume normal speed
                          isSlow = false;
                         enemy.speed = enemy.speed * 2f;
                        //change states
                        SetCurrentState(EnemyBehaviourState.Flee);
                        GetCurrentState();
                    }
                        break;

                case EnemyBehaviourState.Flee:
                    enemy.Flee();
                    if (enemy.HealedYet())//if health goes above 50%, return to patrol. 
                    {
                        SetCurrentState(EnemyBehaviourState.Patrol);
                        GetCurrentState();
                    }
                    break;

            }

        }


    }
}
