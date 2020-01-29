using System.Linq;

using UnityEngine;

using Newtonsoft.Json;

namespace EndlessNameless.TileSystem
{
    [JsonObject (MemberSerialization.OptIn)]
    public class Layer
    {
        [JsonProperty]
        public string Name { get; protected set; }
        [JsonProperty]
        public int SortingOrder { get; protected set; }
        [JsonProperty]
        public int CollisionLayer { get; protected set; }
        [JsonProperty]
        public bool Visible { get; protected set; }
        [JsonProperty]
        public bool Locked { get; protected set; }
        
        [JsonProperty]
        private Tile [,] tiles;

        private GameObject gameObject;

        private int width = 25;
        private int height = 14;

        public void Initialize (GameObject gameObject, string name, int sortingOrder, int collisionLayer, bool visible, bool locked)
        {
            Name = name;
            SortingOrder = sortingOrder;
            CollisionLayer = collisionLayer;
            Visible = visible;
            Locked = locked;
            this.gameObject = gameObject;

            tiles = new Tile [width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    CreateTile (x, y);
                }
            }
        }

        public void LoadLayer (Layer layer)
        {
            ChangeSortingOrder (layer.SortingOrder);
            ChangeCollisionLayer (layer.CollisionLayer);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile myTile = GetTile (x, y);
                    Tile copyTile = layer.GetTile (x, y);

                    if (copyTile.TileAssetName == null)
                        continue;

                    myTile.Flip (copyTile.Flipped);
                    TileAsset asset = Resources.Load<TileAsset> ($@"TileAssets\LevelSet1\{copyTile.TileAssetName}");
                    myTile.ChangeTileAsset (asset);

                    if (copyTile.UseManualSprite)
                    {
                        myTile.ChangeManualSprite (((SpriteTileAsset) myTile.TileAsset).RuleSprites.First (s => s.name == copyTile.ManualSpriteName));
                    }
                }
            }
        }

        private void CreateTile (int x, int y)
        {
            GameObject tileObject = new GameObject
            {
                name = $"{x}\t{y}"
            };
            tileObject.transform.SetParent (gameObject.transform);
            tileObject.transform.position = new Vector3 (x, y);
            tileObject.layer = CollisionLayer;
            if (tileObject.activeInHierarchy != Visible)
                tileObject.SetActive (Visible);
            tileObject.AddComponent<SpriteRenderer> ();
            tiles [x, y] = new Tile (x, y, this, tileObject);
        }

        public Tile GetTile (int x, int y)
        {
            if (x < 0 || x > width - 1 || y < 0 || y > height - 1)
                return null;

            return tiles [x, y];
        }

        public void Clear ()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GetTile (x, y).ClearTile ();
                }
            }
            Object.Destroy (gameObject);
        }

        public void ChangeSortingOrder (int sortingOrder)
        {
            SortingOrder = sortingOrder;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GetTile (x, y).SpriteRenderer.sortingOrder = sortingOrder;
                }
            }
        }

        public void ChangeVisibility (ref bool value)
        {
            if (Locked)
            {
                value = Locked;
                return;
            }

            Visible = value;

            gameObject.SetActive (value);
        }

        public void ChangeLock (bool value)
        {
            Locked = value;
        }

        public void ChangeCollisionLayer (int value)
        {
            gameObject.layer = value;

            foreach (Transform t in gameObject.transform)
                t.gameObject.layer = value;

            CollisionLayer = value;
        }
    }
}