using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Popup : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TMP_Text titleText;
    public TMP_Text descText;
    public Button closeBtn;
    public RectTransform optionsRootTransform;

    [Header("Settings")]
    [SerializeField] private RectTransform dragArea;
    [SerializeField] private bool clampToCanvas = true;

    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector2 offset;

    private bool dragging;


    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if (!dragging)
        {
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Vars.Instance.input.mousePos,
            Camera.main,
            out Vector2 localPoint
        );

        Vector2 targetPos = localPoint - offset;

        if (clampToCanvas)
        {
            targetPos = ClampToCanvas(targetPos);
        }

        rectTransform.anchoredPosition = targetPos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }

    private Vector2 ClampToCanvas(Vector2 pos)
    {
        RectTransform canvasRect = canvas.transform as RectTransform;

        Vector2 min = canvasRect.rect.min - rectTransform.rect.min;
        Vector2 max = canvasRect.rect.max - rectTransform.rect.max;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        return pos;
    }
}