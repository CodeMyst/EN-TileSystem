using UnityEngine;

namespace EndlessNameless.TileSystem
{
    public abstract class TileAsset : ScriptableObject
    {
        public abstract void Initialize (Tile tile);
        public abstract Sprite PreviewSprite { get; protected set; }
    }
}