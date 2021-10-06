using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.ServiceModel;
using TableDependency.EventArgs;
using TableDependency.SqlClient;
using WebSocket.Services.Interfaces;
using WebSocket.Services.Contracts;

namespace WebSocket.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
     [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class LineService : ILineService
    {
        private readonly SqlTableDependency<Line> _sqlTableDependency;
        private readonly List<ILineServiceCallback> _callbackList = new List<ILineServiceCallback>();
        private readonly string _connectionString;

        public LineService()
        {
             _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            _sqlTableDependency = new SqlTableDependency<Line>(_connectionString, "Lines");

            _sqlTableDependency.OnChanged += TableDependency_Changed;
            _sqlTableDependency.OnError += (sender, args) => Console.WriteLine($"Error: {args.Message}");
            _sqlTableDependency.Start();

            Console.WriteLine(@"Waiting for receiving notifications...");
        }

        private void TableDependency_Changed(object sender, RecordChangedEventArgs<Line> e)
        {
            this.SendLine(e.Entity);
        }

        private ILineServiceCallback Subscriber
        {
            get
            {
                if (OperationContext.Current != null)
                {
                    return OperationContext.Current.GetCallbackChannel<ILineServiceCallback>();
                }

                return Subscriber;
            }
        }

        public void SendLine(Line line)
        {
            LineManager.Instance.SendLine(line);
        }

        public void SubscribeForLine()
        {
            LineManager.Instance.AddSubscriber(Subscriber);
        }

        public void UnsubscribeForLine()
        {
            LineManager.Instance.RemoveSubscriber(Subscriber);
        }

        public List<short> GetLinesId()
        {
            List<short> returnModel = new List<short>();
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = $"SELECT LINE FROM [Lines]";

                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            var LINE = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("LINE"));

                            returnModel.Add(LINE);
                        }
                    }
                }
            }
            return returnModel;
        }

        public Line GetSelectedLine(string line)
        {
            Line returnModel = null;
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = $"SELECT * FROM [Lines] where LINE = {line}";

                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            var indexMATNR = sqlDataReader.GetOrdinal("MATNR");
                            var indexMAKTX = sqlDataReader.GetOrdinal("MAKTX");
                            var indexLINE = sqlDataReader.GetOrdinal("LINE");

                            var MATNR = "";
                            if (!sqlDataReader.IsDBNull(indexMATNR))
                                MATNR = sqlDataReader.GetString(indexMATNR);

                            var MAKTX = "";
                            if (!sqlDataReader.IsDBNull(indexMAKTX))
                                MAKTX = sqlDataReader.GetString(indexMAKTX);

                            short LINE = 0;
                            if (!sqlDataReader.IsDBNull(indexLINE))
                                LINE = sqlDataReader.GetInt16(indexLINE);

                            returnModel = new Line
                            {
                                MATNR = MATNR,
                                MAKTX = MAKTX,
                                LINE = LINE
                            };
                        }
                    }
                }
            }
            return returnModel;
        }
    }
}
