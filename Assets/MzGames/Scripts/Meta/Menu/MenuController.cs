using System;
using MzGames.Scripts.Data;
using MzGames.Scripts.Simulation;
using UnityEngine;
using UnityEngine.UI;

namespace MzGames.Scripts.Meta.Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Slider gridSlider;   // N
        [SerializeField] private Slider countSlider;  // M
        [SerializeField] private Slider speedSlider;  // V
        [SerializeField] private Button newButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Text maxCount;
        [SerializeField] private Text currentGrid;
        [SerializeField] private Text currentCount;
        [SerializeField] private Text currentSpeed;

        public event Action<SimulationConfig> NewSimulationRequested;
        public event Action ContinueRequested;

        public void Initialize(SimulationConfig defaults, bool continueAvailable)
        {
            ConfigureSliders(defaults);

            if (continueButton != null)
                continueButton.interactable = continueAvailable;

            if (gridSlider != null)
                gridSlider.onValueChanged.AddListener(value =>
                {
                    ClampCountToGrid();
                    currentGrid.text = "Grid " + (int)value;
                });
            if (countSlider != null)
                countSlider.onValueChanged.AddListener(value => currentCount.text = "Count" + (int)value);
            if (speedSlider != null)
                speedSlider.onValueChanged.AddListener(value => currentSpeed.text = "Speed " + value);
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
                currentGrid.text = "Grid " + defaults.GridSize;
            }

            if (speedSlider != null)
            {
                speedSlider.wholeNumbers = false;
                speedSlider.minValue = SimulationConfig.MinSpeed;
                speedSlider.maxValue = SimulationConfig.MaxSpeed;
                speedSlider.value = defaults.Speed;
                currentSpeed.text = "Speed " + defaults.Speed;
            }

            if (countSlider != null)
            {
                countSlider.wholeNumbers = true;
                countSlider.minValue = 0;
                countSlider.maxValue = SimulationConfig.MaxCount(defaults.GridSize);
                countSlider.value = defaults.Count;
                currentCount.text = "Count" + defaults.Count;
            }

            ClampCountToGrid();
        }

        private void ClampCountToGrid()
        {
            if (countSlider == null || gridSlider == null)
                return;

            countSlider.maxValue = SimulationConfig.MaxCount((int)gridSlider.value);
            maxCount.text = SimulationConfig.MaxCount((int)gridSlider.value).ToString();
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
