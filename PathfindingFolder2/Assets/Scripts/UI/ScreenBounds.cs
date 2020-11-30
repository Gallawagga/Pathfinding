using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -36f, 36f), Mathf.Clamp(transform.position.y, -20f, 19f), transform.position.z);    
    }
}
