using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022C4 RID: 8900
	public class QuestNode_Raid : QuestNode
	{
		// Token: 0x0600DB8B RID: 56203 RVA: 0x0051CD85 File Offset: 0x0051AF85
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Exists("map", false) && slate.Exists("enemyFaction", false);
		}

		// Token: 0x0600DB8C RID: 56204 RVA: 0x0051CDBC File Offset: 0x0051AFBC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			float points = QuestGen.slate.Get<float>("points", 0f, false);
			Faction faction = QuestGen.slate.Get<Faction>("enemyFaction", null, false);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.debugLabel = "raid";
			questPart_Incident.incident = IncidentDefOf.RaidEnemy;
			int num = 0;
			IncidentParms incidentParms;
			PawnGroupMakerParms defaultPawnGroupMakerParms;
			IEnumerable<PawnKindDef> enumerable;
			do
			{
				incidentParms = this.GenerateIncidentParms(map, points, faction, slate, questPart_Incident);
				defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms, true);
				defaultPawnGroupMakerParms.points = IncidentWorker_Raid.AdjustedRaidPoints(defaultPawnGroupMakerParms.points, incidentParms.raidArrivalMode, incidentParms.raidStrategy, defaultPawnGroupMakerParms.faction, PawnGroupKindDefOf.Combat, incidentParms.target, null);
				enumerable = PawnGroupMakerUtility.GeneratePawnKindsExample(defaultPawnGroupMakerParms);
				num++;
			}
			while (!enumerable.Any<PawnKindDef>() && num < 50);
			if (!enumerable.Any<PawnKindDef>())
			{
				string[] array = new string[6];
				array[0] = "No pawnkinds example for ";
				array[1] = QuestGen.quest.root.defName;
				array[2] = " parms=";
				int num2 = 3;
				PawnGroupMakerParms pawnGroupMakerParms = defaultPawnGroupMakerParms;
				array[num2] = ((pawnGroupMakerParms != null) ? pawnGroupMakerParms.ToString() : null);
				array[4] = " iterations=";
				array[5] = num.ToString();
				Log.Error(string.Concat(array));
			}
			questPart_Incident.SetIncidentParmsAndRemoveTarget(incidentParms);
			questPart_Incident.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_Incident);
			QuestGen.AddQuestDescriptionRules(new List<Rule>
			{
				new Rule_String("raidPawnKinds", PawnUtility.PawnKindsToLineList(enumerable, "  - ", ColoredText.ThreatColor)),
				new Rule_String("raidArrivalModeInfo", incidentParms.raidArrivalMode.textWillArrive.Formatted(faction))
			});
		}

		// Token: 0x0600DB8D RID: 56205 RVA: 0x0051CF94 File Offset: 0x0051B194
		private IncidentParms GenerateIncidentParms(Map map, float points, Faction faction, Slate slate, QuestPart_Incident questPart)
		{
			IncidentParms incidentParms = new IncidentParms
			{
				forced = true,
				target = map,
				points = Mathf.Max(points, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null)),
				faction = faction,
				pawnGroupMakerSeed = new int?(Rand.Int),
				inSignalEnd = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalLeave.GetValue(slate)),
				questTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate)),
				canTimeoutOrFlee = (map.CanEverExit && this.canTimeoutOrFlee.GetValue(slate).GetValueOrDefault(true))
			};
			if (this.raidPawnKind.GetValue(slate) != null)
			{
				incidentParms.pawnKind = this.raidPawnKind.GetValue(slate);
				incidentParms.pawnCount = Mathf.Max(1, Mathf.RoundToInt(incidentParms.points / incidentParms.pawnKind.combatPower));
			}
			if (this.arrivalMode.GetValue(slate) != null)
			{
				incidentParms.raidArrivalMode = this.arrivalMode.GetValue(slate);
			}
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
			IncidentWorker_Raid incidentWorker_Raid = (IncidentWorker_Raid)questPart.incident.Worker;
			incidentWorker_Raid.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
			incidentWorker_Raid.ResolveRaidArriveMode(incidentParms);
			incidentWorker_Raid.ResolveRaidAgeRestriction(incidentParms);
			if (incidentParms.raidArrivalMode.walkIn)
			{
				incidentParms.spawnCenter = (this.walkInSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			}
			else
			{
				incidentParms.spawnCenter = (this.dropSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("dropSpot", null, false) ?? IntVec3.Invalid));
			}
			return incidentParms;
		}

		// Token: 0x04008CA6 RID: 36006
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008CA7 RID: 36007
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x04008CA8 RID: 36008
		public SlateRef<IntVec3?> dropSpot;

		// Token: 0x04008CA9 RID: 36009
		public SlateRef<string> customLetterLabel;

		// Token: 0x04008CAA RID: 36010
		public SlateRef<string> customLetterText;

		// Token: 0x04008CAB RID: 36011
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x04008CAC RID: 36012
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x04008CAD RID: 36013
		public SlateRef<PawnsArrivalModeDef> arrivalMode;

		// Token: 0x04008CAE RID: 36014
		public SlateRef<PawnKindDef> raidPawnKind;

		// Token: 0x04008CAF RID: 36015
		public SlateRef<bool?> canTimeoutOrFlee;

		// Token: 0x04008CB0 RID: 36016
		[NoTranslate]
		public SlateRef<string> inSignalLeave;

		// Token: 0x04008CB1 RID: 36017
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x04008CB2 RID: 36018
		private const string RootSymbol = "root";
	}
}
