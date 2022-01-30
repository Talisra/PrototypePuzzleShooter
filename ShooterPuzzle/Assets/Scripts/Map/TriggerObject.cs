using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    private bool isOn;

    public void TurnOn()
    {
        if (!isOn)
        {
            isOn = true;
            this.gameObject.SetActive(false);
        }
    }
}
