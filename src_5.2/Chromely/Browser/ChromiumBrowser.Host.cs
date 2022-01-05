// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#nullable disable

namespace Chromely.Browser;

public abstract partial class ChromiumBrowser
{
    public void CloseBrowser(bool forceClose = false)
    {
        BrowserHost?.CloseBrowser(forceClose);
    }

    public bool TryCloseBrowser()
    {
        return BrowserHost is not null && BrowserHost.TryCloseBrowser();
    }

    public void SetFocus(bool focus)
    {
        BrowserHost?.SetFocus(focus);
    }

    public IntPtr GetWindowHandle()
    {
        return BrowserHost is null ? IntPtr.Zero : BrowserHost.GetWindowHandle();
    }

    public IntPtr GetOpenerWindowHandle()
    {
        return BrowserHost is null ? IntPtr.Zero : BrowserHost.GetOpenerWindowHandle();
    }

    public bool HasView
    {
        get { return BrowserHost is not null && BrowserHost.HasView; }
    }

    public CefClient GetClient()
    {
        return BrowserHost?.GetClient();
    }


    /// <summary>
    /// Returns the request context for this browser.
    /// </summary>
    public CefRequestContext GetRequestContext()
    {
        return BrowserHost?.GetRequestContext();
    }

    /// <summary>
    /// Get the current zoom level. The default zoom level is 0.0. This method can
    /// only be called on the UI thread.
    /// </summary>
    public double GetZoomLevel()
    {
        return BrowserHost is null ? 0 : BrowserHost.GetZoomLevel();
    }

    /// <summary>
    /// Change the zoom level to the specified value. Specify 0.0 to reset the
    /// zoom level. If called on the UI thread the change will be applied
    /// immediately. Otherwise, the change will be applied asynchronously on the
    /// UI thread.
    /// </summary>
    public void SetZoomLevel(double value)
    {
        BrowserHost?.SetZoomLevel(value);
    }

    /// <summary>
    /// Call to run a file chooser dialog. Only a single file chooser dialog may be
    /// pending at any given time. |mode| represents the type of dialog to display.
    /// |title| to the title to be used for the dialog and may be empty to show the
    /// default title ("Open" or "Save" depending on the mode). |default_file_path|
    /// is the path with optional directory and/or file name component that will be
    /// initially selected in the dialog. |accept_filters| are used to restrict the
    /// selectable file types and may any combination of (a) valid lower-cased MIME
    /// types (e.g. "text/*" or "image/*"), (b) individual file extensions (e.g.
    /// ".txt" or ".png"), or (c) combined description and file extension delimited
    /// using "|" and ";" (e.g. "Image Types|.png;.gif;.jpg").
    /// |selected_accept_filter| is the 0-based index of the filter that will be
    /// selected by default. |callback| will be executed after the dialog is
    /// dismissed or immediately if another dialog is already pending. The dialog
    /// will be initiated asynchronously on the UI thread.
    /// </summary>
    public void RunFileDialog(CefFileDialogMode mode, string title, string defaultFilePath, string[] acceptFilters, int selectedAcceptFilter, CefRunFileDialogCallback callback)
    {
        BrowserHost?.RunFileDialog(mode, title, defaultFilePath, acceptFilters, selectedAcceptFilter, callback);
    }

    /// <summary>
    /// Download the file at |url| using CefDownloadHandler.
    /// </summary>
    public void StartDownload(string url)
    {
        BrowserHost?.StartDownload(url);
    }

    /// <summary>
    /// Download |image_url| and execute |callback| on completion with the images
    /// received from the renderer. If |is_favicon| is true then cookies are not
    /// sent and not accepted during download. Images with density independent
    /// pixel (DIP) sizes larger than |max_image_size| are filtered out from the
    /// image results. Versions of the image at different scale factors may be
    /// downloaded up to the maximum scale factor supported by the system. If there
    /// are no image results &lt;= |max_image_size| then the smallest image is resized
    /// to |max_image_size| and is the only result. A |max_image_size| of 0 means
    /// unlimited. If |bypass_cache| is true then |image_url| is requested from the
    /// server even if it is present in the browser cache.
    /// </summary>
    public void DownloadImage(string imageUrl, bool isFavIcon, uint maxImageSize, bool bypassCache, CefDownloadImageCallback callback)
    {
        BrowserHost?.DownloadImage(imageUrl, isFavIcon, maxImageSize, bypassCache, callback);
    }

