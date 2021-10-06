using System.Collections.Generic;
using System.ServiceModel;
using WebSocket.Services.Contracts;

namespace WebSocket.Services.Interfaces
{
    [ServiceContract(CallbackContract = typeof(ILineServiceCallback))]
    public interface ILineService
    {
        [OperationContract]
        void SendLine(Line line);

        [OperationContract]
        void SubscribeForLine();

        [OperationContract]
        void UnsubscribeForLine();

        [OperationContract]
        List<short> GetLinesId();

        [OperationContract]
        Line GetSelectedLine(string line);
    }
}
