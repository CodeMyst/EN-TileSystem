using UnityEngine;
using UnityEngine.UI;

using SubjectNerd.Utilities;

namespace EndlessNameless.TileSystem
{
    public class TileEditor : MonoBehaviour
    {
        [SerializeField]
        private GameObject tileOptionPrefab;
        [SerializeField]
        private Transform tileOptionsParent;
        [Reorderable]
        [SerializeField]
        private TileAsset [] tileAssets;

        public TileOption SelectedTile { get; protected set; }

        private void Start ()
        {
            foreach (TileAsset asset in tileAssets)
            {
                GameObject go = Instantiate (tileOptionPrefab, tileOptionsParent);
                go.GetComponent<TileOption> ().Initialize (asset);
            }
        }

        public void SelectOption (TileOption option)
        {
            if (SelectedTile == option)
                return;

            if (SelectedTile != null)
                SelectedTile.AnimationObject.OpenCloseObjectAnimation ();

            option.AnimationObject.OpenCloseObjectAnimation ();

            SelectedTile = option;
        }
    }
}