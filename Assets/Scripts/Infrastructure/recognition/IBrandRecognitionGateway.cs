using System;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Infrastructure
{
    public interface IBrandRecognitionGateway 
    {
        public IObservable<RecognitionResponse> GetRecogntion(RecognitionServiceViewModel recognitionService, string imageUrl);
    }
}