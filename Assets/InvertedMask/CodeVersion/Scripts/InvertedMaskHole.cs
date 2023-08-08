using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Kamgam.InvertedMask
{
    [AddComponentMenu("UI/InvertedMask Hole", 14)]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class InvertedMaskHole : MonoBehaviour, IMaterialModifier
    {
        public bool m_ShowMaskGraphic = false;

        /// <summary>
        /// Show the graphic that is associated with the inverted Mask render area.
        /// </summary>
        public bool showMaskGraphic
        {
            get { return m_ShowMaskGraphic; }
            set
            {
                if (m_ShowMaskGraphic == value)
                    return;

                m_ShowMaskGraphic = value;
                if (graphic != null)
                    graphic.SetMaterialDirty();
            }
        }

        public StencilOp Operation = StencilOp.IncrementSaturate;

        [NonSerialized]
        private Graphic m_Graphic;

        /// <summary>
        /// The graphic associated with the Mask.
        /// </summary>
        public Graphic graphic
        {
            get { return m_Graphic ?? (m_Graphic = GetComponent<Graphic>()); }
        }

        protected void OnEnable()
        {
#if UNITY_EDITOR
            var maskableGraphic = GetComponent<MaskableGraphic>();
            if (maskableGraphic != null)
            {
                // Since the graphics lose their ability to be masked we disable
                // the "maskable" property to signal this to the user.
                // It's not strictly necessary for the functionality.
                maskableGraphic.maskable = false;

                // We also set the material to null to avoid an confustion. The actual
                // material comes from GetModifiedMaterial().
                maskableGraphic.material = null;
            }
#endif
        }

        public Material GetModifiedMaterial(Material baseMaterial)
        {
            return InvertedMaskMaterialPool.GetStencilIncreaseMaterial(baseMaterial, m_ShowMaskGraphic, Operation);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (graphic != null)
                graphic.SetMaterialDirty();

            Event e = Event.current;
            if (e?.type == EventType.ExecuteCommand && (e.commandName == "Duplicate" || e.commandName == "Paste"))
            {
                Reset();
            }
        }

        public void Reset()
        {
            Operation = StencilOp.IncrementSaturate;

            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (transform.parent != null)
                {
                    // delay by one frame to avoid executing this in Awake() due to OnValidate().
                    void setSiblingDelayed()
                    {
                        UnityEditor.EditorApplication.update -= setSiblingDelayed;

                        int holeIndex = 0;
                        for (int i = 0; i < transform.parent.childCount; i++)
                        {
                            if (transform.parent.GetChild(i) == transform)
                                continue;

                            if (transform.parent.GetChild(i).GetComponent<InvertedMaskHole>() != null)
                                holeIndex = i + 1;
                            else
                                break;
                        }

                        if (holeIndex < transform.GetSiblingIndex())
                            transform.SetSiblingIndex(holeIndex);
                    }

                    UnityEditor.EditorApplication.update += setSiblingDelayed;
                }
            }

            if (graphic != null)
                graphic.SetMaterialDirty();

            MaskUtilities.NotifyStencilStateChanged(this);
        }
#endif
    }
}
