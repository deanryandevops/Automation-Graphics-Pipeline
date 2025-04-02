using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class JenkinsEntryPoint
{
    // Method to be called from command line or editor
    public static void ConvertModel()
    {
        SaveCurrentScene();
        
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
        //string newName = "DEAN IS STAR";
        AssetDatabase.RenameAsset(assetPath, currentModelFile);
        //Debug.Log("Asset renamed to: " + newName);

        // Create prefab
        string prefabPath = Path.Combine("Assets/ModelToConvert", $"{currentModelFile}.prefab");
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

        BuildAddressables(settings);
    }


    private static void BuildAddressables(AddressableAssetSettings settings)
    {
        Debug.Log("Starting Addressables build");
        
        // 2. Handle profile switching more safely
        string targetProfileName = "Remote"; // Change this if your profile has a different name
        bool profileFound = false;

        // Alternative way to find profile that works in newer Unity versions
        var profileIds = settings.profileSettings.GetAllProfileNames();
        foreach (var profileId in profileIds)
        {
            if (settings.profileSettings.GetProfileName(profileId) == targetProfileName)
            {
                settings.activeProfileId = profileId;
                Debug.Log($"Switched to profile: {targetProfileName} (ID: {profileId})");
                profileFound = true;
                break;
            }
        }

        if (profileFound == false)
        {
            Debug.Log("Not Found");
        }
        
        // Clean previous build
        AddressableAssetSettings.CleanPlayerContent();

        // Build with default script
        AddressableAssetSettings.BuildPlayerContent();

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
    }}