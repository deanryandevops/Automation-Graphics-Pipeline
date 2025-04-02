#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public static class AddressablesRemoteConfig
{
    [MenuItem("Tools/Configure for Remote Loading")]
    public static void ConfigureRemoteLoading()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("Addressable Asset Settings not found");
            return;
        }

        // 1. Configure remote paths
        string serverUrl = "http://51.186.164.197:8000/addressables/";
        settings.profileSettings.SetValue(settings.activeProfileId, 
            "RemoteBuildPath", "ServerData/[BuildTarget]");
        settings.profileSettings.SetValue(settings.activeProfileId,
            "RemoteLoadPath", $"{serverUrl}[BuildTarget]/");

        // 2. Enable remote catalog
        settings.BuildRemoteCatalog = true;
        
        // 3. Select the packed mode builder (recommended for WebGL)
        var packedModeScript = AssetDatabase.LoadAssetAtPath<ScriptableObject>(
            "Assets/AddressableAssetsData/DataBuilders/BuildScriptPackedMode.asset");
        
        settings.ActivePlayModeDataBuilderIndex = 
            settings.DataBuilders.IndexOf(packedModeScript);
        settings.ActivePlayerDataBuilderIndex = 
            settings.DataBuilders.IndexOf(packedModeScript);

        // 4. Save changes
        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
        
        Debug.Log($"Configured for remote loading from {serverUrl}");
    }
}
#endif