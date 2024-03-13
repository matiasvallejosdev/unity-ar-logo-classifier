using System;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Infrastructure
{
    public interface IStorageImageGateway 
    {
        public IObservable<StorageResponse> PostImage(StorageServiceViewModel storageService, Texture2D texture);
    }
}