using System.Threading.Tasks;
using MzGames.Scripts.Meta;
using MzGames.Scripts.Meta.HUD;
using MzGames.Scripts.Meta.Menu;
using UnityEngine;

namespace MzGames.Scripts.Infra.Factorises.Interfaces
{
    public interface IUIFactory
    {
        LoadingScreen LoadingScreen { get; }

        Task WarmUp();
        Task<MenuController> GetOrCreateMenu();
        Task<LoadingScreen> GetOrCreateLoadingScreen();
        Task<Canvas> GetOrCreateUIRoot();
        Task<HUDController> GetOrCreateHUD();
        void DestroyMenu();
        void CleanupAll();
    }
}
