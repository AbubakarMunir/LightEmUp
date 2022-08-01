using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScale : MonoBehaviour
{
    //public float fromScale, toScale;
    public float time;
    public Vector3 toScale;
    public LeanTweenType tweenType;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(gameObject, toScale, time).setLoopPingPong();
    }

    
}
