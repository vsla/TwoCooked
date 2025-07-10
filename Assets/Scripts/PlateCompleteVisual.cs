using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjetSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
        // Add the reference of the icon image here, to be added on the ui
        // public Image iconImage; // Uncomment if you want to use an Image component for the icon
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjetSO_GameObject> kitchenObjectSO_GameObjectList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (var item in kitchenObjectSO_GameObjectList)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (var item in kitchenObjectSO_GameObjectList)
        {
            if (item.kitchenObjectSO == e.kitchenObjectSO)
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}
