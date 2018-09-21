using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class RealWaveJoysticInputEvent : MonoBehaviour
{

    private UnityAction someListener;

    void Awake()
    {
        someListener = new UnityAction(SomeFunction);
    }

    void OnEnable()
    {
        Bike4JoyStickEvent.StartListening("test", someListener);
        Bike4JoyStickEvent.StartListening("Spawn", SomeOtherFunction);
        Bike4JoyStickEvent.StartListening("Destroy", SomeThirdFunction);
    }

    void OnDisable()
    {
        Bike4JoyStickEvent.StopListening("test", someListener);
        Bike4JoyStickEvent.StopListening("Spawn", SomeOtherFunction);
        Bike4JoyStickEvent.StopListening("Destroy", SomeThirdFunction);
    }
    
    void SomeFunction()
    {
        Debug.Log("Some Function was called!");
    }

    void SomeOtherFunction()
    {
        Debug.Log("Some Other Function was called!");
    }

    void SomeThirdFunction()
    {
        Debug.Log("Some Third Function was called!");
    }
}