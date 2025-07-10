using UnityEngine;
using System.Collections.Generic;
using System;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeDelivered;
    public event EventHandler onRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeSOList recipeListSO;
    private List<RecipeSO> waitingRecipes;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount = 0;


    private void Awake()
    {
        Instance = this;
        if (recipeListSO == null)
        {
            Debug.LogError("Recipe List ScriptableObject is not assigned in the DeliveryManager.");
            return;
        }

        waitingRecipes = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            SpawnNewRecipe();
        }
    }


    private void SpawnNewRecipe()
    {
        if (recipeListSO.recipeSOList.Count == 0)
        {
            Debug.LogWarning("No recipes available to spawn.");
            return;
        }
        if (waitingRecipes.Count < waitingRecipesMax)
        {
            int randomIndex = UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count);
            RecipeSO newRecipe = recipeListSO.recipeSOList[randomIndex];
            waitingRecipes.Add(newRecipe);
            // Here you would typically instantiate a UI element or game object to represent the new recipe

            OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        }

    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipes.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipes[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentMatchesRecipe = true;
                // If the number of ingredients matches, we can check each ingredient
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        plateContentMatchesRecipe = false;
                        break;
                    }
                }

                if (plateContentMatchesRecipe)
                {
                    // Player has delivered a matching recipe
                    waitingRecipes.RemoveAt(i);
                    successfulRecipesAmount++;

                    OnRecipeDelivered?.Invoke(this, EventArgs.Empty);
                    onRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }


        // If we reach here, the recipe was not successful
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }


    public List<RecipeSO> GetWaitingRecipesSOList()
    {
        return waitingRecipes;
    }

    public int GetSuccesfullRecipesAmout()
    {
        return successfulRecipesAmount;
    }

}
