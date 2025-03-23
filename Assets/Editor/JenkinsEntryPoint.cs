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
        string modelsPath = "ModelToConvert";

        string fullModelPath = System.IO.Path.Combine(assetsPath, modelsPath);
        // Get all files in the directory
        string[] modelFiles = Directory.GetFiles(fullModelPath)
            .Where(file => !file.EndsWith(".meta"))
            .ToArray();
        string currentModelFile = modelFiles.First();
        Debug.Log("currentModelFile: " + currentModelFile);

        Debug.Log("Process complete!");
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(currentModelFile);
        Debug.Log("fileNameWithoutExtension:"+fileNameWithoutExtension);

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

        // Rename the object
        string newName = "DEAN IS STAR";
        selectedObject.name = newName;
        Debug.Log("Object renamed to: " + newName);

        // Save the changes to the asset
        AssetDatabase.SaveAssets();
        Debug.Log("Asset changes saved.");
    }
}