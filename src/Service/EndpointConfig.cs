
using System.Collections.Generic;
using System.Diagnostics;
using Contracts;
using Data;
using NPoco;

namespace Service
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<InMemoryPersistence>();
            //configuration.Transactions().DisableDistributedTransactions();
            //configuration.Transactions().DoNotWrapHandlersExecutionInATransactionScope();
            //configuration.Transactions().Disable();
        }
    }

    public class Startup : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            
            using (var db = new Database("connstr"))
            {
                var pocos = new List<Widget>();
                db.Execute("DELETE FROM Widgets");

                for (var o = 0; o < 100; o++)
                {
                    for (var i = 0; i < 10000; i++)
                    {
                        pocos.Add(new Widget() { Id = KeyGen.NewKey });
                    }
                        
                    db.InsertBulk(pocos);
                    pocos.Clear();
                }
            }

            Timer = new Stopwatch();
            Timer.Start();

            for (var i = 0; i < 10000; i++)
                Bus.SendLocal(new BuildWidgetCommand());

            Bus.SendLocal(new BuildWidgetCommand() { Tracer = true });
        }

        public static Stopwatch Timer { get; set; }

        public void Stop()
        {
            
        }
    }
}
