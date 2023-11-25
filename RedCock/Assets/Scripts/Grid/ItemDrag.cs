using RedCock.Gameplay;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public int Grade { get; private set; } = 0;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image itemVisual;

    private Slot currentSlot;
    private Vector3 initialPosition;
    private float scaleFactor;

    private GridManager gridManagerRef;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Init(float scaleFactor, GridManager gridRef)
    {
        this.scaleFactor = scaleFactor;
        SetGrade();
        gridManagerRef = gridRef;
    }

    private void SetGrade()
    {
        var sprite = GameManager.Instance.GetItemSprite(Grade);
        itemVisual.sprite = sprite;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
        GameEventsManager.Instance.PostEvent("beginDrag");
        rectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        GameEventsManager.Instance.PostEvent("endDrag");

        if (!eventData.pointerEnter.TryGetComponent<Slot>(out var slot))
        {
            ResetPosition();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("On Drop event");
        var item = eventData.pointerDrag.TryGetComponent<ItemDrag>(out var cup);
        if (item && cup.Grade == Grade && Grade <= 1)
        {
            Debug.Log("Another cup found");
            Grade++;
            Destroy(cup.gameObject);
            SetGrade();
        }
        else
        {
            //cup.ResetPosition();
        }
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = initialPosition;
    }

    public void SnapToSlot(Vector3 position, Slot newSlot)
    {
        if (currentSlot != null) currentSlot.ClearSlot();
        currentSlot = newSlot;
        GetComponent<RectTransform>().anchoredPosition = position;
    }

    public void NextGrade(bool next)
    {
        if (next)
        {
            Grade++;
            Grade %= 4;
            SetGrade();
        }
        else
        {
            Grade--;
            if (Grade == -1) Grade = 3;
            SetGrade();
        }
    }

    private void OnDestroy()
    {
        if (currentSlot != null) currentSlot.ClearSlot();
        gridManagerRef.RemoveItem(this);
        GameEventsManager.Instance.PostEvent(Constants.ITEM_COMBINED_EVENT);
    }
}

[Serializable]
public class GridItemConfig
{
    public int ItemPrice;
    public int ItemGrade;
    public Sprite ItemTexture;
    public int MinInOrder;
    public int MaxInOrder;
}