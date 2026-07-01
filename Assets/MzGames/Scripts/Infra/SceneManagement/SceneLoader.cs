using System;
using System.Threading.Tasks;
using MzGames.Scripts.Infra.AssetManagement;
using MzGames.Scripts.Infra.Factorises.Interfaces;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace MzGames.Scripts.Infra.SceneManagement
{
    public class SceneLoader
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IUIFactory _uiFactory;

        public SceneLoader(IAssetProvider assetProvider, IUIFactory uiFactory)
        {
            _assetProvider = assetProvider;
            _uiFactory = uiFactory;
        }

        public async Task<SceneInstance> Load(SceneName sceneName, Action<SceneName> onLoaded = null)
        {
            var operationHandle = _assetProvider.LoadScene(sceneName);

            if (_uiFactory.LoadingScreen != null)
                _uiFactory.LoadingScreen.Progress(operationHandle);

            var scene = (SceneInstance)await operationHandle.Task;
            scene.ActivateAsync();
            onLoaded?.Invoke(sceneName);
            return scene;
        }
    }
}
