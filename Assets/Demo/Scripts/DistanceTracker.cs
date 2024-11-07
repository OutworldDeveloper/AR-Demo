using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(QRDetector))]
public class DistanceTracker : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _distanceTextLabel;
    [SerializeField] private TextMeshProUGUI _distanceLabel;
    [SerializeField] private Material _lineMaterial;

    private QRDetector _detector;
    private Transform _line;

    private void Awake()
    {
        _detector = GetComponent<QRDetector>();
        _detector.Changed += UpdateDisplay;
        _line = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        _line.GetComponent<MeshRenderer>().material = _lineMaterial;
        _line.GetComponent<Collider>().enabled = false;
    }

    private void OnDestroy()
    {
        _detector.Changed -= UpdateDisplay;
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        bool showDistance = _detector.QRA != null && _detector.QRB != null;

        _distanceTextLabel.gameObject.SetActive(showDistance);
        _line.gameObject.SetActive(showDistance);

        if (!showDistance)
        {
            _distanceLabel.text = "NO QRs DETECTED";
            return;
        }

        float distance = Vector3.Distance(_detector.QRA.transform.position, _detector.QRB.transform.position);
        _distanceLabel.text = Mathf.FloorToInt(distance * 100f).ToString();

        Vector3 midPoint = Vector3.Lerp(_detector.QRA.transform.position, _detector.QRB.transform.position, 0.5f);
        _line.position = midPoint;
        _line.transform.forward = (_detector.QRA.transform.position - _detector.QRB.transform.position).normalized;

        _line.transform.localScale = new Vector3(0.007f, 0.007f, Mathf.Max(0f, distance - 0.1f));
    }

}
