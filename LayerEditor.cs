using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace EndlessNameless.TileSystem
{
    public class LayerEditor : MonoBehaviour
    {
        private List<LayerOption> layers = new List<LayerOption> ();

        [SerializeField]
        private Transform layerParent;
        [SerializeField]
        private GameObject layerOptionPrefab;
        private Color layerNormalColor;
        [SerializeField]
        private Color layerSelectedColor;

        private LayerOption selectedOption;
        public Layer SelectedLayer
        {
            get
            {
                if (selectedOption == null)
                    return null;

                return selectedOption.Layer;
            }
        }

        [SerializeField]
        private TMP_InputField createLayerInputField;

        [SerializeField]
        private TMP_Dropdown collisionLayerDropdown;

        private void Start ()
        {
            List<string> options = new List<string>
            {
                LayerMask.LayerToName (0)
            };

            for (int i = 8; i < 31; i++)
            {
                string layerName = LayerMask.LayerToName (i);

                if (layerName == string.Empty)
                    continue;

                options.Add (LayerMask.LayerToName (i));
            }

            collisionLayerDropdown.AddOptions (options);

            collisionLayerDropdown.interactable = false;
        }

        public void AddLayer (Layer layer)
        {
            GameObject layerOb = Instantiate (layerOptionPrefab, layerParent);
            LayerOption option = layerOb.GetComponent<LayerOption> ();
            option.Initialize (layer);
            layers.Add (option);
            layer.ChangeSortingOrder (layerOb.transform.GetSiblingIndex ());
        }

        public void SelectOption (LayerOption option)
        {
            if (selectedOption == option)
                return;

            Image image = option.GetComponent<Image> ();
            layerNormalColor = image.color;
            image.color = layerSelectedColor;

            if (selectedOption != null)
                selectedOption.GetComponent<Image> ().color = layerNormalColor;

            selectedOption = option;

            string collisionLayer = LayerMask.LayerToName (selectedOption.Layer.CollisionLayer);
            collisionLayerDropdown.value = collisionLayerDropdown.options.FindIndex (o => o.text == collisionLayer);
            collisionLayerDropdown.interactable = true;
        }

        public void CreateLayerButton ()
        {
            if (string.IsNullOrEmpty (createLayerInputField.text))
                return;

            LevelManager.Instance.Level.CreateLayer (createLayerInputField.text, 0, 0, true, false);

            createLayerInputField.text = string.Empty;
        }

        public void DeleteSelectedLayer ()
        {
            if (selectedOption == null)
                return;

            LevelManager.Instance.Level.DeleteLayer (selectedOption.Layer);

            Destroy (selectedOption.gameObject);

            selectedOption = null;
            collisionLayerDropdown.interactable = false;
        }

        public void ChangeOrder (Layer layer)
        {
            LayerOption s = layers.Find (l => l.Layer.Name == layer.Name);
            if (s == null)
                return;

            s.transform.SetSiblingIndex (layer.SortingOrder);
        }

        public void ChangeCollisionLayer (int value)
        {
            string layerName = collisionLayerDropdown.options [value].text;
            selectedOption.Layer.ChangeCollisionLayer (LayerMask.NameToLayer (layerName));
        }
    }
}