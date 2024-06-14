/* filename PlayerManager.cs
 * author   Tonnattan Chankasemporn
 * email:   juvereturn@gmail.com
 * 
 * Brief Description: 
 * Manages Player Related Components Including Human And Sunlight Characters
 * /*/

using UnityEngine; /*MonoBehaviour*/

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private Human _human;
    public Human Human => _human;

    [SerializeField]
    private Sunlight _sunlight;
    public Sunlight Sunlight => _sunlight;

    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        _human.SetPlayerManager(this);
        _sunlight.SetPlayerManager(this);
    }
}
