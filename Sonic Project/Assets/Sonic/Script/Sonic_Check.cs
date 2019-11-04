using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonic_Check : MonoBehaviour
{
    private Sonic player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Sonic>();
    }
    //verifica lo que esta chocando
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
           player.grounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            
            player.grounded = false;
        }
    }
}

