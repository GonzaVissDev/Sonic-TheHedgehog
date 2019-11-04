using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSript : MonoBehaviour
{
    private Rigidbody2D rgAnimal;
     Animator anim;
   
  
    void Start()
    {
       rgAnimal = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

   
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if ( collision.gameObject.tag == "Ground")
        {
            anim.SetTrigger("Run");
          rgAnimal.AddForce(Vector2.left * 1f, ForceMode2D.Impulse);
           Invoke("DestroyObject", 5f);
        }
    }

    void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
