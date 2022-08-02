using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    CharacterBehaviour characterBehaviour;
    Rigidbody2D rb;
    CameraController camController;
    AnimatorStateManager animatorStateManager;
    static StateManager instance;
    public enum STATE
    {
        STATIC = 0,
        FREE = 1,
        CANTJUMP = 2,
        GROUNDED = 3,
        HANGING = 4,
        DEAD = 5,
        JUMPING =6
    }

    private void Awake()
    {
        characterBehaviour = GetComponent<CharacterBehaviour>();
        animatorStateManager = GetComponent<AnimatorStateManager>();
        rb = GetComponent<Rigidbody2D>();
        camController = FindObjectOfType<CameraController>();
        instance = this;
    }

    private static STATE currentState;
    

    public static void SetState(STATE state)
    {
        if(state==STATE.FREE)
        {
            instance.SetToFree();
        }
        else if(state==STATE.HANGING)
        {
            instance.animatorStateManager.SetToHanging();
        }
        else if(state==STATE.GROUNDED)
        {
            instance.animatorStateManager.SetToStatic();
            instance.rb.bodyType = RigidbodyType2D.Static;
        }
        else if(state == STATE.JUMPING)
        {
            instance.animatorStateManager.SetToJump();
        }
        currentState = state;
    }

    public static STATE GetState()
    {
        return currentState;
    }

    void SetToFree()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        characterBehaviour.jcount = 1;
        GameManager.player.transform.parent = null;
        camController.moveToObject = false;
    }

    void SetToHanging()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        characterBehaviour.jcount = 1;
        GameManager.player.transform.parent = null;
        camController.moveToObject = false;
    }
}
