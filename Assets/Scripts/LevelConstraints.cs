using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConstraints : MonoBehaviour
{

    public int playerHealth;
    public static LevelConstraints Instance;

    private void Awake()
    {
        Instance = this;
    }

}
