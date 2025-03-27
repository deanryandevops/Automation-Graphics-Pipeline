using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ModelConvert
{
    [MenuItem("Tools/Convert Model")]
    public static void ConvertModel()
    {
        Debug.Log("Starting model conversion in the Unity Editor!");

        SaveCurrentScene();
        
        // Get the path to the Assets folder
        string assetsPath = Application.dataPath;
        Debug.Log("Assets Folder Path: " + assetsPath);

        // Define the path to the models folder
        string modelsPath = "ModelToConvert";
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

        // Process the first model file
        string currentModelFile = modelFiles.First();
        Debug.Log("Current Model File: " + currentModelFile);

        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(currentModelFile);
        Debug.Log("File Name Without Extension: " + fileNameWithoutExtension);

        // Find the object in the project
        string[] assetGuids = AssetDatabase.FindAssets(fileNameWithoutExtension);
        if (assetGuids.Length == 0)
        {
            Debug.LogError("Object not found in the project: " + fileNameWithoutExtension);
            return;
        }

        string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[0]);
        GameObject selectedObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

        if (selectedObject == null)
        {
            Debug.LogError("Failed to load object at path: " + assetPath);
            return;
        }

        Debug.Log("Selected Object Name: " + selectedObject.name);

        // Rename the asset
        string newName = "DEAN IS STAR";
        AssetDatabase.RenameAsset(assetPath, newName);
        Debug.Log("Asset renamed to: " + newName);

        // Create prefab
        string prefabPath = Path.Combine("Assets/ModelToConvert", "Working.prefab");
        var modelPrefab = PrefabUtility.SaveAsPrefabAsset(selectedObject, prefabPath, out bool success);

        if (!success || modelPrefab == null)
        {
            Debug.LogError("Failed to create prefab");
            return;
        }

        Debug.Log("Prefab created successfully");
        Selection.activeObject = modelPrefab;

        // Addressables configuration
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        // FIXED: Correct null check
        if (settings == null)
        {
            Debug.LogError("Addressable Asset Settings not found. Please configure Addressables first.");
            return;
        }

        var group = settings.DefaultGroup;
        string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(modelPrefab));
        var entry = settings.CreateOrMoveEntry(guid, group);

        if (entry == null)
        {
            Debug.LogError("Failed to create addressable entry");
            return;
        }

        var modelAddress = "models/" + modelPrefab.name.ToLower().Replace(" ", "-");
        entry.address = modelAddress;
        entry.labels.Add("model");

        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            $"Successfully made {modelPrefab.name} addressable in group '{group.Name}' with address: {modelAddress}");

        

        BuildAddressables();
    }


    private static void BuildAddressables()
    {
        Debug.Log("Starting Addressables build");
        
        // Clean previous build
        AddressableAssetSettings.CleanPlayerContent();

        // Build with default script
        AddressableAssetSettings.BuildPlayerContent();

        // Alternative: Specific build script
        // var buildScript = AddressableAssetSettingsDefaultObject.Settings
        //     .DataBuilders.FirstOrDefault(d => d.name.Contains("PackedMode"));
        // AddressableAssetSettings.BuildPlayerContent(buildScript);

        Debug.Log("Addressables build completed");
    }

    private static void SaveCurrentScene()
    {
        var activeScene = EditorSceneManager.GetActiveScene();
        if (activeScene.IsValid() && activeScene.isLoaded)
        {
            if (activeScene.isDirty)
            {
                Debug.Log("Saving current scene: " + activeScene.path);
                EditorSceneManager.SaveScene(activeScene);
            }
            else
            {
                Debug.Log("Scene is not dirty - no need to save");
            }
        }
        else
        {
            Debug.Log("No active scene to save");
        }
    }
}