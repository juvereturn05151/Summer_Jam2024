using UnityEngine;

public enum TutorialType
{
    WalkToPoint,
    AttackTheEnemy,
    WalkToDropWater,
    ExplainTheThirstyBar,
    CreateABigPond,
    WalkToBigPond,
    ExpandSunRadius,
    EndTutorial
}


[CreateAssetMenu(menuName = "TutorialStep")]
public class TutorialStep : ScriptableObject
{
    public TutorialType Type;

    public string[] DialogueLines = {
            "Hello there!",
            "Welcome to our game.",
            "Enjoy your adventure!"
    };

    public bool ShowTutorialGuideOnStart;

    public bool ShowTutorialGuideBackground = true;

    public bool ShowOnSecondDialogue;

    public bool ShowOnLastDialogue;

    public string ObjectiveDialogue = "Kill";

    public string WhatToDoDialogue = "Kill";

    public TutorialAttribute TutorialAttribute;

    public void StartOperating() 
    {
        TutorialAttribute.SetBegin();
    }

    public bool IsWaterDecreasable() 
    {
        return (Type == TutorialType.ExpandSunRadius || Type == TutorialType.ExplainTheThirstyBar
            || Type == TutorialType.WalkToDropWater || Type == TutorialType.CreateABigPond
            || Type == TutorialType.AttackTheEnemy);
    }
}
