using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Database.Importer;
using SpinWheel.Scripts.Database.Importer.Containers;
using SpinWheel.Scripts.SceneControl;
using SpinWheel.Scripts.Utility.Event;
using UnityEditor;
using UnityEngine;

namespace SpinWheel.Scripts.Database
{
    public class ImportFromGoogleSheet : MonoBehaviour
    {
        public ItemRuntimeTableData ItemRuntimeTableData;
        private const string GoogleSheetId = "1IlnCMq963SZgldrbcn8ccRRocPT5Ftkicj3DkhqPd7U"; // doc ID
        private SheetContainer _container;

        private void Awake()
        {
            StartCoroutine(Import());
        }

        private IEnumerator Import()
        {
            string credPath = "";
            ResourceRequest textureRequest = Resources.LoadAsync("GoogleCredential");
            yield return textureRequest;
            credPath = textureRequest.asset.ToString();

            new SceneLoadProgressEvent(0.3f).Raise();
            Debug.Log($"[BakingSheet] Sheet ID: {GoogleSheetId}");

            var logger = new UnityLogger();
            _container = new SheetContainer(logger);
            var converter = new GoogleSheetConverter(GoogleSheetId, credPath);
            JsonSheetConverter jsonConverter = new(Application.persistentDataPath); //new("Assets/Database/JSONFiles");
            Task task = _container.Bake(converter);
            yield return new WaitUntil(() => task.IsCompleted);
            task = _container.Store(jsonConverter);
            yield return new WaitUntil(() => task.IsCompleted);

            Debug.Log("[BakingSheet] Google Sheet import success.");
            ProcessRewardData();

            new SceneLoadProgressEvent(0.7f).Raise();
            yield return new WaitForSeconds(1f); // slight delay
            new SceneLoadProgressEvent(0.9f).Raise();
            yield return new WaitForSeconds(0.3f); // slight delay

            Debug.Log("[BakingSheet] Reward Item Data imported.");
            new SceneLoadProgressEvent(1f).Raise();
        }

        private void ProcessRewardData()
        {
            if (ItemRuntimeTableData.ItemDataList.Count > 0) return;

            foreach (ItemContainer.Row row in _container.ItemContainer)
            {
                ItemData itemData = CreateItemData(row);
                ItemRuntimeTableData.ItemDataList.Add(itemData);
            }
        }

        private ItemData CreateItemData(ItemContainer.Row row)
        {
            if (row.ItemType == ItemType.Currency)
            {
                Debug.Log("currency created");
                ResourceType resourceType = DetermineResourceType(row.Name);
                return new CurrencyData(row.Id, row.Name, row.Amount, row.IconName, row.ItemType, resourceType);
            }

            return new ItemData(row.Id, row.Name, row.Amount, row.IconName, row.ItemType);
        }

        private ResourceType DetermineResourceType(string itemName)
        {
            return itemName.Contains("Gold") ? ResourceType.Gold : ResourceType.Cash;
        }
    }
}