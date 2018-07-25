using UnityEngine;
using UnityEngine.UI;

public class MarketDemoController : MonoBehaviour
{
    [SerializeField] private float _sizeDelta;
    [SerializeField] private Button _btnSizeMinusX;
    [SerializeField] private Button _btnSizeMinusY;
    [SerializeField] private Button _btnSizePlusX;
    [SerializeField] private Button _btnSizePlusY;
    [SerializeField] private UniWebView _webUniWebView;

    private void Awake()
    {
        _btnSizePlusX.onClick.AddListener(() =>
            ChangeContentSize(new Vector2(_sizeDelta, 0)));
        _btnSizeMinusX.onClick.AddListener(() =>
            ChangeContentSize(new Vector2(-_sizeDelta, 0)));
        _btnSizePlusY.onClick.AddListener(() =>
            ChangeContentSize(new Vector2(0, _sizeDelta)));
        _btnSizeMinusY.onClick.AddListener(() =>
            ChangeContentSize(new Vector2(0, -_sizeDelta)));
    }

    private void ChangeContentSize(Vector2 value)
    {
        var content = _webUniWebView.ReferenceRectTransform;
        var newSize = content.sizeDelta + value;
        content.sizeDelta = newSize;
        _webUniWebView.UpdateFrame();
    }
}