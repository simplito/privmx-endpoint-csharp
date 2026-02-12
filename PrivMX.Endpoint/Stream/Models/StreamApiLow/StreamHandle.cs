namespace PrivMX.Endpoint.Stream.Models.StreamApiLow
{
    public class StreamHandle
    {
        /// <summary>
        /// Stream handle
        /// </summary>
        private long value;

        /// <summary>
        /// StreamHandle constructor
        /// </summary>
        /// <param name="value">Value</param>
        public StreamHandle(long value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets stream handls
        /// </summary>
        /// <returns>Value of stream handle</returns>
        public long GetValue()
        {
            return value;
        }
    }
}