using UnityEngine;
using UnityEngine.InputSystem;

public class SkillTreeMouseMover : MonoBehaviour
{
    private RectTransform rectTransform;

    [Header("Sürüşdürmə (Drag) Ayarları")]
    public float dragSpeed = 1f;      // Siçan hərəkəti ilə panelin hərəkət sürəti

    [Header("Zoom (Böyütmə) Ayarları")]
    public float zoomSpeed = 0.1f;    // Böyümə/kiçilmə sürəti
    public float minScale = 0.5f;     // Ən çox kiçilə biləcəyi ölçü (50%)
    public float maxScale = 1.5f;     // Ən çox böyüyə biləcəyi ölçü (150%)

    [Header("Hərəkət Sərhədləri")]
    public float minX = -500f;
    public float maxX = 500f;
    public float minY = -400f;
    public float maxY = 400f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        HandleDragging();
        HandleZoom();
    }

    void HandleDragging()
    {
        if (Mouse.current == null) return;

        // Əgər siçanın sol düyməsi basılıdırsa (Hold)
        if (Mouse.current.leftButton.isPressed)
        {
            // Siçanın son freymdən bəri ekranda nə qədər sürüşdüyünü tapırıq (X və Y oxu üzrə)
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            Vector2 currentPosition = rectTransform.anchoredPosition;

            // Panelin mövqeyinə siçanın hərəkətini əlavə edirik
            currentPosition += mouseDelta * dragSpeed;

            // Panelin kənara çıxmasının qarşısını alırıq (Sərhədlər)
            currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
            currentPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);

            rectTransform.anchoredPosition = currentPosition;
        }
    }

    void HandleZoom()
    {
        if (Mouse.current != null)
        {
            float scrollValue = Mouse.current.scroll.y.ReadValue();

            if (scrollValue != 0f)
            {
                Vector3 currentScale = rectTransform.localScale;

                // Təkər yuxarı və ya aşağı fırladıldıqda miqyası dəyişirik
                float zoomChange = (scrollValue > 0f) ? zoomSpeed : -zoomSpeed;

                float newScale = Mathf.Clamp(currentScale.x + zoomChange, minScale, maxScale);
                rectTransform.localScale = new Vector3(newScale, newScale, 1f);
            }
        }
    }
}