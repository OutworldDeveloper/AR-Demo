using TMPro;
using UnityEngine;

public sealed class QRDemoDebug : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _label;

    public void Push(string action)
    {
        _label.text = action;
    }

}
