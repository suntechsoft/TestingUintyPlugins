// WWebViewDemo2D.cs : WWebViewDemo2D implementation file
//
// Description      : WWebViewDemo3D
// Author           : icodes (icodes.studio@gmail.com)
// Maintainer       : icodes
// Created          : 2018/04/07
// Last Update      : 2018/04/08
// Repository       : https://github.com/icodes-studio/WWebView
// Official         : http://www.icodes.studio/
//
// (C) ICODES STUDIO. All rights reserved. 
//

using UnityEngine;
using ICODES.STUDIO.WWebView;
using UnityEngine.UI;

public class WWebViewDemo2D : WWebViewDemo
{
    private TestMode testMode = TestMode.Nothing;

    private enum TestMode
    {
        Nothing = 0,
        NativeWindow,
        TransparentWindow,
        CustomInput,
    }


    public Button _btnClose = null;
    public Button _btnNativeWindow = null;
    public Button _btnTransparentWindow = null;
    public Button _btnCustomInput = null;

    protected override void Awake()
    {
        base.Awake();
        _btnNativeWindow.onClick.AddListener(OnTransparentClick);
        _btnTransparentWindow.onClick.AddListener(OnNativeWindowClick);
        _btnCustomInput.onClick.AddListener(OnCustomInput);
        _btnClose.onClick.AddListener(OnCloseClick);
        UpdateButtonEnabling();

    }

    private void OnCustomInput()
    {
        if (!webView.IsShowed)
        {
            webView.Show();
            UpdateButtonEnabling();
        }
        CustomInput();
    }

    private void OnNativeWindowClick()
    {
        if (!webView.IsShowed)
        {
            webView.Show();
            UpdateButtonEnabling();
        }
        NativeWindow();
    }

    private void OnTransparentClick()
    {
        if (!webView.IsShowed)
        {
            webView.Show();
            UpdateButtonEnabling();
        }
        TransparentWindow();
    }

    private void OnCloseClick()
    {
        webView.Hide();
        DestroyTexture();
        UpdateButtonEnabling();
    }

    private void UpdateButtonEnabling()
    {
        var isShowed = webView.IsShowed;
        _btnTransparentWindow.gameObject.SetActive(!isShowed);
        _btnNativeWindow.gameObject.SetActive(!isShowed);
        _btnCustomInput.gameObject.SetActive(!isShowed);
        _btnClose.gameObject.SetActive(isShowed);
    }
    public void NativeWindow()
    {
        webView.Navigate(url);
        webView.Alpha = 1f;
        webView.Show();
        testMode = TestMode.NativeWindow;
    }

    public void TransparentWindow()
    {
#if UNITY_EDITOR_WIN || ((UNITY_STANDALONE_WIN || UNITY_WSA) && !UNITY_EDITOR)
        webView.Navigate(url);
        webView.Alpha = 0.005f;
        webView.Show();
        webView.SetTexture(CreateTexture());
#else
        Debug.LogWarning("Texturing feature is only supported on Win32/WSA/Windows Editor.");
#endif
        testMode = TestMode.TransparentWindow;
    }

    public void CustomInput()
    {
#if UNITY_EDITOR_WIN || ((UNITY_STANDALONE_WIN || UNITY_WSA) && !UNITY_EDITOR)
        webView.Hide();
        webView.Navigate(url);
        webView.SetTexture(CreateTexture());
#else
        Debug.LogWarning("Texturing feature is only supported on Win32/WSA/Windows Editor.");
#endif
        testMode = TestMode.CustomInput;
    }

    private Texture2D CreateTexture()
    {
        Texture2D texture = new Texture2D(webView.GetActualWidth(), webView.GetActualHeight(), TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Trilinear; texture.Apply();
        background.texture = texture;
        return texture;
    }

    private void DestroyTexture()
    {
        background.texture = null;
    }


    private void Update()
    {
        if (testMode == TestMode.CustomInput)
        {
#if UNITY_EDITOR_WIN || ((UNITY_STANDALONE_WIN || UNITY_WSA) && !UNITY_EDITOR)
            int state = 0;
            int key = 0;

            if (InputTools.GetMouseState(ref state, ref key))
            {
                Vector2 position = new Vector2();
                Rect target = new Rect(0, 0, webView.GetActualWidth(), webView.GetActualHeight());
                position.x = Input.mousePosition.x * (target.width / Screen.width);
                position.y = (Screen.height - margine - Input.mousePosition.y) * (target.height / (Screen.height - margine));

                if (target.Contains(position))
                    webView.InputEvent(state, key, (int)position.x, (int)position.y);
            }
#else
            Debug.LogWarning("Texturing feature is only supported on Win32/WSA/Windows Editor.");
#endif
        }
    }
}
