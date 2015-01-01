using Contracts;
using Data;
using NPoco;
using NServiceBus;
using NServiceBus.Logging;

namespace Service
{
    public class BuildWidgetHandler : IHandleMessages<BuildWidgetCommand>
    {
        public void Handle(BuildWidgetCommand message)
        {
            using (var db = new Database("connstr"))
            {
                var widget = new Widget() { Id = KeyGen.NewKey };
                db.Insert("Widgets", "id", false, widget);
            }

            if (!message.Tracer) return;

            Startup.Timer.Stop();
            LogManager.GetLogger(GetType()).InfoFormat("Elapsed seconds: {0}", Startup.Timer.Elapsed.TotalSeconds);
        }
    }
}