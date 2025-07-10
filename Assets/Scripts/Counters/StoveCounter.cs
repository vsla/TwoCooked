using System;
using System.Collections;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{

    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public class OnStateChangeEventArgs : EventArgs
    {
        public State state;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer = 0f; // Timer for burning state, if needed
    FryingRecipeSO fryingRecipeSO;
    BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
        fryingTimer = 0f;
        burningTimer = 0f; // Initialize burning timer
        fryingRecipeSO = null;

        // Optionally, you can initialize the fryingRecipeSO if needed
        // For example, if you want to set a default recipe or handle it in some way
        // fryingRecipeSO = GetFryingRecipeSO(someDefaultKitchenObjectSO);
    }

    private void Update()
    {
        if (hasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    // Do nothing, waiting for interaction
                    break;
                case State.Frying:
                    FryKitchenObjectIfExists();
                    break;
                case State.Fried:
                    BurnKitchenObjectIfExists();
                    break;
                case State.Burned:
                    // Handle burned state if needed
                    break;
            }
        }
    }

    private void FryKitchenObjectIfExists()
    {
        fryingTimer += Time.deltaTime;

        float actualFryingTimer = fryingTimer / fryingRecipeSO.fryingTimerMax;
        NotifyTheFryingProgressChanged(actualFryingTimer);

        if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
        {
            // Destroy the current KitchenObject on this counter
            GetKitchenObject().DestroySelf();
            // Set the new KitchenObject as the current KitchenObject on this counter
            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);


            fryingTimer = 0; // Reset the timer
            state = State.Fried; // Set the state to Fried
            OnStateChange?.Invoke(this, new OnStateChangeEventArgs { state = state });
            burningRecipeSO = GetBurningRecipeSO(GetKitchenObject().GetKitchenObjectSO());
            // notifyTheFryingProgressChanged();
        }



    }
    private void BurnKitchenObjectIfExists()
    {
        burningTimer += Time.deltaTime;

        float actualBurningTimer = burningTimer / burningRecipeSO.burningTimerMax;
        NotifyTheFryingProgressChanged(actualBurningTimer);

        if (burningTimer >= burningRecipeSO.burningTimerMax)
        {
            // Destroy the current KitchenObject on this counter
            GetKitchenObject().DestroySelf();
            // Set the new KitchenObject as the current KitchenObject on this counter
            KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

            burningTimer = 0; // Reset the timer
            state = State.Burned; // Set the state to Fried
            OnStateChange?.Invoke(this, new OnStateChangeEventArgs { state = state });

            NotifyTheFryingProgressChanged(0f);
        }
    }


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
                    // Player has a KitchenObject that can be fryied
                    // Set the KitchenObject's parent to this counter
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSO(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying; // Set the state to Frying
                    fryingTimer = 0f; // Reset the frying timer

                    NotifyOnStateChanged();

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

                        ResetFryingState(player);
                    }
                }
            }
            else
            {
                // If the player don't have a KitchenObject, add the current KitchenObject to the player
                GetKitchenObject().SetKitchenObjectParent(player);
                ResetFryingState(player);
            }
        }
    }

    private bool HasRecipeForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSO(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return true; // A matching recipe exists
        }
        return false; // No matching recipe found
    }

    private FryingRecipeSO GetFryingRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null; // No matching recipe found
    }
    private BurningRecipeSO GetBurningRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null; // No matching recipe found
    }

    private void NotifyOnStateChanged()
    {
        OnStateChange?.Invoke(this, new OnStateChangeEventArgs { state = state });
    }

    private void NotifyTheFryingProgressChanged(float progressNormalized)
    {
        if (fryingRecipeSO != null)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = progressNormalized
            });
        }
    }

    private void ResetFryingState(Player player)
    {

        state = State.Idle; // Set the state to Idle
        NotifyOnStateChanged();
        NotifyTheFryingProgressChanged(0f); // Notify listeners about the progress change
    }
}
