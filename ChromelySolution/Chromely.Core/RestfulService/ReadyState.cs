using System;
using System.Collections.Generic;
using System.Text;

namespace Chromely.Core.RestfulService
{
    public enum ReadyState
    {
        // Request not initialized 
        NotInitialized,

        //  Server connection established
        ServerConnectionEstablished,

        // Request received 
        RequestReceived,

        // Processing request
        ProcessingRequest,

        // Request finished and response is ready
        ResponseIsReady
    }
}
