using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

    public static event EventHandler OnAnyObjectPlacedOnCounter;
    // Static method to reset static data
    public static void ResetStaticData()
    {
        OnAnyObjectPlacedOnCounter = null;
    }

    [SerializeField] private Transform counterTopPoint;


    private KitchenObject kitchenObject;
    public virtual void Interact(Player player)
    {
        // This method can be overridden by derived classes to provide specific interaction behavior
        Debug.LogError("Interacting with BaseCounter");
    }

    public virtual void InteractAlternate(Player player)
    {

    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            // Notify that an object has been placed on the counter
            OnAnyObjectPlacedOnCounter?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        if (kitchenObject == null)
        {
            Debug.LogError("No KitchenObject set on this counter.");
            return null;
        }
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool hasKitchenObject()
    {
        return kitchenObject != null;
    }
}
