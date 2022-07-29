using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    public enum STATE
    {
        STATIC = 0,
        FREE=1,
        CANTJUMP = 2,
    }

    public STATE currentState;
    public int jcount=1;
    private void Awake()
    {
        currentState = STATE.STATIC;
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //ClampPositions();
      
    }

    public void JumpRight()
    {
        CheckAndUpdateState();
        if (currentState == STATE.CANTJUMP)
            return;
        rb.AddForce(new Vector2(1.5f, 10), ForceMode2D.Impulse);
    }

    public void JumpLeft()
    {
        CheckAndUpdateState();
        if (currentState == STATE.CANTJUMP)
            return;
        rb.AddForce(new Vector2(-1.5f, 10), ForceMode2D.Impulse);
    }

    private void CheckAndUpdateState()
    {
        if (currentState == STATE.STATIC)
        {
            currentState = STATE.FREE;
            rb.bodyType = RigidbodyType2D.Dynamic;
            jcount = 1;
        }

        else if(currentState==STATE.FREE)
        {
            jcount++;
            if (jcount<=2)
            {         
                rb.bodyType = RigidbodyType2D.Static;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            if (jcount > 2)
            {
                currentState = STATE.CANTJUMP;
                //jcount = 0;
            }
                
            
        }

        
    }

    private void ClampPositions()
    {
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x,-3.8f,3.8f);
        currentPos.y = Mathf.Clamp(currentPos.y,-6,8);
        transform.position = currentPos;
    }

    public void StopHere()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }

    
}



//_current = Mathf.MoveTowards(_current, _target, speed * Time.deltaTime);
//transform.position = Vector3.Lerp(Vector3.zero, goal, curve.Evaluate(_current));