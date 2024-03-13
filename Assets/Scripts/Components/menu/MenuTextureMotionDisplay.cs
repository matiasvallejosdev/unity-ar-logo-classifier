using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;
using UniRx;

namespace Components
{
    public class MenuTextureMotionDisplay : MonoBehaviour
    {
        public RecognitionManagerViewModel recognitionManager;
        public Animator animator;

        void Start()
        {
            recognitionManager.OnRecognitionStart
                .Subscribe(isActive => {
                    animator.SetBool("Loading", isActive);
                })
                .AddTo(this);
        }
    }
}
