using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSerializer : MonoBehaviour
{
    JSONSerializer jsonSerializer = new JSONSerializer();
    CSVSerializer CSVSerializer = new CSVSerializer();

    void Start()
    {
		JumpStartEvent trackerEvent = new JumpStartEvent(1);

        Debug.Log(CSVSerializer.Serialize(trackerEvent));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
