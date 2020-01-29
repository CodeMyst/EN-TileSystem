using UnityEngine;

namespace EndlessNameless.TileSystem
{
    [CreateAssetMenu (menuName = "Tiles/Sprite Tile Asset")]
    public class SpriteTileAsset : TileAsset
    {
        [SerializeField]
        private Sprite defaultSprite;
        [SerializeField]
        private Sprite [] ruleSprites;
        [SerializeField]
        private bool ignoreHorizontalTiles;
        [SerializeField]
        private bool ignoreVerticalTiles;
        [SerializeField]
        private ColliderType colliderType;

        public Sprite DefaultSprite => defaultSprite;
        public Sprite [] RuleSprites => ruleSprites;
        public bool IgnoreHorizontalTiles => ignoreHorizontalTiles;
        public bool IgnoreVerticalTiles => ignoreVerticalTiles;
        public ColliderType ColliderType => colliderType;

        public override Sprite PreviewSprite
        {
            get
            {
                return defaultSprite;
            }
            protected set { }
        }

        public override void Initialize (Tile tile)
        {
            // TODO: refactor this
            switch (ColliderType)
            {
                case ColliderType.None:
                    if (tile.Collider != null)
                    {
                        Destroy (tile.GameObject.GetComponent<Collider2D> ());
                    }
                    break;

                case ColliderType.Box:
                    if (tile.Collider == null)
                    {
                        tile.GameObject.AddComponent<BoxCollider2D> ();
                    }
                    else
                    {
                        if (tile.Collider is BoxCollider2D == false)
                        {
                            Destroy (tile.Collider);
                            tile.GameObject.AddComponent<BoxCollider2D> ();
                        }
                    }
                    break;

                case ColliderType.Polygon:
                    if (tile.Collider == null)
                    {
                        tile.GameObject.AddComponent<PolygonCollider2D> ();
                    }
                    else
                    {
                        Destroy (tile.Collider);
                        tile.GameObject.AddComponent<PolygonCollider2D> ();
                    }
                    break;
            }
        }
    }
}