using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{

    public static event EventHandler OnAnyCuttingCounterProgressChanged;
    new public static void ResetStaticData()
    {
        OnAnyCuttingCounterProgressChanged = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress = 0;
    public override void Interact(Player player)
    {
        if (!hasKitchenObject())
        {
            // There is no KitchenObject on this counter
            if (player.hasKitchenObject())
            {
                // If the player has a KitchenObject, clear it and set this counter's KitchenObject
                if (HasRecipeForInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player has a KitchenObject that can be cut
                    // Set the KitchenObject's parent to this counter
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0; // Reset cutting progress

                    // Notify listeners about the cutting progress change
                    notifyTheCuttingProgressChanged();
                }
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
                        GetKitchenObject().DestroySelf();
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

    public override void InteractAlternate(Player player)
    {
        KitchenObjectSO input = GetKitchenObject().GetKitchenObjectSO();
        if (hasKitchenObject() &&
            HasRecipeForInput(input))
        {
            // Spawn a new KitchenObject from the KitchenObjectSO
            // This simulates cutting the KitchenObject
            cuttingProgress++;

            // Start cutting animation
            OnCut?.Invoke(this, EventArgs.Empty); // Notify that cutting has started
            // Notify any listeners that the cutting progress has changed
            
            OnAnyCuttingCounterProgressChanged?.Invoke(this, EventArgs.Empty);


            // Get the cutting recipe for the input KitchenObject
            // and check if the cutting progress has reached the maximum
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(input);

            notifyTheCuttingProgressChanged();

            int cuttingProgressMax = cuttingRecipeSO.cuttingProgressMax;

            if (cuttingProgress >= cuttingProgressMax)
            {
                // Cutting is complete after 4 interactions
                CutKitchenObject();
                cuttingProgress = 0; // Reset cutting progress
            }

        }
    }

    private void CutKitchenObject()
    {
        KitchenObjectSO outputKitchenObjectSO = GEtOutputForInput(GetKitchenObject().GetKitchenObjectSO());
        if (outputKitchenObjectSO != null)
        {
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }

    private bool HasRecipeForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return true; // A matching recipe exists
        }
        return false; // No matching recipe found
    }

    private KitchenObjectSO GEtOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        return null; // No matching recipe found
    }

    private CuttingRecipeSO GetCuttingRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null; // No matching recipe found
    }

    private void notifyTheCuttingProgressChanged()
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
        if (cuttingRecipeSO != null)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
        }
    }
}
