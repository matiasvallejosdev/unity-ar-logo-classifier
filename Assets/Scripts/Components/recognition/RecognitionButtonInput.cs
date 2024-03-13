using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewModel;
using UniRx;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class RecognitionButtonInput : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public RecognitionCmdFactory gameCmdFactory;
    public RecognitionManagerViewModel recognitionManager;
    public RecognitionServiceViewModel recognitionService;
    public StorageServiceViewModel storageService;
    private Button button;
    public AudioSource audioSource;

    void Start(){
        button = GetComponent<Button>();
        recognitionManager.recognitionActive.Subscribe(active => {
            button.interactable = active;
        }).AddTo(this);
        recognitionManager.recognitionActive.Value = true;
    }
    
    public void OnClick()
    {
        audioSource.Play();
        var arCamera = Camera.main;
        var recognizeTurnCmd = gameCmdFactory.RecognitionInput(recognitionManager, recognitionService, storageService, trackedImageManager, arCamera);
        recognizeTurnCmd.Execute();
    }
}
