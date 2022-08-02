using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateManager : MonoBehaviour
{
    Animator animator;
    string[] trigs = new string[3] { "jump", "idle", "hanging" };
    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void SetToHanging()
    {
        SetBoolsToFalse();
        animator.SetBool("hanging",true);
    }

   
    public void SetToJump()
    {
        SetBoolsToFalse();
        animator.SetBool("jump", true);
    }

    public void SetToStatic()
    {
        SetBoolsToFalse();
        animator.SetBool("idle", true);
    }



    void SetBoolsToFalse()
    {
        for(int i=0;i<trigs.Length;i++)
        {
            animator.SetBool(trigs[i],false);
        }
    }
}
