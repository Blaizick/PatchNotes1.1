using System;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
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

    public RectTransform breakSelectionImageRootTransform;
    public GameObject breakSelectionImageRoot;

    [NonSerialized] public bool breaking = false;
    [NonSerialized] public Vector2 breakSelection0;
    [NonSerialized] public Vector2 breakSelection1;

    public RectTransform tooltipTransform;

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

        {
            breakSelection1 = mousePos;
            float minX = Mathf.Min(breakSelection0.x, breakSelection1.x);
            float minY = Mathf.Min(breakSelection0.y, breakSelection1.y);
            float maxX = Mathf.Max(breakSelection0.x, breakSelection1.x);
            float maxY = Mathf.Max(breakSelection0.y, breakSelection1.y);
            Vector2 size = new Vector2(maxX - minX, maxY - minY);
            Vector2 pos = new Vector2(minX, minY) + size * 0.5f;
            Vector2 worldMin = Camera.main.ScreenToWorldPoint(new Vector2(minX, minY));
            Vector2 worldMax = Camera.main.ScreenToWorldPoint(new Vector2(maxX, maxY));
            Vector2 worldSize = worldMax - worldMin;
            Vector2 worldPos = worldMin + worldSize * 0.5f;
            if (actions.Player.Break.IsPressed() && !breaking)
            {
                breakSelection0 = mousePos;
            }
            if (!actions.Player.Break.IsPressed() && breaking)
            {
                List<Complex> complexes = new();
                var hits = Physics2D.OverlapBoxAll(worldPos, worldSize, 0);
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent<Complex>(out var c) && c.CanBreak)
                    {
                        complexes.Add(c);
                    }
                }
                if (complexes.Count > 0)
                {
                    Vars.Instance.ui.ShowConfirmDialog($"Are you sure you want to destroy {complexes.Count} complexes?", () =>
                    {
                        foreach (var i in complexes)
                        {
                            Vars.Instance.buildSystem.DestroyBuild(i);
                        }
                    }, null);    
                }
            }
            breaking = actions.Player.Break.IsPressed();
            breakSelectionImageRootTransform.sizeDelta = size;
            breakSelectionImageRootTransform.anchoredPosition = pos;
            breakSelectionImageRoot.SetActive(breaking);
        }

        Vars.Instance.tooltip.SetPosition(mousePos);
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