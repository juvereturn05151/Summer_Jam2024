/* filename QuitOnClick.cs
 * author   Tonnattan Chankasemporn
 * email:   juvereturn@gmail.com
 * 
 * Brief Description: 
 * Quit the game, that's it
 * /*/

using UnityEngine;/*Application.Quit*/

public class QuitOnClick : OnButtonClick
{
    /*Brief
     * After the fading is gone, it should load the tutorial scene next
     */
    public void ExitTheGame()
    {
        base.OnClick();
        FadingUI.OnStopFading += Quit;
    }

    private void Quit()
    {
        Application.Quit();
    }
}
