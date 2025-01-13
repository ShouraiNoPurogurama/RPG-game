using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Player _player;

    void Start()
    {
        //Get the Player script object in the parent
        _player = GetComponentInParent<Player>();
    }

    /// <summary>
    /// This trigger will be activated at the end of the attack animation
    /// </summary>
    private void AnimationTrigger()
    {
        _player.AttackOver();
    }
}