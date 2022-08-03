using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsContainer : MonoBehaviour
{
    public List<ObjectManager> objs = new List<ObjectManager>();

    private void Awake()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            objs.Add(transform.GetChild(i).GetComponent<ObjectManager>());
        }
    }
   
    public bool IsObjectRemaining()
    {
        foreach(ObjectManager o in objs)
        {
            if (!o.caughtOnce)
                return true;
        }

        return false;
    }

    public ObjectManager ReturnObjectRemaining()
    {
        foreach (ObjectManager o in objs)
        {
            if (!o.caughtOnce)
                return o;
        }

        return null;
    }
}
