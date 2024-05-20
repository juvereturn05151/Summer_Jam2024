using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TutorialAttribute/BigPondTutorial")]
public class BigPondTutorialAttribute : TutorialAttribute
{
    public override void CheckingObjective()
    {
        _clear = GameManager.Instance.BigPondDrank >= 2;
    }
}
