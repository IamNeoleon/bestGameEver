using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Collider : MonoBehaviour
{
    private Behaviour behaviourScript;

    private void Start()
    {
        behaviourScript = GetComponentInParent<Behaviour>();
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.CompareTag("car"))
        {
            behaviourScript.runAway();
        }
    }
}
