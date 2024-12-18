using UnityEngine;

[CreateAssetMenu(menuName = "SettingsSaveConfiguration")]
public class SettingsSaveConfiguration : ScriptableObject, ISerializableSO
{
    // Реализация ISerializableSO
    public string SerializeToJson()
    {
        return JsonUtility.ToJson(this, true);
    }

    public void DeserializeFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }


    [Header("Audio Settings")] 
    public float MusicVolumeParam;
    public float EffectsVolumeParam;
    
    // [Header("Screen Mode Settings")]
    //
    // [Header("Screen Resolution Settings")]
}
