using System;

using UnityEngine;

using Rotorz.Games.Reflection;

namespace EndlessNameless.TileSystem
{
    [CreateAssetMenu (menuName = "Tiles/Effector Tile Asset")]
    public class EffectorTileAsset : SpriteTileAsset
    {
        [SerializeField]
        private LayerMask collisionMask;

        [Space ()]

        [SerializeField]
        [ClassExtends (typeof (EffectorTile))]
        private ClassTypeReference effectorTileType;

        public Type ComponentType => effectorTileType.Type;

        public override void Initialize (Tile tile)
        {
            base.Initialize (tile);

            EffectorTileAsset effectorTileAsset = (EffectorTileAsset) tile.TileAsset;
            Type componentType = effectorTileAsset.ComponentType;
            if (tile.GameObject.GetComponent (componentType) != true)
                tile.GameObject.AddComponent (componentType);

            bool hasTriggerCol = false;
            Collider2D [] colliders = tile.GameObject.GetComponents<Collider2D> ();
            foreach (Collider2D col in colliders)
            {
                if (col.isTrigger)
                {
                    hasTriggerCol = true;
                    break;
                }
            }

            if (hasTriggerCol == false)
            {
                BoxCollider2D col = tile.GameObject.AddComponent<BoxCollider2D> ();
                col.isTrigger = true;
                col.size = new Vector2 (0.95f, 0.05f);
                col.offset = new Vector2 (0.5f, 1f);
            }
        }
    }
}