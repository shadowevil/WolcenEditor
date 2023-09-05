using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolcenEditor
{
    public class ItemInfo
    {
        // Common properties
        public string Name { get; set; } = "";
        public int DropWeight { get; set; }
        public int Level { get; set; }
        public string Keywords { get; set; } = "";
        public int Rarity { get; set; }
        public int X_GridSize { get; set; }
        public int Y_GridSize { get; set; }

        // Weapon-specific properties
        public string BodyPart { get; set; } = "";
        public string ImplicitAffixes { get; set; } = "";
        public int HighDamage_Max { get; set; }
        public int HighDamage_Min { get; set; }
        public bool IsTwoHanded { get; set; }
        public int LowDamage_Max { get; set; }
        public int LowDamage_Min { get; set; }
        public double Range { get; set; }
        public int SpeedFactorBonus { get; set; }
        public string UIName { get; set; } = "";
        public string WeaponProperties { get; set; } = "";
        public int MinSocketOverride { get; set; }
        public int MaxSocketOverride { get; set; }
        public int ResourceGain_Min { get; set; }
        public int ResourceGain_Max { get; set; }
        public string GameplayVersionBaseStats { get; set; } = "";
        public string Version { get; set; } = "";

        // Enneract-specific properties
        public string EnneractPresetID { get; set; } = "";
        public int GoldValue { get; set; }
        public int LevelPrereq { get; set; }
        public int MinDropLevel { get; set; }
        public int MaxDropLevel { get; set; }
        public int SkillLevelMax { get; set; }
        public int SkillLevelMin { get; set; }

        // Armor-specific properties
        public string ArmorType { get; set; } = "";
        public int Defense { get; set; }
        public string ResistanceType { get; set; } = "";
        public int ResistanceValue { get; set; }

        // Potion-specific properties
        public int RecoveryAmount { get; set; }
        public int Duration { get; set; }
        public string PotionType { get; set; } = ""; // Health, Rage, or Umbra
    }
}
