using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;
using Random = UnityEngine.Random;
using SimpleJSON;

namespace Infrastructure
{
    public class StorageImageGateway : IStorageImageGateway
    {
        public IObservable<StorageResponse> PostImage(StorageServiceViewModel storageService, Texture2D texture)
        {
            return Observable.FromCoroutine<StorageResponse>(observer => PostImage(observer, storageService, texture));
        }

        IEnumerator PostImage(IObserver<StorageResponse> observer, StorageServiceViewModel storageService, Texture2D texture)
        {
            string API_KEY = storageService.API_KEY;
            string API_KEY_HEADER = storageService.API_KEY_HEADER;

            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string postUrl = storageService.API_URL + storageService.API_VERSION + "/image";
            StorageRequest storageRequest = new StorageRequest()
            {
                key = $"unity_{datetime}.png",
                expiresin = 3000,
            };
            Debug.Log($"URL: {postUrl}");
            Debug.Log($"storageRequest: {storageRequest}");

            string jsonData = JsonUtility.ToJson(storageRequest);


            UnityWebRequest wwwGetS3UrlPost = new UnityWebRequest(postUrl, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            wwwGetS3UrlPost.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            wwwGetS3UrlPost.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            // Set the content type to application/json
            wwwGetS3UrlPost.SetRequestHeader("Content-Type", "application/json");

            // Add the x-api-key to the request headers
            wwwGetS3UrlPost.SetRequestHeader(API_KEY_HEADER, API_KEY);

            yield return wwwGetS3UrlPost.SendWebRequest();

            if (wwwGetS3UrlPost.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to get AWS upload URL: {wwwGetS3UrlPost.error}");
                observer.OnError(new Exception($"Failed to get AWS upload URL: {wwwGetS3UrlPost.error}"));
                yield break;
            }

            JSONNode response = JSON.Parse(wwwGetS3UrlPost.downloadHandler.text);
            JSONNode body = response["body"];
            JSONNode fields = body["fields"];
            string postUrltoUpload = body["url"].Value;
            string fileName = fields["key"].Value;

            Debug.Log($"URL: {postUrltoUpload}");
            Debug.Log($"Fields: {fields}");

            // Use the corrected BuildUploadRequest method
            WWWForm form = AWSWebFormBuilder.BuildUploadRequest(fields, texture, fileName);

            UnityWebRequest wwwUploadImage = UnityWebRequest.Post(postUrltoUpload, form);
            yield return wwwUploadImage.SendWebRequest();

            if (wwwUploadImage.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to upload image to AWS: {wwwUploadImage.error}");
                observer.OnError(new Exception($"Failed to upload image to AWS: {wwwUploadImage.error}"));
            }

            // string bucketName = "ai-logo-recognition-serverless";// Your S3 Bucket name
            // string region = "us-east-2"; // Your AWS Region
            string region = storageService.BUCKET_REGION; // Your AWS Region
            string bucketName = storageService.BUCKET_NAME; // Your S3 Bucket name
            string objectKey = fields["key"].Value; // Assuming this is the full path of the file within the bucket

            string uploadedImageUrl = $"https://{bucketName}.s3.{region}.amazonaws.com/{objectKey}";
            Debug.Log("Uploaded Image URL: " + uploadedImageUrl);

            var storageResponse = new StorageResponse()
            {
                imageUrl = uploadedImageUrl,
                statusCode = wwwUploadImage.responseCode.ToString(),
                error = false,
            };

            observer.OnNext(storageResponse);
            observer.OnCompleted();
        }
    }
}