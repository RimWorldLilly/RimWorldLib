using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022C3 RID: 8899
	public class QuestNode_ManhunterPack : QuestNode
	{
		// Token: 0x0600DB88 RID: 56200 RVA: 0x0051CA78 File Offset: 0x0051AC78
		protected override bool TestRunInt(Slate slate)
		{
			PawnKindDef pawnKindDef;
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Exists("map", false) && AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(slate.Get<float>("points", 0f, false), slate.Get<Map>("map", null, false), out pawnKindDef);
		}

		// Token: 0x0600DB89 RID: 56201 RVA: 0x0051CAD4 File Offset: 0x0051ACD4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			float points = QuestGen.slate.Get<float>("points", 0f, false);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.incident = IncidentDefOf.ManhunterPack;
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = map;
			incidentParms.points = points;
			incidentParms.questTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			incidentParms.spawnCenter = (this.walkInSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			incidentParms.pawnCount = this.animalCount.GetValue(slate);
			PawnKindDef pawnKindDef;
			if (AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(points, map, out pawnKindDef))
			{
				incidentParms.pawnKind = pawnKindDef;
			}
			slate.Set<PawnKindDef>("animalKindDef", pawnKindDef, false);
			int num = (incidentParms.pawnCount > 0) ? incidentParms.pawnCount : AggressiveAnimalIncidentUtility.GetAnimalsCount(pawnKindDef, points);
			QuestGen.slate.Set<int>("animalCount", num, false);
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
			List<Rule> list = new List<Rule>();
			list.Add(new Rule_String("animalKind_label", pawnKindDef.label));
			list.Add(new Rule_String("animalKind_labelPlural", pawnKindDef.GetLabelPlural(num)));
			QuestGen.AddQuestDescriptionRules(list);
			QuestGen.AddQuestNameRules(list);
		}

		// Token: 0x04008C9D RID: 35997
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008C9E RID: 35998
		public SlateRef<string> customLetterLabel;

		// Token: 0x04008C9F RID: 35999
		public SlateRef<string> customLetterText;

		// Token: 0x04008CA0 RID: 36000
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x04008CA1 RID: 36001
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x04008CA2 RID: 36002
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x04008CA3 RID: 36003
		public SlateRef<int> animalCount;

		// Token: 0x04008CA4 RID: 36004
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x04008CA5 RID: 36005
		private const string RootSymbol = "root";
	}
}
