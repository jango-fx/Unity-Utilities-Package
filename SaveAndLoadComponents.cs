#if UNITY_EDITOR
using System.IO;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;

// ensure class initializer is called whenever scripts recompile
[InitializeOnLoadAttribute]
public class SaveAndLoadComponents : MonoBehaviour
{
    public string presetFolder = "tmpPresets";
    public bool autoSaveLoad = true;
    [Space(10)]
    [ContextMenuItem("load preset", "LoadThePreset")]
    [ContextMenuItem("reset", "ClearThePreset")]
    public Preset preset;
    void LoadThePreset() {  LoadPreset(preset, this.gameObject);  }
    void ClearThePreset() {  preset = null;  }
    [Space(10)]
    public GameObject[] additionalTrackedObjects;

    void Start()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    private void LogPlayModeState(PlayModeStateChange state)
    {
        Debug.Log("[STATE CHANGE]: " + state);
        if (autoSaveLoad)
            switch (state)
            {
                case PlayModeStateChange.ExitingPlayMode:
                    Debug.Log("[EXIT PLAY]");
                    SaveAllComponents();
                    break;
                case PlayModeStateChange.EnteredEditMode:                           // not working, since GO instance doesnt exist anymore
                    Debug.Log("[ENTER EDIT]");
                    LoadAllComponents();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    Debug.Log("[ENTER PLAY]");
                    LoadAllComponents();
                    break;
            }
    }

    void OnValidate() { if (autoSaveLoad) LoadAllComponents(); }


    [ContextMenu("Load Transform")]
    void LoadTransform()
    { if (preset != null) ApplyPresetAsset(preset, this.gameObject.transform); }

    [ContextMenu("Save Transform")]
    void SaveTransform()
    { CreatePresetAsset(this.gameObject.transform, presetFolder); }


    [ContextMenu("Save All Components")]
    void SaveAllComponents()
    {
        SaveAllComponents(this.gameObject);
        foreach(GameObject go in additionalTrackedObjects)
            SaveAllComponents(go);
    }

    void SaveAllComponents(GameObject go)
    {
        foreach (Component c in go.GetComponents(typeof(Component)))
        {
            if (c.GetType() != this.GetType())
                CreatePresetAsset(c, CreateFileName(c, go));
        }
    }


    [ContextMenu("Load All Components")]
    void LoadAllComponents()
    {
        LoadAllComponents(this.gameObject);
        foreach(GameObject go in additionalTrackedObjects)
            LoadAllComponents(go);
    }

    void LoadAllComponents(GameObject go)       // TODO: other way around, look in folder for presets and apply all, creating new components if necessary
    {
        foreach (Component c in go.GetComponents(typeof(Component)))
        {
            if (c.GetType() != this.GetType())
            {
                string path = CreateFileName(c, go);

                Preset preset = AssetDatabase.LoadAssetAtPath<Preset>(path);
                ApplyPresetAsset(preset, c);
            }
        }
    }

    void LoadPreset(Preset preset, GameObject go)
    {
        if (preset != null)
        {
            System.Type type = System.Type.GetType(preset.GetTargetFullTypeName() + ", UnityEngine");
            if (type != null)
            {
                var component = go.GetComponent(type);
                ApplyPresetAsset(preset, component);
            }
        }
    }

    string CreateFileName(Component c, GameObject go)
    {
        return "Assets/" + presetFolder + "/" + go.name+go.GetInstanceID() + "/" + c.GetType().Name + ".preset";
    }

    string CreateFolderPath(GameObject go)
    {
        return "Assets/" + presetFolder + "/" + go.name+go.GetInstanceID() + "/";
    }

    public bool CopyObjectSerialization(Object source, Object target)
    {
        Preset preset = new Preset(source);
        return preset.ApplyTo(target);
    }

    // This method creates a Preset from a given Object and save it as an asset in the project.
    public bool ApplyPresetAsset(Preset preset, Object target)
    {
        return preset.ApplyTo(target);
    }

    // This method creates a Preset from a given Object and save it as an asset in the project.
    public void CreatePresetAsset(Object source, string filePath)
    {
        Preset preset = new Preset(source);
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        
        AssetDatabase.CreateAsset(preset, filePath);
    }
}
#endif