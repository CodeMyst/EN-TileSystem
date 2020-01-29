using UnityEngine;
using UnityEngine.EventSystems;

using LeonLaci.MystWindowSystem;

namespace EndlessNameless.TileSystem
{
    public class LevelEditor : MonoBehaviour
    {
        private static LevelEditor _instance;
        public static LevelEditor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<LevelEditor> ();

                    if (_instance == null)
                        Debug.LogError ("There isn't an instance of LevelEditor in the scene.");
                }

                return _instance;
            }
        }

        [SerializeField]
        private MystWindow editorWindow;
        [SerializeField]
        private TileEditor tileEditor;
        public TileEditor TileEditor => tileEditor;
        [SerializeField]
        private LayerEditor layerEditor;
        public LayerEditor LayerEditor => layerEditor;
        [SerializeField]
        private TileChanger tileChanger;

        private bool Enabled => editorWindow.gameObject.activeInHierarchy;

        [Header ("Graphics")]
        [SerializeField]
        private GameObject tileHoverObject;

        private bool shouldFlipSprite = false;

        private void Update ()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint (UnityEngine.Input.mousePosition);

            if (UnityEngine.Input.GetKeyDown (KeyCode.M))
            {
                editorWindow.gameObject.SetActive (!Enabled);
                if (Enabled == false)
                    tileChanger.Hide ();
            }

            tileHoverObject.SetActive (false);

            if (Enabled == false)
                return;
            if (layerEditor.SelectedLayer == null)
                return;
            if (EventSystem.current.IsPointerOverGameObject ())
                return;

            Tile tile = GetTileAtWorldCoord (LayerEditor.SelectedLayer, mousePosition);
            if (tile == null)
                return;

            tileHoverObject.SetActive (true);

            tileHoverObject.transform.position = new Vector3 (tile.X, tile.Y);

            if (UnityEngine.Input.GetMouseButton (0) && UnityEngine.Input.GetKey (KeyCode.LeftShift))
            {
                tile.ClearTile ();
            }
            else if (UnityEngine.Input.GetMouseButton (0))
            {
                if (tileEditor.SelectedTile == null)
                    return;

                tile.Flip (shouldFlipSprite);
                tile.ChangeTileAsset (tileEditor.SelectedTile.TileAsset);
            }
            else if (UnityEngine.Input.GetMouseButtonDown (1))
            {
                tileChanger.Open (tile);
            }

            if (UnityEngine.Input.GetKeyDown (KeyCode.LeftBracket))
                shouldFlipSprite = true;
            else if (UnityEngine.Input.GetKeyDown (KeyCode.RightBracket))
                shouldFlipSprite = false;
        }

        private Tile GetTileAtWorldCoord (Layer layer, Vector3 position)
        {
            int x = Mathf.FloorToInt (position.x);
            int y = Mathf.FloorToInt (position.y);

            return layer.GetTile (x, y);
        }
    }
}