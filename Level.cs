using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json;

using EndlessNameless.Serialization;

namespace EndlessNameless.TileSystem
{
    [JsonObject (MemberSerialization.OptIn)]
    public class Level
    {
        [JsonProperty]
        private List<Layer> layers = new List<Layer> ();
        public List<Layer> Layers => layers;
        [JsonProperty]
        private int width;
        [JsonProperty]
        private int height;

        public int Width => width;
        public int Height => height;

        Transform levelParent;

        public Level (int width, int height, Transform levelParent)
        {
            this.width = width;
            this.height = height;
            this.levelParent = levelParent;
        }

        public Layer CreateLayer (string name, int sortingOrder, int collisionLayer, bool visible, bool locked)
        {
            GameObject go = new GameObject ();
            go.transform.SetParent (levelParent);
            go.name = name;
            go.layer = collisionLayer;
            go.SetActive (visible);

            Layer layer = new Layer ();
            layer.Initialize (go, name, sortingOrder, collisionLayer, visible, locked);

            LevelEditor.Instance.LayerEditor.AddLayer (layer);
            layers.Add (layer);

            return layer;
        }

        public void DeleteLayer (Layer layer)
        {
            layer.Clear ();
            layers.Remove (layer);
        }
    }
}