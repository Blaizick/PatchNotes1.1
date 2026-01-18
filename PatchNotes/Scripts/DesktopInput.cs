using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DesktopInput : MonoBehaviour
{
    public InputSystem_Actions actions;

    public Vector2 mousePos;
    public Vector2 mouseWorldPos;

    public List<InputAction> speedActions;

    [NonSerialized] public float dragStartTime;
    [NonSerialized] public bool dragging;
    private Complex m_Complex0;
    private Complex m_Complex1;

    [NonSerialized] public bool selectingComplexesForChef;
    [NonSerialized] public Chef selectedChef;

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
            if (actions.Player.UpArrow.IsPressed())
            {
                Vars.Instance.Restart();
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
        
        if (selectedChef != null && selectedChef.dead)
        {
            selectingComplexesForChef = false;
            selectedChef = null;
        }

        if (actions.Player.LMB.WasPressedThisFrame())
        {
            if (selectingComplexesForChef)
            {
                Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorldPos);

                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent<Complex>(out var c) && c.IsChefAllowed)
                    {
                        selectedChef?.SwitchComplex(c);
                        break;
                    }
                }
            }
            else
            {
                Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorldPos);

                m_Complex0 = null;
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent<Complex>(out var c))
                    {
                        m_Complex0 = c;
                        dragging = true;
                        dragStartTime = Time.time;
                        m_Complex0.OnPointerDown();
                        break;
                    }
                }
            }
        }
        if (actions.Player.LMB.WasReleasedThisFrame())
        {
            if (dragging)
            {
                Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorldPos);

                m_Complex1 = null;
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent<Complex>(out var c))
                    {
                        if (Time.time - dragStartTime < 0.2f)
                        {
                            if (!EventSystem.current.IsPointerOverGameObject() || EventSystem.current.gameObject
                                 || EventSystem.current.gameObject.layer != Vars.Instance.layerMasks.uiMask)
                            {
                                m_Complex0.OnPointerClick();
                            }
                        }
                        m_Complex1 = c;
                        break;
                    }
                }
                
                dragging = false;
                m_Complex0?.OnPointerUp(m_Complex1);
            }
        }

        foreach (var i in Vars.Instance.buildSystem.complexes)
        {
            i.selectionFrameRoot.SetActive(false);
        }
        if (selectedChef != null && selectingComplexesForChef)
        {
            foreach (var i in selectedChef.complexes)
            {
                i.selectionFrameRoot.SetActive(true);
            }
        }

        Camera.main.transform.position = (Vector2)Camera.main.transform.position + 
            (actions.Player.Move.ReadValue<Vector2>() * Time.deltaTime * 3 * Camera.main.orthographicSize);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -10);
        Camera.main.orthographicSize += -actions.Player.Scroll.ReadValue<float>() * 0.5f;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 2, 12);
    }

    public void SwitchSelectingComplexesForChefState(Chef chef)
    {
        if (selectedChef == chef)
        {
            selectingComplexesForChef = !selectingComplexesForChef;
        }
        else
        {
            selectingComplexesForChef = true;
        }
        selectedChef = selectingComplexesForChef ? chef : null;
    }
}