using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float destroyTime = 1f;
    public float randomXRange = 30f;

    private TextMeshProUGUI textMesh;
    private Color startColor;
    private RectTransform rectTransform;
    private Vector3 originalScale;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        if (textMesh != null)
        {
            startColor = textMesh.color;
        }

        if (rectTransform != null)
        {
            float randomX = Random.Range(-randomXRange, randomXRange);
            rectTransform.anchoredPosition += new Vector2(randomX, 0);
            originalScale = rectTransform.localScale;
            rectTransform.localScale = originalScale * 0.5f;
        }

        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime);
            if (rectTransform.localScale.x < originalScale.x)
            {
                rectTransform.localScale += Vector3.one * (Time.deltaTime * 3f);
            }
        }
        if (textMesh != null)
        {
            float alpha = textMesh.color.a - (Time.deltaTime / destroyTime);
            textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
        }
    }
}