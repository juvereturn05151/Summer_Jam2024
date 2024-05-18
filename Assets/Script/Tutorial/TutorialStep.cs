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

    public string ObjectiveDialogue = "Kill";

    public TutorialAttribute TutorialAttribute;

    public void StartOperating() 
    {
        TutorialAttribute.SetBegin();
    }
}
