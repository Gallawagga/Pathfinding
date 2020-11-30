using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pathfinding
{
    public class Creature : MonoBehaviour //parent class which will be inherited by Player and Enemy. 
    {
        [Header("Creature Variables")]
        [SerializeField]
        protected float health = 100f;
        [SerializeField]
        protected float damage = 10f;
        [SerializeField]
        public float speed = 5f;
        [SerializeField]
        public HealthBar healthBar;
        protected float creatureMaxHealth;

        [Header("References")]
        [SerializeField] GameObject GOreference;

       
       
        protected Rigidbody2D rigidBody;

        protected virtual void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            creatureMaxHealth = health;
            healthBar.SetMaxHealth(creatureMaxHealth);

            StartCoroutine(ConstantHealing());

        }

        public void ReceiveDamage(float damageTaken)
        {
            //the creature receives damage from an attack
            health -= damageTaken;
            healthBar.SetHealth(health);

            if (health <= 0f)
            {
                GOreference.GetComponent<GOMenu>().EndGame(); //run the endgame function, located in the Game Over Menu script.
                //Destroy(gameObject);
            }

        }

        IEnumerator ConstantHealing() 
        {
            while (true)        //loops forever
                if (health / creatureMaxHealth < 1f)
                {
                    health += 2f;
                    yield return new WaitForSeconds(1f);  // wait for one second before recommencing coroutine code.
                }
                else { yield return null; }
        }

    }
}
