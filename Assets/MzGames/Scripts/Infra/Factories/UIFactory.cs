using System.Threading.Tasks;
using MzGames.Scripts.Infra.AssetManagement;
using MzGames.Scripts.Infra.Factories.Interfaces;
using MzGames.Scripts.Meta.HUD;
using MzGames.Scripts.Meta.Menu;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MzGames.Scripts.Infra.Factories
{
    public class UIFactory : IUIFactory
    {
        private const string MenuAddress = "Menu";
        private const string UIRootAddress = "UIRoot";
        private const string HUDAddress = "HUD";

        private readonly IAssetProvider _assetProvider;
        private readonly IObjectResolver _objectResolver;

        private GameObject _menuPrefab;
        private GameObject _uiRootPrefab;
        private GameObject _hudPrefab;

        private MenuController _menu;
        private Canvas _uiRoot;
        private HUDController _hud;

        public UIFactory(IAssetProvider assetProvider, IObjectResolver objectResolver)
        {
            _assetProvider = assetProvider;
            _objectResolver = objectResolver;
        }

        public async Task WarmUp()
        {
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

            _assetProvider.Release(UIRootAddress);
            _assetProvider.Release(HUDAddress);

            _uiRootPrefab = null;
            _hudPrefab = null;
        }
    }
}
