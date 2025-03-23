using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class JenkinsEntryPoint
{
    // Method to be called from command line
    public static void ConvertModel()
    {
        Debug.Log("Starting headless unity!");
        string assetsPath = Application.dataPath;
        Debug.Log("Assets Folder Path: " + assetsPath);

        // Define the build path
        string modelsPath = "/ModelToConvert";

        // Ensure the build directory exists
        if (!System.IO.Directory.Exists(modelsPath))
        {
            System.IO.Directory.CreateDirectory(modelsPath);
        }

        Debug.Log("models path: " + modelsPath);
        
        string fullModelPath = System.IO.Path.Combine(assetsPath, modelsPath);
        // Get all files in the directory
        string[] modelFiles = Directory.GetFiles(fullModelPath);
        string currentModelFile = modelFiles.First();
        Debug.Log("currentModelFile: " + currentModelFile);

        Debug.Log("Process complete!");
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(currentModelFile);
        Debug.Log("fileNameWithoutExtension:"+fileNameWithoutExtension);
        // Find the object by name
        GameObject selectedObject = GameObject.Find(fileNameWithoutExtension);
        
        if(selectedObject == null) return;
        
        Selection.activeObject = selectedObject;
        
        selectedObject.name = "DEAN IS STAR"; // Rename the object
        Debug.Log("Object renamed to: " + selectedObject.name);
    }
}