using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public GameObject TarjetFollow;
    public Vector2 min_Camera_Pos, max_Camera_Pos;
    public float smoothTime;

    private Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        float posx = Mathf.SmoothDamp(transform.position.x, TarjetFollow.transform.position.x, ref velocity.x, smoothTime);
        float posy = Mathf.SmoothDamp(transform.position.y, TarjetFollow.transform.position.y, ref velocity.y, smoothTime);

        transform.position = new Vector3(
            Mathf.Clamp(posx, min_Camera_Pos.x, max_Camera_Pos.x),
            Mathf.Clamp(posy, min_Camera_Pos.y, max_Camera_Pos.y),transform.position.z
            );

    }
}

