using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastForward : MonoBehaviour
{
    private UnityEngine.UI.Slider _slider;

    private void Start()
    {
        _slider = GetComponent<UnityEngine.UI.Slider>();
    }

    public void OnValueChanged()
    {
        Time.timeScale = _slider.value;
        Debug.Log("Time scale actual: " + Time.timeScale);
    }
}
