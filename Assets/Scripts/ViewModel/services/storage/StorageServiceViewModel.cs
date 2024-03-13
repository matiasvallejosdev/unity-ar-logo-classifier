using UniRx;
using UnityEngine;
using System.Collections.Generic;

namespace ViewModel
{  
    [CreateAssetMenu(fileName = "StorageService", menuName = "Services/Storage Service", order = 0)]
    public class StorageServiceViewModel : ScriptableObject 
    {
        public string API_URL;
        public string API_VERSION;
        public string API_KEY;
        public string API_KEY_HEADER;
        public string BUCKET_NAME; // Your S3 Bucket name
        public string BUCKET_REGION; // Your AWS Region
    }
}