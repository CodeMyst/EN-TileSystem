using System.Collections.Generic;

using UnityEngine;

using LeonLaci.MystWindowSystem;

namespace EndlessNameless.TileSystem
{
    public class TileChanger : MonoBehaviour
    {
        [SerializeField]
        private RectTransform canvas;
        [SerializeField]
        private MystWindow window;
        [SerializeField]
        private Vector2 windowOffset;
        [SerializeField]
        private Transform tileOptionParent;
        [SerializeField]
        private TileChangeOption tileOptionPrefab;

        private List<TileChangeOption> options = new List<TileChangeOption> ();

        private Tile selectedTile;

        private void Start ()
        {
            Hide ();
        }

        public void Open (Tile tile)
        {
            if (tile.TileAsset is SpriteTileAsset == false)
            {
                Hide ();
                return;
            }

            Clear ();

            selectedTile = tile;

            SpriteTileAsset tileAsset = (SpriteTileAsset) tile.TileAsset;

            SetWindowPosition (new Vector2 (tile.X, tile.Y));
            window.gameObject.SetActive (true);

            foreach (Sprite s in tileAsset.RuleSprites)
            {
                TileChangeOption go = Instantiate (tileOptionPrefab, tileOptionParent);
                go.Initialize (this, s);
                options.Add (go);
            }
        }

        public void Hide ()
        {
            window.gameObject.SetActive (false);

            Clear ();
        }

        private void Clear ()
        {
            foreach (TileChangeOption option in options)
                Destroy (option.gameObject);

            options.Clear ();
        }

        public void SelectSprite (TileChangeOption option)
        {
            if (selectedTile == null)
                return;

            selectedTile.ChangeManualSprite (option.Sprite);
        }

        private void SetWindowPosition (Vector2 worldPos)
        {
            worldPos += windowOffset;
            Vector2 viewportPos = Camera.main.WorldToViewportPoint (worldPos);
            Vector2 position = new Vector2
                    (
                        ((viewportPos.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f)),
                        ((viewportPos.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f))
                    );

            window.GetComponent<RectTransform> ().anchoredPosition = new Vector2
                    (
                        Mathf.Clamp (position.x, -860f, 860f),
                        Mathf.Clamp (position.y, -280f, 280f)
                    );
        }
    }
}