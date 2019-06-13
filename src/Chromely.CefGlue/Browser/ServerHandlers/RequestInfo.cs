// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestInfo.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using LitJson;

namespace Chromely.CefGlue.Browser.ServerHandlers
{
    /// <summary>
    /// The request info.
    /// </summary>
    public class RequestInfo
    {
        /// <summary>
        /// The message type.
        /// </summary>
        private readonly MessageType _messageType;

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

            _messageType = MessageType.None;
            var jsonData = JsonMapper.ToObject<JsonData>(requestString);

            TargetName = jsonData.Keys.Contains("targetname") ? jsonData["targetname"].ToString() : string.Empty;
            Url = jsonData.Keys.Contains("url") ? jsonData["url"].ToString() : string.Empty;
            Data = jsonData.Keys.Contains("data") ? jsonData["data"].ToString() : string.Empty;
            string broadcastStr = jsonData.Keys.Contains("broadcast") ? jsonData["broadcast"].ToString() : string.Empty;

            if (string.IsNullOrEmpty(TargetName) 
                && string.IsNullOrEmpty(Url)
                && string.IsNullOrEmpty(Data)
                && string.IsNullOrEmpty(broadcastStr))
            {
                Data = requestString;
                _messageType = MessageType.Echo;
            }

            bool.TryParse(broadcastStr, out var broadcast);
            Broadcast = broadcast;
            _messageType = Broadcast 
                                  ? MessageType.Broadcast
                                  : _messageType;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public MessageType Type
        {
            get
            {
                if (_messageType != MessageType.None)
                {
                    return _messageType;
                }

                if (!string.IsNullOrEmpty(Url))
                {
                    return MessageType.ControllerAction;
                }

                if (!string.IsNullOrEmpty(TargetName))
                {
                    return MessageType.TargetRecepient;
                }

                return Broadcast 
                           ? MessageType.Broadcast 
                           : MessageType.Echo;
            }
        }

        /// <summary>
        /// Gets the target connection id.
        /// </summary>
        public int TargetConnectionId => ConnectionNameMapper.GetConnectionId(TargetName);

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
