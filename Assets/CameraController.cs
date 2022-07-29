using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 initialPos;
    public GameObject activeObject;
    public bool moveToObject;
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveToObject)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(activeObject.transform.position.x,activeObject.transform.position.y,initialPos.z+5), 0.01f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, initialPos, 0.01f);
        }
    }
}
