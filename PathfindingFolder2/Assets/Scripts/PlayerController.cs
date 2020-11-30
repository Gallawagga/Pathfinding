using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pathfinding
{
    public class PlayerController : MonoBehaviour
    {

        Player MyPlayer;

        private void Start()
        {
            MyPlayer = GetComponent<Player>();
        }

        private void Update()
        {
            Movement();
        }

        private void Movement()
        {
            //so the code here basically states when the 'horizontal' binded keys are pressed the input is equal to -1 (left) and 1 (right).
            //ditto for the 'vertical input, they're pushing the player component up or down, left or right.
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            //MyPlayer.Move(moveInput);
        }


    }
}
