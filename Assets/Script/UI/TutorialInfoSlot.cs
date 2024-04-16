using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialInfoSlot : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text detail;
    public Image image;

    public void SetTutorialInfo(TutorialInfo info)
    {
        title.text = info.title;
        detail.text = info.detail;
        image = info.image;
    }
}
