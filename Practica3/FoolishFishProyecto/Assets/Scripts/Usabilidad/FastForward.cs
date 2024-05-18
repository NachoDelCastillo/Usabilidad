using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class FastForward : MonoBehaviour
{
    private TMPro.TMP_Dropdown _dropdown;

    private void Start()
    {
        _dropdown = GetComponent<TMPro.TMP_Dropdown>();
    }

    public void OnValueChanged()
    {
        int value = _dropdown.value;
        Time.timeScale = (value + 1) * 0.25f;
        Debug.Log("Time scale actual: " + Time.timeScale);
    }
}
