using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    CameraController camController;
    [SerializeField] private Rigidbody2D rb;
    private Animator animator;
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
        camController = FindObjectOfType<CameraController>();
        currentState = STATE.STATIC;
        rb = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //ClampPositions();
        transform.rotation = Quaternion.identity;
    }

    public void JumpRight()
    {
        CheckAndUpdateState();
        if (currentState == STATE.CANTJUMP)
            return;
        transform.localScale = new Vector3(1, 1, 1);
        animator.SetBool("flip",true);
        animator.SetBool("idle", false);
        rb.AddForce(new Vector2(1.5f, 10), ForceMode2D.Impulse);
    }

    public void JumpLeft()
    {
        CheckAndUpdateState();
        if (currentState == STATE.CANTJUMP)
            return;
        transform.localScale = new Vector3(-1, 1, 1);
        animator.SetBool("flip", true);
        animator.SetBool("idle", false);
        rb.AddForce(new Vector2(-1.5f, 10), ForceMode2D.Impulse);
    }

    private void CheckAndUpdateState()
    {
        if (currentState == STATE.STATIC)
        {
            //animator.SetBool("idle", true);
            currentState = STATE.FREE;
            rb.bodyType = RigidbodyType2D.Dynamic;
            jcount = 1;
            GameManager.player.transform.parent = null;
            camController.moveToObject = false;
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
        animator.SetBool("idle", true);
        animator.SetBool("flip", false);
        
    }


    public void Flip()
    {

    }
    
}



//_current = Mathf.MoveTowards(_current, _target, speed * Time.deltaTime);
//transform.position = Vector3.Lerp(Vector3.zero, goal, curve.Evaluate(_current));