using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRing : MonoBehaviour
{
    public Transform [] RingSpawn;
    
    public int CR = 10;
    public GameObject Rings;
    public Ring a;
    int counter = 0;
   

    // Start is called before the first frame update
    void Start()
    {
       
        GameObject SonicObject = GameObject.FindWithTag("Ring");

        if (SonicObject != null)
        {
            a = SonicObject.GetComponent<Ring>();

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
        if (counter <RingSpawn.Length)
            {
                for (int i = CR; i > 0; i--){
                // int counter = Random.Range(0,RingSpawn.Length);
                GameObject RS = (GameObject)Instantiate(Rings,RingSpawn[counter].transform.position,RingSpawn[counter].transform.rotation);
                a.Ring_Direction(counter);
                CR--;
                counter++;
                    if (counter>=RingSpawn.Length)
                    {
                        counter = 0;
                    }
                }
            }
        }
    }
}


