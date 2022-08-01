using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAppearOnce : MonoBehaviour
{
    public GameObject objectToActivateAfterDelay;

    private void OnEnable()
    {
        if (PreferenceManager.GetIsFreeSkinRewarded() != 0)
        {
            StartCoroutine(SetActive());
        }

       
    }
    

    IEnumerator SetActive()
    {
        yield return new WaitForSeconds(5);
        Debug.LogError("object activtaed");
        objectToActivateAfterDelay.SetActive(true);
    }
}
