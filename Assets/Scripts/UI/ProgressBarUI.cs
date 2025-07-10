using UnityEngine.UI;
using UnityEngine;
using System;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if(hasProgress == null)
        {
            Debug.LogError("IHasProgress component not found on the specified GameObject.");
            return;
        }
        hasProgress.OnProgressChanged += HasProgress_OnCuttingProgressChanged;
        barImage.fillAmount = 0f; // Initialize the progress bar to empty
        Hide(); // Hide the progress bar initially
    }

    private void HasProgress_OnCuttingProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {

        barImage.fillAmount = e.progressNormalized;
        if(e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide(); // Hide the progress bar when not cutting
        }
        else
        {
            Show(); // Show the progress bar when cutting
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
