using DG.Tweening;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    [Header("WAYPOINTS")]
    [SerializeField]
    Transform[] wps;

    [Space]

    [Header("PARAMETERS")]
    [SerializeField]
    float duration = 5f;
    [SerializeField]
    PathType pathType = PathType.Linear;
    [SerializeField]
    PathMode pathMode = PathMode.Full3D;
    [SerializeField]
    Color gizmoColor = Color.red;
    [SerializeField]
    Ease ease = Ease.Linear;
    [SerializeField]
    float lookAt = 0.01f;
    [SerializeField]
    sbyte loopCount = -1;

    [Space]
    [Header("DEBUG")]
    [SerializeField]
    Vector3[] waypoints;

    Rigidbody rb;

    bool startFlag; //for events

    private void Start()
    {
        waypoints = new Vector3[wps.Length];
        for (int i = 0; i < wps.Length; i++)
        {
            waypoints[i] = wps[i].position;
        }
        rb = GetComponent<Rigidbody>();
        Pathing();
    }

    void Pathing()
    {
        rb.DOPath(waypoints, duration, pathType, pathMode, 10, gizmoColor).SetEase(ease).SetLookAt(lookAt).SetLoops(loopCount);
        return;
    }
}
