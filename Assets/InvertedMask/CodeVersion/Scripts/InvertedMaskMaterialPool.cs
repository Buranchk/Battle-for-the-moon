using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Kamgam.InvertedMask
{
    /// <summary>
    /// Material pool for creating and reusing materials on the fly.
    /// </summary>
    public static class InvertedMaskMaterialPool
    {
        static Dictionary<string, Material> _stencilMaterials = new Dictionary<string, Material>();

        static string keyForMaterial(Material mat, bool showMaskGraphic, StencilOp operation)
        {
            return mat.GetInstanceID()
                + (showMaskGraphic ? "1" : "0")
                + ((int) operation).ToString();
        }
        
        static string keyForMaterial(Material mat)
        {
            return mat.GetInstanceID().ToString();
        }

        public static Material GetStencilIncreaseMaterial(Material baseMat, bool showMaskGraphic, StencilOp operation)
        {
            string key = keyForMaterial(baseMat, showMaskGraphic, operation);

            if (_stencilMaterials.ContainsKey(key) && _stencilMaterials[key] == null)
            {
                _stencilMaterials.Remove(key);
            }

            if (!_stencilMaterials.ContainsKey(key))
            {
                if (!hasStencilProperties(baseMat))
                    return baseMat;

                int stencilID = 0;
                //StencilOp operation = StencilOp.IncrementWrap;
                CompareFunction compareFunction = CompareFunction.Always;
                int readMask = 255;
                int writeMask = 255;
                ColorWriteMask colorWriteMask = showMaskGraphic ? ColorWriteMask.All : 0;
                bool useAlphaClip = operation != StencilOp.Keep && writeMask > 0;

                var customMat = new Material(baseMat);
                customMat.name = string.Format("Stencil Id:{0}, Op:{1}, Comp:{2}, WriteMask:{3}, ReadMask:{4}, ColorMask:{5} AlphaClip:{6} ({7})", stencilID, operation, compareFunction, writeMask, readMask, colorWriteMask, useAlphaClip, baseMat.name);

                customMat.SetInt("_Stencil", stencilID);
                customMat.SetInt("_StencilOp", (int)operation);
                customMat.SetInt("_StencilComp", (int)compareFunction);
                customMat.SetInt("_StencilReadMask", readMask);
                customMat.SetInt("_StencilWriteMask", writeMask);
                customMat.SetInt("_ColorMask", (int)colorWriteMask);
                customMat.SetInt("_UseUIAlphaClip", useAlphaClip ? 1 : 0);

                if (useAlphaClip)
                    customMat.EnableKeyword("UNITY_UI_ALPHACLIP");
                else
                    customMat.DisableKeyword("UNITY_UI_ALPHACLIP");

                _stencilMaterials.Add(key, customMat);
            }

            return _stencilMaterials[key];
        }

        public static Material GetStencilCompareMaterial(Material baseMat)
        {
            string key = keyForMaterial(baseMat);

            if (_stencilMaterials.ContainsKey(key) && _stencilMaterials[key] == null)
            {
                _stencilMaterials.Remove(key);
            }

            if (!_stencilMaterials.ContainsKey(key))
            {
                if (!hasStencilProperties(baseMat))
                    return baseMat;

                int stencilID = 0;
                StencilOp operation = StencilOp.Keep;
                CompareFunction compareFunction = CompareFunction.GreaterEqual;
                int readMask = 255;
                int writeMask = 255;

                var customMat = new Material(baseMat);
                customMat.name = string.Format("Stencil Id:{0}, Op:{1}, Comp:{2}, WriteMask:{3}, ReadMask:{4} ({5})", stencilID, operation, compareFunction, writeMask, readMask, baseMat.name);

                customMat.SetInt("_Stencil", stencilID);
                customMat.SetInt("_StencilOp", (int)operation);
                customMat.SetInt("_StencilComp", (int)compareFunction);
                customMat.SetInt("_StencilReadMask", readMask);
                customMat.SetInt("_StencilWriteMask", writeMask);

                _stencilMaterials.Add(key, customMat);
            }

            return _stencilMaterials[key];
        }

        static bool hasStencilProperties(Material baseMat)
        {
            if (!baseMat.HasProperty("_Stencil"))
            {
                Debug.LogWarning("Material " + baseMat.name + " doesn't have _Stencil property", baseMat);
                return false;
            }
            if (!baseMat.HasProperty("_StencilOp"))
            {
                Debug.LogWarning("Material " + baseMat.name + " doesn't have _StencilOp property", baseMat);
                return false;
            }
            if (!baseMat.HasProperty("_StencilComp"))
            {
                Debug.LogWarning("Material " + baseMat.name + " doesn't have _StencilComp property", baseMat);
                return false;
            }
            if (!baseMat.HasProperty("_StencilReadMask"))
            {
                Debug.LogWarning("Material " + baseMat.name + " doesn't have _StencilReadMask property", baseMat);
                return false;
            }
            if (!baseMat.HasProperty("_StencilWriteMask"))
            {
                Debug.LogWarning("Material " + baseMat.name + " doesn't have _StencilWriteMask property", baseMat);
                return false;
            }
            if (!baseMat.HasProperty("_ColorMask"))
            {
                Debug.LogWarning("Material " + baseMat.name + " doesn't have _ColorMask property", baseMat);
                return false;
            }

            return true;
        }

        public static void Remove(Material baseMat, bool showMaskGraphic, StencilOp operation)
        {
            string key = keyForMaterial(baseMat, showMaskGraphic, operation);

            if (_stencilMaterials.ContainsKey(key))
                _stencilMaterials.Remove(key);
        }

        public static void Remove(Material baseMat)
        {
            string key = keyForMaterial(baseMat);

            if (_stencilMaterials.ContainsKey(key))
                _stencilMaterials.Remove(key);
        }
    }
}
