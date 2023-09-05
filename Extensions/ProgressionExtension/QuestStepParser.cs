using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Xml;
using WolcenData.Telemetry;

namespace WolcenEditor
{
    public class Step
    {
        public int Id { get; set; }
        public string? UIName { get; set; } = "";
        public string? LocalizedQuestStepName
        {
            get
            {
                KeyValuePair<string, string>? localizedName = XMLParser.SearchRowInXml(Directory.GetCurrentDirectory() + "\\Data\\localization\\text_ui_Quests.xml", UIName!)
                    ?? XMLParser.SearchRowInXml(Directory.GetCurrentDirectory() + "\\Data\\localization\\text_ui_A4Dialogs.xml", UIName!)
                    ?? XMLParser.SearchRowInXml(Directory.GetCurrentDirectory() + "\\Data\\localization\\text_ui_SecondaryQuest.xml", UIName!);
                return localizedName?.Value ?? string.Empty;
            }
        }
    }

    public class Quest
    {
        public string Id { get; set; } = "";
        public string? UIName { get; set; } = "";
        public List<Step> Steps { get; set; } = new List<Step>();
        
        public string? LocalizedQuestName
        {
            get
            {
                KeyValuePair<string, string>? localizedName = XMLParser.SearchRowInXml(Directory.GetCurrentDirectory() + "\\Data\\localization\\text_ui_Quests.xml", UIName!)
                    ?? XMLParser.SearchRowInXml(Directory.GetCurrentDirectory() + "\\Data\\localization\\text_ui_A4Dialogs.xml", UIName!)
                    ?? XMLParser.SearchRowInXml(Directory.GetCurrentDirectory() + "\\Data\\localization\\text_ui_SecondaryQuest.xml", UIName!);
                return localizedName?.Value ?? string.Empty;
            }
        }
    }

    public class Act
    {
        public string Id { get; set; } = "";
        public string? UIName { get; set; } = "";
        public List<Quest> Quests { get; set; } = new List<Quest>();

        public string? LocalizedActName
        {
            get
            {
                KeyValuePair<string, string>? localizedName = XMLParser.SearchRowInXml(Directory.GetCurrentDirectory() + "\\Data\\localization\\text_ui_Quests.xml", UIName!)
                    ?? XMLParser.SearchRowInXml(Directory.GetCurrentDirectory() + "\\Data\\localization\\text_ui_A4Dialogs.xml", UIName!)
                    ?? XMLParser.SearchRowInXml(Directory.GetCurrentDirectory() + "\\Data\\localization\\text_ui_SecondaryQuest.xml", UIName!);
                return localizedName?.Value ?? string.Empty;
            }
        }
    }

    public static class QuestStepParser
    {
        public static List<Act> GetActsFromDir(string dir)
        {
            List<Act> actList = new List<Act>();

            foreach (string directory in Directory.GetDirectories(dir))
            {
                if (Directory.Exists(directory))
                {
                    string[] pathSplit = Path.GetDirectoryName(directory + "\\")!.Split("\\");
                    actList.Add(new Act()
                    {
                        Id = pathSplit.Last(),
                        UIName = GetActUIName(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Quests\\MainMenuQuestSelection.xml", pathSplit.Last()),
                        Quests = GetQuestsFromDir(directory)
                    });
                }
            }

            return actList;
        }

        public static string? GetActUIName(string filePath, string actId)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNodeList actNodes = doc.SelectNodes("//Act")!;

            foreach (XmlNode actNode in actNodes)
            {
                if (actNode.Attributes?["Id"] == null || actNode.Attributes?["UIName"] == null)
                {
                    continue;
                }

                string currentActId = actNode.Attributes["Id"]!.Value;

                if (currentActId == actId)
                {
                    return actNode.Attributes["UIName"]!.Value;
                }
            }

            return null;
        }

        public static List<Quest> GetQuestsFromDir(string dir)
        {
            List<Quest> questList = new List<Quest>();
            foreach(string dire in Directory.GetDirectories(dir)) questList.AddRange(GetQuestsFromDir(dire));
            foreach(string path in Directory.GetFiles(dir))
            {
                if(File.Exists(path))
                {
                    questList.AddRange(ExtractQuestData(path));
                }
            }

            return questList;
        }

        public static List<Quest> ExtractQuestData(string filePath)
        {
            List<Quest> quests = new List<Quest>();
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNodeList questNodes = doc.SelectNodes("//Quest")!;

            foreach (XmlNode questNode in questNodes)
            {
                Quest quest = new Quest();

                if (questNode.Attributes?["Name"] != null)
                {
                    quest.Id = questNode.Attributes["Name"]!.Value;
                }

                if (questNode.Attributes?["UIName"] != null)
                {
                    quest.UIName = questNode.Attributes["UIName"]!.Value;
                }

                XmlNodeList stepNodes = questNode.SelectNodes("Step")!;

                foreach (XmlNode stepNode in stepNodes)
                {
                    Step step = new Step();

                    if (stepNode.Attributes?["Order"] != null)
                    {
                        step.Id = int.Parse(stepNode.Attributes["Order"]!.Value);
                    }

                    if (stepNode.Attributes?["UIName"] != null)
                    {
                        step.UIName = stepNode.Attributes["UIName"]!.Value;
                    }

                    quest.Steps.Add(step);
                }

                quests.Add(quest);
            }

            return quests;
        }
    }
}
