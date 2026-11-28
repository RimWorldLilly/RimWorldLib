using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022AF RID: 8879
	public class QuestNode_Root_WandererJoin_WalkIn : QuestNode_Root_WandererJoin
	{
		// Token: 0x0600DB3B RID: 56123 RVA: 0x0051A450 File Offset: 0x00518650
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			quest.Delay(60000, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
		}

		// Token: 0x0600DB3C RID: 56124 RVA: 0x0051A49E File Offset: 0x0051869E
		public override Pawn GeneratePawn()
		{
			return this.GeneratePawn_NewTemp(null);
		}

		// Token: 0x0600DB3D RID: 56125 RVA: 0x0051A4A8 File Offset: 0x005186A8
		public override Pawn GeneratePawn_NewTemp(Map map)
		{
			Slate slate = QuestGen.slate;
			Gender? gender = null;
			PawnGenerationRequest request;
			if (!slate.TryGet<PawnGenerationRequest>("overridePawnGenParams", out request, false))
			{
				PawnKindDef villager = PawnKindDefOf.Villager;
				Faction faction = null;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				PlanetTile? tile = (map != null) ? new PlanetTile?(map.Tile) : null;
				bool forceGenerateNewPawn = true;
				bool allowDead = false;
				bool allowDowned = false;
				bool canGeneratePawnRelations = true;
				bool mustBeCapableOfViolence = false;
				float colonistRelationChanceFactor = 20f;
				bool forceAddFreeWarmLayerIfNeeded = false;
				bool allowGay = true;
				bool allowPregnant = true;
				bool allowFood = true;
				bool allowAddictions = true;
				bool inhabitant = false;
				bool certainlyBeenInCryptosleep = false;
				bool forceRedressWorldPawnIfFormerColonist = false;
				bool worldPawnFactionDoesntMatter = false;
				float biocodeWeaponChance = 0f;
				float biocodeApparelChance = 0f;
				Pawn extraPawnForExtraRelationChance = null;
				float relationWithExtraPawnChanceFactor = 1f;
				Predicate<Pawn> validatorPreGear = null;
				Predicate<Pawn> validatorPostGear = null;
				IEnumerable<TraitDef> forcedTraits = null;
				IEnumerable<TraitDef> prohibitedTraits = null;
				Gender? fixedGender = gender;
				request = new PawnGenerationRequest(villager, faction, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, inhabitant, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, biocodeWeaponChance, biocodeApparelChance, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, validatorPreGear, validatorPostGear, forcedTraits, prohibitedTraits, null, null, null, fixedGender, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false);
			}
			if (Find.Storyteller.difficulty.ChildrenAllowed)
			{
				request.AllowedDevelopmentalStages |= DevelopmentalStage.Child;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
			return pawn;
		}

		// Token: 0x0600DB3E RID: 56126 RVA: 0x0051A5B0 File Offset: 0x005187B0
		protected override void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
		{
			this.signalAccept = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
			this.signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
			quest.Signal(this.signalAccept, delegate
			{
				quest.SetFaction(Gen.YieldSingle<Pawn>(pawn), Faction.OfPlayer, null);
				quest.PawnsArrive(Gen.YieldSingle<Pawn>(pawn), null, map.Parent, null, false, null, null, null, null, null, false, false, true);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.Signal(this.signalReject, delegate
			{
				quest.GiveDiedOrDownedThoughts(pawn, PawnDiedOrDownedThoughtsKind.DeniedJoining, null);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x0600DB3F RID: 56127 RVA: 0x0051A636 File Offset: 0x00518836
		[Obsolete]
		public override void SendLetter(Quest quest, Pawn pawn)
		{
			this.SendLetter_NewTemp(quest, pawn, Find.AnyPlayerHomeMap);
		}

		// Token: 0x0600DB40 RID: 56128 RVA: 0x0051A648 File Offset: 0x00518848
		public override void SendLetter_NewTemp(Quest quest, Pawn pawn, Map map)
		{
			TaggedString label = "LetterLabelWandererJoins".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			TaggedString taggedString = "LetterWandererJoins".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			QuestNode_Root_WandererJoin_WalkIn.AppendCharityInfoToLetter("JoinerCharityInfo".Translate(pawn), ref taggedString);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref label, pawn);
			if (pawn.DevelopmentalStage.Juvenile())
			{
				string arg = (pawn.ageTracker.AgeBiologicalYears * 3600000).ToStringTicksToPeriod(true, false, true, true, false);
				taggedString += "\n\n" + "RefugeePodCrash_Child".Translate(pawn.Named("PAWN"), arg.Named("AGE"));
			}
			QuestNode_Root_WandererJoin_WalkIn.ApplyBestSkillInfoToLetter(ref taggedString, pawn);
			ChoiceLetter_AcceptJoiner choiceLetter_AcceptJoiner = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(label, taggedString, LetterDefOf.AcceptJoiner, null, null);
			choiceLetter_AcceptJoiner.signalAccept = this.signalAccept;
			choiceLetter_AcceptJoiner.signalReject = this.signalReject;
			choiceLetter_AcceptJoiner.quest = quest;
			choiceLetter_AcceptJoiner.overrideMap = map;
			choiceLetter_AcceptJoiner.StartTimeout(60000);
			Find.LetterStack.ReceiveLetter(choiceLetter_AcceptJoiner, null, 0, true);
		}

		// Token: 0x0600DB41 RID: 56129 RVA: 0x0051A778 File Offset: 0x00518978
		public static void AppendCharityInfoToLetter(TaggedString charityInfo, ref TaggedString letterText)
		{
			if (ModsConfig.IdeologyActive)
			{
				IEnumerable<Pawn> source = IdeoUtility.AllColonistsWithCharityPrecept();
				if (source.Any<Pawn>())
				{
					letterText += "\n\n" + charityInfo + "\n\n" + "PawnsHaveCharitableBeliefs".Translate() + ":";
					foreach (IGrouping<Ideo, Pawn> grouping in from c in source
					group c by c.Ideo)
					{
						letterText += "\n  - " + "BelieversIn".Translate(grouping.Key.name.Colorize(grouping.Key.TextColor), grouping.Select((Pawn f) => f.NameShortColored.Resolve()).ToCommaList(false, false));
					}
				}
			}
		}

		// Token: 0x0600DB42 RID: 56130 RVA: 0x0051A8B0 File Offset: 0x00518AB0
		public static void ApplyBestSkillInfoToLetter(ref TaggedString letterText, Pawn pawn)
		{
			if (pawn.skills == null || !pawn.skills.skills.Any<SkillRecord>())
			{
				return;
			}
			SkillRecord skillRecord = pawn.skills.skills.MaxBy((SkillRecord s) => s.Level);
			if (skillRecord == null)
			{
				return;
			}
			letterText += "\n\n" + "BestSkillLetterLabel".Translate(pawn.Named("PAWN")) + ": " + skillRecord.def.LabelCap + " (" + "BestSkillInfoLevel".Translate(skillRecord.Level) + ")";
		}

		// Token: 0x04008C49 RID: 35913
		private const int TimeoutTicks = 60000;

		// Token: 0x04008C4A RID: 35914
		public const float RelationWithColonistWeight = 20f;

		// Token: 0x04008C4B RID: 35915
		private string signalAccept;

		// Token: 0x04008C4C RID: 35916
		private string signalReject;
	}
}
