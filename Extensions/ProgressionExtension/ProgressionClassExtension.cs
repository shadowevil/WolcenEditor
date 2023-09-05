using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WolcenEditor
{
    public static class ProgressionClassExtension
    {
        public static Act? GetActInformation(this WolcenData.Progression progression)
        {
            if (progression == null) return null;
            foreach (var a in MainWindow.ActData)
            {
                if (a.Quests.Any(x => x.Id == progression.QuestId))
                {
                    return a;
                }
            }
            return null;
        }

        public static Quest? GetQuestInformation(this WolcenData.Progression progression)
        {
            if (progression == null) return null;
            foreach (var a in MainWindow.ActData)
            {
                Quest? quest = a.Quests.SingleOrDefault(x => x.Id == progression.QuestId);
                if (quest != null) return quest;
            }
            return null;
        }

        public static Step? GetStepFromQuest(this WolcenData.Progression progression)
        {
            if (progression == null) return null;
            foreach (var a in MainWindow.ActData)
            {
                Quest? quest = a.Quests.SingleOrDefault(x => x.Id == progression.QuestId);
                if (quest != null)
                {
                    Step? step = quest.Steps.SingleOrDefault(x => x.Id == progression.StepId);
                    if(step != null) return step;
                }
            }
            return null;
        }
    }
}
