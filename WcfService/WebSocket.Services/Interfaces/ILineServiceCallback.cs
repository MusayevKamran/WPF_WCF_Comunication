using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Services.Contracts;

namespace WebSocket.Services.Interfaces
{
    public interface ILineServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnLineSend(Line line);
    }
}
