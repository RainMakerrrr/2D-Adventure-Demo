using System.Collections;
using System.Collections.Generic;
using TimeSystem;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        StartCoroutine(TimeManager.Instance.SleepHandler(8));
        Debug.Log("sleep" + "  " +TimeManager.Instance.GameTime.Hour);
    }
}
