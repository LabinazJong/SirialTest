using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slopetest : MonoBehaviour {

    public _Sirial sirial;
    public Vector3 rot;
    float m_fRot;
    bool CubeGo = false;

    void Start () {
        sirial = GameObject.Find("Sirial").GetComponent<_Sirial>();
        Invoke("Start_SendRollPitch", 1f);
    }

    void FixedUpdate()
    {
        if (CubeGo)
        {
            SendRollPitch();
        }
    }

    void Start_SendRollPitch()
    {
        sirial.SendVal('G', 7);
        sirial.SendVal('I', 7);
        sirial.SendRoll(50);
        sirial.SendPitch(50);

        CubeGo = true;
    }

    float GetRot(float rot)
    {
        float r = rot;
        if (r > 180) r -= 360;
        if (r < -180) r += 360;
        return r;
    }

    void SendRollPitch()
    {
        rot = transform.eulerAngles;
        rot.y = Camera.main.transform.localEulerAngles.y;

        rot.y = GetRot(transform.eulerAngles.y) - GetRot(m_fRot);

        rot.x = GetRot(rot.x);
        rot.z = GetRot(rot.z);

        rot.x *= 5;
        rot.z *= 5;

        rot.x += 50;
        rot.z += 50;

        rot.x = 100 - rot.x;
        rot.z = 100 - rot.z;

        // 각도에 따라 시뮬레이터에 값 전달
        if (15f <= (int)(rot.x) && (int)(rot.x) <= 85f)
        {
            sirial.SendRoll((int)(rot.x));
        }
        if (15f <= (int)(rot.z) && (int)(rot.z) <= 85f)
        {
            sirial.SendPitch((int)(rot.z));
        }

        // Debug.Log("X값 : " + (int)(rot.x));
        // Debug.Log("Z값 : " + (int)(rot.z));

    }
}
