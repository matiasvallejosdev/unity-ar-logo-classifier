using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;
using Random = UnityEngine.Random;
using SimpleJSON;
using System.Text;

namespace Infrastructure
{
    public class BrandRecognitionGateway : IBrandRecognitionGateway
    {
        public IObservable<RecognitionResponse> GetRecogntion(RecognitionServiceViewModel recognitionService, string imageUrl)
        {
            // Using UniRx to create an observable from coroutine to get the recognition of the brand
            return Observable.FromCoroutine<RecognitionResponse>(observer => RecognizeBrand(observer, recognitionService, imageUrl));
        }

        IEnumerator RecognizeBrand(IObserver<RecognitionResponse> observer, RecognitionServiceViewModel recognitionService, string imageUrl)
        {
            Debug.Log($"RecognizeBrand: {imageUrl}");
            string postUrl = recognitionService.API_URL + recognitionService.API_VERSION + "/image/recognition";
            var recognitionRequest = new RecognitionRequest()
            {
                image_url = imageUrl
            };
            string API_KEY = recognitionService.API_KEY;
            string API_KEY_HEADER = recognitionService.API_KEY_HEADER;

            string jsonData = JsonUtility.ToJson(recognitionRequest);
            UnityWebRequest www = new UnityWebRequest(postUrl, "POST");
            byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            // Set the content type to application/json
            www.SetRequestHeader("Content-Type", "application/json");

            // Add the x-api-key to the request headers
            www.SetRequestHeader(API_KEY_HEADER, API_KEY);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {www.error}");
                var errorResponse = new RecognitionResponse()
                {
                    error = true,
                    errorMesssage = www.error,
                    statusCode = www.responseCode.ToString()
                };
                observer.OnNext(errorResponse);
            }
            else
            {
                JSONNode response = JSONNode.Parse(www.downloadHandler.text);
                Debug.Log($"Response: {response}");
                JSONNode body = response["body"];
                var recognitionResponse = new RecognitionResponse(){
                    recognition = body["recognition"].Value,
                    image_url = body["image_url"].Value,
                    error = false,
                    statusCode = www.responseCode.ToString()
                };
                recognitionResponse.image_url = imageUrl;
                recognitionResponse.error = false;
                recognitionResponse.statusCode = www.responseCode.ToString();
                observer.OnNext(recognitionResponse);
            }

            observer.OnCompleted();
        }
    }
}