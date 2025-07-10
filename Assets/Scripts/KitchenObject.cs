using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent newKitchenObjectParent)
    {
        if (kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearKitchenObject();
        }
        kitchenObjectParent = newKitchenObjectParent;
        if (newKitchenObjectParent.hasKitchenObject())
        {
            Debug.LogError("Trying to set a KitchenObject on a IKitchenObjectParent that already has a KitchenObject!");
            return;
        }
        newKitchenObjectParent.SetKitchenObject(this);

        transform.SetParent(newKitchenObjectParent.GetKitchenObjectFollowTransform());
        transform.localPosition = Vector3.zero; // Reset position to the IKitchenObjectParent
        transform.localRotation = Quaternion.identity; // Reset rotation to the default
    }
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        if (kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearKitchenObject();
        }
        Destroy(gameObject);
    }

    public bool TyrGetPlateKitchenObject(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }

    }
    
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        if (kitchenObject == null)
        {
            Debug.LogError("KitchenObjectSO prefab does not have a KitchenObject component!");
            return null;
        }
        kitchenObject.kitchenObjectSO = kitchenObjectSO;
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
