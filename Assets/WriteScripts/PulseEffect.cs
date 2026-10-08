using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    private Vector3 originalScale;
    public float pulseSpeed = 3f; // Nəfəs alma sürəti
    public float scaleAmount = 0.15f; // Böyüyüb-kiçilmə həcmi

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Sinus dalğası vasitəsilə hamar böyüyüb-kiçilmə yaradırıq
        float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * scaleAmount;
        transform.localScale = originalScale * scale;
    }
}