using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestScene : MonoBehaviour
{
    [Inject]
    GameManager_Test gm;

    void Start()
    {
        Debug.Log("Call from: " + gm.GetPlayerSpeed.ToString());
        Debug.Log("Call from: " + gm._paramsLibrary.SomeParams);
    }
}
