using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTextEffect : MonoBehaviour
{
    SpriteRenderer srenderer;
    
    private void Start()
    {
        srenderer = GetComponent<SpriteRenderer>();
    }
    public void ShowEffect(float delay)
    {
        StartCoroutine(ShowEffectAfterDelay(delay));   
    }

    IEnumerator ShowEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LeanTween.alpha(gameObject, 1, 0.01f);
        LeanTween.alpha(gameObject, 0, 1);
        LeanTween.moveY(gameObject, transform.position.y + 3, 1);
        StartCoroutine(SetInactive());
    }
    IEnumerator SetInactive()
    {
        yield return new WaitForSeconds(1.1f);
        gameObject.SetActive(false);
    }
}
