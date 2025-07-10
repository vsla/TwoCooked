using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
   private bool isFirstUpdate = true;
    // Update is called once per frame
    void Update()
    {
        if(isFirstUpdate)
        {
            isFirstUpdate = false;
            // Load the main menu scene
           Loader.LoaderCallback();
        }
    }
}
    