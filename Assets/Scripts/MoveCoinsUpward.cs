using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCoinsUpward : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        transform.DOLocalMoveY(transform.localPosition.y + 3, 0.7f);
        AudioManager.Instance.PlayCharacterHitCoin();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
