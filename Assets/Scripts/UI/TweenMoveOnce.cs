using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMoveOnce : MonoBehaviour
{
    public float speed, delay;
    public Vector2 target;
    public Vector2 tempPos;

    // Start is called before the first frame update
    private void OnEnable()
    {
        tempPos = transform.position;
        StartCoroutine(moveObject());
        //PopoutEffect();
    }
    IEnumerator moveObject()
    {
        yield return new WaitForSecondsRealtime(delay);
        LeanTween.move(GetComponent<RectTransform>(), target, speed);//.setOnComplete(ResetObject);
    }

    void ResetObject()
    {
        gameObject.SetActive(false);
        transform.position = tempPos;   
    }
    void PopoutEffect() 
    {
        LeanTween.scale(GetComponent<RectTransform>(),new Vector3(1.25f,1.25f,1.25f),3f);
    }
    private void OnDisable()//added
    {
        ResetObject();
    }
}
