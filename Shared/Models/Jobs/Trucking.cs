using System.ComponentModel.DataAnnotations;

namespace Hive.Library.Models.Jobs
{
    public class JobTruckingMissions
    {
        [Key]
        public int id { get; set; }
        public int difficulty { get; set; }
        public string MissionName { get; set; }
        public string MissionDescription { get; set; }
        public string StartPosX { get; set; }
        public string StartPosY { get; set; }
        public string StartPosZ { get; set; }
        public string EndPosX { get; set; }
        public string EndPosY { get; set; }
        public string EndPosZ { get; set; }
        public long Reward { get; set; }
    }
    public class JobTruckingMissionsLog
    {
        [Key]
        public int id { get; set; }
        public string MissionID { get; set; }
        public string PlayerPosX { get; set; }
        public string PlayerPosY { get; set; }
        public string PlayerPosZ { get; set; }
        public long Reward { get; set; }
        public string Occurance { get; set; }
    }
}