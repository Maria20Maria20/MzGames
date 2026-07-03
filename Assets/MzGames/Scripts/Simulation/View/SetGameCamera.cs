using UnityEngine;

namespace MzGames.Scripts.Simulation.View
{
    [RequireComponent(typeof(Camera))]
    public class SetGameCamera : MonoBehaviour
    {
        [SerializeField] private float margin = 1.5f;
        [SerializeField] private float height = 10f;

        private Camera _camera;

        private void Awake() => _camera = GetComponent<Camera>();

        public void Frame(int gridSize)
        {
            if (_camera == null)
                _camera = GetComponent<Camera>();

            float half = gridSize * 0.5f;
            float aspect = _camera.aspect > 0.0001f ? _camera.aspect : 16f / 9f;

            _camera.orthographic = true;
            _camera.orthographicSize = half / Mathf.Min(1f, aspect) + margin;
            _camera.transform.position = new Vector3(half, gridSize + height, half);
            _camera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            _camera.nearClipPlane = 0.3f;
            _camera.farClipPlane = gridSize * 2f + 50f;
        }
    }
}
