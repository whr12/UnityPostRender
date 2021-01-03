namespace PostRender
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AccumulationBuffer : PostEffectsBase
    {
        public Shader accumulationBufferShader;

        [Range(0.0f, 0.9f)]
        public float blurAmount = 0.5f;

        private Material m_accumulateMaterial;
        public Material material
        {
            get
            {
                m_accumulateMaterial = CheckShaderAndCreateMaterial(accumulationBufferShader, m_accumulateMaterial);
                return m_accumulateMaterial;
            }
        }

        private RenderTexture m_accumulateTexture;

        private void OnDisable()
        {
            DestroyImmediate(m_accumulateTexture);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (material != null)
            {
                if (m_accumulateTexture == null || m_accumulateTexture.width != source.width || m_accumulateTexture.height != source.height)
                {
                    DestroyImmediate(m_accumulateTexture);
                    m_accumulateTexture = new RenderTexture(source.width, source.height, 0);
                    m_accumulateTexture.hideFlags = HideFlags.DontSave;
                    Graphics.Blit(source, m_accumulateTexture);
                }
            }

            m_accumulateTexture.MarkRestoreExpected();

            //float blur = blurAmount * Time.deltaTime;
            //blur = Mathf.Clamp(blur, 0, 0.9f);
            //Debug.Log(blur);
            material.SetFloat("_BlurAmount", 1.0f - blurAmount);

            Graphics.Blit(source, m_accumulateTexture, material);
            Graphics.Blit(m_accumulateTexture, destination);
        }
    }
}
