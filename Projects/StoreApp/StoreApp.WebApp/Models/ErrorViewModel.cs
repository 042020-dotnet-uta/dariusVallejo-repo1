using System;

namespace StoreApp.WebApp.Models
{
    /// <summary>
    /// Standard builtin error view model
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
