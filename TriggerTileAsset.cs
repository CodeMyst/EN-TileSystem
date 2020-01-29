using System;

using UnityEngine;

using Rotorz.Games.Reflection;

namespace EndlessNameless.TileSystem
{
    [CreateAssetMenu (menuName = "Tiles/Trigger Tile Asset")]
    public class TriggerTileAsset : SpriteTileAsset
    {
        [SerializeField]
        [ClassExtends (typeof (TriggerTile))]
        private ClassTypeReference triggerTileType;

        public Type ComponentType => triggerTileType.Type;

        public override void Initialize (Tile tile)
        {
            base.Initialize (tile);

            foreach (Collider2D collider in tile.GameObject.GetComponents<Collider2D> ())
                collider.isTrigger = true;

            TriggerTileAsset asset = (TriggerTileAsset) tile.TileAsset;
            Type componentType = asset.ComponentType;
            if (tile.GameObject.GetComponent (componentType) != true)
                tile.GameObject.AddComponent (componentType);
        }
    }
}