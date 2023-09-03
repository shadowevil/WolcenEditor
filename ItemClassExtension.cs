using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WolcenData.Items;

namespace WolcenEditor
{
    public static class ItemClassExtension
    {
        public static string ItemName(this Item item)
        {
            string? itemName = item.Armor?.Name ?? item.Weapon?.Name ?? item.Potion?.Name ?? item.Enneract?.Name ?? "Unknown";

            ItemInfo? itemInfo =
                ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Weapons\\Weapons.xml", itemName)
                ?? ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Weapons\\UniqueWeapons.xml", itemName)
                ?? ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Weapons\\UniqueWeaponsMax.xml", itemName)
                ?? ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Weapons\\UniqueWeaponsMaxMax.xml", itemName)
                ?? ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Weapons\\UniqueWeaponsUltimate.xml", itemName)

                ?? ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Armors\\Armors.xml", itemName)
                ?? ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Armors\\Armors_uniques.xml", itemName)
                ?? ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Armors\\UniqueArmorsMax.xml", itemName)
                ?? ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Armors\\UniqueArmorsMaxMax.xml", itemName)

                ?? ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Potions\\HealthPotions.xml", itemName)
                ?? ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Potions\\RagePotions.xml", itemName)
                ?? ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Potions\\UmbraPotions.xml", itemName)

                ?? ItemXmlParser.GetItemByName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Enneracts\\Items\\eneracts.xml", itemName);

            KeyValuePair<string, string>? itemLocalizedName;

            if (item.ItemTypeCategory == ItemTypeEnum.Enneract)
            {
                var presetPair = ItemXmlParser.SearchEnneractInXml(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Loot\\Enneracts\\Presets\\presets.xml", itemInfo!.EnneractPresetID);
                string ui_ASTString = "ui_AST_";
                string? presetValue = presetPair!.Value.Value.Replace("player_", "");
                presetValue = presetValue.Replace("_", "");
                string ui_ASTString_Full = ui_ASTString + presetValue;
                itemLocalizedName = ItemXmlParser.SearchRowInXml(Directory.GetCurrentDirectory() + "\\Data\\localization\\text_ui_Activeskills.xml", ui_ASTString_Full!);
            }
            else
            {
                itemLocalizedName = ItemXmlParser.SearchRowInXml(Directory.GetCurrentDirectory() + "\\Data\\localization\\text_ui_Loot.xml", itemInfo!.UIName);
            }
            return itemLocalizedName.HasValue ? itemLocalizedName.Value.Value : "unknown";
        }
    }
}
