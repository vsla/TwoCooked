using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler onPlayerGrabbedObject;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player)
    {
        if (!player.hasKitchenObject())
        {
            // Player does not have a KitchenObject, so we can give them one from this counter
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            onPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }

    }

}
