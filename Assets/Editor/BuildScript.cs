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
        string buildPath = "Builds/HeadlessBuild";

        // Ensure the build directory exists
        if (!System.IO.Directory.Exists(buildPath))
        {
            System.IO.Directory.CreateDirectory(buildPath);
        }

        Debug.Log("Build path: " + buildPath);
        
        // Perform the build
        BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.StandaloneWindows64, BuildOptions.None);

        Debug.Log("Build complete!");
    }
}