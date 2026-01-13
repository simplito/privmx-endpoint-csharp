namespace PrivMX.Endpoint.Core.Models
{
    /// <summary>
    /// Bridge server identification details.
    /// </summary>
    public class BridgeIdentity
    {
        /// <summary>
        /// Bridge URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Bridge public Key.
        /// </summary>
        public string? PubKey  { get; set; }
        
        /// <summary>
        /// Bridge instance Id given by PKI.
        /// </summary>
        public string? InstanceId { get; set; }
    }
}