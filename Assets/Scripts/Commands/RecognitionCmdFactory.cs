using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using ViewModel;

[CreateAssetMenu(fileName = "Recognition Command Factory", menuName = "Factory/Recognition Factory")]
public class RecognitionCmdFactory : ScriptableObject
{
    public RecognizeTurnCmd RecognitionInput(RecognitionManagerViewModel recognitionManager, RecognitionServiceViewModel recognitionService, StorageServiceViewModel storageService, ARTrackedImageManager trackedImageManager, Camera arCamera)
    {
        var brandRecognitionGateway = new BrandRecognitionGateway();
        var storageImageGateway = new StorageImageGateway();
        var runtimeLibraryGateway = new RuntimeLibraryGateway();

        return new RecognizeTurnCmd(recognitionManager, recognitionService, storageService, trackedImageManager, arCamera, brandRecognitionGateway, storageImageGateway, runtimeLibraryGateway);
    }
}