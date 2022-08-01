using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMove : MonoBehaviour
{
    public LeanTweenType moveType, rotateType;
    public bool canMove, canRotate;
    public Vector3 moveTo;
    public float rotateTo;
    public float moveSpeed, moveDelay;
    public float rotateSpeed, rotateDelay;

    private void OnEnable()
    {
        if (canMove)
            LeanTween.move(GetComponent<RectTransform>(), moveTo, moveSpeed);//.setLoopType(moveType).setDelay(moveDelay).setIgnoreTimeScale(true); ;
        if (canRotate)
            LeanTween.rotate(GetComponent<RectTransform>(), rotateTo, rotateSpeed).setLoopType(rotateType).setDelay(rotateDelay).setIgnoreTimeScale(true); ;

    }
}
