using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
    }
}
