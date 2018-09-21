using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWaveSafetyKeyInput : MonoBehaviour {
    PostQueue queue;
	// Use this for initialization
	void OnEnable () {
        queue = PostQueue.GetInstance;
	}
    private void OnDisable()
    {
        queue = null;
    }
    // Update is called once per frame
    void FixedUpdate () {
        if (queue != null)
        {
            string data = queue.GetData();
            if (data != string.Empty)
            {
                Bike4JoyStick.instance.InputJoystic(data);
            }
        }
    }
}
