#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Database.Importer; // for ItemType if needed
using UnityEditor;
using UnityEngine;
using System.Globalization;

namespace SpinWheel.Scripts.Database.Editor
{
    public static class ImportFromGoogleSheetEditor
    {
        private const string GoogleSheetId = "1IlnCMq963SZgldrbcn8ccRRocPT5Ftkicj3DkhqPd7U";

        private const string RewardTargetFolder = "Assets/SpinWheel/ScriptableObjects/Database/Item";

        [MenuItem("Tools/SpinWheel/Import Rewards From Google Sheet")]
        public static async void ImportRewardsMenu()
        {
            await ImportRewardsAsync();
        }

        private static ResourceType DetermineResourceType(string itemName)
        {
            return itemName?.IndexOf("Gold", StringComparison.OrdinalIgnoreCase) >= 0
                ? ResourceType.Gold
                : ResourceType.Cash;
        }

        private static async Task ImportRewardsAsync()
        {
            CheckFolder(RewardTargetFolder);

            var cred = Resources.Load<TextAsset>("GoogleCredential");
            if (cred == null || string.IsNullOrWhiteSpace(cred.text))
            {
                Debug.LogError("GoogleCredential (Resources/GoogleCredential.txt or .json) not found or empty.");
                return;
            }

            SheetContainer container;
            try
            {
                container = new SheetContainer(new UnityLogger());
                var converter = new GoogleSheetConverter(GoogleSheetId, cred.text);
                await container.Bake(converter);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Google Sheet import failed: {ex.Message}\n{ex.StackTrace}");
                return;
            }

            if (container.ItemContainer == null || container.ItemContainer.Count == 0)
            {
                Debug.LogError("No ItemContainer data found after baking. Check your sheet & mapping.");
                return;
            }

            int created = 0, updated = 0;
            foreach (var row in container.ItemContainer)
            {
                string safeId = MakeSafeFileName(row.Name);
                string assetPath = $"{RewardTargetFolder}/so_item_{safeId}.asset";

                ItemDataSO asset;
                bool isNew = false;

                if (row.ItemType == ItemType.Currency)
                {
                    var currencyAsset = AssetDatabase.LoadAssetAtPath<CurrencyDataSO>(assetPath);
                    isNew = currencyAsset == null;

                    if (isNew)
                    {
                        currencyAsset = ScriptableObject.CreateInstance<CurrencyDataSO>();
                        AssetDatabase.CreateAsset(currencyAsset, assetPath);
                        created++;
                    }
                    else
                    {
                        updated++;
                    }

                    ResourceType resourceType = row.Name.IndexOf("Gold", StringComparison.OrdinalIgnoreCase) >= 0
                        ? ResourceType.Gold
                        : ResourceType.Cash;

                    var currencyData = new CurrencyData(
                        row.Id,
                        row.Name,
                        row.Amount,
                        row.IconName,
                        row.ItemType,
                        resourceType
                    );

                    currencyAsset.ItemData = currencyData;
                    currencyAsset.CurrencyData = currencyData;

                    asset = currencyAsset;
                }
                else
                {
                    asset = AssetDatabase.LoadAssetAtPath<ItemDataSO>(assetPath);
                    isNew = asset == null;

                    if (isNew)
                    {
                        asset = ScriptableObject.CreateInstance<ItemDataSO>();
                        AssetDatabase.CreateAsset(asset, assetPath);
                        created++;
                    }
                    else
                    {
                        updated++;
                    }

                    asset.ItemData = new ItemData(row.Id, row.Name, row.Amount, row.IconName, row.ItemType);
                }

                EditorUtility.SetDirty(asset);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[Item Import] Created {created}, Updated {updated}. Assets → {RewardTargetFolder}");

            FillItemTable(RewardTargetFolder);
        }

        private static void FillItemTable(string rewardFolder)
        {
            if (string.IsNullOrEmpty(rewardFolder) || !rewardFolder.Replace("\\", "/").StartsWith("Assets/"))
            {
                Debug.LogError("[ItemTable] rewardFolder must start with 'Assets/'. Given: " + rewardFolder);
                return;
            }

            var rewardGuids = AssetDatabase.FindAssets("t:ItemDataSO", new[] { rewardFolder });
            var allRewards = rewardGuids
                .Select(g => AssetDatabase.LoadAssetAtPath<ItemDataSO>(AssetDatabase.GUIDToAssetPath(g)))
                .Where(r => r != null)
                .Distinct()
                .OrderBy(r => r.name) 
                .ToList();

            if (allRewards.Count == 0)
            {
                Debug.LogWarning("[ItemTable] No ItemDataSO assets found to fill tables.");
                return;
            }

            var tableGuids = AssetDatabase.FindAssets("t:ItemTable");
            if (tableGuids.Length == 0)
            {
                Debug.LogWarning("[ItemTable] No ItemTable assets found.");
                return;
            }

            AssetDatabase.StartAssetEditing();
            try
            {
                foreach (var tableGuid in tableGuids)
                {
                    var tablePath = AssetDatabase.GUIDToAssetPath(tableGuid);
                    var table = AssetDatabase.LoadAssetAtPath<ItemTable>(tablePath);
                    if (table == null) continue;

                    Undo.RecordObject(table, "Fill ItemTable");

                    if (table.Items == null)
                        table.Items = new List<ItemDataSO>();
                    else
                        table.Items.Clear();

                    table.Items.AddRange(allRewards);

                    EditorUtility.SetDirty(table);
                    Debug.Log($"[ItemTable] Filled {table.name} with {table.Items.Count} ItemDataSO objects.");
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            Debug.Log($"[ItemTable] Completed: Added {allRewards.Count} ItemDataSO assets to each ItemTable.");
        }

        private static void CheckFolder(string path)
        {
            path = path.Replace("\\", "/");
            if (AssetDatabase.IsValidFolder(path)) return;

            var parts = path.Split('/');
            string acc = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                var next = $"{acc}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(acc, parts[i]);
                acc = next;
            }
        }

        private static string MakeSafeFileName(string raw)
        {
            var invalid = Path.GetInvalidFileNameChars();
            return new string(raw.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
        }
    }
}
#endif