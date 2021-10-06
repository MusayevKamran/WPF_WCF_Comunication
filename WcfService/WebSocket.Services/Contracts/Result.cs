using System.Runtime.Serialization;

namespace WebSocket.Services.Contracts
{
    [DataContract]
    public class Result
    {

        [DataMember]
        public char ResultId { get; set; }
        [DataMember]
        public string ResultText { get; set; }
    }
}
