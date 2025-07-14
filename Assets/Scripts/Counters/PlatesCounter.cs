using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField]
    private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private const float spawnPlateInterval = 4f;
    private int plateSpawnedAmount = 0;
    private int plateSpawnMaxAmout = 4;

    private void Start()
    {
        SpawnPlate();
    }

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if (GameManager.Instance.IsGamePlaying() && spawnPlateTimer >= spawnPlateInterval)
        {
            spawnPlateTimer = 0f;
            if (plateSpawnedAmount < plateSpawnMaxAmout)
            {
                SpawnPlate();
            }
        }
    }

    private void SpawnPlate()
    {
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        plateSpawnedAmount++;
    }

    public override void Interact(Player player)
    {
        if (!player.hasKitchenObject())
        {
            // Player does not have a kitchen object
            // Spawn a plate kitchen object and set it as the player's kitchen object
            if (plateSpawnedAmount > 0)
            {
                plateSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            // Player already has a plate
            // Do nothing or handle as needed
        }
    }
}
