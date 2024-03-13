
using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Infrastructure
{
    public class RuntimeLibraryGateway : IRuntimeLibraryGateway
    {
        public IObservable<Unit> AddImageRuntimeLibrary(Texture2D screenShotTex, ARTrackedImageManager trackedImageManager, string imageRecognition)
        {
            if (screenShotTex == null)
            {
                Debug.LogError("Texture2D is null");
                return Observable.Throw<Unit>(new ArgumentNullException(nameof(screenShotTex)));
            }

            // This returns an observable that starts a coroutine to add the image
            return Observable.FromCoroutine<Unit>((observer) => AddImageCoroutine(observer, trackedImageManager, screenShotTex, imageRecognition));
        }

        private IEnumerator AddImageCoroutine(IObserver<Unit> observer, ARTrackedImageManager trackedImageManager, Texture2D texture2D, string imageRecognition)
        {
            if (!(trackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary))
            {
                observer.OnError(new InvalidOperationException("TrackedImageManager's reference library is not mutable."));
                yield break;
            }

            Debug.Log("Adding image to the runtime library");

            // Define image
            string id = Guid.NewGuid().ToString();
            string imageName = $"{imageRecognition}_{id}";

            // Schedule the add image job
            var addImageJob = mutableLibrary.ScheduleAddImageWithValidationJob(texture2D, imageName, 0.1f); // Assume 0.1m physical size, adjust as necessary

            // Wait for the job to complete
            yield return new WaitUntil(() => addImageJob.jobHandle.IsCompleted);

            addImageJob.jobHandle.Complete();

            if (addImageJob.status == AddReferenceImageJobStatus.Success)
            {
                Debug.Log($"Successfully added or updated the image '{imageRecognition}' in the runtime reference image library.");
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
            }
            else
            {
                string errorMessage = $"Failed to add the image '{imageRecognition}' to the runtime reference image library. Status: {addImageJob.status}";
                Debug.LogError(errorMessage);
                observer.OnError(new Exception(errorMessage));
            }
        }
    }
}