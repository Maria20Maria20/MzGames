using System.Collections.Generic;
using System.Threading.Tasks;
using MzGames.Scripts.Infra.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace MzGames.Scripts.Infra.AssetManagement
{
    public interface IAssetProvider: IInitializable
    {
        public Task<T> Load<T>(string key) where T : class;
        public AsyncOperationHandle LoadScene(SceneName sceneName, LoadSceneMode mode = LoadSceneMode.Single);
        public AsyncOperationHandle OperationHandle { get; }
        public void Release(string key);
        public void Cleanup();


        bool IsSceneLoaded(SceneName sceneName);
        IReadOnlyCollection<SceneName> LoadedScenes { get; }
        AsyncOperationHandle UnloadSceneAsync(SceneName sceneName);
    }
}