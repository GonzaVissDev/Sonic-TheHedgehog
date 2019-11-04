using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    private Rigidbody2D R2dE;
    private Animator e_anim;
    public GameObject animal_Prefab;
    // Start is called before the first frame update
    void Start()
    {
        R2dE = GetComponent<Rigidbody2D>();
        e_anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sonic")
        {
            if (collision.gameObject.GetComponent<Sonic>() != null)
            {
                Sonic Sonic_Object = collision.gameObject.GetComponent<Sonic>();
                if (Sonic_Object.Sstate == Sonic.SonicState.Spinning)
                {
                    e_anim.SetTrigger("Hit");
                    Invoke("SummonAnimal", 0f);
                }
                else if (Sonic_Object.Sstate == Sonic.SonicState.Moving)
                {
                   collision.SendMessage("SonicKnockBack", transform.position.x);
                }
            }
        }
    }
    void SummonAnimal()
    {
        Destroy(this.gameObject, 0.5f);
        Instantiate(animal_Prefab,transform.position, Quaternion.identity);
    }
}

