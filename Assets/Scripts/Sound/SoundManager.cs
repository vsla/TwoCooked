using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton instance
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DeliveryManager.Instance.onRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        CuttingCounter.OnAnyCuttingCounterProgressChanged += CuttingCounter_OnAnyCuttingCounterProgressChanged;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedOnCounter += BaseCounter_OnAnyObjectPlacedOnCounter;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, EventArgs e)
    {
        PlaySoundList(audioClipRefsSO.recipeSpawn, Player.Instance.transform.position, 10000f);
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
        Debug.Log(transform.position);
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

    private void PlaySoundList(AudioClip[] audioClipArray, Vector3 position, float volume = 1.0f)
    {
        AudioSource.PlayClipAtPoint(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }

    public void PlayFootstepSound(Vector3 position)
    {
        PlaySoundList(audioClipRefsSO.footsteeps, position, 10000f);
    }
}
