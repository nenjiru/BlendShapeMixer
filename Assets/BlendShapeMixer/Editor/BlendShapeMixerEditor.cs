using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlendShapeMixer))]
public class BlendShapeMixerEditor : Editor
{
    BlendShapeMixer _shapeMixer;
    SerializedProperty _presets;
    int expandIndex = -1;

    void OnEnable()
    {
        _shapeMixer = (BlendShapeMixer)target;
        _presets = serializedObject.FindProperty("presets");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.Space();
        if (GUILayout.Button("Clear Weight"))
        {
            _shapeMixer.Clear();
        }
        EditorGUILayout.Space();

        for (int i = 0; i < _presets.arraySize; i++)
        {
            var preset = _presets.GetArrayElementAtIndex(i);
            var name = preset.FindPropertyRelative("name");
            var handlers = preset.FindPropertyRelative("handlers");

            if (i == expandIndex)
            {
                handlers.isExpanded = true;
                expandIndex = -1;
            }

            handlers.isExpanded = EditorGUILayout.Foldout(handlers.isExpanded, name.stringValue, true);
            if (handlers.isExpanded)
            {
                EditorGUILayout.PropertyField(name, new GUIContent("Name"));
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Capture", EditorStyles.miniButtonLeft))
                    {
                        _shapeMixer.Capture(i);
                    }
                    if (GUILayout.Button("Apply", EditorStyles.miniButtonMid))
                    {
                        _shapeMixer.Apply(i);
                    }
                    if (GUILayout.Button("Duplicate", EditorStyles.miniButtonMid))
                    {
                        _presets.InsertArrayElementAtIndex(i);
                    }
                    if (GUILayout.Button("Remove", EditorStyles.miniButtonMid))
                    {
                        _presets.DeleteArrayElementAtIndex(i);
                    }
                    if (GUILayout.Button("▲", EditorStyles.miniButtonMid))
                    {
                        expandIndex = i - 1;
                        handlers.isExpanded = false;
                        _presets.MoveArrayElement(i, i - 1);
                    }
                    if (GUILayout.Button("▼", EditorStyles.miniButtonRight))
                    {
                        expandIndex = i + 1;
                        handlers.isExpanded = false;
                        _presets.MoveArrayElement(i, i + 1);
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (handlers.arraySize > 0)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    _drawMeshHandler(handlers);
                    EditorGUILayout.EndVertical();
                }
            }
        }
        serializedObject.ApplyModifiedProperties();
    }

    void _drawMeshHandler(SerializedProperty handlers)
    {
        for (int i = 0; i < handlers.arraySize; i++)
        {
            var handler = handlers.GetArrayElementAtIndex(i);
            var mesh = handler.FindPropertyRelative("skinnedMeshRenderer");
            var taget = handler.FindPropertyRelative("targets");
            EditorGUILayout.PropertyField(mesh, new GUIContent("Skinned Mesh Renderer"));
            if (mesh.objectReferenceValue != null)
            {
                _drawTargetIndex(mesh, taget);
            }
        }
    }

    void _drawTargetIndex(SerializedProperty mesh, SerializedProperty targets)
    {
        EditorGUI.indentLevel++;
        var names = _getMeshBlendNames((SkinnedMeshRenderer)mesh.objectReferenceValue);
        for (int i = 0; i < targets.arraySize; i++)
        {
            var target = targets.GetArrayElementAtIndex(i);
            var index = target.FindPropertyRelative("index");
            var weight = target.FindPropertyRelative("weight");
            EditorGUILayout.BeginHorizontal();
            {
                index.intValue = EditorGUILayout.Popup(index.intValue, names);
                weight.floatValue = EditorGUILayout.FloatField(weight.floatValue, GUILayout.Width(100));
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUI.indentLevel--;
    }

    string[] _getMeshBlendNames(SkinnedMeshRenderer item)
    {
        var mesh = item.sharedMesh;
        var names = new string[mesh.blendShapeCount];
        for (int i = 0; i < names.Length; ++i)
        {
            names[i] = mesh.GetBlendShapeName(i);
        }
        return names;
    }
}