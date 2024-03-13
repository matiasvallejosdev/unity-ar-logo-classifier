using UniRx;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New Recognition Manager", menuName = "Manager/Recognition Manager")]
    public class RecognitionManagerViewModel : ScriptableObject
    {
        [Header("Recognition Experience")]
        public ARObjectViewModel[] recogntionObjects;
        public int textureWidth;
        public int textureHeight;
        public bool saveLocalScreenshoot = false;
        
        [Header("AR Settings")]
        public BoolReactiveProperty arRecognitionEnable = new BoolReactiveProperty(true);
        public int maxNumberOfImages;
        public XRReferenceImageLibrary runtimeImageLibrary;

        [Header("Runtime")]
        public ISubject<bool> OnRecognitionStart = new Subject<bool>();

        public BoolReactiveProperty recognitionActive = new BoolReactiveProperty(false);
        public IntReactiveProperty recognitionObjectsCount = new IntReactiveProperty(0);
        public ReactiveProperty<Texture> lastScreenshoot = new ReactiveProperty<Texture>();
        public ReactiveProperty<RecognitionResponse> recognitionResponse = new ReactiveProperty<RecognitionResponse>();
    }
}
