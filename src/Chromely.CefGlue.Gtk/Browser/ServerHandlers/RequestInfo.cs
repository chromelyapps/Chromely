// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestInfo.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
// See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.Browser.ServerHandlers
{
    using LitJson;

    /// <summary>
    /// The request info.
    /// </summary>
    public class RequestInfo
    {
        /// <summary>
        /// The m message type.
        /// </summary>
        private readonly MessageType mMessageType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestInfo"/> class.
        /// </summary>
        /// <param name="requestString">
        /// The request string.
        /// </param>
        public RequestInfo(string requestString)
        {
            requestString = string.IsNullOrEmpty(requestString) 
                                ? string.Empty 
                                : requestString;

            this.mMessageType = MessageType.None;
            var jsonData = JsonMapper.ToObject<JsonData>(requestString);

            this.TargetName = jsonData.Keys.Contains("targetname") ? jsonData["targetname"].ToString() : string.Empty;
            this.Url = jsonData.Keys.Contains("url") ? jsonData["url"].ToString() : string.Empty;
            this.Data = jsonData.Keys.Contains("data") ? jsonData["data"].ToString() : string.Empty;
            string broadcastStr = jsonData.Keys.Contains("broadcast") ? jsonData["broadcast"].ToString() : string.Empty;

            if (string.IsNullOrEmpty(this.TargetName) 
                && string.IsNullOrEmpty(this.Url)
                && string.IsNullOrEmpty(this.Data)
                && string.IsNullOrEmpty(broadcastStr))
            {
                this.Data = requestString;
                this.mMessageType = MessageType.Echo;
            }

            bool.TryParse(broadcastStr, out var broadcast);
            this.Broadcast = broadcast;
            this.mMessageType = this.Broadcast 
                                  ? MessageType.Broadcast
                                  : this.mMessageType;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public MessageType Type
        {
            get
            {
                if (this.mMessageType != MessageType.None)
                {
                    return this.mMessageType;
                }

                if (!string.IsNullOrEmpty(this.Url))
                {
                    return MessageType.ControllerAction;
                }

                if (!string.IsNullOrEmpty(this.TargetName))
                {
                    return MessageType.TargetRecepient;
                }

                return this.Broadcast 
                           ? MessageType.Broadcast 
                           : MessageType.Echo;
            }
        }

        /// <summary>
        /// Gets the target connection id.
        /// </summary>
        public int TargetConnectionId => ConnectionNameMapper.GetConnectionId(this.TargetName);

        /// <summary>
        /// Gets the target name.
        /// </summary>
        public string TargetName { get; }

        /// <summary>
        /// Gets the url.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Gets a value indicating whether broadcast.
        /// </summary>
        public bool Broadcast { get; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public string Data { get; }
    }
}
