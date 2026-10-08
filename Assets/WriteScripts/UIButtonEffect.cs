using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    // 1. Siçan düymənin üstünə gələndə (Böyüyür)
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * 1.08f;
    }

    // 2. Siçan düymənin üstündən gedəndə (Orijinala qayıdır)
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    // 3. Düyməyə basanda (Əzilir)
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * 0.90f;
    }

    // 4. Düyməni buraxanda (Yenidən böyük "Hover" vəziyyətinə qayıdır)
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale * 1.08f;
        AudioScripts.Instance.PlayClickSound();
    }

    // Əgər düymə ekran arxasında sönərsə, ölçüsünü xilas edirik
    void OnDisable()
    {
        transform.localScale = originalScale;
    }
}