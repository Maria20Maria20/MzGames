using MzGames.Scripts.Simulation;
using MzGames.Scripts.Simulation.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace MzGames.Scripts.Meta.HUD
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private Slider speedSlider;
        [SerializeField] private Text speedLabel;

        private ISimulationClock _clock;

        [Inject]
        public void Construct(ISimulationClock clock)
        {
            _clock = clock;
        }

        private void Start()
        {
            speedSlider.minValue = SimulationClock.MinSpeed;
            speedSlider.maxValue = SimulationClock.MaxSpeed;
            speedSlider.wholeNumbers = false;
            speedSlider.SetValueWithoutNotify(_clock.Speed);
            speedSlider.onValueChanged.AddListener(OnSpeedChanged);

            UpdateLabel(_clock.Speed);
        }

        private void OnSpeedChanged(float value)
        {
            _clock.Speed = value;
            UpdateLabel(_clock.Speed);
        }

        private void UpdateLabel(float value)
        {
            speedLabel.text = Mathf.Approximately(value, 0f) ? "Pause" : $"x{value:0.#}";
        }

        private void OnDestroy()
        {
            if (speedSlider != null)
                speedSlider.onValueChanged.RemoveListener(OnSpeedChanged);
        }
    }
}
