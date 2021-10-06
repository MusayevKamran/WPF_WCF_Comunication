using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WpfClient.WcfService;

namespace WpfClient
{
    public class LineServiceClient : DuplexClientBase<ILineService>, ILineService
    {
        public LineServiceClient(InstanceContext callbackInstance) :
            base(callbackInstance)
        {
        }

        public LineServiceClient(InstanceContext callbackInstance, string endpointConfigurationName) :
            base(callbackInstance, endpointConfigurationName)
        {
        }

        public void SendLine(Line line)
        {
            Channel.SendLine(line);
        }

        public void SubscribeForLine()
        {
            Channel.SubscribeForLine();
        }

        public void UnsubscribeForLine()
        {
            Channel.UnsubscribeForLine();
        }

        public short[] GetLinesId()
        {
            return Channel.GetLinesId();
        }

        public Line GetSelectedLine(string line)
        {
            return Channel.GetSelectedLine(line);
        }

        public Task SendLineAsync(Line line)
        {
            throw new NotImplementedException();
        }

        public Task SubscribeForLineAsync()
        {
            throw new NotImplementedException();
        }

        public Task UnsubscribeForLineAsync()
        {
            throw new NotImplementedException();
        }

        public Task<short[]> GetLinesIdAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Line> GetSelectedLineAsync(string line)
        {
            throw new NotImplementedException();
        }
    }
}
