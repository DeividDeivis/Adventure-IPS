using System;
using UnityEngine;

public class SaveTotemController : MonoBehaviour
{
    private InputSystem_Actions _Inputs;
    private bool inputActive = false;

    void Awake()
    {
        _Inputs = new InputSystem_Actions();

        _Inputs.Player.Interact.started += (ctx) => inputActive = true;
        _Inputs.Player.Interact.canceled += (ctx) => inputActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            DataManager.instance.SaveGameData();
    }
}
