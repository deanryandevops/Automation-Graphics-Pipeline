public class BuildScript
{
    // Method to be called from command line
    public static void PerformBuild()
    {
        Debug.Log("Starting headless build!");

        string[] scenes = { "Assets/Scenes/SampleScene.unity" }; // Adjust to your scenes
        string buildPath = "Builds/HeadlessBuild";

        BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.StandaloneWindows64, BuildOptions.None);

        Debug.Log("Build complete!");
    }
}