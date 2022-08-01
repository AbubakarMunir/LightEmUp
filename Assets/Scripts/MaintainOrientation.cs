using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainOrientation : MonoBehaviour
{
    Vector3 rotation;
    private void Awake()
    {
        rotation = transform.rotation.eulerAngles;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(rotation);
    }
}
