using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfClient.Properties;
using WpfClient.WcfService;

namespace WpfClient
{
    public class ClientService : ILineServiceCallback
    {
        private LineServiceClient _lineServiceClient;
        private InstanceContext _instanceContext;
        private MainWindow _window;

        public ClientService(MainWindow window)
        {
            _window = window;
        }
        public InstanceContext CallbackInstance
        {
            get { return _instanceContext ?? (_instanceContext = new InstanceContext(this)); }
        }

        public LineServiceClient LineServiceClient
        {
            get
            {
                return _lineServiceClient ??
                       (_lineServiceClient = new LineServiceClient(CallbackInstance, "netHttpBinding_ILineService"));
            }

        }


        public void SubscribeForLine()
        {
            LineServiceClient.SubscribeForLine();
        }

        public void UnsubscribeForLine()
        {
            LineServiceClient.UnsubscribeForLine();
        }

        public void OnLineSend(Line line)
        {
            var currentLine = Settings.Default.LineId;
            if (currentLine == line.LINE.ToString())
            {
                _window.DataContext = line;
            }
        }

        public short[] GetLinesId()
        {
            return LineServiceClient.GetLinesId();
        }

        public Line GetSelectedLine(string line)
        {
            return LineServiceClient.GetSelectedLine(line);
        }

    }
}
