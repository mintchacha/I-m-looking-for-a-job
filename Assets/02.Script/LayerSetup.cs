#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class LayerSetup
{
    [MenuItem("Tools/Setup Required Layers")]
    public static void SetupLayers()
    {
        AddLayer("Ground");
        AddLayer("Enemy");
        AddLayer("Player");
        Debug.Log("레이어 설정 완료");
    }

    static void AddLayer(string layerName)
    {
        SerializedObject tagManager = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

        SerializedProperty layersProp = tagManager.FindProperty("layers");

        for (int i = 0; i < layersProp.arraySize; i++)
        {
            SerializedProperty sp = layersProp.GetArrayElementAtIndex(i);

            if (sp.stringValue == layerName)
                return;

            if (string.IsNullOrEmpty(sp.stringValue))
            {
                sp.stringValue = layerName;
                tagManager.ApplyModifiedProperties();
                return;
            }
        }

        Debug.LogWarning($"레이어 슬롯 부족: {layerName}");
    }
}
#endif