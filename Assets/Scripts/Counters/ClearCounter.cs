using UnityEngine;

public class ClearCounter : BaseCounter
{

    public override void Interact(Player player)
    {
        if (!hasKitchenObject())
        {
            // There is no KitchenObject on this counter
            if (player.hasKitchenObject())
            {
                // If the player has a KitchenObject, clear it and set this counter's KitchenObject
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            // There is a KitchenObject on this counter
            if (player.hasKitchenObject())
            {
                // Is carrying something
                if (player.GetKitchenObject().TyrGetPlateKitchenObject(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is carrying a PlateKitchenObject
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        Debug.Log("Added ingredient to plate.");
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // Player is carrying a KitchenObject that is not a PlateKitchenObject
                    if (GetKitchenObject().TyrGetPlateKitchenObject(out plateKitchenObject))
                    {
                        // Counter is holding a Plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // If the player has a KitchenObject, clear it and set this counter's KitchenObject
                GetKitchenObject().SetKitchenObjectParent(player);
            }

        }
    }


}

