using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME_KEY = "SoundEffectVolume";
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    private AudioClipRefsSO audioClipRefsSO;

    [SerializeField]
    private float volume = 1.0f;

    private void Awake()
    {
        Instance = this;

        volume = GetPrefsDefaultVolume();
    }

    private void Start()
    {
        DeliveryManager.Instance.onRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        CuttingCounter.OnAnyCuttingCounterProgressChanged +=
            CuttingCounter_OnAnyCuttingCounterProgressChanged;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedOnCounter += BaseCounter_OnAnyObjectPlacedOnCounter;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, EventArgs e)
    {
        PlaySoundList(audioClipRefsSO.recipeSpawn, Player.Instance.transform.position);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        PlaySoundList(audioClipRefsSO.trash, ((TrashCounter)sender).transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedOnCounter(object sender, EventArgs e)
    {
        PlaySoundList(audioClipRefsSO.kitchenObjectDrop, ((BaseCounter)sender).transform.position);
    }

    private void Player_OnPickedSomething(object sender, EventArgs e)
    {
        PlaySoundList(audioClipRefsSO.kitchenObjectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCuttingCounterProgressChanged(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySoundList(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySoundList(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySoundList(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void PlaySoundList(
        AudioClip[] audioClipArray,
        Vector3 position,
        float volumeMultiplier = 1.0f
    )
    {
        AudioSource.PlayClipAtPoint(
            audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)],
            position,
            volumeMultiplier * volume
        );
    }

    public void PlayFootstepSound(Vector3 position, float volume)
    {
        PlaySoundList(audioClipRefsSO.footsteeps, position, volume);
    }

    public void PlayCountdownSound(float volume)
    {
        PlaySoundList(audioClipRefsSO.warning, Vector3.zero, volume);
    }

    public void ChangeVolume(float receivingVolume)
    {
        volume = receivingVolume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME_KEY, receivingVolume);
        PlayerPrefs.Save();
    }

    public float GetPrefsDefaultVolume()
    {
        return PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME_KEY, 10f);
    }
}
