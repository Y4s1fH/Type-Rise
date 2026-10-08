using UnityEngine;

public class SkillEffectManager : MonoBehaviour
{
    public static SkillEffectManager Instance;
    SkillData skilldata;
    public GameObject Stamp;
    private float constantmultiplier = 2f;
    void Awake()
    {
        Instance = this;
    }
    public void ApplySkillEffect(string skillid)
    {
        switch (skillid)
        {
            case "1":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.alphabetList[0].letterincome += 0.1f;
                }
                break;
            case "2":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.resetmoneygain += 20f;
                }
                break;
            case "3":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.alphabetList[0].charsPerClick += 1;
                }
                break;
            case "4":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.greencolorChance += 1f;
                    GameManager.Instance.greencolorMultiplier = 3f;
                }
                break;
            case "5":
                if (GameManager.Instance != null)
                {
                    if (!GameManager.Instance.isFrenzyUnlocked)
                    {
                        GameManager.Instance.UnlockFrenzy();
                    }
                    GameManager.Instance.frenzyDuration+=1f;
                }
                break;
            case "6":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.alphabetList[0].letterincome += 1f;
                }
                break;
            case "7":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.resetmoneybonus += 0.25f;
                }
                break;
            case "8":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.resetmoneygain += 200f;
                }
                break;
            case "9":
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.greencolorChance += 2.5f;
                }
                break;
            case "10":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.greencolorChance += 5f;
                }
                break;
            case "11":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.greencolorChance += 10f;
                }
                break;
            case "12":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.alphabetList[0].charsPerClick += 1;
                }
                break;
            case "13":
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.UnlockLetter(UnityEngine.InputSystem.Key.B);
                }
                break;
            case "14":
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.greencolorMultiplier += 0.5f;
                }
                break;
            case "15":
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.greencolorMultiplier += 1f;
                }
                break;
            case "16":
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.greencolorMultiplier += 2f;
                }
                break;
            case "17":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.alphabetList[1].letterincome += 100f;
                }
                break;
            case "18":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.alphabetList[1].charsPerClick += 1;
                }
                break;
            case "19":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.alphabetList[1].canBeGreen = true;
                }
                break;
            case "20":
                if(PassiveIncomeManager.Instance != null)
                {
                    PassiveIncomeManager.Instance.isUnlocked = true;
                    PassiveIncomeManager.Instance.PenObject.SetActive(true);
                    PassiveIncomeManager.Instance.sticknoteobject.SetActive(true);
                }
                break;
            case "21":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.redcolorChance += 1f;
                    GameManager.Instance.greencolorChance -= 1f;
                    GameManager.Instance.redcolorMultiplier = 30f;
                }
                break;
            case "22":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.resetmoneygain += 1000f;
                }
                break;
            case "23":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.isAutoResetUnlocked = true;
                }
                break;
            case "24":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.resetmoneygain += 5000f;
                }
                break;
            case "25":
                if(PassiveIncomeManager.Instance != null)
                {
                    PassiveIncomeManager.Instance.incomeMultiplier += 1f;
                }
                break;
            case "26":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.isPossessedQuillUnlocked = true;
                }
                break;
            case "27":
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.redcolorChance += 2.5f;
                }
                break;
            case "28":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.redcolorChance += 5f;
                }
                break;
            case "29":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.redcolorChance += 10f;
                }
                break;
            case "30":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.redcolorMultiplier += 5f;
                }
                break;
            case "31":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.redcolorMultiplier += 10f;
                }
                break;
            case "32":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.redcolorMultiplier += 20f;
                }
                break;
            case "33":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.UnlockLetter(UnityEngine.InputSystem.Key.C);
                }
                break;
            case "34":
                if(GameManager.Instance != null)
                {
                    float oldMultiplier = (constantmultiplier == 2f) ? 1f : (constantmultiplier - 2f);
                    GameManager.Instance.alphabetList[0].letterincome *= constantmultiplier / oldMultiplier;
                    GameManager.Instance.alphabetList[1].letterincome *= constantmultiplier / oldMultiplier;
                    constantmultiplier += 2f;
                }
                break;
            case "35":
                if(GameManager.Instance != null)
                {
                   Stamp.SetActive(true);
                   AutoStampTimer.Instance.isSkillUnlocked = true;
                }
                break;
            case "36":
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.alphabetList[2].letterincome += 10000f;
                }
                break;
            case "37":
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.alphabetList[2].canBeGreen =true;
                }
                break;
            case "38":
                if (TypingTestManager.Instance != null)
                {
                    TypingTestManager.Instance.isTypingUnlocked = true;
                    TypingTestManager.Instance.lockedicon.SetActive(false);
                }
                break;
            case "39":
                if (GameManager.Instance != null)
                {

                }
                break;
            case "40":
                if (GameManager.Instance != null)
                {

                }
                break;
            case "41":
                if (GameManager.Instance != null)
                {

                }
                break;
            case "42":
                if (GameManager.Instance != null)
                {

                }
                break;
        }
    }
}
