using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002229 RID: 8745
	public class QuestNode_QuestUnique : QuestNode
	{
		// Token: 0x0600D8AB RID: 55467 RVA: 0x00505067 File Offset: 0x00503267
		public static string GetProcessedTag(string tag, Faction faction)
		{
			if (faction == null)
			{
				return tag;
			}
			return tag + "_" + faction.Name;
		}

		// Token: 0x0600D8AC RID: 55468 RVA: 0x0050507F File Offset: 0x0050327F
		private string GetProcessedTag(Slate slate)
		{
			return QuestNode_QuestUnique.GetProcessedTag(this.tag.GetValue(slate), this.faction.GetValue(slate));
		}

		// Token: 0x0600D8AD RID: 55469 RVA: 0x005050A0 File Offset: 0x005032A0
		protected override void RunInt()
		{
			string processedTag = this.GetProcessedTag(QuestGen.slate);
			QuestUtility.AddQuestTag(ref QuestGen.quest.tags, processedTag);
			if (this.storeProcessedTagAs.GetValue(QuestGen.slate) != null)
			{
				QuestGen.slate.Set<string>(this.storeProcessedTagAs.GetValue(QuestGen.slate), processedTag, false);
			}
		}

		// Token: 0x0600D8AE RID: 55470 RVA: 0x005050F8 File Offset: 0x005032F8
		protected override bool TestRunInt(Slate slate)
		{
			string processedTag = this.GetProcessedTag(slate);
			if (this.storeProcessedTagAs.GetValue(slate) != null)
			{
				slate.Set<string>(this.storeProcessedTagAs.GetValue(slate), processedTag, false);
			}
			foreach (Quest quest in Find.QuestManager.questsInDisplayOrder)
			{
				if (quest.State == QuestState.Ongoing && quest.tags.Contains(processedTag))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04008A5D RID: 35421
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x04008A5E RID: 35422
		public SlateRef<Faction> faction;

		// Token: 0x04008A5F RID: 35423
		[NoTranslate]
		public SlateRef<string> storeProcessedTagAs;
	}
}
