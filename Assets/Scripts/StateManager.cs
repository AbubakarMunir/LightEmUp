using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum STATE
    {
        STATIC = 0,
        FREE = 1,
        CANTJUMP = 2,
        GROUNDED = 3,
        HANGING = 4,
        DEAD = 5

    }

    private static STATE currentState;
    

    public static void SetState(STATE state)
    {
        currentState = state;
    }

    public static STATE GetState()
    {
        return currentState;
    }


}
