namespace PostRender
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PostEffectsBase : MonoBehaviour
    {
        protected virtual void CheckResources()
        {
            bool isSupported = CheckSupport();
            if (!isSupported)
            {
                NotSupported();
            }
        }

        protected virtual bool CheckSupport()
        {
            return true;
        }

        protected virtual void NotSupported()
        {
            enabled = false;
        }

        protected virtual void Start()
        {
            CheckResources();
        }

        protected virtual Material CheckShaderAndCreateMaterial(Shader shader, Material material)
        {
            if (shader == null)
            {
                return null;
            }

            if (shader.isSupported && material && material.shader == shader)
            {
                return material;
            }

            if (!shader.isSupported)
            {
                return null;
            }
            else
            {
                material = new Material(shader);
                material.hideFlags = HideFlags.DontSave;
                if (material)
                {
                    return material;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}