using UnityEngine;

using EndlessNameless.Physics;

namespace EndlessNameless.TileSystem
{
    public abstract class TriggerTile : MonoBehaviour
    {
        public abstract void Trigger (GameObject affectedObject);
        public abstract void Deactivate (GameObject affectedObject);
    }
}