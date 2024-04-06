using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSerializer : MonoBehaviour
{
    JSONSerializer jsonSerializer = new JSONSerializer();
    void Start()
    {
		JumpStartEvent trackerEvent = new JumpStartEvent(1);

        Debug.Log(jsonSerializer.Serialize(trackerEvent));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
