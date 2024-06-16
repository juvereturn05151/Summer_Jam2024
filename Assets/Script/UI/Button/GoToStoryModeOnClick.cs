/* filename GoToStoryModeOnClick.cs
 * author   Tonnattan Chankasemporn
 * email:   juvereturn@gmail.com
 * 
 * Brief Description: 
 * This class loads the story mode by going to the tutorial scene first (In the future, we should do a system that asks if players)
 * /*/

using UnityEngine.SceneManagement; /*SceneManager*/

public class GoToStoryModeOnClick : OnButtonClick
{
    /*Brief
     * After the fading is gone, it should load the tutorial scene next
     */
    public void GoToStoryMode() 
    {
        base.OnClick();
        FadingUI.OnStopFading += LoadTutorialScene;
    }

    private void LoadTutorialScene()
    {
        SceneManager.LoadScene(SceneNames.TutorialScene);
    }
}
