using UnityEngine;

public abstract class TutorialAttribute : ScriptableObject
{
    protected bool _clear = false;
    public bool Clear => _clear;

    public abstract void CheckingObjective();

    public virtual void SetBegin() 
    {
        _clear = false;
    }
}
