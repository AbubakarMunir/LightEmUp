using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 initialPos;
    Vector3 deathPos = new Vector3(0,0,-40);
    public GameObject activeObject;
    public bool moveToObject;
    public bool death;
    public ObjectsContainer objcontainer;
    public float waitToZoomOut=0;
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (StateManager.GetState()==StateManager.STATE.STATIC)
            return;
        if(death)
        {
            transform.position = Vector3.Lerp(transform.position, deathPos, 0.3f);
        }
        else if(StateManager.GetState() == StateManager.STATE.HANGING && waitToZoomOut>=1.5f) //&& Constants.blockCount>1)
        {
            if (objcontainer.IsObjectRemaining())
            {
                ObjectManager objmanager = objcontainer.ReturnObjectRemaining();
                transform.position = Vector3.Lerp(transform.position, new Vector3(objmanager.transform.position.x, objmanager.transform.position.y, initialPos.z + 5), 0.002f);
            }
        }

        else if(StateManager.GetState()==StateManager.STATE.JUMPING)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(GameManager.player.transform.position.x, GameManager.player.transform.position.y, initialPos.z+10), 0.01f);
        }
        else
        {
            if (StateManager.GetState() == StateManager.STATE.HANGING)
                waitToZoomOut += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, new Vector3(GameManager.player.transform.position.x, GameManager.player.transform.position.y, initialPos.z + 10), 0.01f);

        }
            

        //if(moveToObject)
        //{
        //    transform.position = Vector3.Lerp(transform.position, new Vector3(activeObject.transform.position.x,activeObject.transform.position.y,initialPos.z+5), 0.01f);
        //}
        //else if(death)
        //{
        //    transform.position = Vector3.Lerp(transform.position, deathPos, 0.3f);
        //}
        //else
        //{
        //    transform.position = Vector3.Lerp(transform.position, initialPos, 0.01f);
        //}
    }



}
