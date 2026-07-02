using System.Threading.Tasks;
using MzGames.Scripts.Meta.HUD;
using MzGames.Scripts.Meta.Menu;
using UnityEngine;

namespace MzGames.Scripts.Infra.Factories.Interfaces
{
    public interface IUIFactory
    {
        Task<bool> WarmUp();
        Task<MenuController> GetOrCreateMenu();
        Task<Canvas> GetOrCreateUIRoot();
        Task<HUDController> GetOrCreateHUD();
        void DestroyMenu();
        void CleanupAll();
    }
}
