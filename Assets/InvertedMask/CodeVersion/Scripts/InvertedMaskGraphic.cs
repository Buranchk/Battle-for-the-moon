using System;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kamgam.InvertedMask
{
    /// <summary>
    /// This component is added automatically to any MaskableGraphic inside the InvertedMask.<br />
    /// It does implement the IMaterialModifier. It takes the assigned material and overrides the stencil settings.
    /// </summary>
    [AddComponentMenu("UI/InvertedMask Graphic", 15)]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class InvertedMaskGraphic : MonoBehaviour, IMaterialModifier
    {
        [SerializeField]
        protected bool m_Masked = true;
        public bool Masked
        {
            get => m_Masked;
            set
            {
                if (value == m_Masked)
                    return;

                m_Masked = value;

                if (graphic != null)
                    graphic.SetMaterialDirty();
            }
        }

        [NonSerialized]
        private Graphic m_Graphic;

        public Graphic graphic
        {
            get { return m_Graphic ?? (m_Graphic = GetComponent<Graphic>()); }
        }

        public Material GetModifiedMaterial(Material baseMaterial)
        {
            if (!m_Masked)
                return baseMaterial;
            else
                return InvertedMaskMaterialPool.GetStencilCompareMaterial(baseMaterial);
        }

        private void OnDestroy()
        {
            if (graphic != null)
                graphic.SetMaterialDirty();
        }

#region Editor

#if UNITY_EDITOR
        const string CommandNameDuplicate = "Duplicate";
        const string CommandNamePaste = "Paste";

        protected void OnEnable()
        {
            onEnableInEditor();

            EditorApplication.hierarchyChanged -= onHierarchyChanged;
            EditorApplication.hierarchyChanged += onHierarchyChanged;
        }
        
        protected void OnDisable()
        {
            EditorApplication.hierarchyChanged -= onHierarchyChanged;
        }

        protected void onEnableInEditor()
        {
            var maskableGraphic = GetComponent<MaskableGraphic>();
            if (maskableGraphic != null)
            {
                // Since the graphics lose their ability to be masked we disable
                // the "maskable" property to signal this to the user.
                // It's not strictly necessary for the functionality.
                maskableGraphic.maskable = false;
            }
        }

        private void onHierarchyChanged()
        {
            // Stop if in play mode to properly simulate build behaviour.
            // It would not execute in a build and we want the play mode to
            // behave as close to a build as possible.
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            // Destroy self if moved outside of an inverted mask.
            var activeGO = Selection.activeGameObject;
            if (activeGO != null && this != null)
            {
                var comp = transform.GetComponentInParent<InvertedMask>();
                if(comp == false)
                {
                    InvertedMask.DestroyComponent(this);
                    return;
                }
            }
        }

        public void OnValidate()
        {
            if (graphic != null)
                graphic.SetMaterialDirty();

            Event e = Event.current;
            if (e?.type == EventType.ExecuteCommand && (e.commandName == CommandNameDuplicate || e.commandName == CommandNamePaste))
            {
                Reset();
            }
        }

        public void Reset()
        {
            if (graphic != null)
                graphic.SetMaterialDirty();

            MaskUtilities.NotifyStencilStateChanged(this);
        }
#endif

#endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(InvertedMaskGraphic))]
    public class InvertedMaskGraphicEditor : Editor
    {
        SerializedProperty maskedProp;


        private void OnEnable()
        {
            maskedProp = serializedObject.FindProperty("m_Masked");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(maskedProp, new GUIContent("Masked", "Should this graphic be affected by the inverted masks?"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

}
