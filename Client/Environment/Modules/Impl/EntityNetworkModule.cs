using CitizenFX.Core;
using CitizenFX.Core.Native;
using Hive.Client.Environment.Entities;
using System.Threading.Tasks;

namespace Hive.Client.Environment.Entities.Modules.Impl
{
    public class EntityNetworkModule : EntityModule
    {
        protected override void Begin(HiveEntity entity, int id)
        {
            if (!API.NetworkGetEntityIsNetworked(id)) API.NetworkRegisterEntityAsNetworked(id);
        }

        public bool IsClaimed()
        {
            return API.NetworkHasControlOfEntity(Id);
        }

        public async Task Claim()
        {
            API.NetworkRequestControlOfEntity(Id);

            while (!IsClaimed())
            {
                await BaseScript.Delay(100);
            }
        }

        public int GetId()
        {
            return API.NetworkGetNetworkIdFromEntity(Id);
        }

    }
}