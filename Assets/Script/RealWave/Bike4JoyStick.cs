using System;
using UnityEngine;

public class Bike4JoyStick : MonoBehaviour
{
    public bool LUp = false;
    public bool LDown = false;
    public bool LLeft = false;
    public bool LRight = false;
    public bool LCercle = false;
    public bool LRectangle = false;

    public bool RUp = false;
    public bool RDown = false;
    public bool RLeft = false;
    public bool RRight = false;
    public bool RCercle = false;
    public bool RRectangle = false;

    public int xPos = 50;
    public int yPos = 50;
    public static Bike4JoyStick instance;
    public bool debug;


    private static string result = "";


    public void OnEnable()
    {
        instance = this;
    }

    public void OnDisable()
    {
        if (instance != null)
            instance = null;
    }


    public string InputJoystic(string commend)
    {
        if (commend.Contains("IU"))
        {
            if (LDown)
            {
                LDown = false;
                Bike4JoyStickEvent.TriggerEvent("LDown_KeyUp");
            }
            LUp = true;
            result = "IU";
            Bike4JoyStickEvent.TriggerEvent("LUp_KeyDown");
            //leftJoysticUpKeyDownEvent();
        }
        else if (commend.Contains("Iu"))
        {
            LUp = false;
            Bike4JoyStickEvent.TriggerEvent("LUp_KeyUp");
            result = "Iu";
        }
        else if (commend.Contains("ID"))
        {
            if (LUp)
            {
                LUp = false;
                Bike4JoyStickEvent.TriggerEvent("LUp_KeyUp");
            }
            LDown = true;
            result = "ID";
            Bike4JoyStickEvent.TriggerEvent("LDown_KeyDown");
            //leftJoysticDownKeyDownEvent();
        }
        else if (commend.Contains("Id"))
        {
            LDown = false;
            result = "Id";
            Bike4JoyStickEvent.TriggerEvent("LDown_KeyUp");
        }
        else if (commend.Contains("IL"))
        {
            if (LRight)
            {
                LRight = false;
                Bike4JoyStickEvent.TriggerEvent("LRight_KeyUp");
            }
            LLeft = true;
            Bike4JoyStickEvent.TriggerEvent("LLeft_KeyDown");
            result = "IL";
        }
        else if (commend.Contains("Il"))
        {
            LLeft = false;
            result = "Il";
            Bike4JoyStickEvent.TriggerEvent("LLeft_KeyUp");
        }
        else if (commend.Contains("IR"))
        {
            if (LLeft)
            {
                LLeft = false;
                Bike4JoyStickEvent.TriggerEvent("LLeft_KeyUp");
            }
            LRight = true;
            Bike4JoyStickEvent.TriggerEvent("LRight_KeyDown");
            result = "IR";
        }
        else if (commend.Contains("Ir"))
        {
            LRight = false;
            Bike4JoyStickEvent.TriggerEvent("LRight_KeyUp");
            result = "Ir";
            //leftJoysticRightKeyUpEvent();
        }
        else if (commend.Contains("IA"))
        {
            LCercle = true;
            Bike4JoyStickEvent.TriggerEvent("LCercle_KeyDown");
            result = "IA";
        }
        else if (commend.Contains("Ia"))
        {
            LCercle = false;
            Bike4JoyStickEvent.TriggerEvent("LCercle_KeyUp");
            result = "Ia";
            //leftJoysticCircleButtonKeyUpEvent();
        }
        else if (commend.Contains("IB"))
        {
            LRectangle = true;
            Bike4JoyStickEvent.TriggerEvent("LRectangle_KeyDown");
            result = "IB";
        }
        else if (commend.Contains("Ib"))
        {
            LRectangle = false;
            Bike4JoyStickEvent.TriggerEvent("LRectangle_KeyUp");
            result = "Ib";
            //leftJoysticRectangleButtonKeyUpEvent();
        }
        if (commend.Contains("JU"))
        {
            if (RDown)
            {
                RDown = false;
                Bike4JoyStickEvent.TriggerEvent("RDown_KeyUp");
            }
            RUp = true;
            Bike4JoyStickEvent.TriggerEvent("RUp_KeyDown");
            result = "JU";
            //rightJoysticUpKeyDownEvent();
        }
        else if (commend.Contains("Ju"))
        {
            RUp = false;
            Bike4JoyStickEvent.TriggerEvent("RUp_KeyUp");
            result = "Ju";
            //rightJoysticUpKeyUpEvent();
        }
        else if (commend.Contains("JD"))
        {
            if (RUp)
            {
                RUp = false;
                Bike4JoyStickEvent.TriggerEvent("RUp_KeyUp");
            }
            RDown = true;
            Bike4JoyStickEvent.TriggerEvent("RDown_KeyDown");
            result = "JD";
            //rightJoysticDownKeyDownEvent();
        }
        else if (commend.Contains("Jd"))
        {
            RDown = false;
            Bike4JoyStickEvent.TriggerEvent("RDown_KeyUp");
            result = "Jd";
            //rightJoysticDownKeyUpEvent();
        }
        else if (commend.Contains("JL"))
        {
            if (RRight)
            {
                RRight = false;
                Bike4JoyStickEvent.TriggerEvent("RRight_KeyUp");
            }
            RLeft = true;
            Bike4JoyStickEvent.TriggerEvent("RLeft_KeyDown");
            result = "JL";
        }
        else if (commend.Contains("Jl"))
        {
            RLeft = false;
            Bike4JoyStickEvent.TriggerEvent("RLeft_KeyUp");
            result = "Jl";
            //rightJoysticLeftKeyUpEvent();
        }
        else if (commend.Contains("JR"))
        {
            if (RLeft)
            {
                RLeft = false;
                Bike4JoyStickEvent.TriggerEvent("RLeft_KeyUp");
            }
            RRight = true;
            Bike4JoyStickEvent.TriggerEvent("RRight_KeyDown");
            result = "JR";
        }
        else if (commend.Contains("Jr"))
        {
            RRight = false;
            Bike4JoyStickEvent.TriggerEvent("RRight_KeyUp");
            result = "Jr";
            //rightJoysticRightKeyUpEvent();
        }
        else if (commend.Contains("JA"))
        {
            RCercle = true;
            Bike4JoyStickEvent.TriggerEvent("RCercle_KeyDown");
            result = "JA";
        }
        else if (commend.Contains("Ja"))
        {
            RCercle = false;
            Bike4JoyStickEvent.TriggerEvent("RCercle_KeyUp");
            result = "Ja";
            //rightJoysticCircleButtonKeyUpEvent();
        }
        else if (commend.Contains("JB"))
        {
            RRectangle = true;
            Bike4JoyStickEvent.TriggerEvent("RRectangle_KeyDown");
            result = "JB";
        }
        else if (commend.Contains("Jb"))
        {
            RRectangle = false;
            Bike4JoyStickEvent.TriggerEvent("RRectangle_KeyUp");
            result = "Jb";
            //rightJoysticRectangleButtonKeyUpEvent();
        }

        if (commend.IndexOf("A") == 0)
        {
            yPos = Convert.ToInt32(commend.Substring(1, commend.Length - 1));
        }
        if (commend.IndexOf("a") == 0)
        {
            xPos = Convert.ToInt32(commend.Substring(1, commend.Length - 1));
        }


        if (debug)
        {
            if (LUp)
            {
                Debug.Log("왼쪽조이스틱 : 위 UP");
            }
            if (LDown)
            {
                Debug.Log("왼쪽조이스틱 : 아래 UP");
            }
            if (LLeft)
            {
                Debug.Log("왼쪽조이스틱 : 왼쪽 UP");
            }
            if (LRight)
            {
                Debug.Log("왼쪽조이스틱 : 오른쪽 UP");
            }
            if (LCercle)
            {
                Debug.Log("왼쪽조이스틱 : 동그라미 UP");
            }
            if (LRectangle)
            {
                Debug.Log("왼쪽조이스틱 : 네모 UP");
            }


            if (RUp)
            {
                Debug.Log("오른쪽 조이스틱 : 위 UP");
            }
            if (RDown)
            {
                Debug.Log("오른쪽 조이스틱 : 아래 UP");
            }
            if (RLeft)
            {
                Debug.Log("오른쪽 조이스틱 : 왼쪽 UP");
            }
            if (RRight)
            {
                Debug.Log("오른쪽 조이스틱 : 오른쪽 UP");
            }
            if (RCercle)
            {
                Debug.Log("오른쪽 조이스틱 : 동그라미 UP");
            }
            if (RRectangle)
            {
                Debug.Log("오른쪽 조이스틱 : 네모 UP");
            }

        }

        return result;
    }
}