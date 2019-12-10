using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnim : MonoBehaviour
{
    //put this on the player

    public Animator anim;
    private Player player;// create a reference for the player script. This script utilizes casting
    private Vector3 previousPosition;
    public float curSpeed;

    private void Awake()
    {
        player = GetComponent<Player>();
        player.attack += Player_Attack;// allow the player script to access the player_attack function that is defined below
        player.walk += Player_Cancel;// same as above

    }
    private void Player_Attack()//basic anim functions
    {
        anim.SetTrigger("Attack");
    }
    
    private void Player_Cancel()
    {
        anim.SetTrigger("Cancel_1");
    }
    void Update()// to utilize float parameters in animator, this is what i do. However, feel free to use bool if you are more comfortable
    {
        //these are for walking animation
        Vector3 curMove = transform.position - previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;
        var agent = GetComponent<NavMeshAgent>();
        anim.SetFloat("Speed", curSpeed);
        
    }
}
