using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{

    public enum Scene
    {
        MainMenu,
        GameScene,
        LoadingScene,
        // DeliveryScene,
        // Settings,
        // Credits
    }
    private static Scene targetScene;

    public static void Load(Scene targetSceneName)
    {
        targetScene = targetSceneName;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