    /// <summary>
    /// Print the current browser contents.
    /// </summary>
    public void Print()
    {
        BrowserHost?.Print();
    }

    /// <summary>
    /// Print the current browser contents to the PDF file specified by |path| and
    /// execute |callback| on completion. The caller is responsible for deleting
    /// |path| when done. For PDF printing to work on Linux you must implement the
    /// CefPrintHandler::GetPdfPaperSize method.
    /// </summary>
    public void PrintToPdf(string path, CefPdfPrintSettings settings, CefPdfPrintCallback callback)
    {
        BrowserHost?.PrintToPdf(path, settings, callback);
    }

    /// <summary>
    /// Search for |searchText|. |identifier| must be a unique ID and these IDs
    /// must strictly increase so that newer requests always have greater IDs than
    /// older requests. If |identifier| is zero or less than the previous ID value
    /// then it will be automatically assigned a new valid ID. |forward| indicates
    /// whether to search forward or backward within the page. |matchCase|
    /// indicates whether the search should be case-sensitive. |findNext| indicates
    /// whether this is the first request or a follow-up. The CefFindHandler
    /// instance, if any, returned via CefClient::GetFindHandler will be called to
    /// report find results.
    /// </summary>
    public void Find(int identifier, string searchText, bool forward, bool matchCase, bool findNext)
    {
        BrowserHost?.Find(identifier, searchText, forward, matchCase, findNext);
    }

    /// <summary>
    /// Cancel all searches that are currently going on.
    /// </summary>
    public void StopFinding(bool clearSelection)
    {
        BrowserHost?.StopFinding(clearSelection);
    }

    /// <summary>
    /// Open developer tools (DevTools) in its own browser. The DevTools browser
    /// will remain associated with this browser. If the DevTools browser is
    /// already open then it will be focused, in which case the |windowInfo|,
    /// |client| and |settings| parameters will be ignored. If |inspect_element_at|
    /// is non-empty then the element at the specified (x,y) location will be
    /// inspected. The |windowInfo| parameter will be ignored if this browser is
    /// wrapped in a CefBrowserView.
    /// </summary>
    public void ShowDevTools(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings browserSettings, CefPoint inspectElementAt)
    {
        BrowserHost?.ShowDevTools(windowInfo, client, browserSettings, inspectElementAt);
    }

    /// <summary>
    /// Explicitly close the associated DevTools browser, if any.
    /// </summary>
    public void CloseDevTools()
    {
        BrowserHost?.CloseDevTools();
    }

    /// <summary>
    /// Returns true if this browser currently has an associated DevTools browser.
    /// Must be called on the browser process UI thread.
    /// </summary>
    public bool HasDevTools
    {
        get
        {
            return BrowserHost is not null && BrowserHost.HasDevTools;
        }
    }

    /// <summary>
    /// Send a method call message over the DevTools protocol. |message| must be a
    /// UTF8-encoded JSON dictionary that contains "id" (int), "method" (string)
    /// and "params" (dictionary, optional) values. See the DevTools protocol
    /// documentation at https://chromedevtools.github.io/devtools-protocol/ for
    /// details of supported methods and the expected "params" dictionary contents.
    /// |message| will be copied if necessary. This method will return true if
    /// called on the UI thread and the message was successfully submitted for
    /// validation, otherwise false. Validation will be applied asynchronously and
    /// any messages that fail due to formatting errors or missing parameters may
    /// be discarded without notification. Prefer ExecuteDevToolsMethod if a more
    /// structured approach to message formatting is desired.
    /// Every valid method call will result in an asynchronous method result or
    /// error message that references the sent message "id". Event messages are
    /// received while notifications are enabled (for example, between method calls
    /// for "Page.enable" and "Page.disable"). All received messages will be
    /// delivered to the observer(s) registered with AddDevToolsMessageObserver.
    /// See CefDevToolsMessageObserver::OnDevToolsMessage documentation for details
    /// of received message contents.
    /// Usage of the SendDevToolsMessage, ExecuteDevToolsMethod and
    /// AddDevToolsMessageObserver methods does not require an active DevTools
    /// front-end or remote-debugging session. Other active DevTools sessions will
    /// continue to function independently. However, any modification of global
    /// browser state by one session may not be reflected in the UI of other
    /// sessions.
    /// Communication with the DevTools front-end (when displayed) can be logged
    /// for development purposes by passing the
    /// `--devtools-protocol-log-file=&lt;path&gt;` command-line flag.
    /// </summary>
    public bool SendDevToolsMessage(IntPtr message, int messageSize)
    {
        return BrowserHost is not null && BrowserHost.SendDevToolsMessage(message, messageSize);
    }

