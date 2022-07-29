using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameObject player;

    public GameObject _player;

    private void Awake()
    {
        player = _player;
    }
}
