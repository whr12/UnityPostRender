namespace PostRender
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DepthTextureBlur : PostEffectsBase
    {
        public Shader m_depthTextureBlurShader;
        private Material m_depthTextureBlurMaterial;
        public Material material
        {
            get
            {
                m_depthTextureBlurMaterial = CheckShaderAndCreateMaterial(m_depthTextureBlurShader, m_depthTextureBlurMaterial);
                return m_depthTextureBlurMaterial;
            }
        }

        [Range(0.0f, 1.0f)]
        public float blurSize = 0.5f;

        private Camera m_renderCamera;
        public new Camera camera
        {
            get
            {
                if (m_renderCamera == null)
                {
                    m_renderCamera = GetComponent<Camera>();
                }
                return m_renderCamera;
            }
        }

        private Matrix4x4 previousViewProjectionMatrix;

        private void OnEnable()
        {
            if (camera == null)
            {
                this.enabled = false;
            }
            camera.depthTextureMode |= DepthTextureMode.Depth;  
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (material != null)
            {
                material.SetFloat("_BlurSize", blurSize);

                material.SetMatrix("_PreviousViewProjectionMatrix", previousViewProjectionMatrix);
                Matrix4x4 currentViewProjectionMatrix = camera.projectionMatrix * camera.worldToCameraMatrix;
                Matrix4x4 currentViewProjectionInverseMatrix = currentViewProjectionMatrix.inverse;
                material.SetMatrix("_CurrentViewProjectionInverseMatrix", currentViewProjectionInverseMatrix);
                previousViewProjectionMatrix = currentViewProjectionMatrix;

                Graphics.Blit(source, destination, material);

            }
            else
            {
                Graphics.Blit(source, destination);
            }
        }
    }
}