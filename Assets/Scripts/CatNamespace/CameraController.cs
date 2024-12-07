using ScriptableObjects;
using Unity.Cinemachine;
using UnityEngine;

namespace CatNamespace
{
    public class CameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameConstants gameConstants;
        [SerializeField] private CinemachineInputAxisController cinemachineInputAxisController;

        private void Start()
        {
            cinemachineInputAxisController.Controllers[0].Input.Gain = gameConstants.cameraSensitivity * (gameConstants.isXInverted ? -1 : 1);
            cinemachineInputAxisController.Controllers[1].Input.Gain = gameConstants.cameraSensitivity * (gameConstants.isYInverted ? -1 : 1);
        }
    }
}