using UnityEditor;
using UnityEngine;

public static class JenkinsEntryPoint
{
    // Method to be called from command line
    public static void ConvertModel()
    {
        Debug.Log("Starting headless unity!");

        // Define the build path
        string modelsPath = "Assets/ModelToConvert";

        // Ensure the build directory exists
        if (!System.IO.Directory.Exists(modelsPath))
        {
            System.IO.Directory.CreateDirectory(modelsPath);
        }

        Debug.Log("models path: " + modelsPath);

        Debug.Log("Process complete!");
    }
}