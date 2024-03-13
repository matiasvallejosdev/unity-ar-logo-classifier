using UnityEngine;
using ViewModel;
using UniRx;
using System.Collections;

namespace Components
{
    public class MenuMessageDisplay : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI responseLabel;
        public TMPro.TextMeshProUGUI statusLabel;
        public TMPro.TextMeshProUGUI loadingLabel;

        public int responseTime = 4;

        public RecognitionManagerViewModel recognitionServiceViewModel;
        private Coroutine _loadingCounter;

        void Start()
        {
            recognitionServiceViewModel.recognitionResponse.Subscribe(response =>
            {
                responseLabel.gameObject.SetActive(true);
                statusLabel.gameObject.SetActive(true);
                responseLabel.text = response?.recognition;
                statusLabel.text = response?.statusCode.ToString();
            }).AddTo(this);

            recognitionServiceViewModel.OnRecognitionStart.Subscribe(isActive =>
            {
                if (isActive)
                {
                    _loadingCounter = StartCoroutine(LoadingCounter());
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(ActiveResponse(recognitionServiceViewModel.recognitionResponse.Value));
                    loadingLabel.text = "Press Recognize to start";
                }
            }).AddTo(this);

            responseLabel.gameObject.SetActive(false);
            statusLabel.gameObject.SetActive(false);
        }

        private IEnumerator ActiveResponse(RecognitionResponse response)
        {
            yield return new WaitForSeconds(responseTime);
            responseLabel.gameObject.SetActive(false);
            statusLabel.gameObject.SetActive(false);
        }

        private IEnumerator LoadingCounter()
        {
            int counter = 0;
            while (loadingLabel.IsActive())
            {
                loadingLabel.text = $"Loading... {counter++}s";
                yield return new WaitForSeconds(1);
            }
        }
    }
}