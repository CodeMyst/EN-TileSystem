using UnityEngine;
using UnityEngine.Events;

using EndlessNameless.Serialization;

namespace EndlessNameless.TileSystem
{
    public class LevelManager : MonoBehaviour, ISerializable
    {
        private static LevelManager _instance;
        public static LevelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<LevelManager> ();

                    if (_instance == null)
                        Debug.LogError ("There isn't an instance of LevelEditor in the scene.");
                }

                return _instance;
            }
        }

        public Level Level { get; protected set; }

        [SerializeField]
        private int width;
        [SerializeField]
        private int height;

        [SerializeField]
        private UnityEvent onLoad;

        private void Start ()
        {
            Level = new Level (width, height, gameObject.transform);

            Serializer.RegisterSeparate (this, "LevelSet1");

            Serializer.DeserializeSeparate (this);
        }

        private void NewLevel ()
        {
            Level = new Level (width, height, gameObject.transform);

            Level.CreateLayer ("Default", 0, 0, true, false);
        }

        public void Save ()
        {
            Serializer.SerializeSeparate (this);
        }

        public object GetData ()
        {
            return Level;
        }

        public void SetData (object saveData)
        {
            Level copyLevel = (Level) saveData;

            Level = new Level (width, height, gameObject.transform);

            foreach (Layer layerToCopy in copyLevel.Layers)
            {
                Layer layer = Level.CreateLayer (layerToCopy.Name, layerToCopy.SortingOrder, layerToCopy.CollisionLayer, layerToCopy.Visible, layerToCopy.Locked);
                layer.LoadLayer (layerToCopy);
                LevelEditor.Instance.LayerEditor.ChangeOrder (layer);

                bool visible = false;
                layer.ChangeVisibility (ref visible);
            }

            onLoad?.Invoke ();
        }
    }
}