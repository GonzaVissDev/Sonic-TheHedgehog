using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonic : MonoBehaviour
{
    public float speed = 25f;
    public float maxspeed = 5f;
    private Rigidbody2D rb2d;
    private Animator anim;
    public bool grounded;
    public float sonic_JumpForce = 5.0f;
    private SpriteRenderer m_SpriteRenderer;
    public enum SonicState {Moving,Spinning, Hit,Death};
    public SonicState Sstate = SonicState.Moving;
    private bool sonic_Jump;
    private bool sonic_Slowroll = false;
    private bool sonic_Fastroll = false;
    private bool sonic_Down = false;
    private bool sonic_Up = false;
    private bool Control_Stop = true;
    private float sonic_Direction = 0f;
  

    //Rings
    public Transform[] RingSpawn;
    public int CR = 10;
    public GameObject Rings;
    public Ring a;
    int counter = 0;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        //Configuracion de Animaciones (Mantiene Actualizado los valores necesarios para ser ejecutada la animacion)

        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x)); //Valor absoluto = siempre positivo.
        anim.SetBool("Ground", grounded);
        anim.SetBool("Fast_Roll", sonic_Fastroll);
        anim.SetBool("Up", sonic_Up);
        anim.SetBool("Down", sonic_Down);

        //Configuracion de Teclado

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {  sonic_Down = true;//Speed = 0 evita que el personaje se mueva mientra este mirando hacia abajo.     
        }//Sonic mirando para abajo.
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        { sonic_Down = false;}// Sonic ya no mira para abajo.
        
        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
        { sonic_Up = true;} // Sonic Mira para Arriba
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        { sonic_Up = false; }

        //Sonic girando mientras camina.
        if (Input.GetKeyDown(KeyCode.DownArrow) && speed > 0.1f)
        { sonic_Slowroll = true;}
        
        //Sonic Saltando.
        if (Input.GetKeyDown(KeyCode.H) && grounded)
        { sonic_Jump = true; }
       

        //Sonic Giro de impulso.
        if (Input.GetKeyDown(KeyCode.J) && sonic_Down == true)
        {anim.SetBool("Fast_Roll", true);}
        else if (Input.GetKeyUp(KeyCode.J) && sonic_Down == true)
        { sonic_Fastroll = true; }

    }

    //Ejecucion en tiempo correcto
    private void FixedUpdate()
    {
        float Sonic_x = Input.GetAxis("Horizontal");
        if (!Control_Stop) Sonic_x = 0;
        rb2d.AddForce(Vector2.right * speed * Sonic_x);
        float limitedSpeed = Mathf.Clamp(rb2d.velocity.x, -maxspeed, maxspeed);
        rb2d.velocity = new Vector2(limitedSpeed, rb2d.velocity.y);

   //Esta configuracion sirve para cambiar la direccion del personaje,dependiendo en donde este mirando.
        if (Sonic_x > 0.1f)
        {
            sonic_Direction = 1f;
            transform.localScale = new Vector3(4f, 4f, 4f);
        }
        if (Sonic_x < -0.1f)
        {
            sonic_Direction = -1f;
            transform.localScale = new Vector3(-4f, 4f, 4f);
         //PD:donde declaramos "new vector3" por lo general se utiliza (1,1,1).En este caso es asi,porque los sprites que consegui no estaban en la misma escala.
        }
        
        //Configuraciones de Movimientos

        //Salto
        if (sonic_Jump == true)
        {
            if (Sstate != SonicState.Death)
            {
                Sstate = SonicState.Spinning;
            rb2d.AddForce(Vector2.up * sonic_JumpForce , ForceMode2D.Impulse);
            //forcemode2d.impulse es para no manejar numeros grandes.
            sonic_Jump = false;
            Invoke("Cancel_Roll", 1.5f);

            }
        }
        //Hacer un roll mientras Corre
        if (sonic_Slowroll == true)
        {
            float sonic_Speed =speed;
            Sstate = SonicState.Spinning;
            Control_Stop = false;
            anim.SetBool("Slow_Roll", sonic_Slowroll);
            speed -= 1f;
            rb2d.AddForce(new Vector2(sonic_Speed * sonic_Direction, 0f));

            if (speed <= 0f)
            {
                sonic_Slowroll = false;
                Invoke("Cancel_Roll", 0f); //Invocamos Cancel_Roll para recuperar nuestra speed inicial.
                anim.SetBool("Slow_Roll", sonic_Slowroll);
            }
        }
        //Roll Cargado 

        if (sonic_Fastroll == true)
        {
            Sstate = SonicState.Spinning;
            rb2d.AddForce(new Vector2(800 * sonic_Direction, 0f));
            Control_Stop = false;
            Invoke("Cancel_Roll", 1f);
        }
    }
    // Ahora configuremos para cuando sonic colisiona con un enemigo.
    public void SonicKnockBack (float enmyPos)
    {
        if (Sstate != SonicState.Spinning)
        {
            sonic_Jump = true;
            anim.SetTrigger("Sonic_Hit");
            float side = Mathf.Sign(enmyPos - transform.position.x);//el mathf.sign devuelve -1 /0 /1 eso nos permitira saber en que lado tiene que saltar.
            Sstate = SonicState.Hit;
            Control_Stop = false;
            rb2d.AddForce(Vector2.left * side *10f, ForceMode2D.Impulse);
            Invoke("Cancel_Roll", 1.5f);

            // En el caso de que no tenga monedas.
            if (CR == 0)
            {
                sonic_Jump = true;
                anim.SetTrigger("Sonic_Death");
                Sstate = SonicState.Death;
            }
            //Utilizaremos el siguiente for para invocar todas nuestras monedas hasta quedarnos en "0".
            if (counter < RingSpawn.Length)
            {
                for (int i = CR; i > 0; i--)
                {   GameObject RS = (GameObject)Instantiate(Rings, RingSpawn[counter].transform.position, RingSpawn[counter].transform.rotation);
                    a.Ring_Direction(counter);
                    CR--;
                    counter++;
                    if (counter >= RingSpawn.Length)
                    {
                        //Este "if" nos permitira invocar desde el principio de los transform.position que creamos.
                        counter = 0;
                    }
                }
            }

            if (Sstate == SonicState.Death)
            {
                transform.Translate(0f, 3f, 0f * Time.deltaTime);
                rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
                if (transform.position.y >= 3f)
                {
                    transform.Translate(0f, -50f, 0f * Time.deltaTime);
                }
            }
        }
    }

    void Cancel_Roll()
    {
     speed = 25f;
     sonic_Fastroll = false;
     Control_Stop = true;
     Sstate = SonicState.Moving;
   }

}

