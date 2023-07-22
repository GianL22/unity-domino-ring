using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 0.1f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
