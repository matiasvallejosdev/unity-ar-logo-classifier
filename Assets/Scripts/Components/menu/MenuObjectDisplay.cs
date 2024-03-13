using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using ViewModel;

namespace Components
{
    public class MenuObjectDisplay : MonoBehaviour
    {
        public GameObject recognizeButton;
        public GameObject optionsButton;
        public GameObject messagePanel;
        public GameObject screenCapturePanel;
        public GameObject optionsPanel;
        public GameObject greetingPanel;

        public void OnStart()
        {
            greetingPanel.SetActive(false);
            recognizeButton.SetActive(true);
            messagePanel.SetActive(true);
            screenCapturePanel.SetActive(true);
        }

        public void OnOptions()
        {
            optionsPanel.SetActive(optionsPanel.activeSelf ? false : true);
        }
    }
}