    /// <summary>
    /// Execute a method call over the DevTools protocol. This is a more structured
    /// version of SendDevToolsMessage. |message_id| is an incremental number that
    /// uniquely identifies the message (pass 0 to have the next number assigned
    /// automatically based on previous values). |method| is the method name.
    /// |params| are the method parameters, which may be empty. See the DevTools
    /// protocol documentation (linked above) for details of supported methods and
    /// the expected |params| dictionary contents. This method will return the
    /// assigned message ID if called on the UI thread and the message was
    /// successfully submitted for validation, otherwise 0. See the
    /// SendDevToolsMessage documentation for additional usage information.
    /// </summary>
    public int ExecuteDevToolsMethod(int messageId, string method, CefDictionaryValue parameters)
    {
        return BrowserHost is null ? 0 : BrowserHost.ExecuteDevToolsMethod(messageId, method, parameters);
    }

    /// <summary>
    /// Add an observer for DevTools protocol messages (method results and events).
    /// The observer will remain registered until the returned Registration object
    /// is destroyed. See the SendDevToolsMessage documentation for additional
    /// usage information.
    /// </summary>
    public CefRegistration AddDevToolsMessageObserver(CefDevToolsMessageObserver observer)
    {
        return BrowserHost?.AddDevToolsMessageObserver(observer);
    }

    /// <summary>
    /// Retrieve a snapshot of current navigation entries as values sent to the
    /// specified visitor. If |current_only| is true only the current navigation
    /// entry will be sent, otherwise all navigation entries will be sent.
    /// </summary>
    public void GetNavigationEntries(CefNavigationEntryVisitor visitor, bool currentOnly)
    {
        BrowserHost?.GetNavigationEntries(visitor, currentOnly);
    }

    /// <summary>
    /// If a misspelled word is currently selected in an editable node calling
    /// this method will replace it with the specified |word|.
    /// </summary>
    public void ReplaceMisspelling(string word)
    {
        BrowserHost?.ReplaceMisspelling(word);
    }

    /// <summary>
    /// Add the specified |word| to the spelling dictionary.
    /// </summary>
    public void AddWordToDictionary(string word)
    {
        BrowserHost?.AddWordToDictionary(word);
    }

    /// <summary>
    /// Returns true if window rendering is disabled.
    /// </summary>
    public bool IsWindowRenderingDisabled
    {
        get
        {
            return BrowserHost is not null && BrowserHost.IsWindowRenderingDisabled;
        }
    }

    /// <summary>
    /// Notify the browser that the widget has been resized. The browser will first
    /// call CefRenderHandler::GetViewRect to get the new size and then call
    /// CefRenderHandler::OnPaint asynchronously with the updated regions. This
    /// method is only used when window rendering is disabled.
    /// </summary>
    public void WasResized()
    {
        BrowserHost?.WasResized();
    }

    /// <summary>
    /// Notify the browser that it has been hidden or shown. Layouting and
    /// CefRenderHandler::OnPaint notification will stop when the browser is
    /// hidden. This method is only used when window rendering is disabled.
    /// </summary>
    public void WasHidden(bool hidden)
    {
        BrowserHost?.WasHidden(hidden);
    }

    /// <summary>
    /// Send a notification to the browser that the screen info has changed. The
    /// browser will then call CefRenderHandler::GetScreenInfo to update the
    /// screen information with the new values. This simulates moving the webview
    /// window from one display to another, or changing the properties of the
    /// current display. This method is only used when window rendering is
    /// disabled.
    /// </summary>
    public void NotifyScreenInfoChanged()
    {
        BrowserHost?.NotifyScreenInfoChanged();
    }

    /// <summary>
    /// Invalidate the view. The browser will call CefRenderHandler::OnPaint
    /// asynchronously. This method is only used when window rendering is
    /// disabled.
    /// </summary>
    public void Invalidate(CefPaintElementType type)
    {
        BrowserHost?.Invalidate(type);
    }

