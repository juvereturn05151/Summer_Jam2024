using UnityEngine;

public abstract class AttackBase : MonoBehaviour, ISetPlayerManager
{
    protected abstract bool IsHolding { get; set; }

    public virtual void SetPlayerManager(PlayerController playerManager) { }

    public virtual void OnTapAttack(GameState gameState) { }

    public virtual void OnReleaseAttack(GameState gameState) { }

    public virtual void OnHoldAttack(GameState gameState) { }

    protected virtual void UpdateAttack(GameState gameState) { }

}
