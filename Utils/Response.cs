using System;
using System.Collections.Generic;

namespace DreamingHome.Utils
{
    public class Response
    {
        public Response()
        {
        }

        public Response(bool success, string message, object data)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
        }

        public string Message { get; set; }
        public object Data { get; set; }
        public bool Success { get; set; }
    }
}