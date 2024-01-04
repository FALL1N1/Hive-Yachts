using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Deps;
using Client.Managers;
using Hive.Client.Environment.Entities;

namespace Client
{
    public abstract class Client : BaseScript
    {
        public static Client Instance { get; private set; }
        public Logger log = new Logger();
        public Client()
        {
            ResourceName = API.GetCurrentResourceName();
            Instance = this;

            log.outInfo("============================================================");
            log.outInfo($"Initializing..."); 
            Tick += initialize;
            Tick += garbageCollect;
            log.outInfo($"Initialized!");
            log.outInfo("============================================================");

        } 

        public static string ResourceName { get; private set; }
        public string Name { get { return ResourceName; } }
        protected bool UseGarbageCollector { get; set; } = false;

        protected bool UsePlayerDeadResurectWatcher
        {
            get
            {
                return _usePlayerDeadResurectWatcher;
            }
            set
            {
                if (value == false)
                {
                    deadPlayers.Clear();
                }
                _usePlayerDeadResurectWatcher = value;
            }
        }
        private bool _usePlayerDeadResurectWatcher = false;
        private readonly List<Player> deadPlayers;

        private async Task initialize()
        {
            var ped = Game.PlayerPed.Handle;
            Tick -= initialize; 

            await Delay(0);

            log.outInfo($"Initialized!");
            TriggerServerEvent($"internal:{Name}:onPlayerClientInitialized");
            HiveEntity PlayerPed = new HiveEntity(ped);

            var yachtMgr = new YachtManager();
            yachtMgr.InitializeYachts();

        }
          

        private async Task garbageCollect()
        {
            if (!UseGarbageCollector)
            {
                await Delay(1);
                return;
            }

            await Delay(60000);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public PlayerList GetPlayers()
        {
            return Players;
        }

        protected void AddEvent(string eventName, Delegate action)
        {
            EventHandlers[eventName] += action;
            log.outInfo($"Event {eventName} added!");
        }

        protected void RemoveEvent(string eventName, Delegate action)
        {
            EventHandlers[eventName] -= action;
            log.outInfo($"Event {eventName} removed!");
        }

        public void AddTick(Func<Task> action)
        {
            Tick += action;
            log.outInfo($"Added Tick {action.Method.Name}!");
        }

        public void RemoveTick(Func<Task> action)
        {
            Tick -= action;
            log.outInfo($"Removed Tick {action.Method.Name}!");
        }

        protected ExportDictionary GetExports()
        {
            return Exports;
        }

        protected dynamic GetExport(string resourceName)
        {
            return Exports[resourceName];
        }

        protected void SetExport(string name, Delegate method)
        {
            Exports.Add(name, method);
        }
    }
}
