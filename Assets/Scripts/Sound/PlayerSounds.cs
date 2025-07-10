using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footsteepTimer;
    private float footstepTimerMax = 0.2f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footsteepTimer -= Time.deltaTime;
        if (footsteepTimer <= 0f)
        {
            footsteepTimer = footstepTimerMax;

            if (player.IsWalking())
            {
                SoundManager.Instance.PlayFootstepSound(player.transform.position);
            }
        }
    }
}
