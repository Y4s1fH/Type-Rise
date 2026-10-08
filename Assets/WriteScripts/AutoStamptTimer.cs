using UnityEngine;
using System.Collections;

public class AutoStampTimer : MonoBehaviour
{
    public static AutoStampTimer Instance;
    [Header("Ayarlar")]
    public float stampInterval = 2.0f; 

    [Header("Damğa Skriptini Bura Sürüklə")]
    public UIStampController myStampController;
    public bool isSkillUnlocked = false;

    private bool isRunning = true;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (myStampController != null)
        {
            StartCoroutine(StampLoop());
        }
    }

    IEnumerator StampLoop()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(stampInterval);
            if(isSkillUnlocked == true)
            {
                myStampController.TriggerStamp();
            }
        }
    }
    public void StopTimer() { isRunning = false; }
    public void StartTimer() { isRunning = true; StartCoroutine(StampLoop()); }
}