    /// <summary>
    /// Issue a BeginFrame request to Chromium.  Only valid when
    /// CefWindowInfo::external_begin_frame_enabled is set to true.
    /// </summary>
    public void SendExternalBeginFrame()
    {
        BrowserHost?.SendExternalBeginFrame();
    }

    /// <summary>
    /// Send a key event to the browser.
    /// </summary>
    public void SendKeyEvent(CefKeyEvent keyEvent)
    {
        BrowserHost?.SendKeyEvent(keyEvent);
    }

    /// <summary>
    /// Send a mouse click event to the browser. The |x| and |y| coordinates are
    /// relative to the upper-left corner of the view.
    /// </summary>
    public void SendMouseClickEvent(CefMouseEvent @event, CefMouseButtonType type, bool mouseUp, int clickCount)
    {
        BrowserHost?.SendMouseClickEvent(@event, type, mouseUp, clickCount);
    }

    /// <summary>
    /// Send a mouse move event to the browser. The |x| and |y| coordinates are
    /// relative to the upper-left corner of the view.
    /// </summary>
    public void SendMouseMoveEvent(CefMouseEvent @event, bool mouseLeave)
    {
        BrowserHost?.SendMouseMoveEvent(@event, mouseLeave);
    }

    /// <summary>
    /// Send a mouse wheel event to the browser. The |x| and |y| coordinates are
    /// relative to the upper-left corner of the view. The |deltaX| and |deltaY|
    /// values represent the movement delta in the X and Y directions respectively.
    /// In order to scroll inside select popups with window rendering disabled
    /// CefRenderHandler::GetScreenPoint should be implemented properly.
    /// </summary>
    public void SendMouseWheelEvent(CefMouseEvent @event, int deltaX, int deltaY)
    {
        BrowserHost?.SendMouseWheelEvent(@event, deltaX, deltaY);
    }

    /// <summary>
    /// Send a touch event to the browser for a windowless browser.
    /// </summary>
    public void SendTouchEvent(CefTouchEvent @event)
    {
        BrowserHost?.SendTouchEvent(@event);
    }

    /// <summary>
    /// Send a capture lost event to the browser.
    /// </summary>
    public void SendCaptureLostEvent()
    {
        BrowserHost?.SendCaptureLostEvent();
    }

    /// <summary>
    /// Notify the browser that the window hosting it is about to be moved or
    /// resized. This method is only used on Windows and Linux.
    /// </summary>
    public void NotifyMoveOrResizeStarted()
    {
        BrowserHost?.NotifyMoveOrResizeStarted();
    }

    /// <summary>
    /// Returns the maximum rate in frames per second (fps) that CefRenderHandler::
    /// OnPaint will be called for a windowless browser. The actual fps may be
    /// lower if the browser cannot generate frames at the requested rate. The
    /// minimum value is 1 and the maximum value is 60 (default 30). This method
    /// can only be called on the UI thread.
    /// </summary>
    public int GetWindowlessFrameRate()
    {
        return BrowserHost is null ? 0 : BrowserHost.GetWindowlessFrameRate();
    }

    /// <summary>
    /// Set the maximum rate in frames per second (fps) that CefRenderHandler::
    /// OnPaint will be called for a windowless browser. The actual fps may be
    /// lower if the browser cannot generate frames at the requested rate. The
    /// minimum value is 1 and the maximum value is 60 (default 30). Can also be
    /// set at browser creation via CefBrowserSettings.windowless_frame_rate.
    /// </summary>
    public void SetWindowlessFrameRate(int frameRate)
    {
        BrowserHost?.SetWindowlessFrameRate(frameRate);
    }

    /// <summary>
    /// Begins a new composition or updates the existing composition. Blink has a
    /// special node (a composition node) that allows the input method to change
    /// text without affecting other DOM nodes. |text| is the optional text that
    /// will be inserted into the composition node. |underlines| is an optional set
    /// of ranges that will be underlined in the resulting text.
    /// |replacement_range| is an optional range of the existing text that will be
    /// replaced. |selection_range| is an optional range of the resulting text that
    /// will be selected after insertion or replacement. The |replacement_range|
    /// value is only used on OS X.
    /// This method may be called multiple times as the composition changes. When
    /// the client is done making changes the composition should either be canceled
    /// or completed. To cancel the composition call ImeCancelComposition. To
    /// complete the composition call either ImeCommitText or
    /// ImeFinishComposingText. Completion is usually signaled when:
    /// A. The client receives a WM_IME_COMPOSITION message with a GCS_RESULTSTR
    /// flag (on Windows), or;
    /// B. The client receives a "commit" signal of GtkIMContext (on Linux), or;
    /// C. insertText of NSTextInput is called (on Mac).
    /// This method is only used when window rendering is disabled.
    /// </summary>
    public void ImeSetComposition(string text,
        int underlinesCount,
        CefCompositionUnderline underlines,
        CefRange replacementRange,
        CefRange selectionRange)
    {
        BrowserHost?.ImeSetComposition(text, underlinesCount, underlines, replacementRange, selectionRange);
    }

