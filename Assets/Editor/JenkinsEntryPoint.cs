using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class JenkinsEntryPoint
{
    // Method to be called from command line or editor
    public static void ConvertModel()
    {
        Debug.Log("Starting model conversion in the Unity Editor!");

        // Get the path to the Assets folder
        string assetsPath = Application.dataPath;
        Debug.Log("Assets Folder Path: " + assetsPath);

        // Define the path to the models folder
        string modelsPath = "ModelToConvert";

        // Combine the paths to get the full path to the models folder
        string fullModelPath = Path.Combine(assetsPath, modelsPath);

        // Get all files in the directory, excluding meta files
        string[] modelFiles = Directory.GetFiles(fullModelPath)
                                      .Where(file => !file.EndsWith(".meta"))
                                      .ToArray();

        if (modelFiles.Length == 0)
        {
            Debug.LogError("No model files found in the directory: " + fullModelPath);
            return;
        }

        // Get the first model file
        string currentModelFile = modelFiles.First();
        Debug.Log("Current Model File: " + currentModelFile);

        // Get the file name without extension
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(currentModelFile);
        Debug.Log("File Name Without Extension: " + fileNameWithoutExtension);

        // Find the object in the project (e.g., a prefab or model)
        string[] assetGuids = AssetDatabase.FindAssets(fileNameWithoutExtension);
        if (assetGuids.Length == 0)
        {
            Debug.LogError("Object not found in the project: " + fileNameWithoutExtension);
            return;
        }

        // Get the first matching asset
        string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[0]);
        GameObject selectedObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

        if (selectedObject == null)
        {
            Debug.LogError("Failed to load object at path: " + assetPath);
            return;
        }

        Debug.Log("Selected Object Name: " + selectedObject.name);

        // Set the object as the active selection in the Editor
        Selection.activeObject = selectedObject;
        Debug.Log("Selected object in the Editor: " + selectedObject.name);

        // Focus the Project window on the selected object
        EditorGUIUtility.PingObject(selectedObject);
        Debug.Log("Pinged object in the Project window.");

        // Rename the asset
        string newName = "DEAN IS STAR";
        AssetDatabase.RenameAsset(assetPath, newName);
        Debug.Log("Asset renamed to: " + newName);

        // Refresh the AssetDatabase to reflect the changes
        AssetDatabase.Refresh();
        Debug.Log("AssetDatabase refreshed.");

        // Save the changes to the asset
        AssetDatabase.SaveAssets();
        Debug.Log("Asset changes saved.");
    }
}