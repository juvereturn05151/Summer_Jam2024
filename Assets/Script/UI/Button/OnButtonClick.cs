/* filename OnButtonClick.cs
 * author   Tonnattan Chankasemporn
 * email:   juvereturn@gmail.com
 * 
 * Brief Description: 
 * This class should always be attached to the MenuButtonBase script
 * Please implement this class, so whenever on click happens, it plays the shared functionality
 * I expected users to put the OnClick Method to the OnClick event on the Button component
 * /*/

using UnityEngine; /*MonoBehaviour*/

public class OnButtonClick : MonoBehaviour
{
    public virtual void OnClick() 
    {
        SoundManager.Instance.PlayOneShot("SFX_Click");
        FadingUI.Instance.StartFadeIn();
    }
}
