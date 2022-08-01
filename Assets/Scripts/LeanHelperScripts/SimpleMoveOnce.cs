using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveOnce : MonoBehaviour
{
    public Vector2 moveTo;
    public float moveTime, delay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveObject());
    }
    IEnumerator MoveObject()
    {
        yield return new WaitForSecondsRealtime(delay);
        LeanTween.move(GetComponent<RectTransform>(), moveTo, moveTime);
    }
    
}
