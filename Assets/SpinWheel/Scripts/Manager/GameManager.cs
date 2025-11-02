using System;
using System.Net.Mime;
using Sirenix.OdinInspector;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Utility;
using UnityEngine;

namespace SpinWheel.Scripts.Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public int StartIndex;
        public bool IsRuntimeUsed { get; set; }
        public ItemRuntimeTableData ItemRuntimeTableData;

        [InfoBox(
            "Data must be imported before using this ItemTable asset. If you didn't import data, this asset will be empty." +
            " Please do import from Tools/SpinWheel/Import - Otherwise ItemRuntimeTableData will be used to create temporary item list.")]
        public ItemTable ItemTable;

        private void Awake()
        {
            InitializeSingleton();
            IsRuntimeUsed = !HasItems();
        }

        private void Start()
        {
            QualitySettings.vSyncCount = 0;
        }

        private bool HasItems()
        {
            return ItemTable.Items.Count > 0;
        }
    }
}