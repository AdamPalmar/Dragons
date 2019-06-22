using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject knightRagdoll;
    
    public void Destroy()
    {
        Instantiate(knightRagdoll, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
