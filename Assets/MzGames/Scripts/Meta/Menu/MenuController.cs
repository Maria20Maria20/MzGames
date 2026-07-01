using System;
using MzGames.Scripts.Simulation;
using UnityEngine;
using UnityEngine.UI;

namespace MzGames.Scripts.Meta.Menu
{
    /// <summary>
    /// Start / new-simulation menu view: N / M / V sliders + New simulation / Continue
    /// buttons. Raises events that MenuState listens to. Spawned through IUIFactory.
    /// The slider/button references are wired on the prefab.
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Slider gridSlider;   // N
        [SerializeField] private Slider countSlider;  // M
        [SerializeField] private Slider speedSlider;  // V
        [SerializeField] private Button newButton;
        [SerializeField] private Button continueButton;

        public event Action<SimulationConfig> NewSimulationRequested;
        public event Action ContinueRequested;

        public void Initialize(SimulationConfig defaults, bool continueAvailable)
        {
            ConfigureSliders(defaults);

            if (continueButton != null)
                continueButton.interactable = continueAvailable;

            if (gridSlider != null)
                gridSlider.onValueChanged.AddListener(_ => ClampCountToGrid());
            if (newButton != null)
                newButton.onClick.AddListener(RaiseNew);
            if (continueButton != null)
                continueButton.onClick.AddListener(RaiseContinue);
        }

        private void ConfigureSliders(SimulationConfig defaults)
        {
            if (gridSlider != null)
            {
                gridSlider.wholeNumbers = true;
                gridSlider.minValue = SimulationConfig.MinGrid;
                gridSlider.maxValue = SimulationConfig.MaxGrid;
                gridSlider.value = defaults.GridSize;
            }

            if (speedSlider != null)
            {
                speedSlider.wholeNumbers = false;
                speedSlider.minValue = SimulationConfig.MinSpeed;
                speedSlider.maxValue = SimulationConfig.MaxSpeed;
                speedSlider.value = defaults.Speed;
            }

            if (countSlider != null)
            {
                countSlider.wholeNumbers = true;
                countSlider.minValue = 0;
                countSlider.value = defaults.Count;
            }

            ClampCountToGrid();
        }

        // M ≤ N²/2 — keep the count slider's max in sync with the chosen grid size.
        private void ClampCountToGrid()
        {
            if (countSlider == null || gridSlider == null)
                return;

            countSlider.maxValue = SimulationConfig.MaxCount((int)gridSlider.value);
        }

        private void RaiseNew() => NewSimulationRequested?.Invoke(ReadConfig());

        private void RaiseContinue() => ContinueRequested?.Invoke();

        private SimulationConfig ReadConfig() => new SimulationConfig
        {
            GridSize = gridSlider != null ? (int)gridSlider.value : SimulationConfig.MinGrid,
            Count = countSlider != null ? (int)countSlider.value : 0,
            Speed = speedSlider != null ? speedSlider.value : SimulationConfig.MinSpeed
        }.Validated();

        private void OnDestroy()
        {
            if (newButton != null) newButton.onClick.RemoveListener(RaiseNew);
            if (continueButton != null) continueButton.onClick.RemoveListener(RaiseContinue);
        }
    }
}
