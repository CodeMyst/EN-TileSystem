using System.Collections.Generic;

using UnityEngine;

using EndlessNameless.Physics;

namespace EndlessNameless.TileSystem
{
    public abstract class EffectorTile : MonoBehaviour
    {
        public abstract void Activate (GameObject affectedObject);
        public abstract void Deactivate (GameObject affectedObject);
    }
}