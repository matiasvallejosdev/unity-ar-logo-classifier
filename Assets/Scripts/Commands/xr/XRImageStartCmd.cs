using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using ViewModel;

namespace Commands
{
    public class XRImageStartCmd : ICommand
    {
        public RecognitionManagerViewModel recognitionManager;
        public ARTrackedImageManager trackedImageManager;

        public XRImageStartCmd(RecognitionManagerViewModel recognitionManagerViewModel, ARTrackedImageManager trackedImageManager)
        {
            this.recognitionManager = recognitionManagerViewModel;
            this.trackedImageManager = trackedImageManager;
        }

        public void Execute()
        {
            Debug.Log("Creating Runtime Mutable Image Library");
            var lib = trackedImageManager.CreateRuntimeLibrary(recognitionManager.runtimeImageLibrary);
            trackedImageManager.referenceLibrary = lib;

            trackedImageManager.requestedMaxNumberOfMovingImages = recognitionManager.maxNumberOfImages;
            recognitionManager.recognitionActive.Value = true;

            recognitionManager.recognitionObjectsCount.Value = 0;
        }
    }
}