    /// <summary>
    /// Completes the existing composition by optionally inserting the specified
    /// |text| into the composition node. |replacement_range| is an optional range
    /// of the existing text that will be replaced. |relative_cursor_pos| is where
    /// the cursor will be positioned relative to the current cursor position. See
    /// comments on ImeSetComposition for usage. The |replacement_range| and
    /// |relative_cursor_pos| values are only used on OS X.
    /// This method is only used when window rendering is disabled.
    /// </summary>
    public void ImeCommitText(string text, CefRange replacementRange, int relativeCursorPos)
    {
        BrowserHost?.ImeCommitText(text, replacementRange, relativeCursorPos);
    }

    /// <summary>
    /// Completes the existing composition by applying the current composition node
    /// contents. If |keep_selection| is false the current selection, if any, will
    /// be discarded. See comments on ImeSetComposition for usage.
    /// This method is only used when window rendering is disabled.
    /// </summary>
    public void ImeFinishComposingText(bool keepSelection)
    {
        BrowserHost?.ImeFinishComposingText(keepSelection);
    }

    /// <summary>
    /// Cancels the existing composition and discards the composition node
    /// contents without applying them. See comments on ImeSetComposition for
    /// usage.
    /// This method is only used when window rendering is disabled.
    /// </summary>
    public void ImeCancelComposition()
    {
        BrowserHost?.ImeCancelComposition();
    }

    /// <summary>
    /// Call this method when the user drags the mouse into the web view (before
    /// calling DragTargetDragOver/DragTargetLeave/DragTargetDrop).
    /// |drag_data| should not contain file contents as this type of data is not
    /// allowed to be dragged into the web view. File contents can be removed using
    /// CefDragData::ResetFileContents (for example, if |drag_data| comes from
    /// CefRenderHandler::StartDragging).
    /// This method is only used when window rendering is disabled.
    /// </summary>
    public void DragTargetDragEnter(CefDragData dragData, CefMouseEvent mouseEvent, CefDragOperationsMask allowedOps)
    {
        BrowserHost?.DragTargetDragEnter(dragData, mouseEvent, allowedOps);
    }

    /// <summary>
    /// Call this method each time the mouse is moved across the web view during
    /// a drag operation (after calling DragTargetDragEnter and before calling
    /// DragTargetDragLeave/DragTargetDrop).
    /// This method is only used when window rendering is disabled.
    /// </summary>
    public void DragTargetDragOver(CefMouseEvent mouseEvent, CefDragOperationsMask allowedOps)
    {
        BrowserHost?.DragTargetDragOver(mouseEvent, allowedOps);
    }

    /// <summary>
    /// Call this method when the user drags the mouse out of the web view (after
    /// calling DragTargetDragEnter).
    /// This method is only used when window rendering is disabled.
    /// </summary>
    public void DragTargetDragLeave()
    {
        BrowserHost?.DragTargetDragLeave();
    }

    /// <summary>
    /// Call this method when the user completes the drag operation by dropping
    /// the object onto the web view (after calling DragTargetDragEnter).
    /// The object being dropped is |drag_data|, given as an argument to
    /// the previous DragTargetDragEnter call.
    /// This method is only used when window rendering is disabled.
    /// </summary>
    public void DragTargetDrop(CefMouseEvent mouseEvent)
    {
        BrowserHost?.DragTargetDrop(mouseEvent);
    }

