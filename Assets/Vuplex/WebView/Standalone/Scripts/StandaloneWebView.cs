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
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using Vuplex.WebView.Internal;

namespace Vuplex.WebView {

    /// <summary>
    /// The base IWebView implementation used by 3D WebView for Windows and macOS.
    /// This class also includes extra methods for Standalone-specific functionality.
    /// </summary>
    public abstract partial class StandaloneWebView : BaseWebView,
                                                      IWithCursorType,
                                                      IWithDownloads,
                                                      IWithFileSelection,
                                                      IWithKeyDownAndUp,
                                                      IWithMovablePointer,
                                                      IWithMutableAudio,
                                                      IWithPixelDensity,
                                                      IWithPointerDownAndUp,
                                                      IWithPopups,
                                                      IWithTouch {

        /// <summary>
        /// Indicates that a server requested [HTTP authentication](https://developer.mozilla.org/en-US/docs/Web/HTTP/Authentication)
        /// to make the browser show its built-in authentication UI.
        /// </summary>
        /// <remarks>
        /// If no handler is attached to this event, then the host's authentication request will be ignored
        /// and the page will not be paused. If a handler is attached to this event, then the page will
        /// be paused until Continue() or Cancel() is called.
        ///
        /// You can test basic HTTP auth using [this page](https://jigsaw.w3.org/HTTP/Basic/)
        /// with the username "guest" and the password "guest".
        /// </remarks>
        /// <remarks>
        /// This event is not raised for most websites because most sites implement a custom sign-in page
        /// instead of using HTTP authentication to show the browser's built-in authentication UI.
        /// </remarks>
        /// <example>
        /// <code>
        /// await webViewPrefab.WaitUntilInitialized();
        /// #if UNITY_STANDALONE
        ///     var standaloneWebView = webViewPrefab.WebView as StandaloneWebView;
        ///     standaloneWebView.AuthRequested += (sender, eventArgs) => {
        ///         Debug.Log("Auth requested by " + eventArgs.Host);
        ///         eventArgs.Continue("myUsername", "myPassword");
        ///     };
        /// #endif
        /// </code>
        /// </example>
        public event EventHandler<AuthRequestedEventArgs> AuthRequested {
            add {
                _assertSingletonEventHandlerUnset(_authRequestedHandler, "AuthRequested");
                _authRequestedHandler = value;
                WebView_setAuthEnabled(_nativeWebViewPtr, true);
            }
            remove {
                if (_authRequestedHandler == value) {
                    _authRequestedHandler = null;
                    WebView_setAuthEnabled(_nativeWebViewPtr, false);
                }
            }
        }

        /// <see cref="IWithCursorType"/>
        public event EventHandler<EventArgs<string>> CursorTypeChanged {
            add {
                _cursorTypeChanged += value;
                WebView_setCursorTypeEventsEnabled(_nativeWebViewPtr, true);
            }
            remove {
                _cursorTypeChanged -= value;
                if (_cursorTypeChanged == value) {
                    _authRequestedHandler = null;
                    WebView_setCursorTypeEventsEnabled(_nativeWebViewPtr, false);
                }
            }
        }

        /// <see cref="IWithDownloads"/>
        public event EventHandler<DownloadChangedEventArgs> DownloadProgressChanged;

        /// <see cref="IWithFileSelection"/>
        public event EventHandler<FileSelectionEventArgs> FileSelectionRequested {
            add {
                _assertSingletonEventHandlerUnset(_fileSelectionHandler, "FileSelectionRequested");
                _fileSelectionHandler = value;
                WebView_setFileSelectionEnabled(_nativeWebViewPtr, true);
            }
            remove {
                if (_fileSelectionHandler == value) {
                    _fileSelectionHandler = null;
                    WebView_setFileSelectionEnabled(_nativeWebViewPtr, false);
                }
            }
        }

        /// <see cref="IWithPopups"/>
        public event EventHandler<PopupRequestedEventArgs> PopupRequested;

        /// <see cref="IWithPixelDensity"/>
        public float PixelDensity { get; private set; } = 1f;

        public override Task<bool> CanGoBack() {

            OnCanGoBack();
            return base.CanGoBack();
        }

        public override Task<bool> CanGoForward() {

            OnCanGoForward();
            return base.CanGoForward();
        }

        public override Task<byte[]> CaptureScreenshot() {

            OnCaptureScreenshot();
            return base.CaptureScreenshot();
        }

        public static void ClearAllData() {

            if (WebView_browserProcessIsRunning()) {
                _throwAlreadyInitializedException("ClearAllData");
            }
            var cachePath = _getCachePath();
            if (Directory.Exists(cachePath)) {
                Directory.Delete(cachePath, true);
            }
        }

        public override void Copy() {

            _assertValidState();
            WebView_copy(_nativeWebViewPtr);
            OnCopy();
        }

        public override void Cut() {

            _assertValidState();
            WebView_cut(_nativeWebViewPtr);
            OnCut();
        }

        /// <summary>
        /// Deletes all cookies for all URLs and returns a `Task&lt;bool&gt;` indicating whether the deletion succeeded.
        /// </summary>
        /// <example>
        /// <code>
        /// #if UNITY_STANDALONE
        ///     var succeeded = await StandaloneWebView.DeleteAllCookies();
        /// #endif
        /// </code>
        /// </example>
        /// <seealso cref="Web.ClearAllData"/>
        public static Task<bool> DeleteAllCookies()  => _deleteCookies();

        public static Task<bool> DeleteCookies(string url, string cookieName = null) {

            if (url == null) {
                // Enforce this to match the ICookieManager behavior of other platforms.
                throw new ArgumentException("The url cannot be null.");
            }
            return _deleteCookies(url, cookieName);
        }

        /// <see cref="IWithTouch"/>
        public void SendTouchEvent(TouchEvent touchEvent) {

            _assertValidState();
            var pixelsPoint = _convertNormalizedToPixels(touchEvent.Point);
            WebView_sendTouchEvent(
                _nativeWebViewPtr,
                touchEvent.TouchID,
                (int)touchEvent.Type,
                pixelsPoint.x,
                pixelsPoint.y,
                touchEvent.RadiusX,
                touchEvent.RadiusY,
                touchEvent.RotationAngle,
                touchEvent.Pressure
            );
        }

        /// <summary>
        /// Like Web.EnableRemoteDebugging(), but starts the DevTools session on the
        /// specified port instead of the default port 8080.
        /// </summary>
        /// <param name="portNumber">Port number in the range 1024 - 65535.</param>
        /// <example>
        /// <code>
        /// void Awake() {
        ///     StandaloneWebView.EnableRemoteDebugging(9000);
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Web.EnableRemoteDebugging"/>
        public static void EnableRemoteDebugging(int portNumber) {

            if (!(1024 <= portNumber && portNumber <= 65535)) {
                throw new ArgumentException($"The given port number ({portNumber}) is not in the range from 1024 to 65535.");
            }
            var success = WebView_enableRemoteDebugging(portNumber);
            if (!success) {
                _throwAlreadyInitializedException("EnableRemoteDebugging");
            }
        }

        public override void ExecuteJavaScript(string javaScript, Action<string> callback) {

            base.ExecuteJavaScript(javaScript, callback);
            OnExecuteJavaScript();
        }

        public static Task<Cookie[]> GetCookies(string url, string cookieName = null) {

            if (url == null) {
                throw new ArgumentException("The url cannot be null.");
            }
            var taskSource = new TaskCompletionSource<Cookie[]>();
            var resultCallbackId = Guid.NewGuid().ToString();
            _pendingGetCookiesResultCallbacks[resultCallbackId] = taskSource.SetResult;
            WebView_getCookies(url, cookieName, resultCallbackId);
            return taskSource.Task;
        }

        public override Task<byte[]> GetRawTextureData() {

            OnGetRawTextureData();
            return base.GetRawTextureData();
        }

        public static void GloballySetUserAgent(bool mobile) {

            var success = WebView_globallySetUserAgentToMobile(mobile);
            if (!success) {
                _throwAlreadyInitializedException("SetUserAgent");
            }
        }

        public static void GloballySetUserAgent(string userAgent) {

            var success = WebView_globallySetUserAgent(userAgent);
            if (!success) {
                _throwAlreadyInitializedException("SetUserAgent");
            }
        }

        public override void GoBack() {

            base.GoBack();
            OnGoBack();
        }

        public override void GoForward() {

            base.GoForward();
            OnGoForward();
        }

        public async Task Init(int width, int height) {

            await _initBase(width, height, asyncInit: true);
            _nativeWebViewPtr = WebView_new(gameObject.name, width, height, PixelDensity, null);
            if (_nativeWebViewPtr == IntPtr.Zero) {
                throw new WebViewUnavailableException("Failed to instantiate a new webview. This could indicate that you're using an expired trial version of 3D WebView.");
            }
            await _initTaskSource.Task;
        }

        /// <see cref="IWithKeyDownAndUp"/>
        public void KeyDown(string key, KeyModifier modifiers) {

            _assertValidState();
            WebView_keyDown(_nativeWebViewPtr, key, (int)modifiers);
        }

        /// <see cref="IWithKeyDownAndUp"/>
        public void KeyUp(string key, KeyModifier modifiers) {

            _assertValidState();
            WebView_keyUp(_nativeWebViewPtr, key, (int)modifiers);
        }

        public override void LoadHtml(string html) {

            base.LoadHtml(html);
            OnLoadHtml();
        }

        public override void LoadUrl(string url) {

            base.LoadUrl(url);
            OnLoadUrl(url);
        }

        public override void LoadUrl(string url, Dictionary<string, string> additionalHttpHeaders) {

            if (additionalHttpHeaders != null) {
                foreach (var headerName in additionalHttpHeaders.Keys) {
                    if (headerName.Equals("Accept-Language", StringComparison.InvariantCultureIgnoreCase)) {
                        WebViewLogger.LogError("On Windows and macOS, the Accept-Language request header cannot be set with LoadUrl(url, headers). For more info, please see this article: <em>https://support.vuplex.com/articles/how-to-change-accept-language-header</em>");
                    }
                }
            }
            base.LoadUrl(url, additionalHttpHeaders);
        }

        /// <see cref="IWithMovablePointer"/>
        public void MovePointer(Vector2 normalizedPoint) {

            _assertValidState();
            var pixelsPoint = _convertNormalizedToPixels(normalizedPoint);
            WebView_movePointer(_nativeWebViewPtr, pixelsPoint.x, pixelsPoint.y);
        }

        public override void Paste() {

            _assertValidState();
            WebView_paste(_nativeWebViewPtr);
            OnPaste();
        }

        /// <see cref="IWithPointerDownAndUp"/>
        public void PointerDown(Vector2 point) => _pointerDown(point, MouseButton.Left, 1);

        /// <see cref="IWithPointerDownAndUp"/>
        public void PointerDown(Vector2 point, PointerOptions options) {

            if (options == null) {
                options = new PointerOptions();
            }
            _pointerDown(point, options.Button, options.ClickCount);
        }

        /// <see cref="IWithPointerDownAndUp"/>
        public void PointerUp(Vector2 point) => _pointerUp(point, MouseButton.Left, 1);

        /// <see cref="IWithPointerDownAndUp"/>
        public void PointerUp(Vector2 point, PointerOptions options) {

            if (options == null) {
                options = new PointerOptions();
            }
            _pointerUp(point, options.Button, options.ClickCount);
        }

        public override void SelectAll() {

            _assertValidState();
            WebView_selectAll(_nativeWebViewPtr);
        }

        /// <see cref="IWithMutableAudio"/>
        public void SetAudioMuted(bool muted) {

            _assertValidState();
            WebView_setAudioMuted(_nativeWebViewPtr, muted);
        }

        public static void SetAutoplayEnabled(bool enabled) {

            var success = WebView_setAutoplayEnabled(enabled);
            if (!success) {
                _throwAlreadyInitializedException("SetAutoplayEnabled");
            }
        }

        /// <summary>
        /// By default, Chromium's cache is saved at the file path Application.persistentDataPath/Vuplex.WebView/chromium-cache,
        /// but you can call this method to specify a custom file path for the cache instead. This is useful, for example, to
        /// allow multiple instances of your app to run on the same machine, because multiple instances of Chromium cannot
        /// simultaneously share the same cache.
        /// </summary>
        /// <remarks>
        /// This method cannot be executed while the Chromium browser process is running. So, you will likely need to call it from Awake() to ensure that it's executed before Chromium is started. Alternatively, you can manually terminate Chromium prior to calling this method using StandaloneWebView.TerminateBrowserProcess().
        /// </remarks>
        /// <example>
        /// <code>
        /// void Awake() {
        ///     #if UNITY_STANDALONE
        ///         var customCachePath = Path.Combine(Application.persistentDataPath, "your-chromium-cache");
        ///         StandaloneWebView.SetCachePath(customCachePath);
        ///     #endif
        /// }
        /// </code>
        /// </example>
        public static void SetCachePath(string absoluteFilePath) {

            _cachePathOverride = absoluteFilePath;
            _setCachePath(absoluteFilePath, "SetCachePath");
        }

        public static new void SetCameraAndMicrophoneEnabled(bool enabled) {

            var success = WebView_setCameraAndMicrophoneEnabled(enabled);
            if (!success) {
                _throwAlreadyInitializedException("SetCameraAndMicrophoneEnabled");
            }
        }

        /// <summary>
        /// Sets the log level for the Chromium logs. The default is ChromiumLogLevel.Warning. Log locations: <br/>
        /// - Windows editor: {project path}\Assets\Vuplex\WebView\Standalone\Windows\Plugins\VuplexWebViewChromium\log-chromium.txt~ <br/>
        /// - Windows player: {app path}\{app name}_Data\Plugins\{architecture}\VuplexWebViewChromium\log-chromium.txt <br/>
        /// - macOS: ~/Library/Logs/Vuplex/log-chromium.txt
        /// </summary>
        /// <remarks>
        /// This method cannot be executed while the Chromium browser process is running. So, you will likely need to call it from Awake() to ensure that it's executed before Chromium is started. Alternatively, you can manually terminate Chromium prior to calling this method using StandaloneWebView.TerminateBrowserProcess().
        /// </remarks>
        /// <example>
        /// <code>
        /// void Awake() {
        ///     #if UNITY_STANDALONE
        ///         StandaloneWebView.SetChromiumLogLevel(ChromiumLogLevel.Disabled);
        ///     #endif
        /// }
        /// </code>
        /// </example>
        public static void SetChromiumLogLevel(ChromiumLogLevel level) {

            var success = WebView_setChromiumLogLevel((int)level);
            if (!success) {
                _throwAlreadyInitializedException("SetChromiumLogLevel");
            }
        }

        /// <summary>
        /// Sets additional command line arguments to pass to Chromium.
        /// For reference, [here's an unofficial list of Chromium command line arguments](https://peter.sh/experiments/chromium-command-line-switches/).
        /// </summary>
        /// <remarks>
        /// This method cannot be executed while the Chromium browser process is running. So, you will likely need to call it from Awake() to ensure that it's executed before Chromium is started. Alternatively, you can manually terminate Chromium prior to calling this method using StandaloneWebView.TerminateBrowserProcess().
        /// </remarks>
        /// <example>
        /// <code>
        /// void Awake() {
        ///     #if UNITY_STANDALONE
        ///         StandaloneWebView.SetCommandLineArguments("--ignore-certificate-errors --disable-web-security");
        ///     #endif
        /// }
        /// </code>
        /// </example>
        public static void SetCommandLineArguments(string args) {

            var success = WebView_setCommandLineArguments(args);
            if (!success) {
                _throwAlreadyInitializedException("SetCommandLineArguments");
            }
        }

        public static Task<bool> SetCookie(Cookie cookie) {

            if (cookie == null) {
                throw new ArgumentException("Cookie cannot be null.");
            }
            if (!cookie.IsValid) {
                throw new ArgumentException("Cannot set invalid cookie: " + cookie);
            }
            var taskSource = new TaskCompletionSource<bool>();
            var resultCallbackId = Guid.NewGuid().ToString();
            _pendingModifyCookiesResultCallbacks[resultCallbackId] = taskSource.SetResult;
            WebView_setCookie(cookie.ToJson(), resultCallbackId);
            return taskSource.Task;
        }

        /// <see cref="IWithDownloads"/>
        public void SetDownloadsEnabled(bool enabled) {

            _assertValidState();
            var downloadsDirectoryPath = enabled ? Path.Combine(Application.temporaryCachePath, Path.Combine("Vuplex.WebView", "downloads")) : "";
            WebView_setDownloadsEnabled(_nativeWebViewPtr, downloadsDirectoryPath);
        }

        public static void SetIgnoreCertificateErrors(bool ignore) {

            var success = WebView_setIgnoreCertificateErrors(ignore);
            if (!success) {
                _throwAlreadyInitializedException("SetIgnoreCertificateErrors");
            }
        }

        /// <summary>
        /// The native file picker for file input elements is enabled by default,
        /// but it can be disabled with this method.
        /// </summary>
        /// <example>
        /// <code>
        /// await webViewPrefab.WaitUntilInitialized();
        /// #if UNITY_STANDALONE
        ///     var standaloneWebView = webViewPrefab.WebView as StandaloneWebView;
        ///     standaloneWebView.SetNativeFileDialogEnabled(false);
        /// #endif
        /// </code>
        /// </example>
        public void SetNativeFileDialogEnabled(bool enabled) {

            _assertValidState();
            WebView_setNativeFileDialogEnabled(_nativeWebViewPtr, enabled);
        }

        /// <summary>
        /// Native popups triggered by JavaScript APIs like window.alert() are enabled by default,
        /// but they can be disabled with this method.
        /// </summary>
        /// <example>
        /// <code>
        /// await webViewPrefab.WaitUntilInitialized();
        /// #if UNITY_STANDALONE
        ///     var standaloneWebView = webViewPrefab.WebView as StandaloneWebView;
        ///     standaloneWebView.SetNativeScriptDialogEnabled(false);
        /// #endif
        /// </code>
        /// </example>
        public void SetNativeScriptDialogEnabled(bool enabled) {

            _assertValidState();
            WebView_setNativeScriptDialogEnabled(_nativeWebViewPtr, enabled);
        }

        /// <see cref="IWithPixelDensity"/>
        public void SetPixelDensity(float pixelDensity) {

            if (pixelDensity <= 0f || pixelDensity > 10) {
                throw new ArgumentException($"Invalid pixel density: {pixelDensity}. The pixel density must be between 0 and 10 (exclusive).");
            }
            PixelDensity = pixelDensity;
            if (IsInitialized) {
                _resize();
            }
        }

        /// <see cref="IWithPopups"/>
        public void SetPopupMode(PopupMode popupMode) {

            _assertValidState();
            WebView_setPopupMode(_nativeWebViewPtr, (int)popupMode);
        }

        /// <summary>
        /// By default, web pages cannot share the device's screen
        /// via JavaScript. Invoking `SetScreenSharingEnabled(true)` allows
        /// **all web pages** to share the screen.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The screen that is shared is the default screen, and there isn't currently
        /// support for sharing a different screen or a specific application window.
        /// This is a limitation of Chromium Embedded Framework (CEF), which 3D WebView
        /// uses to embed Chromium.
        /// </para>
        /// <para>
        /// This method cannot be executed while the Chromium browser process is running. So, you will likely need to call it from Awake() to ensure that it's executed before Chromium is started. Alternatively, you can manually terminate Chromium prior to calling this method using StandaloneWebView.TerminateBrowserProcess().
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// void Awake() {
        ///     #if UNITY_STANDALONE
        ///         StandaloneWebView.SetScreenSharingEnabled(true);
        ///     #endif
        /// }
        /// </code>
        /// </example>
        public static void SetScreenSharingEnabled(bool enabled) {

            var success = WebView_setScreenSharingEnabled(enabled);
            if (!success) {
                _throwAlreadyInitializedException("SetScreenSharingEnabled");
            }
        }

        public static void SetStorageEnabled(bool enabled) {

            var cachePath = enabled ? _getCachePath() : "";
            _setCachePath(cachePath, "SetStorageEnabled");
        }

        /// <summary>
        /// Sets the target web frame rate. The default is `60`, which is also the maximum value.
        /// Specifying a target frame rate of `0` disables the frame rate limit.
        /// </summary>
        /// <remarks>
        /// This method cannot be executed while the Chromium browser process is running. So, you will likely need to call it from Awake() to ensure that it's executed before Chromium is started. Alternatively, you can manually terminate Chromium prior to calling this method using StandaloneWebView.TerminateBrowserProcess().
        /// </remarks>
        /// <example>
        /// <code>
        /// void Awake() {
        ///     #if UNITY_STANDALONE
        ///         // Disable the frame rate limit.
        ///         StandaloneWebView.SetTargetFrameRate(0);
        ///     #endif
        /// }
        /// </code>
        /// </example>
        public static void SetTargetFrameRate(uint targetFrameRate) {

            var success = WebView_setTargetFrameRate(targetFrameRate);
            if (!success) {
                _throwAlreadyInitializedException("SetTargetFrameRate");
            }
        }

        /// <summary>
        /// Sets the zoom level to the specified value. Specify `0.0` to reset the zoom level.
        /// </summary>
        /// <example>
        /// <code>
        /// // Set the zoom level to 1.75 after the page finishes loading.
        /// await webViewPrefab.WaitUntilInitialized();
        /// webViewPrefab.WebView.LoadProgressChanged += (sender, eventArgs) => {
        ///     if (eventArgs.Type == ProgressChangeType.Finished) {
        ///         #if UNITY_STANDALONE
        ///             var standaloneWebView = webViewPrefab.WebView as StandaloneWebView;
        ///             standaloneWebView.SetZoomLevel(1.75f);
        ///         #endif
        ///     }
        /// };
        /// </code>
        /// </example>
        public void SetZoomLevel(float zoomLevel) {

            _assertValidState();
            WebView_setZoomLevel(_nativeWebViewPtr, zoomLevel);
        }

        /// <summary>
        /// Terminates the Chromium browser process.
        /// </summary>
        /// <remarks>
        /// When 3D WebView for Windows and macOS initializes the application's first webview, it starts Chromium in a separate process.
        /// That Chromium process runs until the application closes, at which point 3D WebView automatically calls this
        /// method to terminate the Chromium process. However, in some rare cases, you may want your application to call
        /// this method manually to terminate Chromium while the app is still running. For example, some 3D WebView APIs
        /// like Web.ClearAllData() cannot be called while Chromium is running, but your application can use this method to terminate
        /// Chromium so that it can call those APIs. Prior to calling this method, your application must destroy all of the
        /// application's existing webviews (e.g. using WebViewPrefab.Destroy()). After Chromium finishes terminating,
        /// you can restart Chromium by creating new webview instances. (e.g. using WebViewPrefab.Instantiate()).
        /// </remarks>
        /// <example>
        /// <code>
        /// async void ClearDataAtRuntime() {
        ///     #if UNITY_STANDALONE || UNITY_EDITOR
        ///         // 1. Destroy all the webviews in the application.
        ///         var webViewPrefabs = GameObject.FindObjectsOfType&lt;BaseWebViewPrefab&gt;();
        ///         foreach (var prefab in webViewPrefabs) {
        ///             prefab.Destroy();
        ///         }
        ///
        ///         // 2. Terminate Chromium.
        ///         await StandaloneWebView.TerminateBrowserProcess();
        ///
        ///         // 3. Call the API that can't be called while Chromium is running.
        ///         Web.ClearAllData();
        ///
        ///         // 4. TODO: Create new webviews with WebViewPrefab.Instantiate() if needed.
        ///     #else
        ///         // On other platforms, Web.ClearAllData() can be called while the browser process is running.
        ///         Web.ClearAllData();
        ///     #endif
        /// }
        /// </code>
        /// </example>
        public static Task TerminateBrowserProcess() {

            if (_terminationTaskSource != null) {
                return _terminationTaskSource.Task;
            }
            var processWasRunning = WebView_terminateBrowserProcess();
            if (!processWasRunning) {
                return Task.FromResult(true);
            }
            _terminationTaskSource = new TaskCompletionSource<bool>();
            return _terminationTaskSource.Task;
        }

    #region Non-public members
        EventHandler<AuthRequestedEventArgs> _authRequestedHandler;
        static string _cachePathOverride;
        event EventHandler<EventArgs<string>> _cursorTypeChanged;
        EventHandler<FileSelectionEventArgs> _fileSelectionHandler;
        static Dictionary<string, Action<Cookie[]>> _pendingGetCookiesResultCallbacks = new Dictionary<string, Action<Cookie[]>>();
        static Dictionary<string, Action<bool>> _pendingModifyCookiesResultCallbacks = new Dictionary<string, Action<bool>>();
        static TaskCompletionSource<bool> _terminationTaskSource;

        static Task<bool> _deleteCookies(string url = null, string cookieName = null) {

            var taskSource = new TaskCompletionSource<bool>();
            var resultCallbackId = Guid.NewGuid().ToString();
            _pendingModifyCookiesResultCallbacks[resultCallbackId] = taskSource.SetResult;
            WebView_deleteCookies(url, cookieName, resultCallbackId);
            return taskSource.Task;
        }

        protected static string _getCachePath() {

            return _cachePathOverride ?? Path.Combine(Application.persistentDataPath, "Vuplex.WebView", "chromium-cache");
        }

        // Invoked by the native plugin.
        void HandleAuthRequested(string host) {

            if (_authRequestedHandler == null) {
                // This shouldn't happen.
                WebViewLogger.LogWarning("The native webview sent an auth request, but no event handler is attached to AuthRequested.");
                WebView_cancelAuth(_nativeWebViewPtr);
                return;
            }
            var eventArgs = new AuthRequestedEventArgs(
                host,
                (username, password) => WebView_continueAuth(_nativeWebViewPtr, username, password),
                () => WebView_cancelAuth(_nativeWebViewPtr)
            );
            _authRequestedHandler?.Invoke(this, eventArgs);
        }

        // Invoked by the native plugin.
        void HandleCursorTypeChanged(string type) => _cursorTypeChanged?.Invoke(this, new EventArgs<string>(type));

        // Invoked by the native plugin.
        void HandleDownloadProgressChanged(string serializedMessage) {

            DownloadProgressChanged?.Invoke(this, DownloadMessage.FromJson(serializedMessage).ToEventArgs());
        }

        // Invoked by the native plugin.
        void HandleFileSelectionRequested(string serializedMessage) {

            var message = FileSelectionMessage.FromJson(serializedMessage);
            Action<string[]> continueCallback = (filePaths) => {
                var serializedFilePaths = JsonUtility.ToJson(new JsonArrayWrapper<string>(filePaths));
                WebView_continueFileSelection(_nativeWebViewPtr, serializedFilePaths);
            };
            Action cancelCallback = () => WebView_cancelFileSelection(_nativeWebViewPtr);
            var eventArgs = new FileSelectionEventArgs(
                message.AcceptFilters,
                message.MultipleAllowed,
                continueCallback,
                cancelCallback
            );
            _fileSelectionHandler(this, eventArgs);
        }

        [AOT.MonoPInvokeCallback(typeof(Action<string, string>))]
        static void _handleGetCookiesResult(string resultCallbackId, string serializedCookies) {

            var callback = _pendingGetCookiesResultCallbacks[resultCallbackId];
            _pendingGetCookiesResultCallbacks.Remove(resultCallbackId);
            var cookies = Cookie.ArrayFromJson(serializedCookies);
            callback(cookies);
        }

        // Invoked by the native plugin.
        void HandlePopup(string message) {

            if (PopupRequested == null) {
                return;
            }
            var components = message.Split(new char[] { ',' }, 2);
            var url = components[0];
            var popupBrowserId = components[1];

            if (popupBrowserId.Length == 0) {
                PopupRequested?.Invoke(this, new PopupRequestedEventArgs(url, null));
                return;
            }
            var popupWebView = _instantiate();
            Dispatcher.RunOnMainThread(async () => {
                await popupWebView._initPopup(Size.x, Size.y, PixelDensity, popupBrowserId);
                PopupRequested?.Invoke(this, new PopupRequestedEventArgs(url, popupWebView as IWebView));
            });
        }

        [AOT.MonoPInvokeCallback(typeof(Action<string, bool>))]
        static void _handleModifyCookiesResult(string resultCallbackId, bool success) {

            var callback = _pendingModifyCookiesResultCallbacks[resultCallbackId];
            _pendingModifyCookiesResultCallbacks.Remove(resultCallbackId);
            callback?.Invoke(success);
        }

        [AOT.MonoPInvokeCallback(typeof(Action))]
        static void _handleTerminationFinished() {

            if (_terminationTaskSource != null) {
                _terminationTaskSource.SetResult(true);
                _terminationTaskSource = null;
            }
        }

        async Task _initPopup(int width, int height, float pixelDensity, string popupId) {

            await _initBase(width, height, asyncInit: true);
            PixelDensity = pixelDensity;
            _nativeWebViewPtr = WebView_new(gameObject.name, width, height, PixelDensity, popupId);
            await _initTaskSource.Task;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void _initializePlugin() {

            WebView_initializePlugin(
                Marshal.GetFunctionPointerForDelegate<Action>(_handleTerminationFinished),
                Marshal.GetFunctionPointerForDelegate<Action<string>>(_logInfo),
                Marshal.GetFunctionPointerForDelegate<Action<string>>(_logWarning),
                Marshal.GetFunctionPointerForDelegate<Action<string>>(_logError),
                Marshal.GetFunctionPointerForDelegate<Action<string, string, string>>(_unitySendMessage),
                Marshal.GetFunctionPointerForDelegate<Action<string, string>>(_handleGetCookiesResult),
                Marshal.GetFunctionPointerForDelegate<Action<string, bool>>(_handleModifyCookiesResult)
            );
            // cache, cookies, and storage are enabled by default
            _setCachePath(_getCachePath(), "_initializePlugin");
        }

        protected abstract StandaloneWebView _instantiate();

        [AOT.MonoPInvokeCallback(typeof(Action<string>))]
        static void _logInfo(string message) => WebViewLogger.Log(message, false);

        [AOT.MonoPInvokeCallback(typeof(Action<string>))]
        static void _logWarning(string message) => WebViewLogger.LogWarning(message, false);

        [AOT.MonoPInvokeCallback(typeof(Action<string>))]
        static void _logError(string message) => WebViewLogger.LogError(message, false);

        protected override void _resize() => WebView_resizeWithPixelDensity(_nativeWebViewPtr, Size.x, Size.y, PixelDensity);

        static void _throwAlreadyInitializedException(string methodName) {

            throw new InvalidOperationException($"Unable to execute {methodName}() because Chromium is already running. On Windows and macOS, {methodName}() can only be called before Chromium is started. The easiest way to resolve this issue is by calling {methodName}() earlier in the application, like by calling it from Awake() instead of from Start(). Alternatively, you can manually terminate Chromium prior calling the method by using StandaloneWebView.TerminateBrowserProcess(): https://developer.vuplex.com/webview/StandaloneWebView#TerminateBrowserProcess");
        }

        // Partial methods implemented by other 3D WebView packages
        // to provide platform-specific warnings in the editor.
        partial void OnCanGoBack();
        partial void OnCanGoForward();
        partial void OnCaptureScreenshot();
        partial void OnCopy();
        partial void OnCut();
        partial void OnExecuteJavaScript();
        partial void OnGetRawTextureData();
        partial void OnGoBack();
        partial void OnGoForward();
        partial void OnLoadHtml();
        partial void OnLoadUrl(string url);
        partial void OnPaste();

        void _pointerDown(Vector2 normalizedPoint, MouseButton mouseButton, int clickCount) {

            _assertValidState();
            var pixelsPoint = _convertNormalizedToPixels(normalizedPoint);
            WebView_pointerDown(_nativeWebViewPtr, pixelsPoint.x, pixelsPoint.y, (int)mouseButton, clickCount);
        }

        void _pointerUp(Vector2 normalizedPoint, MouseButton mouseButton, int clickCount) {

            _assertValidState();
            var pixelsPoint = _convertNormalizedToPixels(normalizedPoint);
            WebView_pointerUp(_nativeWebViewPtr, pixelsPoint.x, pixelsPoint.y, (int)mouseButton, clickCount);
        }

        static void _setCachePath(string path, string methodName) {

            var cachePath = path ?? "";
            var success = WebView_setCachePath(cachePath);
            if (!success) {
                _throwAlreadyInitializedException(methodName);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(Action<string, string, string>))]
        static void _unitySendMessage(string gameObjectName, string methodName, string message) {

            Dispatcher.RunOnMainThread(() => {
                var gameObj = GameObject.Find(gameObjectName);
                if (gameObj == null) {
                    WebViewLogger.LogWarning($"Unable to deliver a message from the native plugin to a webview GameObject because there is no longer a GameObject named '{gameObjectName}'. This can sometimes happen directly after destroying a webview. In that case, it is benign and this message can be ignored.");
                    return;
                }
                gameObj.SendMessage(methodName, message);
            });
        }

        [DllImport(_dllName)]
        static extern bool WebView_browserProcessIsRunning();

        [DllImport(_dllName)]
        static extern void WebView_cancelAuth(IntPtr webViewPtr);

        [DllImport(_dllName)]
        static extern void WebView_cancelFileSelection(IntPtr webViewPtr);

        [DllImport(_dllName)]
        static extern void WebView_continueAuth(IntPtr webViewPtr, string username, string password);

        [DllImport(_dllName)]
        static extern void WebView_continueFileSelection(IntPtr webViewPtr, string serializedFilePaths);

        [DllImport(_dllName)]
        static extern void WebView_copy(IntPtr webViewPtr);

        [DllImport(_dllName)]
        static extern void WebView_cut(IntPtr webViewPtr);

        [DllImport(_dllName)]
        static extern void WebView_deleteCookies(string url, string cookieName, string resultCallbackId);

        [DllImport(_dllName)]
        static extern bool WebView_enableRemoteDebugging(int portNumber);

        [DllImport(_dllName)]
        static extern void WebView_getCookies(string url, string cookieName, string resultCallbackId);

        [DllImport(_dllName)]
        static extern bool WebView_globallySetUserAgentToMobile(bool mobile);

        [DllImport(_dllName)]
        static extern bool WebView_globallySetUserAgent(string userAgent);

        [DllImport(_dllName)]
        static extern void WebView_initializePlugin(
            IntPtr terminationFinishedCallback,
            IntPtr logInfoFunction,
            IntPtr logWarningFunction,
            IntPtr logErrorFunction,
            IntPtr unitySendMessageFunction,
            IntPtr getCookiesCallback,
            IntPtr modifyCookiesCallback
        );

        [DllImport(_dllName)]
        static extern void WebView_keyDown(IntPtr webViewPtr, string key, int modifiers);

        [DllImport(_dllName)]
        static extern void WebView_keyUp(IntPtr webViewPtr, string key, int modifiers);

        [DllImport (_dllName)]
        static extern void WebView_movePointer(IntPtr webViewPtr, int x, int y);

        [DllImport(_dllName)]
        static extern IntPtr WebView_new(string gameObjectName, int width, int height, float pixelDensity, string popupBrowserId);

        [DllImport(_dllName)]
        static extern void WebView_paste(IntPtr webViewPtr);

        [DllImport (_dllName)]
        static extern void WebView_pointerDown(IntPtr webViewPtr, int x, int y, int mouseButton, int clickCount);

        [DllImport (_dllName)]
        static extern void WebView_pointerUp(IntPtr webViewPtr, int x, int y, int mouseButton, int clickCount);

        [DllImport(_dllName)]
        protected static extern void WebView_resizeWithPixelDensity(IntPtr webViewPtr, int width, int height, float pixelDensity);

        [DllImport(_dllName)]
        static extern void WebView_selectAll(IntPtr webViewPtr);

        [DllImport (_dllName)]
        static extern void WebView_sendTouchEvent(
            IntPtr webViewPtr,
            int touchID,
            int type,
            float pointX,
            float pointY,
            float radiusX,
            float radiusY,
            float rotationAngle,
            float pressure
        );

        [DllImport(_dllName)]
        static extern void WebView_setAudioMuted(IntPtr webViewPtr, bool muted);

        [DllImport (_dllName)]
        static extern void WebView_setAuthEnabled(IntPtr webViewPtr, bool enabled);

        [DllImport(_dllName)]
        static extern bool WebView_setAutoplayEnabled(bool enabled);

        [DllImport(_dllName)]
        static extern bool WebView_setCachePath(string cachePath);

        [DllImport(_dllName)]
        static extern bool WebView_setCameraAndMicrophoneEnabled(bool enabled);

        [DllImport(_dllName)]
        static extern bool WebView_setChromiumLogLevel(int level);

        [DllImport(_dllName)]
        static extern bool WebView_setCommandLineArguments(string args);

        [DllImport(_dllName)]
        static extern void WebView_setCookie(string serializedCookie, string resultCallbackId);

        [DllImport (_dllName)]
        static extern void WebView_setCursorTypeEventsEnabled(IntPtr webViewPtr, bool enabled);

        [DllImport (_dllName)]
        static extern void WebView_setDownloadsEnabled(IntPtr webViewPtr, string downloadsDirectoryPath);

        [DllImport (_dllName)]
        static extern void WebView_setFileSelectionEnabled(IntPtr webViewPtr, bool enabled);

        [DllImport(_dllName)]
        static extern bool WebView_setIgnoreCertificateErrors(bool ignore);

        [DllImport(_dllName)]
        static extern void WebView_setNativeFileDialogEnabled(IntPtr webViewPtr, bool enabled);

        [DllImport(_dllName)]
        static extern void WebView_setNativeScriptDialogEnabled(IntPtr webViewPtr, bool enabled);

        [DllImport(_dllName)]
        static extern void WebView_setPopupMode(IntPtr webViewPtr, int popupMode);

        [DllImport(_dllName)]
        static extern bool WebView_setScreenSharingEnabled(bool enabled);

        [DllImport(_dllName)]
        static extern bool WebView_setTargetFrameRate(uint targetFrameRate);

        [DllImport(_dllName)]
        static extern void WebView_setZoomLevel(IntPtr webViewPtr, float zoomLevel);

        [DllImport(_dllName)]
        static extern bool WebView_terminateBrowserProcess();
    #endregion

    #region Obsolete APIs
        // Added in v3.16, deprecated in v4.1.
        [Obsolete("StandaloneWebView.DispatchTouchEvent() has been renamed to SendTouchEvent(). Please switch to using IWithTouch.SendTouchEvent(): https://developer.vuplex.com/webview/IWithTouch")]
        public void DispatchTouchEvent(TouchEvent touchEvent) => SendTouchEvent(touchEvent);

        // Added in v3.9, deprecated in v4.0.
        [Obsolete("StandaloneWebView.GetCookie() is now deprecated. Please switch to using Web.CookieManager.GetCookies(): https://developer.vuplex.com/webview/CookieManager#GetCookies")]
        public static async void GetCookie(string url, string cookieName, Action<Cookie> callback) {
            var cookie = await GetCookie(url, cookieName);
            callback(cookie);
        }

        // Added in v3.9, deprecated in v4.0.
        [Obsolete("StandaloneWebView.GetCookie() is now deprecated. Please switch to Web.CookieManager.GetCookies(): https://developer.vuplex.com/webview/CookieManager#GetCookies")]
        public static async Task<Cookie> GetCookie(string url, string cookieName) {
            var cookies = await GetCookies(url, cookieName);
            return cookies.Length > 0 ? cookies[0] : null;
        }

        // Added in v3.11, deprecated in v4.0.
        [Obsolete("StandaloneWebView.GetCookies(url, callback) is now deprecated. Please switch to Web.CookieManager.GetCookies(): https://developer.vuplex.com/webview/CookieManager#GetCookies")]
        public static async void GetCookies(string url, Action<Cookie[]> callback) {
            var cookies = await GetCookies(url);
            callback(cookies);
        }

        // Added in v3.3, deprecated in v4.0.
        [Obsolete("StandaloneWebView.SetAudioAndVideoCaptureEnabled() is now deprecated. Please switch to Web.SetCameraAndMicrophoneEnabled(): https://developer.vuplex.com/webview/Web#SetCameraAndMicrophoneEnabled")]
        public static void SetAudioAndVideoCaptureEnabled(bool enabled) => SetCameraAndMicrophoneEnabled(enabled);

        // Added in v3.10, deprecated in v4.0.
        [Obsolete("StandaloneWebView.SetCookie(cookie, callback) is now deprecated. Please switch to Web.CookieManager.SetCookie(): https://developer.vuplex.com/webview/CookieManager#SetCookie")]
        public static async void SetCookie(Cookie cookie, Action<bool> callback) {
            var result = await SetCookie(cookie);
            callback(result);
        }

        // Deprecated in v4.2.
        [Obsolete("StandaloneWebView.TerminatePlugin() has been replaced with StandaloneWebView.TerminateBrowserProcess(). Please switch to TerminateBrowserProcess().")]
        public static void TerminatePlugin() => TerminateBrowserProcess();
    #endregion
    }
}
#endif
