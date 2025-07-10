using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private PlatesCounter platesCounter;
    private List<GameObject> plateVisualGameObjects;

    private void Start()
    {
        plateVisualGameObjects = new List<GameObject>();
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
    {
        if (plateVisualGameObjects.Count > 0)
        {
            GameObject lastPlateVisual = plateVisualGameObjects[plateVisualGameObjects.Count - 1];
            plateVisualGameObjects.RemoveAt(plateVisualGameObjects.Count - 1);
            Destroy(lastPlateVisual);
        }
    }

    private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjects.Count, 0);

        plateVisualGameObjects.Add(plateVisualTransform.gameObject);
    }
}
