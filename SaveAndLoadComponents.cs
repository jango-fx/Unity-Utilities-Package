#if UNITY_EDITOR
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
    [ContextMenuItem("Load the Preset", "LoadThePreset")]
    public Preset thePreset;
    void LoadThePreset() { LoadPreset(thePreset); }

    // register an event handler when the class is initialized
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
                    Debug.Log("[EXIT PLAY]: " + gameObject.transform.position);
                    SaveTransform();
                    break;
                case PlayModeStateChange.EnteredEditMode:                           // not working, since GO instance doesnt exist anymore
                    Debug.Log("[ENTER EDIT]: " + gameObject.transform.position);
                    LoadTransform();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    Debug.Log("[ENTER PLAY]: " + gameObject.transform.position);
                    LoadTransform();
                    break;
            }
    }

    void OnValidate() { if (autoSaveLoad) LoadAllComponents(); }


    [ContextMenu("Load Transform")]
    void LoadTransform()
    { if (thePreset != null) ApplyPresetAsset(thePreset, this.gameObject.transform); }

    [ContextMenu("Save Transform")]
    void SaveTransform()
    { CreatePresetAsset(this.gameObject.transform, presetFolder); }


    [ContextMenu("Save All Components")]
    void SaveAllComponents()
    {
        foreach (Component c in gameObject.GetComponents(typeof(Component)))
        {
            if (c.GetType() != this.GetType())
                CreatePresetAsset(c, CreatFileName(c));
        }
    }

    [ContextMenu("Load All Components")]
    void LoadAllComponents()
    {
        foreach (Component c in gameObject.GetComponents(typeof(Component)))
        {
            if (c.GetType() != this.GetType())
            {

                string path = CreatFileName(c);
                Preset preset = AssetDatabase.LoadAssetAtPath<Preset>(path);
                LoadPreset(preset);
            }
        }
    }

    void LoadPreset(Preset preset)
    {
        Debug.Log(preset);
        if (preset != null)
        {
            System.Type type = System.Type.GetType(preset.GetTargetFullTypeName() + ", UnityEngine");
            if (type != null)
            {
                var component = this.gameObject.GetComponent(type);
                ApplyPresetAsset(preset, component);
            }
        }
    }

    string CreatFileName(Component c)
    {
        return "Assets/" + presetFolder + "/" + this.gameObject.name + "/" + c.GetType().Name + ".preset";
    }
    string CreatFileName(Preset p)
    {
        return "Assets/" + presetFolder + "/" + this.gameObject.name + "/" + p.GetTargetTypeName() + ".preset";
    }

    string CreateFolderPath()
    {
        return "Assets/" + presetFolder + "/" + this.gameObject.name + "/";
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
    public void CreatePresetAsset(Object source, string name)
    {
        Preset preset = new Preset(source);
        if (!System.IO.Directory.Exists(CreateFolderPath()))
        { System.IO.Directory.CreateDirectory(CreateFolderPath()); }
        AssetDatabase.CreateAsset(preset, CreatFileName(preset));
    }
}
#endif