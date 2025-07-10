using UnityEngine;

public class DeliveryCounter : BaseCounter
{

    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public override void Interact(Player player)
    {
        if (player.hasKitchenObject())
        {
            // Player has a KitchenObject, check if it's a PlateKitchenObject
            if (player.GetKitchenObject().TyrGetPlateKitchenObject(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                plateKitchenObject.DestroySelf(); // Destroy the PlateKitchenObject
            }
            else
            {
                // Player is carrying a KitchenObject that is not a PlateKitchenObject
                Debug.Log("Cannot deliver non-plate KitchenObjects.");
            }
        }
        else
        {
            // Player has no KitchenObject to deliver
            Debug.Log("No KitchenObject to deliver.");
        }
    }
}
