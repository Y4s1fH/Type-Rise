using UnityEngine;
[System.Serializable]
public class LetterData 
{
    public UnityEngine.InputSystem.Key key; 
    public string letterStr;               
    public float letterincome = 0.1f;           
    public bool canBeGreen = false;         
    public bool isUnlocked = false;
    public int charsPerClick = 1;
    public int totalTimesPressed = 0;      
    public float totalMoneyGenerated = 0f;
}

