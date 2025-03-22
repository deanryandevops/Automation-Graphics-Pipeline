using UnityEditor;
using UnityEngine;

public static class BuildScript
{
    // Method to be called from command line
    public static void PerformBuild()
    {
        Debug.Log("Starting headless build!");

        // Define the scenes to include in the build
        string[] scenes = { "Assets/Scenes/SampleScene.unity" }; // Adjust to your scenes

        // Define the build path
        string modelsPath = "Assets/ConvertedModels";

        // Ensure the build directory exists
        if (!System.IO.Directory.Exists(modelsPath))
        {
            System.IO.Directory.CreateDirectory(modelsPath);
        }

        Debug.Log("models path: " + modelsPath);

        Debug.Log("Process complete!");
    }
}