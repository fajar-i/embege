using UnityEngine;
using UnityEditor;
using System.Text;

public class HierarchyToText : Editor
{
    [MenuItem("Tools/Copy Selected Hierarchy to Text")]
    public static void CopyHierarchy()
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.LogWarning("Pilih objek di Hierarchy terlebih dahulu!");
            return;
        }

        StringBuilder sb = new StringBuilder();
        foreach (GameObject obj in Selection.gameObjects)
        {
            BuildString(obj.transform, sb, 0);
        }

        EditorGUIUtility.systemCopyBuffer = sb.ToString();
        Debug.Log("Hierarchy berhasil disalin ke clipboard!");
    }

    private static void BuildString(Transform t, StringBuilder sb, int indent)
    {
        sb.AppendLine(new string(' ', indent * 4) + "- " + t.name);
        foreach (Transform child in t)
        {
            BuildString(child, sb, indent + 1);
        }
    }
}