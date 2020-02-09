using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapingAI : MonoBehaviour
{




    private void Awake()
    {

        GameManager.AllEscapingAI.Add(this);
        GameManager.AllEscapingTransform.Add(gameObject.transform);

        GameManager.EscapingCount++;

    }

    private void OnDisable()
    {

        GameManager.AllEscapingAI.Remove(this);
        GameManager.AllEscapingTransform.Remove(gameObject.transform);

    }

    private void OnDestroy()
    {

        GameManager.AllEscapingAI.Remove(this);
        GameManager.AllEscapingTransform.Remove(gameObject.transform);

        GameManager.EscapingCount--;

    }

    private void OnEnable()
    {

        GameManager.AllEscapingAI.Add(this);
        GameManager.AllEscapingTransform.Add(gameObject.transform);

    }


}
