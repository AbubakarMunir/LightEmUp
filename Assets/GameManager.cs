using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameObject player;

    public GameObject _player;

    public GameObject startButton;

    private void Awake()
    {
        player = _player;
    }

    public void EnableCharacter()
    {
        _player.transform.GetChild(0).gameObject.SetActive(true);
        _player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        startButton.SetActive(false);
    }

    public void Replay()
    {
        SceneManager.LoadScene("Game");
    }
}
