using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [Header("UI Elementləri")]
    public GameObject notificationPanel;
    public RectTransform panelRect;
    public Image achievementIcon;
    public TextMeshProUGUI nameText;

    [Header("Animasiya Ayarları")]
    public float slideSpeed = 5f;
    public float displayTime = 3f;
    private Vector2 offScreenPosition;
    private Vector2 onScreenPosition;

    private struct NotificationInfo
    {
        public string name;
        public Sprite icon;
    }

    private Queue<NotificationInfo> notificationQueue = new Queue<NotificationInfo>();
    private bool isShowing = false;

    private void Awake()
    {
        Instance = this;

        if (panelRect != null)
        {
            onScreenPosition = panelRect.anchoredPosition;
            offScreenPosition = new Vector2(onScreenPosition.x + panelRect.rect.width + 100f, onScreenPosition.y);
            panelRect.anchoredPosition = offScreenPosition;
        }
        notificationPanel.SetActive(false);
    }

    public void ShowNotification(string achName, Sprite achIcon)
    {
        NotificationInfo newNotif = new NotificationInfo();
        newNotif.name = achName;
        newNotif.icon = achIcon;

        notificationQueue.Enqueue(newNotif);

        if (!isShowing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isShowing = true;
        notificationPanel.SetActive(true);

        while (notificationQueue.Count > 0)
        {
            NotificationInfo currentInfo = notificationQueue.Dequeue();

            nameText.text = currentInfo.name;
            if (currentInfo.icon != null)
            {
                achievementIcon.sprite = currentInfo.icon;
            }

            // SÜRÜŞƏRƏK EKRANA GƏLMƏ
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * slideSpeed;
                panelRect.anchoredPosition = Vector2.Lerp(offScreenPosition, onScreenPosition, Mathf.SmoothStep(0, 1, t));
                yield return null;
            }
            panelRect.anchoredPosition = onScreenPosition;

            // Ekranda gözləmə
            yield return new WaitForSeconds(displayTime);

            // SÜRÜŞƏRƏK GERİ ÇIXMA
            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * slideSpeed;
                panelRect.anchoredPosition = Vector2.Lerp(onScreenPosition, offScreenPosition, Mathf.SmoothStep(0, 1, t));
                yield return null;
            }
            panelRect.anchoredPosition = offScreenPosition;

            yield return new WaitForSeconds(0.5f);
        }

        notificationPanel.SetActive(false);
        isShowing = false;
    }
}