/* filename GoToTutorialModeOnClick.cs
 * author   Tonnattan Chankasemporn
 * email:   juvereturn@gmail.com
 * 
 * Brief Description: 
 * This class loads the the tutorial mode without asking the player
 * /*/

using UnityEngine.SceneManagement; /*SceneManager*/

public class GoToTutorialModeOnClick : OnButtonClick
{
    /*Brief
 * After the fading is gone, it should load the tutorial scene next
 */
    public void GoToTutorialMode()
    {
        base.OnClick();
        FadingUI.OnStopFading += LoadTutorialScene;
    }

    private void LoadTutorialScene()
    {
        SceneManager.LoadScene(SceneNames.TutorialScene);
    }
}
