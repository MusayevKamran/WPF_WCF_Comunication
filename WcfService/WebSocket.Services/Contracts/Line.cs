using System.Runtime.Serialization;

namespace WebSocket.Services.Contracts
{
    [DataContract]
    public class Line
    {
        [DataMember]
        public string MATNR { get; set; }
        [DataMember]
        public string MAKTX { get; set; }
        [DataMember]
        public short LINE { get; set; }
    }
}