    /// <summary>
    /// Call this method when the drag operation started by a
    /// CefRenderHandler::StartDragging call has ended either in a drop or
    /// by being cancelled. |x| and |y| are mouse coordinates relative to the
    /// upper-left corner of the view. If the web view is both the drag source
    /// and the drag target then all DragTarget* methods should be called before
    /// DragSource* mthods.
    /// This method is only used when window rendering is disabled.
    /// </summary>
    public void DragSourceEndedAt(int x, int y, CefDragOperationsMask op)
    {
        BrowserHost?.DragSourceEndedAt(x, y, op);
    }

    /// <summary>
    /// Call this method when the drag operation started by a
    /// CefRenderHandler::StartDragging call has completed. This method may be
    /// called immediately without first calling DragSourceEndedAt to cancel a
    /// drag operation. If the web view is both the drag source and the drag
    /// target then all DragTarget* methods should be called before DragSource*
    /// mthods.
    /// This method is only used when window rendering is disabled.
    /// </summary>
    public void DragSourceSystemDragEnded()
    {
        BrowserHost?.DragSourceSystemDragEnded();
    }

    /// <summary>
    /// Returns the current visible navigation entry for this browser. This method
    /// can only be called on the UI thread.
    /// </summary>
    public CefNavigationEntry GetVisibleNavigationEntry()
    {
        return BrowserHost?.GetVisibleNavigationEntry();
    }

    /// <summary>
    /// Set accessibility state for all frames. |accessibility_state| may be
    /// default, enabled or disabled. If |accessibility_state| is STATE_DEFAULT
    /// then accessibility will be disabled by default and the state may be further
    /// controlled with the "force-renderer-accessibility" and
    /// "disable-renderer-accessibility" command-line switches. If
    /// |accessibility_state| is STATE_ENABLED then accessibility will be enabled.
    /// If |accessibility_state| is STATE_DISABLED then accessibility will be
    /// completely disabled.
    /// For windowed browsers accessibility will be enabled in Complete mode (which
    /// corresponds to kAccessibilityModeComplete in Chromium). In this mode all
    /// platform accessibility objects will be created and managed by Chromium's
    /// internal implementation. The client needs only to detect the screen reader
    /// and call this method appropriately. For example, on macOS the client can
    /// handle the @"AXEnhancedUserInterface" accessibility attribute to detect
    /// VoiceOver state changes and on Windows the client can handle WM_GETOBJECT
    /// with OBJID_CLIENT to detect accessibility readers.
    /// For windowless browsers accessibility will be enabled in TreeOnly mode
    /// (which corresponds to kAccessibilityModeWebContentsOnly in Chromium). In
    /// this mode renderer accessibility is enabled, the full tree is computed, and
    /// events are passed to CefAccessibiltyHandler, but platform accessibility
    /// objects are not created. The client may implement platform accessibility
    /// objects using CefAccessibiltyHandler callbacks if desired.
    /// </summary>
    public void SetAccessibilityState(CefState accessibilityState)
    {
        BrowserHost?.SetAccessibilityState(accessibilityState);
    }

    /// <summary>
    /// Enable notifications of auto resize via CefDisplayHandler::OnAutoResize.
    /// Notifications are disabled by default. |min_size| and |max_size| define the
    /// range of allowed sizes.
    /// </summary>
    public void SetAutoResizeEnabled(bool enabled, CefSize minSize, CefSize maxSize)
    {
        BrowserHost?.SetAutoResizeEnabled(enabled, minSize, maxSize);
    }

    /// <summary>
    /// Returns the extension hosted in this browser or NULL if no extension is
    /// hosted. See CefRequestContext::LoadExtension for details.
    /// </summary>
    public CefExtension GetExtension()
    {
        return BrowserHost?.GetExtension();
    }

    /// <summary>
    /// Returns true if this browser is hosting an extension background script.
    /// Background hosts do not have a window and are not displayable. See
    /// CefRequestContext::LoadExtension for details.
    /// </summary>
    public bool IsBackgroundHost
    {
        get
        {
            return BrowserHost is not null && BrowserHost.IsBackgroundHost;
        }
    }

    /// <summary>
    /// Set whether the browser's audio is muted.
    /// </summary>
    public void SetAudioMuted(bool value)
    {
        BrowserHost?.SetAudioMuted(value);
    }

    /// <summary>
    /// Returns true if the browser's audio is muted.  This method can only be
    /// called on the UI thread.
    /// </summary>
    public bool IsAudioMuted
    {
        get
        {
            return BrowserHost is not null && BrowserHost.IsAudioMuted;
        }
    }
}