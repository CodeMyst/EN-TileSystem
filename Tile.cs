using System;
using System.Linq;

using UnityEngine;

using Newtonsoft.Json;

namespace EndlessNameless.TileSystem
{
    [JsonObject (MemberSerialization.OptIn)]
    public class Tile
    {
        [JsonProperty]
        public int X { get; protected set; }
        [JsonProperty]
        public int Y { get; protected set; }

        public Layer Layer { get; protected set; }

        private GameObject gameObject;
        public GameObject GameObject => gameObject;

        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

                return _spriteRenderer;
            }
        }

        private Collider2D _collider;
        public Collider2D Collider
        {
            get
            {
                if (_collider == null)
                    _collider = gameObject.GetComponent<Collider2D> ();

                return _collider;
            }
        }

        public TileAsset TileAsset { get; protected set; }
        
        [JsonProperty (PropertyName = "T")]
        public string TileAssetName { get; private set; }

        [JsonProperty (PropertyName = "F")]
        public bool Flipped { get; private set; }

        [JsonProperty (PropertyName = "U")]
        public bool UseManualSprite { get; private set; }

        [JsonProperty (PropertyName = "S")]
        public string ManualSpriteName { get; private set; }

        public Tile (int x, int y, Layer layer, GameObject gameObject)
        {
            X = x;
            Y = y;
            Layer = layer;
            this.gameObject = gameObject;
        }

        public void ChangeManualSprite (Sprite sprite)
        {
            if (TileAsset == null)
                return;
            if (TileAsset is SpriteTileAsset == false)
                return;

            UseManualSprite = true;

            SpriteRenderer.sprite = sprite;
            ManualSpriteName = sprite.name;
        }

        public void ChangeTileAsset (TileAsset asset)
        {
            TileAsset = asset;
            TileAssetName = TileAsset.name;
            
            UpdateNeighbourTiles ();

            TileAsset.Initialize (this);
        }

        public void ClearTile ()
        {
            if (TileAsset is PrefabTileAsset)
                UnityEngine.Object.Destroy (GameObject.transform.Find ("Prefab").gameObject);
            TileAsset = null;
            TileAssetName = null;
            SpriteRenderer.sprite = null;
            if (Collider != null)
                UnityEngine.Object.Destroy (Collider);

            UpdateNeighbourTiles ();
        }

        /// <summary>
        /// Refreshes the graphic. Only gets called if the TileAsset is a SpriteTileAsset.
        /// </summary>
        public void RefreshGraphic ()
        {
            if (TileAsset == null)
                return;
            if (TileAsset is SpriteTileAsset == false)
                return;

            SpriteTileAsset tileAsset = (SpriteTileAsset) TileAsset;

            if (UseManualSprite)
            {
                SpriteRenderer.sprite = tileAsset.RuleSprites.First (s => s.name == ManualSpriteName);
                return;
            }

            string spriteName = $"{TileAsset.name}_";

            Tile t = Layer.GetTile (X, Y + 1);
            if (t != null && t.TileAsset != null && t.TileAsset.name == TileAsset.name && tileAsset.IgnoreVerticalTiles == false)
                spriteName += "U";

            t = Layer.GetTile (X + 1, Y);
            if (t != null && t.TileAsset != null && t.TileAsset.name == TileAsset.name && tileAsset.IgnoreHorizontalTiles == false)
                spriteName += "R";

            t = Layer.GetTile (X, Y - 1);
            if (t != null && t.TileAsset != null && t.TileAsset.name == TileAsset.name && tileAsset.IgnoreVerticalTiles == false)
                spriteName += "D";

            t = Layer.GetTile (X - 1, Y);
            if (t != null && t.TileAsset != null && t.TileAsset.name == TileAsset.name && tileAsset.IgnoreHorizontalTiles == false)
                spriteName += "L";

            if (Flipped)
                spriteName += "F";

            Sprite sprite = tileAsset.RuleSprites.FirstOrDefault (s => s.name == spriteName);

            if (sprite == null)
                sprite = tileAsset.DefaultSprite;

            SpriteRenderer.sprite = sprite;

            TileAsset.Initialize (this);
        }

        public void UpdateNeighbourTiles ()
        {
            Tile t = Layer.GetTile (X, Y + 1);
            t?.RefreshGraphic ();
            t = Layer.GetTile (X + 1, Y);
            t?.RefreshGraphic ();
            t = Layer.GetTile (X, Y - 1);
            t?.RefreshGraphic ();
            t = Layer.GetTile (X - 1, Y);
            t?.RefreshGraphic ();

            this?.RefreshGraphic ();
        }

        public void Flip (bool value)
        {
            if (UseManualSprite)
                return;
            Flipped = value;

            RefreshGraphic ();
        }
    }
}