// Copyright (c) 2022 Vuplex Inc. All rights reserved.
//
// Licensed under the Vuplex Commercial Software Library License, you may
// not use this file except in compliance with the License. You may obtain
// a copy of the License at
//
//     https://vuplex.com/commercial-library-license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#if UNITY_WEBGL && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using Vuplex.WebView.Internal;

namespace Vuplex.WebView {

    /// <summary>
    /// The IWebView implementation used by 2D WebView for WebGL.
    /// </summary>
    public class WebGLWebView : BaseWebView,
                                IWebView,
                                IWithNative2DMode {

        /// <summary>
        /// Gets the unique `id` attribute of the webview's &lt;iframe&gt; element.
        /// </summary>
        /// <example>
        /// <code>
        /// await canvasWebViewPrefab.WaitUntilInitialized();
        /// #if UNITY_WEBGL &amp;&amp; !UNITY_EDITOR
        ///     var webGLWebView = canvasWebViewPrefab.WebView as WebGLWebView;
        ///     Debug.Log("IFrame ID: " + webGLWebView.IFrameElementID);
        /// #endif
        /// </code>
        /// </example>
        public string IFrameElementID { get; private set; }

        /// <see cref="IWithNative2DMode"/>
        public bool Native2DModeEnabled { get; private set; }

        public WebPluginType PluginType { get; } = WebPluginType.WebGL;

        /// <see cref="IWithNative2DMode"/>
        public Rect Rect { get { return _rect; }}

        /// <see cref="IWithNative2DMode"/>
        public bool Visible { get; private set; }

        /// <seealso cref="IWithNative2DMode"/>
        public void BringToFront() {

            _assertValidState();
            _assertNative2DModeEnabled();
            WebView_bringToFront(_nativeWebViewPtr);
        }

        /// <summary>
        /// Indicates whether 2D WebView can access the content in the
        /// webview's iframe. If the iframe's content can't be accessed,
        /// then most of the IWebView APIs become disabled. For more
        /// information, please see [this article](https://support.vuplex.com/articles/webgl-limitations#cross-origin-limitation).
        /// </summary>
        /// <example>
        /// <code>
        /// await canvasWebViewPrefab.WaitUntilInitialized();
        /// #if UNITY_WEBGL &amp;&amp; !UNITY_EDITOR
        ///     var webGLWebView = canvasWebViewPrefab.WebView as WebGLWebView;
        ///     if (webGLWebView.CanAccessIFrameContent()) {
        ///         Debug.Log("The iframe content can be accessed 👍");
        ///     }
        /// #endif
        /// </code>
        /// </example>
        public bool CanAccessIFrameContent() {

            _assertValidState();
            return WebView_canAccessIFrameContent(_nativeWebViewPtr);
        }

        public override Task<bool> CanGoBack() => _canGoBackOrForward("CanGoBack");

        public override Task<bool> CanGoForward() => _canGoBackOrForward("CanGoForward");

        public override Task<byte[]> CaptureScreenshot() {

            WebGLWarnings.LogCaptureScreenshotError();
            return Task.FromResult(new byte[0]);
        }

        public override void Click(int xInPixels, int yInPixels, bool preventStealingFocus = false) {

            if (_verifyIFrameCanBeAccessed("Click")) {
                base.Click(xInPixels, yInPixels, preventStealingFocus);
            }
        }

        public override void Copy() => WebGLWarnings.LogCopyError();

        public override Material CreateMaterial() => null;

        public override void Cut() => WebGLWarnings.LogCutError();

        public override void ExecuteJavaScript(string javaScript, Action<string> callback) {

            _assertValidState();
            if (_verifyIFrameCanBeAccessed("ExecuteJavaScript")) {
                var result = WebView_executeJavaScriptSync(_nativeWebViewPtr, javaScript);
                callback?.Invoke(result);
            }
        }

        /// <summary>
        /// Executes the given JavaScript locally in the Unity app's window and returns the result.
        /// </summary>
        /// <example>
        /// <code>
        /// #if UNITY_WEBGL &amp;&amp; !UNITY_EDITOR
        ///     // Gets the Unity app's web page title.
        ///     var title = WebGLWebView.ExecuteJavaScriptLocally("document.title");
        ///     Debug.Log("Title: " + title);
        /// #endif
        /// </code>
        /// </example>
        public static string ExecuteJavaScriptLocally(string javaScript) => WebView_executeJavaScriptLocally(javaScript);

        public static WebGLWebView Instantiate() => new GameObject().AddComponent<WebGLWebView>();

        public override Task<byte[]> GetRawTextureData() {

            WebGLWarnings.LogGetRawTextureDataError();
            return Task.FromResult(new byte[0]);
        }

        public override void GoBack() {

            if (_verifyIFrameCanBeAccessed("GoBack")) {
                base.GoBack();
            }
        }

        public override void GoForward() {

            if (_verifyIFrameCanBeAccessed("GoForward")) {
                base.GoForward();
            }
        }

        public Task Init(int width, int height) {

            var message = "2D WebView for WebGL only supports 2D, so Native 2D Mode must be enabled." + WebGLWarnings.GetArticleLinkText(false);
            WebViewLogger.LogError(message);
            throw new NotImplementedException(message);
        }

        /// <see cref="IWithNative2DMode"/>
        public async Task InitInNative2DMode(Rect rect) {

            Native2DModeEnabled = true;
            _rect = rect;
            Visible = true;
            await _initBase((int)rect.width, (int)rect.height, createTexture: false);
            // Set IFrameElementID *after* base.Init() because it sets gameObject.name.
            IFrameElementID = gameObject.name;
            // Prior to Unity 2019.3, Unity's UI used a pixel density of
            // 1 instead of using window.devicePixelRatio.
            #if UNITY_2019_3_OR_NEWER
                var ignoreDevicePixelRatio = false;
            #else
                var ignoreDevicePixelRatio = true;
            #endif
            _nativeWebViewPtr = WebView_newInNative2DMode(
                gameObject.name,
                (int)rect.x,
                (int)rect.y,
                (int)rect.width,
                (int)rect.height,
                ignoreDevicePixelRatio
            );
        }

        public override void LoadHtml(string html) {

            WebGLWarnings.LogLoadHtmlWarning();
            base.LoadHtml(html);
        }

        public override void LoadUrl(string url, Dictionary<string, string> additionalHttpHeaders) {

            WebViewLogger.LogWarning("LoadUrl(url, headers) was called, but 2D WebView for WebGL is unable to send additional headers when loading a URL due to browser limitations. So, the URL will be loaded without additional headers using LoadUrl(url) instead.");
            LoadUrl(url);
        }

        public override void Paste() => WebGLWarnings.LogPasteError();

        public override void PostMessage(string message) {

            _assertValidState();
            WebView_postMessage(_nativeWebViewPtr, message);
        }

        public override void Scroll(int scrollDeltaXInPixels, int scrollDeltaYInPixels) {

            if (_verifyIFrameCanBeAccessed("Scroll")) {
                base.Scroll(scrollDeltaXInPixels, scrollDeltaYInPixels);
            }
        }

        public override void Scroll(Vector2 normalizedScrollDelta, Vector2 normalizedPoint) {

            if (_verifyIFrameCanBeAccessed("Scroll")) {
                base.Scroll(normalizedScrollDelta, normalizedPoint);
            }
        }

        public override void SendKey(string key) {

            if (_verifyIFrameCanBeAccessed("SendKey")) {
                base.SendKey(key);
            }
        }

        /// <summary>
        /// Overrides the value of `devicePixelRatio` that 2D WebView uses to determine the correct
        /// size and coordinates of webviews.
        /// </summary>
        /// <summary>
        /// Starting in Unity 2019.3, Unity scales the UI by default based on the browser's window.devicePixelRatio
        /// value. However, it's possible for an application to override the `devicePixelRatio` value
        /// by passing a value for `config.devicePixelRatio` to Unity's `createUnityInstance()` JavaScript function. In some
        /// versions of Unity, the default index.html template sets `config.devicePixelRatio = 1` on mobile.
        /// In order for 2D WebView to size and position webviews correctly, it must determine the value of `devicePixelRatio`
        /// to use. Since there's no API to get a reference to the Unity instance that the application created with `createUnityInstance()`,
        /// 2D WebView tries to detect if `config.devicePixelRatio` was passed to `createUnityInstance()` by checking for the presence of a
        /// global `config` JavaScript variable. If there is a global variable named `config` with a `devicePixelRatio` property, then 2D WebView
        /// uses that value, otherwise it defaults to using `window.devicePixelRatio`. This approach works for Unity's default
        /// index.html templates, but it may not work if the application uses a custom HTML template or changes the name of the `config`
        /// variable in the default template. In those cases, the application can use this method to pass the overridden value of `devicePixelRatio` to
        /// 2D WebView.
        /// </summary>
        /// <example>
        /// <code>
        /// void Awake() {
        ///     #if UNITY_WEBGL &amp;&amp; !UNITY_EDITOR
        ///         WebGLWebView.SetDevicePixelRatio(1);
        ///     #endif
        /// }
        /// </code>
        /// </example>
        public static void SetDevicePixelRatio(float devicePixelRatio) {

            #if UNITY_2019_3_OR_NEWER
                WebView_setDevicePixelRatio(devicePixelRatio);
            #else
                WebViewLogger.LogWarning("The call to WebGLWebView.SetDevicePixelRatio() will be ignored because a version of Unity older than 2019.3 is being used, which always uses a devicePixelRatio of 1.");
            #endif
        }

        public void SetNativeZoomEnabled(bool enabled) {

            WebViewLogger.LogWarning("2D WebView for WebGL doesn't support native zooming, so the call to IWithNative2DMode.SetNativeZoomEnabled() will be ignored.");
        }

        /// <see cref="IWithNative2DMode"/>
        public void SetRect(Rect rect) {

            _assertValidState();
            _assertNative2DModeEnabled();
            _rect = rect;
            WebView_setRect(_nativeWebViewPtr, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        }

        /// <summary>
        /// Explicitly sets the HTML element that 2D WebView should use as the Unity app container.
        /// 2D WebView automatically detects the Unity app container element if its ID is set to one of the default values of
        /// "unityContainer" or "unity-container". However, if your app uses a custom WebGL template that
        /// uses a different ID for the container element, you must call this method to set the container element ID.
        /// 2D WebView for WebGL works by adding &lt;iframe&gt; elements to the app container, so it's unable to function correctly
        /// if it's unable to find the Unity app container element.
        /// </summary>
        /// <example>
        /// <code>
        /// void Awake() {
        ///     #if UNITY_WEBGL &amp;&amp; !UNITY_EDITOR
        ///         WebGLWebView.SetUnityContainerElementId("your-custom-id");
        ///     #endif
        /// }
        /// </code>
        /// </example>
        public static void SetUnityContainerElementId(string containerId) => WebView_setUnityContainerElementId(containerId);

        public override void SetRenderingEnabled(bool enabled) => VXUtils.LogNative2DModeWarning("SetRenderingEnabled");

        /// <see cref="IWithNative2DMode"/>
        public void SetVisible(bool visible) {

            _assertValidState();
            _assertNative2DModeEnabled();
            Visible = visible;
            WebView_setVisible(_nativeWebViewPtr, visible);
        }

        public override void ZoomIn() => VXUtils.LogNative2DModeWarning("ZoomIn");

        public override void ZoomOut() => VXUtils.LogNative2DModeWarning("ZoomOut");

    #region Non-public members
        readonly WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

        void _assertNative2DModeEnabled() {

            if (!Native2DModeEnabled) {
                throw new InvalidOperationException("IWithNative2DMode methods can only be called on a webview with Native 2D Mode enabled.");
            }
        }

        Task<bool> _canGoBackOrForward(string methodName) {

            _assertValidState();
            if (_verifyIFrameCanBeAccessed(methodName)) {
                WebGLWarnings.LogCanGoBackOrForwardWarning();
                return Task.FromResult(WebView_canGoBackOrForward(_nativeWebViewPtr));
            }
            return Task.FromResult(false);
        }

        bool _verifyIFrameCanBeAccessed(string methodName) {

            if (CanAccessIFrameContent()) {
                return true;
            }
            // Log an error instead of throwing an exception because
            // exceptions are disabled by default for WebGL.
            WebGLWarnings.LogIFrameContentAccessWarning(methodName);
            return false;
        }

        [DllImport(_dllName)]
        static extern void WebView_bringToFront(IntPtr webViewPtr);

        [DllImport(_dllName)]
        static extern bool WebView_canAccessIFrameContent(IntPtr webViewPtr);

        [DllImport(_dllName)]
        static extern bool WebView_canGoBackOrForward(IntPtr webViewPtr);

        [DllImport(_dllName)]
        static extern string WebView_executeJavaScriptSync(IntPtr webViewPtr, string javaScript);

        [DllImport(_dllName)]
        static extern string WebView_executeJavaScriptLocally(string javaScript);

        [DllImport(_dllName)]
        static extern IntPtr WebView_newInNative2DMode(string gameObjectName, int x, int y, int width, int height, bool ignoreDevicePixelRatio);

        [DllImport (_dllName)]
        static extern void WebView_postMessage(IntPtr webViewPtr, string message);

        [DllImport (_dllName)]
        static extern void WebView_setDevicePixelRatio(float devicePixelRatio);

        [DllImport (_dllName)]
        static extern void WebView_setRect(IntPtr webViewPtr, int x, int y, int width, int height);

        [DllImport (_dllName)]
        static extern void WebView_setUnityContainerElementId(string containerId);

        [DllImport (_dllName)]
        static extern void WebView_setVisible(IntPtr webViewPtr, bool visible);
    #endregion
    }
}
#endif
