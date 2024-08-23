using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GameProject3.Collectibles.Collectible;

namespace GameProject3.Collectibles
{
    public class CollectibleCount : MonoBehaviour
    {
        [SerializeField] private TMP_Text countTextType1;
        [SerializeField] private TMP_Text countTextType2;
        [SerializeField] private TMP_Text countTextType3;
        [SerializeField] private GameObject countUI;
        [SerializeField] GameObject collectibleQuest1;

        public bool canFinishLevel = false;

        private Dictionary<CollectibleType, int> collectibleCounts = new Dictionary<CollectibleType, int>();
        private bool isQuest1Active = false;

        // Start is called before the first frame update
        void Start()
        {
            foreach (CollectibleType type in System.Enum.GetValues(typeof(CollectibleType)))
            {
                collectibleCounts[type] = 0;
            }
            UpdateCount();
        }

        void OnEnable()
        {
            Collectible.OnCollected += OnCollectibleCollected;
        }
        void OnDisable()
        {
            Collectible.OnCollected -= OnCollectibleCollected;
        }
        void OnCollectibleCollected(CollectibleType type)
        {
            if (isQuest1Active)
            {
                collectibleCounts[type]++;
                UpdateCount();
                CheckIfAllCollected();
            }
        }
        public void StartQuest()
        {
            isQuest1Active = true;
            countUI.SetActive(true);
            UpdateCount();
        }

        public void UpdateCount()
        {
            if (isQuest1Active)
            {
                    collectibleQuest1.SetActive(true);
                
                    // countText.text = $"{count} / {Collectible.total}";
                    countTextType1.text = $" - Find all apples {collectibleCounts[CollectibleType.Type1]} / {Collectible.totalPerType[CollectibleType.Type1]}";
                    countTextType2.text = $" - Find all loaf of bread {collectibleCounts[CollectibleType.Type2]} / {Collectible.totalPerType[CollectibleType.Type2]}";
                    countTextType3.text = $" - Find all healing potion {collectibleCounts[CollectibleType.Type3]} / {Collectible.totalPerType[CollectibleType.Type3]}";
            }
        }
        private void CheckIfAllCollected()
        {
            bool allCollected = true;
            foreach (CollectibleType type in collectibleCounts.Keys)
            {
                if (collectibleCounts[type] < Collectible.totalPerType[type])
                {
                    allCollected = false;
                    break;
                }
            }

            if (allCollected)
            {
                canFinishLevel = true;
                Debug.Log("All collectibles for quest 1 collected! You can now finish the level.");
            }
        }
    }
}
