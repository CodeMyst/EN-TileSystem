using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;

namespace EndlessNameless.TileSystem
{
    public class LayerOption : MonoBehaviour, IPointerDownHandler
    {
        public Layer Layer { get; protected set; }

        [Header ("Graphics")]

        [SerializeField]
        private Toggle visibilityToggle;
        [SerializeField]
        private Toggle lockToggle;
        [SerializeField]
        private TextMeshProUGUI layerName;

        public void Initialize (Layer layer)
        {
            Layer = layer;

            visibilityToggle.isOn = layer.Visible;
            lockToggle.isOn = layer.Locked;
            layerName.SetText (layer.Name);
        }

        public void OnPointerDown (PointerEventData eventData)
        {
            LevelEditor.Instance.LayerEditor.SelectOption (this);
        }

        public void MoveUp ()
        {
            if (transform.GetSiblingIndex () <= 0)
                return;

            transform.SetSiblingIndex (transform.GetSiblingIndex () - 1);

            Layer.ChangeSortingOrder (transform.GetSiblingIndex ());
        }

        public void MoveDown ()
        {
            if (transform.GetSiblingIndex () >= transform.childCount - 1)
                return;

            transform.SetSiblingIndex (transform.GetSiblingIndex () + 1);

            Layer.ChangeSortingOrder (transform.GetSiblingIndex ());
        }

        public void OnVisibilityValueChange (bool value)
        {
            bool visible = value;
            Layer.ChangeVisibility (ref visible);
            visibilityToggle.isOn = visible;
        }

        public void OnLockValueChange (bool value)
        {
            Layer.ChangeLock (value);
        }
    }
}