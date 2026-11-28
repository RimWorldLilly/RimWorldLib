using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001158 RID: 4440
	public class FiringIncident : IExposable
	{
		// Token: 0x06006F87 RID: 28551 RVA: 0x0026FFED File Offset: 0x0026E1ED
		public FiringIncident()
		{
		}

		// Token: 0x06006F88 RID: 28552 RVA: 0x00270000 File Offset: 0x0026E200
		public FiringIncident(IncidentDef def, StorytellerComp source, IncidentParms parms = null)
		{
			this.def = def;
			if (parms != null)
			{
				this.parms = parms;
			}
			this.source = source;
		}

		// Token: 0x06006F89 RID: 28553 RVA: 0x0027002B File Offset: 0x0026E22B
		public void ExposeData()
		{
			Scribe_Defs.Look<IncidentDef>(ref this.def, "def");
			Scribe_Deep.Look<IncidentParms>(ref this.parms, "parms", Array.Empty<object>());
		}

		// Token: 0x06006F8A RID: 28554 RVA: 0x00270054 File Offset: 0x0026E254
		public override string ToString()
		{
			string text = this.def.defName;
			if (this.parms != null)
			{
				text = text + ", parms=(" + this.parms.ToString() + ")";
			}
			if (this.source != null)
			{
				string str = text;
				string str2 = ", source=";
				StorytellerComp storytellerComp = this.source;
				text = str + str2 + ((storytellerComp != null) ? storytellerComp.ToString() : null);
			}
			if (this.sourceQuestPart != null)
			{
				string str3 = text;
				string str4 = ", sourceQuestPart=";
				QuestPart questPart = this.sourceQuestPart;
				text = str3 + str4 + ((questPart != null) ? questPart.ToString() : null);
			}
			return text;
		}

		// Token: 0x04004737 RID: 18231
		public IncidentDef def;

		// Token: 0x04004738 RID: 18232
		public IncidentParms parms = new IncidentParms();

		// Token: 0x04004739 RID: 18233
		public StorytellerComp source;

		// Token: 0x0400473A RID: 18234
		public QuestPart sourceQuestPart;
	}
}
