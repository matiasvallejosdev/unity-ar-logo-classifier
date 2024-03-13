using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure
{
    public interface IRuntimeLibraryGateway
    {
        public IObservable<Unit> AddImageRuntimeLibrary(Texture2D screenShotTex, ARTrackedImageManager trackedImageManager, string imageRecognition);
    }
}