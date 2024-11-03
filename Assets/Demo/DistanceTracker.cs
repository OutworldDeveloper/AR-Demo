using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(QRPlacer))]
public class DistanceTracker : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _distanceTextLabel;
    [SerializeField] private TextMeshProUGUI _distanceLabel;
    [SerializeField] private Material _lineMaterial;

    private QRPlacer _placer;
    private Transform _line;

    private void Awake()
    {
        _placer = GetComponent<QRPlacer>();
        _placer.Changed += UpdateDisplay;
        _line = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        _line.GetComponent<MeshRenderer>().material = _lineMaterial;
        _line.GetComponent<Collider>().enabled = false;
    }

    private void OnDestroy()
    {
        _placer.Changed -= UpdateDisplay;
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        bool showDistance = _placer.QRA != null && _placer.QRB != null;

        _distanceTextLabel.gameObject.SetActive(showDistance);
        _line.gameObject.SetActive(showDistance);

        if (!showDistance)
        {
            _distanceLabel.text = "Place two QRs";
            return;
        }

        float distance = Vector3.Distance(_placer.QRA.transform.position, _placer.QRB.transform.position);
        _distanceLabel.text = distance.ToString("0.00");

        Vector3 midPoint = Vector3.Lerp(_placer.QRA.transform.position, _placer.QRB.transform.position, 0.5f);
        _line.position = midPoint;
        _line.transform.forward = (_placer.QRA.transform.position - _placer.QRB.transform.position).normalized;

        _line.transform.localScale = new Vector3(0.01f, 0.01f, Mathf.Max(0f, distance - 0.1f));
    }

}
