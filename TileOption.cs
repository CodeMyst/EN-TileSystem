using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EndlessNameless.TileSystem
{
    public class TileOption : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private EasyTween animationObject;
        public EasyTween AnimationObject => animationObject;
        [SerializeField]
        private Image image;
        
        public TileAsset TileAsset { get; protected set; }

        public void Initialize (TileAsset asset)
        {
            TileAsset = asset;
            image.sprite = TileAsset.PreviewSprite;
        }

        public void OnPointerDown (PointerEventData eventData)
        {
            LevelEditor.Instance.TileEditor.SelectOption (this);
        }
    }
}