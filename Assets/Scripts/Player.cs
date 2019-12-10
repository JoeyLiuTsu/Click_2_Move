using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    //put this on the player

    public GameObject moveCommand;//particle
    public GameObject AttackCommand;//particle
    public GameObject enemy;//reference to target enemy
    public Vector3 target;
    public event Action attack;
    public event Action walk;
    public bool attacking;

    //player will only be attackin in the attacking states, use state machine to destinct btw a move command and a attack command
    public enum PlayerStates { Moving, Attacking }
    public static PlayerStates currentState;
    private void Update()
    {
        var agent = GetComponent<NavMeshAgent>();
        
        // if clicking on ground, then it is move
        // if clicking on enemy, then it is attack
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
               
                if (hit.collider.tag == "Ground")
                {
                    currentState = PlayerStates.Moving;
                    target = hit.point;
                    agent.SetDestination(target);// move to the clicked location
                    Instantiate(moveCommand, target, Quaternion.identity);// particle goes here
                    walk?.Invoke();// canceling attack animation by moving
                    StopAllCoroutines();//canceling attack effect
                }
                if (hit.collider.tag == "Enemy")
                {
                    currentState = PlayerStates.Attacking;

                    enemy = hit.collider.gameObject;// store the clicked enemy as the current target. Important for later use

                    target = hit.point;
                    agent.SetDestination(target);
                    Instantiate(AttackCommand, target, Quaternion.identity);
                    
                }
            }
        }
        // will only start attacking when is in attack command AND is within range
        if (currentState == PlayerStates.Attacking)
        {
            if (enemy != null)// important to not cause error after the target enemy is destroyed
            {
                float dist = Vector3.Distance(enemy.transform.position, transform.position);// this code claculates the distance between you and the target enemy
                if (dist <= 2f)
                {
                    agent.SetDestination(transform.position); //stop your movements
                    StartCoroutine(AutoAttack());// execute attack sequnce
                }
                else if (dist > 2f)// this prevent you from doing super long range force punch by clicking another enemy during the animation.
                {
                    StopAllCoroutines();
                }
            }
        }
      
    }

    private IEnumerator AutoAttack()// attack sequence
    {
        attack?.Invoke();// see PlayerAnim script line 18 and line 22 for reference
        yield return new WaitForSeconds(0.75f);// delay

        enemy.transform.SendMessage("Die", SendMessageOptions.DontRequireReceiver);// call the enemy's kill function to destroy enemy
        enemy = null;// avoid error after enemy is destroyed
        StopAllCoroutines();
        
    }

}
