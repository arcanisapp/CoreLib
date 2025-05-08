﻿using SQLite;

namespace CoreLib.Models.Entitys
{
    [Table("Devices")]
    internal class Device
    {
        /// <summary>
        /// Shake256 (SPK)
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        [NotNull]
        public string AccountId { get; set; }

        [NotNull]
        public string DeviceName { get; set; }

        [NotNull]
        public string SPK { get; set; }

        [NotNull]
        public string SPrK { get; set; }

        /// <summary>
        /// Signature of the SPK
        /// </summary>
        [NotNull]
        public string SPKSignature { get; set; } 
    }
}
