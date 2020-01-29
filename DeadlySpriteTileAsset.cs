using System;
using UnityEngine;

namespace EndlessNameless.TileSystem
{
    [CreateAssetMenu (menuName = "Tiles/Deadly Sprite Tile Asset")]
    public class DeadlySpriteTileAsset : SpriteTileAsset
    {
        public override void Initialize (Tile tile)
        {
            base.Initialize (tile);

            DeadlySpriteTileAsset tileAsset = (DeadlySpriteTileAsset) tile.TileAsset;
            Type componentType = typeof (DeadlyTile);
            if (tile.GameObject.GetComponent (componentType) != true)
            {
                tile.GameObject.AddComponent (componentType);
            }
        }
    }
}