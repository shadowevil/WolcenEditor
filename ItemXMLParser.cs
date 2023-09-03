using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WolcenData.Telemetry;

namespace WolcenEditor
{
    public class ItemXmlParser
    {
        public static ItemInfo? GetItemByName(string xmlFilePath, string itemName)
        {
            XDocument xdoc = XDocument.Load(xmlFilePath);
            var itemElement = xdoc.Descendants("Item").FirstOrDefault(e => e.Attribute("Name")?.Value == itemName);

            if (itemElement == null)
            {
                return null;
            }

            ItemInfo item = new ItemInfo
            {
                // Common properties
                Name = itemElement.Attribute("Name")?.Value ?? "",
                DropWeight = int.TryParse(itemElement.Attribute("DropWeight")?.Value, out int dropWeight) ? dropWeight : 0,
                Level = int.TryParse(itemElement.Attribute("Level")?.Value, out int level) ? level : 0,
                Keywords = itemElement.Attribute("Keywords")?.Value ?? "",
                Rarity = int.TryParse(itemElement.Attribute("Rarity")?.Value, out int rarity) ? rarity : 0,
                X_GridSize = int.TryParse(itemElement.Attribute("X_GridSize")?.Value, out int xGridSize) ? xGridSize : 0,
                Y_GridSize = int.TryParse(itemElement.Attribute("Y_GridSize")?.Value, out int yGridSize) ? yGridSize : 0,

                // Weapon-specific properties
                BodyPart = itemElement.Attribute("BodyPart")?.Value ?? "",
                ImplicitAffixes = itemElement.Attribute("ImplicitAffixes")?.Value ?? "",
                HighDamage_Max = int.TryParse(itemElement.Attribute("HighDamage_Max")?.Value, out int highDamageMax) ? highDamageMax : 0,
                HighDamage_Min = int.TryParse(itemElement.Attribute("HighDamage_Min")?.Value, out int highDamageMin) ? highDamageMin : 0,
                IsTwoHanded = bool.TryParse(itemElement.Attribute("IsTwoHanded")?.Value, out bool isTwoHanded) ? isTwoHanded : false,
                LowDamage_Max = int.TryParse(itemElement.Attribute("LowDamage_Max")?.Value, out int lowDamageMax) ? lowDamageMax : 0,
                LowDamage_Min = int.TryParse(itemElement.Attribute("LowDamage_Min")?.Value, out int lowDamageMin) ? lowDamageMin : 0,
                Range = double.TryParse(itemElement.Attribute("Range")?.Value, out double range) ? range : 0,
                SpeedFactorBonus = int.TryParse(itemElement.Attribute("SpeedFactorBonus")?.Value, out int speedFactorBonus) ? speedFactorBonus : 0,
                UIName = itemElement.Attribute("UIName")?.Value ?? "",
                WeaponProperties = itemElement.Attribute("WeaponProperties")?.Value ?? "",
                MinSocketOverride = int.TryParse(itemElement.Attribute("MinSocketOverride")?.Value, out int minSocketOverride) ? minSocketOverride : 0,
                MaxSocketOverride = int.TryParse(itemElement.Attribute("MaxSocketOverride")?.Value, out int maxSocketOverride) ? maxSocketOverride : 0,
                ResourceGain_Min = int.TryParse(itemElement.Attribute("ResourceGain_Min")?.Value, out int resourceGainMin) ? resourceGainMin : 0,
                ResourceGain_Max = int.TryParse(itemElement.Attribute("ResourceGain_Max")?.Value, out int resourceGainMax) ? resourceGainMax : 0,
                GameplayVersionBaseStats = itemElement.Attribute("GameplayVersionBaseStats")?.Value ?? "",
                Version = itemElement.Attribute("Version")?.Value ?? "",

                // Enneract-specific properties
                EnneractPresetID = itemElement.Attribute("EnneractPresetID")?.Value ?? "",
                GoldValue = int.TryParse(itemElement.Attribute("GoldValue")?.Value, out int goldValue) ? goldValue : 0,
                LevelPrereq = int.TryParse(itemElement.Attribute("LevelPrereq")?.Value, out int levelPrereq) ? levelPrereq : 0,
                MinDropLevel = int.TryParse(itemElement.Attribute("MinDropLevel")?.Value, out int minDropLevel) ? minDropLevel : 0,
                MaxDropLevel = int.TryParse(itemElement.Attribute("MaxDropLevel")?.Value, out int maxDropLevel) ? maxDropLevel : 0,
                SkillLevelMax = int.TryParse(itemElement.Attribute("SkillLevelMax")?.Value, out int skillLevelMax) ? skillLevelMax : 0,
                SkillLevelMin = int.TryParse(itemElement.Attribute("SkillLevelMin")?.Value, out int skillLevelMin) ? skillLevelMin : 0,

                // Armor-specific properties
                ArmorType = itemElement.Attribute("ArmorType")?.Value ?? "",
                Defense = int.TryParse(itemElement.Attribute("Defense")?.Value, out int defense) ? defense : 0,
                ResistanceType = itemElement.Attribute("ResistanceType")?.Value ?? "",
                ResistanceValue = int.TryParse(itemElement.Attribute("ResistanceValue")?.Value, out int resistanceValue) ? resistanceValue : 0,

                // Potion-specific properties
                RecoveryAmount = int.TryParse(itemElement.Attribute("RecoveryAmount")?.Value, out int recoveryAmount) ? recoveryAmount : 0,
                Duration = int.TryParse(itemElement.Attribute("Duration")?.Value, out int duration) ? duration : 0,
                PotionType = itemElement.Attribute("PotionType")?.Value ?? ""
            };

            return item;
        }

        public static KeyValuePair<string, string>? SearchRowInXml(string xmlFilePath, string searchString)
        {
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(xmlFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while loading the XML file: {e.Message}");
                return null;
            }

            XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";

            var rowElement = xdoc.Descendants(ss + "Row")
                                 .FirstOrDefault(row => row.Elements(ss + "Cell")
                                                           .Elements(ss + "Data")
                                                           .Any(data => data.Value.ToLower().Contains(searchString.Substring(1).ToLower())));

            if (rowElement == null)
            {
                return null;
            }

            var cells = rowElement.Elements(ss + "Cell")
                                  .Select(cell => cell.Element(ss + "Data")?.Value)
                                  .ToList();

            if (cells.Count < 2)
            {
                return null;
            }

            return new KeyValuePair<string, string>(cells[0]!, cells[1]!);
        }

        public static KeyValuePair<string, string>? SearchEnneractInXml(string xmlFilePath, string searchString)
        {
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(xmlFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while loading the XML file: {e.Message}");
                return null;
            }

            var presetElement = xdoc.Root!
                                    .Elements("Preset")
                                    .FirstOrDefault(preset => preset.Attribute("PresetID")?.Value != "enneract_preset_all" &&
                                                             preset.Attribute("PresetID")?.Value.Contains(searchString) == true);

            if (presetElement == null)
            {
                return null;
            }

            var presetID = presetElement.Attribute("PresetID")?.Value;
            var skillUID = presetElement.Elements("SkillChoice")
                                        .Select(skill => skill.Attribute("SkillUID")?.Value)
                                        .FirstOrDefault();

            if (presetID == null || skillUID == null)
            {
                return null;
            }

            return new KeyValuePair<string, string>(presetID, skillUID);
        }
    }
}
