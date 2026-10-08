using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class FadeOutImprintUI : MonoBehaviour
{
    public float waitTime = 1.5f;
    public float fadeTime = 1f;
    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
        if (img != null)
        {
            StartCoroutine(FadeAndDestroy());
        }
    }

    IEnumerator FadeAndDestroy()
    {
        yield return new WaitForSeconds(waitTime);
        Color originalColor = img.color;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / fadeTime;
            img.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - t);
            yield return null;
        }
        Destroy(gameObject);
    }
}