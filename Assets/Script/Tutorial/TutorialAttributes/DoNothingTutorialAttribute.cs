using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "TutorialAttribute/DoNothing")]
public class DoNothingTutorialAttribute : TutorialAttribute
{
    public override void CheckingObjective() { _clear = true; }
}
