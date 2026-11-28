using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022AC RID: 8876
	public class QuestPart_MonolithPart : QuestPart_DescriptionPart
	{
		// Token: 0x0600DB21 RID: 56097 RVA: 0x00519CD1 File Offset: 0x00517ED1
		public QuestPart_MonolithPart()
		{
		}

		// Token: 0x0600DB22 RID: 56098 RVA: 0x00519CD9 File Offset: 0x00517ED9
		public QuestPart_MonolithPart(Building_VoidMonolith monolith)
		{
			this.inSignalEnable = "MonolithLevelChanged";
			this.monolith = monolith;
		}

		// Token: 0x1700262F RID: 9775
		// (get) Token: 0x0600DB23 RID: 56099 RVA: 0x00519CF3 File Offset: 0x00517EF3
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_MonolithPart.<get_QuestLookTargets>d__4 <get_QuestLookTargets>d__ = new QuestPart_MonolithPart.<get_QuestLookTargets>d__4(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x0600DB24 RID: 56100 RVA: 0x00519D03 File Offset: 0x00517F03
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == "MonolithLevelChanged" || signal.tag == "EntityDiscovered")
			{
				this.UpdateDescription();
			}
		}

		// Token: 0x0600DB25 RID: 56101 RVA: 0x00519D38 File Offset: 0x00517F38
		private void UpdateDescription()
		{
			if (Find.Anomaly.LevelDef.postEndgame)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = Find.Anomaly.Level;
			if (Find.Anomaly.LevelDef == MonolithLevelDefOf.Gleaming)
			{
				num--;
			}
			stringBuilder.AppendLine(Find.Anomaly.LevelDef.extraQuestDescription);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("CurrentMonolithLevel".Translate() + ": " + num.ToString());
			MonolithLevelDef nextLevelDef = Find.Anomaly.NextLevelDef;
			EntityCategoryDef entityCategoryDef = (nextLevelDef != null) ? nextLevelDef.entityCatagoryCompletionRequired : null;
			if (entityCategoryDef != null)
			{
				stringBuilder.AppendLine();
				int num2 = Find.EntityCodex.DiscoveredCount(entityCategoryDef);
				if (num2 >= Find.Anomaly.NextLevelDef.entityCountCompletionRequired)
				{
					stringBuilder.AppendLine("VoidMonolithAllEntitiesDiscovered".Translate()).AppendLine(Find.Anomaly.LevelDef.activateQuestText);
				}
				else
				{
					int entityCountCompletionRequired = Find.Anomaly.NextLevelDef.entityCountCompletionRequired;
					stringBuilder.AppendLine("VoidMonolithRequiredEntityCategory".Translate(entityCountCompletionRequired, entityCategoryDef.label)).AppendLine().AppendLine("DiscoveredCount".Translate(num2.ToString() + " / " + entityCountCompletionRequired.ToString()).CapitalizeFirst());
				}
			}
			this.resolvedDescriptionPart = stringBuilder.ToString();
		}

		// Token: 0x0600DB26 RID: 56102 RVA: 0x00519EB8 File Offset: 0x005180B8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Building_VoidMonolith>(ref this.monolith, "monolith", false);
		}

		// Token: 0x0600DB27 RID: 56103 RVA: 0x00519ED1 File Offset: 0x005180D1
		public override void Cleanup()
		{
			base.Cleanup();
			this.monolith = null;
		}

		// Token: 0x04008C44 RID: 35908
		private Building_VoidMonolith monolith;
	}
}
