using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDie : MonoBehaviour
{
    public void OnEnd()
    {
        gameObject.SetActive(false);
    }
}
