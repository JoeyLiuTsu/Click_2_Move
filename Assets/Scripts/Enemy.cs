using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //put this script on your enemies prefabs

    public GameObject explosion;// particle
    public void Die()// the player script calls this function to destory the target enemy.
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
