using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Writen by Jiahao Lei
public class Bullet : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
