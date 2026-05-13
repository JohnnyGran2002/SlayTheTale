using UnityEngine;

namespace Tools
{
    public class AttackPreviewController : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform enemyTransform;

        [SerializeField] private MeshRenderer conePreview;
        [SerializeField] private Camera previewCamera;

        private RenderTexture _renderTexture;
        
        public RenderTexture RenderTexture => _renderTexture;

        public void Initialize()
        {
            if (_renderTexture != null)  return;
            _renderTexture = new RenderTexture(1024, 1024, 24);
            
            previewCamera.targetTexture = _renderTexture;
            
            previewCamera.Render();
        }

        public void PreviewCard(CardData cardData)
        {
            Debug.Log($"Previewing {cardData}");
            switch (cardData.AreaType)
            {
                case Attack.AreaType.Cone:
                    UpdateCone(cardData);
                    break;
            }
            previewCamera.Render();
        }

        public void Render()
        {
            previewCamera.Render();
        }

        private void UpdateCone(CardData cardData)
        {
            conePreview.transform.localScale = new Vector3(cardData.Angle, 0, cardData.Range);
        }
    }
}
