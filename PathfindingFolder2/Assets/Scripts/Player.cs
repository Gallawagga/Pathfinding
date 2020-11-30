using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;

namespace Pathfinding
{
    public class Player : Creature
    {
        [Header("References")]
        [SerializeField] Animator animator;
        [SerializeField] SpriteRenderer mySpriteRenderer;
        [Header("Attack Variables")]
        [SerializeField] float attackReach;
        public LayerMask enemyLayers; // used for targeting.
        private Vector2 _moveVelocity;

        protected override void Start()
        {
            base.Start(); // 'base' calls the same function from the parent class
            
        }

        private void Update()
        {
            PlayerMove();

            if (Input.GetKeyDown("space")) // if space is pressed, attack.
            {
                PlayerAttack();
            }
        }

        public void PlayerMove()
        {
            //so the code here basically states when the 'horizontal' binded keys are pressed the input is equal to -1 (left) and 1 (right).
            //ditto for the 'vertical input, they're pushing the player component up or down, left or right.
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _moveVelocity = moveInput.normalized * speed;
            rigidBody.MovePosition(rigidBody.position + _moveVelocity * Time.fixedDeltaTime);
            if (Input.GetKeyDown(KeyCode.A))
            {
                // if the variable isn't empty (we have a reference to our SpriteRenderer)
                if (mySpriteRenderer != null)
                {
                    // flip the sprite
                    mySpriteRenderer.flipX = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                // flip the sprite
                mySpriteRenderer.flipX = false;
            }
        }
        public void PlayerAttack()
        {
            //just play the attack animation, don't bother with transitions, there's only 2 animation states and setting up transitions is a biatch.
            animator.Play("PlayerAttack");
            //deal damage to anyone nearby
            //physics function which generates a circle from a centrepoint (transform.position) to a radius (attackReach)
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackReach, enemyLayers);
            foreach (Collider2D mug in hitEnemies)
            {
                //deal damage
                mug.GetComponent<Enemy>().ReceiveDamage(damage);
            }

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, attackReach);
        }


    }

}