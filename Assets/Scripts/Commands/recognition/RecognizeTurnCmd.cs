using UnityEngine;
using ViewModel;
using UnityEngine.XR.ARFoundation;
using Infrastructure;
using UniRx;
using System.Collections.Generic;
using Components.Utils;
using System.Threading.Tasks;

public class RecognizeTurnCmd : ICommand
{
    private readonly RecognitionManagerViewModel recognitionManager;

    private readonly RecognitionServiceViewModel recognitionService;
    public readonly StorageServiceViewModel storageService;

    private readonly ARTrackedImageManager trackedImageManager;
    private readonly Camera arCamera;

    private readonly IBrandRecognitionGateway brandRecognitionGateway;
    private readonly IStorageImageGateway storageImageGateway;
    private readonly IRuntimeLibraryGateway runtimeLibraryGateway;

    public RecognizeTurnCmd(RecognitionManagerViewModel recognitionManager,
    RecognitionServiceViewModel recognitionService, StorageServiceViewModel storageService, ARTrackedImageManager trackedImageManager,
    Camera arCamera, IBrandRecognitionGateway brandRecognitionGateway,
    IStorageImageGateway storageImageGateway, IRuntimeLibraryGateway runtimeLibraryGateway)
    {
        this.recognitionManager = recognitionManager;
        this.recognitionService = recognitionService;
        this.storageService = storageService;
        this.trackedImageManager = trackedImageManager;
        this.arCamera = arCamera;
        this.brandRecognitionGateway = brandRecognitionGateway;
        this.storageImageGateway = storageImageGateway;
        this.runtimeLibraryGateway = runtimeLibraryGateway;
    }

    public void Execute()
    {
        recognitionManager.recognitionActive.Value = false;
        recognitionManager.OnRecognitionStart.OnNext(true);

        List<Texture2D> textures = GetTexture();
        Texture2D textureCrop = textures[0];
        Texture2D screenShotTex = textures[1];


        recognitionManager.recognitionResponse.Value = new RecognitionResponse()
        {
            recognition = "Uploading Image..",
            statusCode = "Wait a moment"
        };

        storageImageGateway.PostImage(storageService, textureCrop)
            .Do(response =>
            {
                recognitionManager.recognitionResponse.Value = new RecognitionResponse()
                {
                    recognition = "Processing Image..",
                    statusCode = "Wait a moment"
                };
                brandRecognitionGateway.GetRecogntion(recognitionService, response.imageUrl)
                            .Do(response =>
                            {
                                recognitionManager.recognitionResponse.Value = response;
                                string imageName = response.recognition;
                                Debug.Log($"Image Name: {imageName}");
                                runtimeLibraryGateway.AddImageRuntimeLibrary(textureCrop, trackedImageManager, imageName)
                                    .Subscribe();
                                recognitionManager.recognitionActive.Value = true;
                                recognitionManager.recognitionObjectsCount.Value++;
                                recognitionManager.OnRecognitionStart.OnNext(false);
                            })
                            .Subscribe();
            })
            .Subscribe();
    }

    private List<Texture2D> GetTexture()
    {
        // trackData.ARTrackedEnable.Value = false;
        recognitionManager.recognitionResponse.Value = new RecognitionResponse()
        {
            recognition = "Capturing Image..",
            statusCode = "Wait a moment"
        };

        Texture2D screenShotTex = Screenshot.GetScreenShot(arCamera);
        Texture2D textureCrop = Screenshot.ResampleAndCrop(screenShotTex, recognitionManager.textureWidth, recognitionManager.textureHeight);

        if (recognitionManager.saveLocalScreenshoot)
        {
            Screenshot.SaveScreenshot(screenShotTex);
            Screenshot.SaveScreenshot(textureCrop);
        }

        recognitionManager.lastScreenshoot.Value = textureCrop;

        return new List<Texture2D>{
            textureCrop,
            screenShotTex
        };
    }
}