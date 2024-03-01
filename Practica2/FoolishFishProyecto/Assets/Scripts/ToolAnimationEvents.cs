using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToolAnimationEvents : MonoBehaviour
{
    public UnityEvent use;
    public void Use()
    {
        use.Invoke();
    }
}
