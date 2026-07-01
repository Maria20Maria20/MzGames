using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MzGames.Scripts.Infra.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace MzGames.Scripts.Infra.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new(); 
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new(); 
        private readonly Dictionary<SceneName, AsyncOperationHandle> _sceneHandles = new(); 
        private AsyncOperationHandle _operationHandle;
        private Task _initializeTask;

        public AsyncOperationHandle OperationHandle => _operationHandle;

        public bool IsSceneLoaded(SceneName sceneName) => _sceneHandles.ContainsKey(sceneName);

        public IReadOnlyCollection<SceneName> LoadedScenes => _sceneHandles.Keys.ToList();

        public void Initialize() =>
            _initializeTask = Addressables.InitializeAsync().Task;

        public async Task<T> Load<T>(string key) where T : class 
        {
            await EnsureInitializedAsync();

            if (_completedCache.TryGetValue(key, out var completedHandle)) 
                return completedHandle.Result as T; 

            var handle = Addressables.LoadAssetAsync<T>(key); 

            return await RunWithCacheOnComplete(handle: handle, cacheKey: key); 
        }

        private async Task EnsureInitializedAsync()
        {
            if (_initializeTask != null)
                await _initializeTask;
        }

        public AsyncOperationHandle LoadScene(SceneName sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            _operationHandle = Addressables.LoadSceneAsync(
                key: sceneName.ToString(),
                loadMode: mode);

            if (mode == LoadSceneMode.Additive)
            {
                _sceneHandles[sceneName] = _operationHandle;
            }

            return _operationHandle;
        }

        public AsyncOperationHandle UnloadSceneAsync(SceneName sceneName)
        {
            if (!_sceneHandles.TryGetValue(sceneName, out var handle))
            {
                return default;
            }

            _sceneHandles.Remove(sceneName);
            return Addressables.UnloadSceneAsync(handle);
        }
        public void Release(string key) 
        {
            if (!_handles.ContainsKey(key)) 
                return;

            foreach (var handle in _handles[key])
                Addressables.Release(handle);

            _completedCache.Remove(key);
            _handles.Remove(key);
        }

        public void Cleanup() 
        {
            if (_handles.Count > 0)
            {
                foreach (var resourceHandles in _handles.Values)
                    foreach (var handle in resourceHandles)
                        Addressables.Release(handle);

                _completedCache.Clear();
                _handles.Clear();
            }

            foreach (var handle in _sceneHandles.Values)
                Addressables.UnloadSceneAsync(handle);

            _sceneHandles.Clear();
        }


        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey)
            where T : class 
        {
            handle.Completed += completeHandle =>
                            _completedCache[cacheKey] = completeHandle;

            AddHandle(cacheKey, handle);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                UnityEngine.Debug.LogError($"AssetProvider: не удалось загрузить '{cacheKey}'. Status={handle.Status}");
                return null;
            }

            return handle.Result;
        }


        private void AddHandle(string key, AsyncOperationHandle handle) 
        {
            if (!_handles.TryGetValue(key, out var resourceHandles)) 
            {
                resourceHandles = new List<AsyncOperationHandle>(); 
                _handles[key] = resourceHandles; 
            }

            resourceHandles.Add(handle); 
        }
    }
}