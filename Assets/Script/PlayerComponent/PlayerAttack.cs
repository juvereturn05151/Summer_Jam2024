using UnityEngine; /*MonoBehaviour*/

public class PlayerAttack : MonoBehaviour, ISetPlayerManager
{
    [SerializeField]
    private AttackBase _attackType;

    public void SetPlayerManager(PlayerController playerManager)
    {
        _attackType.SetPlayerManager(playerManager);
    }

    public void OnTapAttack() 
    {
        _attackType.OnTapAttack(GameManager.Instance.State);
    }

    public void OnReleaseAttack()
    {
        _attackType.OnReleaseAttack(GameManager.Instance.State);
    }

    public void OnHoldAttack()
    {
        _attackType.OnHoldAttack(GameManager.Instance.State);
    }
}
