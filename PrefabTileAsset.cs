using UnityEngine;

namespace EndlessNameless.TileSystem
{
    [CreateAssetMenu (menuName = "Tiles/Prefab Tile Asset")]
    public class PrefabTileAsset : TileAsset
    {
        [SerializeField]
        private Sprite previewSprite;

        [SerializeField]
        private GameObject prefab;

        public override Sprite PreviewSprite
        {
            get
            {
                return previewSprite;
            }

            protected set { }
        }

        public override void Initialize (Tile tile)
        {
            foreach (Transform t in tile.GameObject.transform)
                if (t.gameObject.name == "Prefab")
                    return;

            GameObject obj = Instantiate (prefab, tile.GameObject.transform.position, Quaternion.identity, tile.GameObject.transform);
            obj.name = "Prefab";
            obj.transform.position = new Vector3 (obj.transform.position.x + 0.5f, obj.transform.position.y + 0.5f);
        }
    }
}