using UniRx;
using UnityEngine;
using System.Collections.Generic;

namespace ViewModel
{  
    [CreateAssetMenu(fileName = "RecognitionService", menuName = "Services/Recognition Service", order = 0)]
    public class RecognitionServiceViewModel : ScriptableObject 
    {
        public string API_URL;
        public string API_VERSION;
        public string API_KEY;
        public string API_KEY_HEADER;
    }
}