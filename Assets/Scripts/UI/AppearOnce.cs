using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearOnce : MonoBehaviour
{
    public GameObject objectToActivateAfterDelay;

    private void OnEnable()
    {
        
       
            StartCoroutine(SetActive());
       


    }


    IEnumerator SetActive()
    {
        yield return new WaitForSeconds(5);
        objectToActivateAfterDelay.SetActive(true);
    }
}
