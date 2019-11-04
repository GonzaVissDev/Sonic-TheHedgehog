using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private Rigidbody2D R;
    private SpawnRing sr;
    Animator Ring_animator;
    public float DestroyRing = 3f;
    public int Direccion = 0;

    void Start()
    {
        sr = GetComponent<SpawnRing>();
        R = GetComponent<Rigidbody2D>();
        Ring_animator = GetComponent<Animator>();
      

        GameObject SonicObject = GameObject.FindWithTag("Sonic");

        if (SonicObject != null)
        {
           sr = SonicObject.GetComponent<SpawnRing>();

        }
        if (Direccion == 0)
        {
         R.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
        if (Direccion == 1 || Direccion ==2 || Direccion == 3)
        {
            R.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
        }
        if (Direccion == 4 || Direccion == 6 || Direccion == 6)
        {
            R.AddForce(Vector2.left* 10f, ForceMode2D.Impulse);
        }
        if (Direccion == 8 || Direccion == 9 )
        {
            R.AddForce(Vector2.down * 10f, ForceMode2D.Impulse);
        }
        //deshabilitamos los collider para que no colisione con sonic apenas salgan.
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(EnableBox(0.2F));
        Invoke("Destroy_Ring",DestroyRing); // Invocamos el metodo "Destroy_Ring" para Destruir el anillo un "X" tiempo.
    }


    //Detecta si sonic la puede recoger o no.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Sonic>() != null)
        {
            Sonic Sonic_Object = collision.gameObject.GetComponent<Sonic>();
            if (Sonic_Object.Sstate != Sonic.SonicState.Hit)
            {
                Destroy(this.gameObject);
            }
        }
    }
  
    //Este metodo sirve para saber en que direccion tiene que ser lanzado el ring cuando sonic es tocado por un enemigo.
    public void  Ring_Direction  (int s)
    {Direccion = s;}


    private void Destroy_Ring()
    {
        //Realiza una animacion y se autodestruye en 3 segundos.
        Ring_animator.SetFloat("Destroy", 0.3f);
        Destroy(this.gameObject,3f);
    }

    IEnumerator EnableBox(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
