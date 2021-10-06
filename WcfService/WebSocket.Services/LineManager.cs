using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using WebSocket.Services.Interfaces;
using WebSocket.Services.Contracts;

namespace WebSocket.Services
{
    public class LineManager
    {
        private static volatile LineManager _lineManager = null;
        private static readonly object SyncLock = new object();
        private LineManager()
        {
            Subscribers = new List<ILineServiceCallback>();
            Lines = new List<Line>();
        }

        public List<ILineServiceCallback> Subscribers { get; private set; }

        public List<Line> Lines { get; private set; }

        public static LineManager Instance
        {
            get
            {
                lock (SyncLock)
                {
                    if (_lineManager == null)
                    {
                        lock (SyncLock)
                        {
                            _lineManager = new LineManager();
                        }
                    }
                }
                return _lineManager;
            }
        }

        public void AddSubscriber(ILineServiceCallback subscriber)
        {
            if (!Subscribers.Contains(subscriber))
            {
                Subscribers.Add(subscriber);
            }
        }

        public void RemoveSubscriber(ILineServiceCallback subscriber)
        {
            if (Subscribers.Contains(subscriber))
            {
                Subscribers.Remove(subscriber);
            }
        }

        public void SendLine(Line line)
        {
            Subscribers.ForEach(delegate (ILineServiceCallback callback)
            {
                if (((ICommunicationObject) callback).State == CommunicationState.Opened)
                {
                    callback.OnLineSend(line);
                }
            });
        }
    }
}