using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Server
{
    public class Server : BaseScript
    {
        public Server()
        {
            ResourceName = API.GetCurrentResourceName();

            Debug.WriteLine("============================================================");
            Log($"Instanciating...");

            EventHandlers["playerConnecting"] += new Action<Player>(onPlayerConnecting);
            EventHandlers["playerDropped"] += new Action<Player, string>(onPlayerDropped);
            EventHandlers[$"internal:{Name}:onPlayerClientInitialized"] += new Action<Player>(onPlayerClientInitialized);

            Tick += garbageCollect;
            Log($"Instanced!");
            Debug.WriteLine("============================================================");
        } 

        public string ResourceName { get; private set; }

        public string Name { get { return ResourceName; } }

        private void onPlayerConnecting([FromSource] Player player)
        {

        }

        private void onPlayerDropped([FromSource] Player player, string reason)
        {

        }

        private void onPlayerClientInitialized([FromSource] Player player)
        {

        }
         
        private async Task garbageCollect()
        {
            await Delay(60000);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public PlayerList GetPlayers()
        {
            return Players;
        }

        public void AddEvent(string eventName, Delegate action)
        {
            EventHandlers[eventName] += action;
            Log($"Event {eventName} added!");
        }

        public void RemoveEvent(string eventName, Delegate action)
        {
            EventHandlers[eventName] -= action;
            Log($"Event {eventName} removed!");
        }

        public void AddTick(Func<Task> action)
        {
            Tick += action;
            Log($"Added Tick {action.Method.Name}!");
        }

        public void RemoveTick(Func<Task> action)
        {
            Tick -= action;
            Log($"Removed Tick {action.Method.Name}!");
        }

        public ExportDictionary GetExports()
        {
            return Exports;
        }

        public dynamic GetExport(string resourceName)
        {
            return Exports[resourceName];
        }

        public void SetExport(string name, Delegate method)
        {
            Exports.Add(name, method);
        }

        public void Log(string message)
        {
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} [INFO] Hive-Yacht: {message}");
        }

        public void LogError(Exception exception, string prefix = "")
        {
            string pre = (prefix != "") ? prefix : "";
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} [ERROR] Hive-Yacht: {exception.Message}\n{exception.StackTrace}");
        }
         
    }
}
