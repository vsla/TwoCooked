using System;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    public static event EventHandler OnAnyObjectTrashed;
    // Static method to reset static data
    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }
    override public void Interact(Player player)
    {
        // If the player has a kitchen object, clear it
        if (player.hasKitchenObject())
        {
            KitchenObject kitchenObject = player.GetKitchenObject();
            kitchenObject.DestroySelf();

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
