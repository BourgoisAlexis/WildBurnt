using UnityEngine;

public class DataLoader : MonoBehaviour {
    public static DataLoader Instance;

    #region Variables
    [SerializeField] private Sprite[] _tileSprites;
    #endregion


    private void Awake() {
        if (Instance == null)
            Instance = this;
        else {
            DestroyImmediate(gameObject);
            Debug.LogError("Doubled instance");
        }
    }

    public Sprite LoadTileSprite(int index) {
        return _tileSprites[index];
    }
}
