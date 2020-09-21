using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AIAnimations))]
public class AIAnimationsEditor : Editor
{
    SerializedObject m_soTarget; 
    SerializedProperty m_idleAnimation;
    SerializedProperty m_walkAnimation;
    SerializedProperty m_fightAnimation;
    SerializedProperty m_hitAnimation;
    SerializedProperty m_dieAnimation;

    void OnEnable()
    {
        m_soTarget = new SerializedObject(target);
        m_idleAnimation = m_soTarget.FindProperty("idleAnimation");        
        m_walkAnimation = m_soTarget.FindProperty("walkAnimation");
        m_fightAnimation = m_soTarget.FindProperty("fightAnimation");
        m_hitAnimation = m_soTarget.FindProperty("hitAnimation");
        m_dieAnimation = m_soTarget.FindProperty("dieAnimation");
    }

    void OnSceneGUI()
    {
    }

    public override void OnInspectorGUI()
    {

        m_soTarget.Update();

        EditorGUILayout.PropertyField(m_idleAnimation);
        EditorGUILayout.PropertyField(m_walkAnimation);
        EditorGUILayout.PropertyField(m_fightAnimation);
        EditorGUILayout.PropertyField(m_hitAnimation);
        EditorGUILayout.PropertyField(m_dieAnimation);
       
        m_soTarget.ApplyModifiedProperties();
        //script.walkAnimation = EditorGUILayout.TextField("WP Prefix", script.walkAnimation);
        //script.fightAnimation = EditorGUILayout.TextField("WP Prefix", script.fightAnimation);
        //script.hitAnimation = EditorGUILayout.TextField("WP Prefix", script.hitAnimation);
        //script.dieAnimation = EditorGUILayout.TextField("WP Prefix", script.dieAnimation);
    }
   
}
