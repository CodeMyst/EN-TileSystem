using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EndlessNameless.TileSystem
{
    public class TileChangeOption : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private EasyTween animationObject;
        public EasyTween AnimationObject => animationObject;
        [SerializeField]
        private Image image;

        private TileChanger tileChanger;

        public Sprite Sprite { get; private set; }

        public void Initialize (TileChanger changer, Sprite sprite)
        {
            tileChanger = changer;
            image.sprite = sprite;
            Sprite = sprite;
        }

        public void OnPointerDown (PointerEventData eventData)
        {
            tileChanger.SelectSprite (this);
        }
    }
}