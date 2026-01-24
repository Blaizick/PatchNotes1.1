using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DesktopInput : MonoBehaviour
{
    public InputSystem_Actions actions;

    public Vector2 mousePos;
    public Vector2 mouseWorldPos;

    public List<InputAction> speedActions;

    private Complex m_Complex0;

    [NonSerialized] public bool selectingComplexesForChef;
    [NonSerialized] public Chef selectedChef;

    public RectTransform breakSelectionImageRootTransform;
    public GameObject breakSelectionImageRoot;

    [NonSerialized] public bool breaking = false;
    [NonSerialized] public Vector2 breakSelection0;
    [NonSerialized] public Vector2 breakSelection1;

    public RectTransform tooltipTransform;

    public GraphicRaycaster graphicRaycaster;

    [NonSerialized] public bool pointerOverUi;

    [NonSerialized] public bool moving;
    [NonSerialized] public Vector2 move0;
    [NonSerialized] public Vector2 move1;

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

    public void Restart()
    {
        selectingComplexesForChef = false;
        selectedChef = null;
    }

    public void Update()
    {
        mousePos = actions.Player.MousePosition.ReadValue<Vector2>();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current){position = mousePos}, results);
            pointerOverUi = false;
            foreach (var i in results)
            {
                if (i.gameObject != null && Utils.MaskContainsLayer(Vars.Instance.layerMasks.uiMask, i.gameObject.layer))
                {
                    pointerOverUi = true;
                    break;
                }
            }
        }

        if (actions.Player.Ctrl.IsPressed() && actions.Player.U.IsPressed() && actions.Player.RightAlt.IsPressed())
        {
            if (actions.Player.UpArrow.IsPressed())
            {
                Vars.Instance.Restart();
            }
            if (actions.Player.LeftArrow.IsPressed())
            {
                CheatsSystem.godMode = true;
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

        if (m_Complex0 && selectingComplexesForChef)
        {
            m_Complex0 = null;
        }

        if (actions.Player.LMB.WasPerformedThisFrame())
        {
            if (!pointerOverUi)
            {
                if (selectingComplexesForChef)
                {
                    Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorldPos);

                    foreach (var hit in hits)
                    {
                        if (Vars.Instance.buildSystem.CanHaveChef(hit.gameObject))
                        {
                            selectedChef?.SwitchComplex(hit.GetComponent<Complex>());
                            break;
                        }
                    }
                }
                else
                {
                    Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorldPos);

                    foreach (var hit in hits)
                    {
                        if (hit.TryGetComponent<Complex>(out var c))
                        {
                            if (m_Complex0 == null)
                            {
                                if (c.type.canHaveNextComplex)
                                {
                                    m_Complex0 = c;
                                }
                            }
                            else
                            {
                                if (m_Complex0 == c)
                                {
                                    m_Complex0 = null;
                                }
                                else if (c.type.canBeNextComplex)
                                {
                                    m_Complex0.SwitchConnecting(c);
                                }
                            }
                        }
                    }
                }    
            }
        }

        foreach (var i in Vars.Instance.buildSystem.complexes)
        {
            if (i.selectionFrameRoot)
            {
                i.selectionFrameRoot.SetActive(false);
            }
            if (i.circleSelectionRoot)
            {
                i.circleSelectionRoot.SetActive(false);
            }
        }
        if (selectedChef != null && selectingComplexesForChef)
        {
            foreach (var i in selectedChef.complexes)
            {
                i.selectionFrameRoot.SetActive(true);
            }
        }
        if (m_Complex0)
        {
            foreach (var i in m_Complex0.nextComplexes)
            {
                if (i.selectionFrameRoot)
                {
                    i.selectionFrameRoot.SetActive(true);
                }
            }
            if (m_Complex0.circleSelectionRoot)
            {
                m_Complex0.circleSelectionRoot.SetActive(true);
            }
        }
        
        var cam = Vars.Instance.cam;
        if (actions.Player.Move.IsPressed())
        {
            cam.transform.position = (Vector2)cam.transform.position - (actions.Player.MouseMoveDelta.ReadValue<Vector2>() * 0.01f * cam.Lens.OrthographicSize / 4);
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -10);
        }
        cam.Lens.OrthographicSize += -actions.Player.Scroll.ReadValue<float>() * 0.5f;
        cam.Lens.OrthographicSize = Mathf.Clamp(cam.Lens.OrthographicSize, 2, 12);

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
                List<GameObject> breaks = new();
                var hits = Physics2D.OverlapBoxAll(worldPos, worldSize, 0);
                foreach (var hit in hits)
                {
                    if (Vars.Instance.buildSystem.CanBreak(hit.gameObject))
                    {
                        breaks.Add(hit.gameObject);
                    }
                }
                if (breaks.Count > 0)
                {
                    Vars.Instance.ui.ShowConfirmDialog($"Are you sure you want to destroy {breaks.Count} complexes?", () =>
                    {
                        foreach (var i in breaks)
                        {
                            Vars.Instance.buildSystem.Break(i);
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
    
        if (actions.Player.PauseMenu.WasPerformedThisFrame())
        {
            Vars.Instance.ui.menuUi.root.SetActive(!Vars.Instance.ui.menuUi.root.activeInHierarchy);
        }
    }

    public void SetSelectingComlexesForChefState(Chef chef, bool v)
    {
        selectedChef = v ?  chef : null;
        selectingComplexesForChef = v;
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