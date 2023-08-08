using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace Kamgam.InvertedMask
{
    /// <summary>
    /// Editor Helper for creating the right materials for the mask holes
    /// (InvertedMaskHole) and the masked graphics (InvertedMaskGraphic).<br />
    /// </summary>
    [AddComponentMenu("UI/InvertedMask", 13)]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class InvertedMask : MonoBehaviour
    {
#if UNITY_EDITOR
        double ignoreHierarchyChangesUntilTime;

        protected void OnEnable()
        {
            // Since the graphics lose their ability to be masked we disable
            // the "maskable" property to signal this to the user.
            // This is not necessary for the functionality.
            var graphic = GetComponent<MaskableGraphic>();
            if (graphic != null)
                graphic.maskable = false;

            UpdateChildren();

            UnityEditor.EditorApplication.hierarchyChanged -= onHierarchyChanged;
            UnityEditor.EditorApplication.hierarchyChanged += onHierarchyChanged;
        }

        protected void OnDisable()
        {
            UnityEditor.EditorApplication.hierarchyChanged -= onHierarchyChanged;
        }

        private void onHierarchyChanged()
        {
            // Stop if in play mode to properly simulate build behaviour.
            // It would not execute in a build and we want the play mode to
            // behave as close to a build as possible.
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            if (UnityEditor.EditorApplication.timeSinceStartup < ignoreHierarchyChangesUntilTime)
                return;

            var activeGO = UnityEditor.Selection.activeGameObject;
            if (activeGO != null && this != null && activeGO.transform.IsChildOf(transform))
            {
                UpdateChildren();
            }
        }
#endif

        /// <summary>
        /// Sets the stencil comparison function for all children which are not a hole.<br />
        /// Call this after adding new ui elements via code or add the InvertedMaskGraphic
        /// component manually or assign a material that uses StencilComp = CompareFunction.GreaterEqual.
        /// <br /><br />
        /// In the Editor if play mode if off then this will be called automatically after every hierarchy change.
        /// </summary>
        public void UpdateChildren()
        {
            var graphics = GetComponentsInChildren<MaskableGraphic>(includeInactive: true);
            foreach (var graphic in graphics)
            {
                // Skip holes
                if (graphic.gameObject.GetComponent<InvertedMaskHole>() != null)
                    continue;

                // Skip graphics which already have the component
                if (graphic.gameObject.GetComponent<InvertedMaskGraphic>() != null)
                    continue;

                graphic.gameObject.AddComponent<InvertedMaskGraphic>();
            }

        }

        /// <summary>
        /// Reverts the materials for all children which are not a hole.<br />
        /// Call this before removing the InvertedMask.
        /// </summary>
        public void RevertChildren()
        {
#if UNITY_EDITOR
            ignoreHierarchyChangesUntilTime = UnityEditor.EditorApplication.timeSinceStartup + 0.3;
#endif

            var graphics = GetComponentsInChildren<InvertedMaskGraphic>(includeInactive: true);
            foreach (var graphic in graphics)
            {
                DestroyComponent(graphic);
            }
        }

        public static void DestroyComponent(Component comp)
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                DestroyImmediate(comp);
            }
            else
            {
#endif
                Destroy(comp);
#if UNITY_EDITOR
            }
#endif
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(InvertedMask))]
    public class InvertedMaskEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            if (GUILayout.Button("Add Hole"))
            {
                var invertedMask = target as InvertedMask;

                var holeObj = new GameObject("Hole", typeof(Image), typeof(InvertedMaskHole));
                holeObj.transform.SetParent(invertedMask.transform);
                holeObj.transform.localPosition = Vector3.zero;
                holeObj.transform.localRotation = Quaternion.identity;
                holeObj.transform.localScale = Vector3.one;

                var rectTransform = holeObj.transform as RectTransform;
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);

                var image = holeObj.GetComponent<Image>();
                image.useSpriteMesh = true;

                var hole = holeObj.GetComponent<InvertedMaskHole>();
                hole.Reset();

                UnityEditor.EditorGUIUtility.PingObject(holeObj);
            }

            if (GUILayout.Button("Add Content"))
            {
                var content = new GameObject("Image", typeof(Image));
                var invertedMask = target as InvertedMask;
                content.transform.SetParent(invertedMask.transform);
                content.transform.localPosition = Vector3.zero;
                content.transform.localRotation = Quaternion.identity;
                content.transform.localScale = Vector3.one;

                var rectTransform = content.transform as RectTransform;
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);

                invertedMask.UpdateChildren();

                UnityEditor.EditorGUIUtility.PingObject(content);
            }

            if (GUILayout.Button("Update Children"))
            {
                var invertedMask = target as InvertedMask;
                invertedMask.UpdateChildren();
            }

            if (GUILayout.Button("Revert Children"))
            {
                var invertedMask = target as InvertedMask;
                invertedMask.RevertChildren();
            }
        }
    }
#endif
}