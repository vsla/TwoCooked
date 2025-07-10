using System;
using UnityEngine;

public class SelectedCounterVisuals : MonoBehaviour
{

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (baseCounter == e.selectedCounter)
        {
            ShowVisual();
        }
        else
        {
            HideVisual();
        }
    }

    private void ShowVisual()
    {
        foreach (GameObject visual in visualGameObjectArray)
        {
            visual.SetActive(true);
        }
    }

    private void HideVisual()
    {
        foreach (GameObject visual in visualGameObjectArray)
        {
            visual.SetActive(false);
        }
    }
}
