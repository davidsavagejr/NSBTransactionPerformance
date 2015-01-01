
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Contracts;
using Data;
using NPoco;
using NServiceBus.Config;
using NServiceBus.Persistence;

namespace Service
{
    using NServiceBus;

    /*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<RavenDBPersistence>();
            configuration.Transactions().DisableDistributedTransactions();
            configuration.Transactions().DoNotWrapHandlersExecutionInATransactionScope();

            //configuration.Transactions().Disable();
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
