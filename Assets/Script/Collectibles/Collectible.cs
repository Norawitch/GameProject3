using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProject3.Collectibles
{
    public class Collectible : MonoBehaviour
    {
        public enum CollectibleType { Type1,  Type2, Type3 }
        public static event Action<CollectibleType> OnCollected;
        public static Dictionary<CollectibleType, int> totalPerType = new Dictionary<CollectibleType, int>();
        [SerializeField] private CollectibleType collectibleType;
        [SerializeField] private AudioSource audiosource;
        [SerializeField] private AudioClip collectSound;

        private Vector3 initialPosition;
        public float floatAmplitude = 0.001f;
        public float floatFrequency = 1f;
        private bool isCollected = false;

        private void Awake()
        {
            initialPosition = transform.localPosition;
            audiosource = GetComponent<AudioSource>();

            if (!totalPerType.ContainsKey(collectibleType))
            {
                totalPerType[collectibleType] = 0;
            }
        }
        private void Start()
        {
            //if (totalPerType[collectibleType] == 0)
            //{
            //    totalPerType[collectibleType] = FindObjectsOfType<Collectible>().Length;
            //}
            totalPerType[collectibleType]++;
        }
        private void Update()
        {
            transform.localRotation = Quaternion.Euler(0, Time.time * 100f, 0);

            // Float the object up and down
            float newY = initialPosition.y + Mathf.Sin(Time.time * floatFrequency * 2) * floatAmplitude / 6;
            transform.localPosition = new Vector3(initialPosition.x, newY, initialPosition.z);
        }


         private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isCollected)
            {
                OnCollected?.Invoke(collectibleType);
                isCollected = true;
                StartCoroutine(PlaySoundAndDestroy());
            }
        }
        private IEnumerator PlaySoundAndDestroy()
        {
            if (audiosource != null && collectSound != null)
            {
                audiosource.PlayOneShot(collectSound);
                yield return new WaitForSeconds(collectSound.length);
            }
            Destroy(gameObject);
        }
        public static void ResetTotal()
        {
            totalPerType.Clear();
        }
    }
}