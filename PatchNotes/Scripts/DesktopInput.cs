using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopInput : MonoBehaviour
{
    public InputSystem_Actions actions;

    public Vector2 mousePos;
    public Vector2 mouseWorldPos;

    public List<InputAction> speedActions;

    public void Init()
    {
        actions = new();
        actions.Enable();

        speedActions = new()
        {
            actions.Player.Speed0,
            actions.Player.Speed1,
            actions.Player.Speed2,
            actions.Player.Speed3,
            actions.Player.Speed4,
        };
    }

    public void Update()
    {
        mousePos = actions.Player.MousePosition.ReadValue<Vector2>();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
    
        if (actions.Player.Ctrl.IsPressed() && actions.Player.U.IsPressed() && actions.Player.RightAlt.IsPressed())
        {
            if (actions.Player.LeftArrow.IsPressed())
            {
                Time.timeScale = 20f;
            }
            if (actions.Player.UpArrow.IsPressed())
            {
                Vars.Instance.Restart();
            }
            if (actions.Player.RightArrow.IsPressed())
            {
                Time.timeScale = 1f;
            }
        }

        for (int i = 0; i < speedActions.Count; i++)
        {
            if (speedActions[i].WasPerformedThisFrame())
            {
                Vars.Instance.speedSystem.SetSpeed(i);
            }
        }
        if (actions.Player.Pause.WasPerformedThisFrame())
        {
            Vars.Instance.speedSystem.ChangePauseState();
        }
    }
}