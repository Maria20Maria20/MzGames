using System.Threading.Tasks;
using MzGames.Scripts.Infra.AssetManagement;
using MzGames.Scripts.Infra.Factorises.Interfaces;
using MzGames.Scripts.Meta;
using MzGames.Scripts.Meta.HUD;
using MzGames.Scripts.Meta.Menu;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MzGames.Scripts.Infra.Factorises
{
    public class UIFactory : IUIFactory
    {
        private const string MenuAddress = "Menu";
        private const string LoadingScreenAddress = "LoadingScreen";
        private const string UIRootAddress = "UIRoot";
        private const string HUDAddress = "HUD";

        private readonly IAssetProvider _assetProvider;
        private readonly IObjectResolver _objectResolver;

        private GameObject _menuPrefab;
        private GameObject _loadingScreenPrefab;
        private GameObject _uiRootPrefab;
        private GameObject _hudPrefab;

        private MenuController _menu;
        private LoadingScreen _loadingScreen;
        private Canvas _uiRoot;
        private HUDController _hud;

        public LoadingScreen LoadingScreen => _loadingScreen;

        public UIFactory(IAssetProvider assetProvider, IObjectResolver objectResolver)
        {
            _assetProvider = assetProvider;
            _objectResolver = objectResolver;
        }

        public async Task WarmUp()
        {
            _loadingScreenPrefab = await _assetProvider.Load<GameObject>(LoadingScreenAddress);
            _uiRootPrefab = await _assetProvider.Load<GameObject>(UIRootAddress);
            _hudPrefab = await _assetProvider.Load<GameObject>(HUDAddress);
        }

        public async Task<MenuController> GetOrCreateMenu()
        {
            if (_menu != null)
                return _menu;

            var uiRoot = await GetOrCreateUIRoot();
            _menuPrefab ??= await _assetProvider.Load<GameObject>(MenuAddress);

            var instance = _objectResolver.Instantiate(_menuPrefab, uiRoot.transform);
            _menu = instance.GetComponent<MenuController>();
            return _menu;
        }

        public async Task<LoadingScreen> GetOrCreateLoadingScreen()
        {
            if (_loadingScreen != null)
                return _loadingScreen;

            _loadingScreenPrefab ??= await _assetProvider.Load<GameObject>(LoadingScreenAddress);

            var instance = _objectResolver.Instantiate(_loadingScreenPrefab);
            Object.DontDestroyOnLoad(instance); 
            _loadingScreen = instance.GetComponent<LoadingScreen>();
            return _loadingScreen;
        }

        public async Task<Canvas> GetOrCreateUIRoot()
        {
            if (_uiRoot != null)
                return _uiRoot;

            _uiRootPrefab ??= await _assetProvider.Load<GameObject>(UIRootAddress);

            var instance = _objectResolver.Instantiate(_uiRootPrefab);
            Object.DontDestroyOnLoad(instance);
            _uiRoot = instance.GetComponent<Canvas>();
            return _uiRoot;
        }

        public async Task<HUDController> GetOrCreateHUD()
        {
            if (_hud != null)
                return _hud;

            var uiRoot = await GetOrCreateUIRoot();
            _hudPrefab ??= await _assetProvider.Load<GameObject>(HUDAddress);

            var instance = _objectResolver.Instantiate(_hudPrefab, uiRoot.transform);
            _hud = instance.GetComponent<HUDController>();
            return _hud;
        }

        public void DestroyMenu()
        {
            if (_menu != null)
            {
                Object.Destroy(_menu.gameObject);
                _menu = null;
            }

            _assetProvider.Release(MenuAddress);
            _menuPrefab = null;
        }

        public void CleanupAll()
        {
            DestroyMenu();

            if (_hud != null)
            {
                Object.Destroy(_hud.gameObject);
                _hud = null;
            }

            if (_uiRoot != null)
            {
                Object.Destroy(_uiRoot.gameObject);
                _uiRoot = null;
            }

            if (_loadingScreen != null)
            {
                Object.Destroy(_loadingScreen.gameObject);
                _loadingScreen = null;
            }

            _assetProvider.Release(LoadingScreenAddress);
            _assetProvider.Release(UIRootAddress);
            _assetProvider.Release(HUDAddress);

            _loadingScreenPrefab = null;
            _uiRootPrefab = null;
            _hudPrefab = null;
        }
    }
}
