using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    CameraController camController;
    [SerializeField] private Rigidbody2D rb;
    AnimatorStateManager animatorStateManager;
    //private Animator animator;
    public int jcount=1;
    private void Awake()
    {
        camController = FindObjectOfType<CameraController>();
        animatorStateManager = GetComponent<AnimatorStateManager>();
        StateManager.SetState(StateManager.STATE.STATIC);
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
       
    }

  
    void Update()
    {
        transform.rotation = Quaternion.identity;
        if(transform.position.y<-4 &&jcount<=0)
        {
            camController.death = true;
            camController.moveToObject = false;
        }
        if(!Physics.Raycast(transform.position, -Vector3.up, 0.2f) && StateManager.GetState()==StateManager.STATE.GROUNDED)
        {
            animatorStateManager.SetToHanging();
            StateManager.SetState(StateManager.STATE.HANGING);
            
        }
        else if(StateManager.GetState() == StateManager.STATE.GROUNDED)
        {
            animatorStateManager.SetToStatic();
            
        }

    }

    public void JumpRight()
    {
        CheckAndUpdateState();
        if (StateManager.GetState() == StateManager.STATE.CANTJUMP)
            return;
        transform.localScale = new Vector3(1, 1, 1);
        animatorStateManager.SetToJump();
        rb.AddForce(new Vector2(1.5f, 10), ForceMode2D.Impulse);
    }

    public void JumpLeft()
    {
        CheckAndUpdateState();
        if (StateManager.GetState() == StateManager.STATE.CANTJUMP)
            return;
        transform.localScale = new Vector3(-1, 1, 1);
        animatorStateManager.SetToJump();
        rb.AddForce(new Vector2(-1.5f, 10), ForceMode2D.Impulse);
    }

    private void CheckAndUpdateState()
    {
        if (StateManager.GetState() == StateManager.STATE.STATIC || StateManager.GetState() == StateManager.STATE.GROUNDED || StateManager.GetState() == StateManager.STATE.HANGING)
        {
            StateManager.SetState(StateManager.STATE.FREE);
            rb.bodyType = RigidbodyType2D.Dynamic;
            jcount = 1;
            GameManager.player.transform.parent = null;
            camController.moveToObject = false;
        }

        else if(StateManager.GetState() == StateManager.STATE.FREE)
        {
            jcount++;
            if (jcount<=2)
            {         
                rb.bodyType = RigidbodyType2D.Static;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            if (jcount > 2)
            {
                StateManager.SetState(StateManager.STATE.CANTJUMP);
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
        animatorStateManager.SetToStatic();
        
    }


    public void Flip()
    {

    }
    
}



