using System;
using System.Threading.Tasks;
using MzGames.Scripts.Infra.AssetManagement;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace MzGames.Scripts.Infra.SceneManagement
{
    public class SceneLoader
    {
        private readonly IAssetProvider _assetProvider;

        public SceneLoader(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async Task<SceneInstance> Load(SceneName sceneName, Action<SceneName> onLoaded = null)
        {
            var operationHandle = _assetProvider.LoadScene(sceneName);

            var scene = (SceneInstance)await operationHandle.Task;
            scene.ActivateAsync();
            onLoaded?.Invoke(sceneName);
            return scene;
        }
    }
}
