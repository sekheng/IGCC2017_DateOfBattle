using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subscrube : MonoBehaviour {

    //void OnEnable()
    //{
    //    ObserverSystemScript.Instance.SubscribeEvent("LOL", saywhat);
    //}

    private void OnDisable()
    {
        ObserverSystemScript.Instance.UnsubscribeEvent("LOL", saywhat);
    }

    void Start()
    {
        ObserverSystemScript.Instance.SubscribeEvent("LOL", saywhat); ObserverSystemScript.Instance.TriggerEvent("LOL");
    }

    void saywhat()
    {
        print("lol");
    }
}
