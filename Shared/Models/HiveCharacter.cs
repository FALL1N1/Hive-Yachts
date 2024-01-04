using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hive.Library.Models
{
    public class HiveCharacter
    {
        [Key] public string Seed { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fullname => $"{Name} {Surname}";
        public string DateOfBirth { get; set; }
        public int LastDigits { get; set; }
        public int Health { get; set; }
        public int Shield { get; set; }
        public double Hunger { get; set; }
        public double Thirst { get; set; }
        public double DepressionStress { get; set; }
        public long Cash { get; set; }
        public long Experience { get; set; }
        public long Level { get; set; }
        public double RoleplayReputation { get; set; }
        public Style Style { get; set; }
        public BankAccount BankAccount { get; set; }
        public bool MarkedAsRegistered { get; set; }
        public CharacterMetadata Metadata { get; set; }

        [Column("Style")]
        public string _Style
        {
            get => JsonConvert.SerializeObject(Style);
            set => Style = JsonConvert.DeserializeObject<Style>(value);
        }

        [Column("BankAccount")]
        public string _BankAccount
        {
            get => JsonConvert.SerializeObject(BankAccount);
            set => BankAccount = JsonConvert.DeserializeObject<BankAccount>(value);
        }

        [Column("Metadata")]
        public string _Metadata
        {
            get => JsonConvert.SerializeObject(Metadata);
            set => Metadata = JsonConvert.DeserializeObject<CharacterMetadata>(value);
        }
    }

    public class CharacterMetadata
    {
        public Employment Employment { get; set; }
        public int EmploymentRole { get; set; }
        public double RoleplayReputation { get; set; }
        public Position LastPosition { get; set; }
        public Tuple<int, int> EquippedMask { get; set; }
        public Dictionary<string, Style> SavedOutfits { get; set; }
    }

    public class BankAccount
    {
        public double Balance { get; set; }
        public List<BankTransaction> History { get; set; }
    }

    public class BankTransaction
    {
        public BankTransactionType Type { get; set; }
        public long Amount { get; set; }
        [JsonIgnore] public DateTime Date { get; set; }
        public string Information { get; set; }

        [JsonProperty("Date")] public string _Date => Date.ToString("MM/dd/yyyy HH:mm:ss");
    }

    public enum BankTransactionType
    {
        Withdraw,
        Deposit
    }

    public enum Employment
    {
        Unemployed,
        Police,
        Ambulance,
        Taxi,
        Trucker,
        Farmer
    }

    public class EmploymentRoles
    {
        public enum Police
        {
            Chief = 4,
            Superintendent = 3,
            Inspector = 2,
            Assistant = 1,
            Trainee = 0
        }

        public enum Ambulance
        {
            Chief = 3,
            Consultant = 2,
            Medic = 1,
            Nurse = 0
        }

        public enum Taxi
        {
            CEO = 3,
            COO = 2,
            Employee = 1,
            Probationary = 0
        }
        public enum Trucker
        {
            Senior = 2,
            Middle = 1,
            Rookie = 0
        }
        public enum Farmer
        {
            Senior = 2,
            Middle = 1,
            Rookie = 0
        }
    }
}