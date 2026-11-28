using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022C1 RID: 8897
	public class QuestNode_Incident : QuestNode
	{
		// Token: 0x0600DB82 RID: 56194 RVA: 0x0051C707 File Offset: 0x0051A907
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x0600DB83 RID: 56195 RVA: 0x0051C71C File Offset: 0x0051A91C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map target = QuestGen.slate.Get<Map>("map", null, false);
			float points = QuestGen.slate.Get<float>("points", 0f, false);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.incident = this.incidentDef.GetValue(slate);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = target;
			incidentParms.points = points;
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					incidentParms.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					incidentParms.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			questPart_Incident.SetIncidentParmsAndRemoveTarget(incidentParms);
			questPart_Incident.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_Incident);
		}

		// Token: 0x04008C8E RID: 35982
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008C8F RID: 35983
		public SlateRef<IncidentDef> incidentDef;

		// Token: 0x04008C90 RID: 35984
		public SlateRef<string> customLetterLabel;

		// Token: 0x04008C91 RID: 35985
		public SlateRef<string> customLetterText;

		// Token: 0x04008C92 RID: 35986
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x04008C93 RID: 35987
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x04008C94 RID: 35988
		private const string RootSymbol = "root";
	}
}
