using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using ViewModel;
using UniRx;

namespace Components
{
    public class XRImageRecognitionMotion : MonoBehaviour 
    {
        public RecognitionManagerViewModel recognitionManager;
        public AudioSource audioSource;

        void Start(){
            recognitionManager.OnRecognitionStart.Subscribe(isActive => {
                if (isActive)
                {
                    audioSource.Play();
                } else {
                    audioSource.Stop();
                }
            }).AddTo(this);
        }
    }
}