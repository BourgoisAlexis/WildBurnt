using System;
using UnityEditor;
using UnityEngine;

public class DataLoader : MonoBehaviour {
    public static DataLoader Instance;

    #region Variables
    [field: SerializeField] public Sprite[] TileSprites { get; private set; }
    [field: SerializeField] public ItemScriptable[] ItemScriptables { get; private set; }
    #endregion


    private void Awake() {
        if (Instance == null)
            Instance = this;
        else {
            DestroyImmediate(gameObject);
            Debug.LogError("Doubled instance");
        }
    }

#if UNITY_EDITOR
    public void UpdateDatas() {
        string[] folders = new string[] { "Assets/Scriptables" };
        string[] guids = AssetDatabase.FindAssets("t:ItemScriptable", folders);
        ItemScriptables = new ItemScriptable[guids.Length];

        for (int i = 0; i < guids.Length; i++)
            ItemScriptables[i] = AssetDatabase.LoadAssetAtPath<ItemScriptable>(AssetDatabase.GUIDToAssetPath(guids[i]));
    }
#endif

    public Sprite LoadTileSprite(TileType tileType) {
        return LoadTileSprite((int)tileType);
    }

    public Sprite LoadTileSprite(int index) {
        return TileSprites[index];
    }

    public ItemModel LoadItemModel(int id) {
        if (id == GameUtilsAndConsts.EMPTY_INT)
            return ItemModel.Empty();

        ItemScriptable item = Array.Find(ItemScriptables, x => x.Id == id);
        return new ItemModel(item);
    }
}
