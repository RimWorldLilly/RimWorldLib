using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.Planet
{
	// Token: 0x020023B1 RID: 9137
	public class PeaceTalks : WorldObject
	{
		// Token: 0x170027FC RID: 10236
		// (get) Token: 0x0600E38F RID: 58255 RVA: 0x00549C00 File Offset: 0x00547E00
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (base.Faction != null)
					{
						color = base.Faction.Color;
					}
					else
					{
						color = Color.white;
					}
					this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, 3550);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x0600E390 RID: 58256 RVA: 0x00549C60 File Offset: 0x00547E60
		public void Notify_CaravanArrived(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
			if (pawn == null)
			{
				Messages.Message("MessagePeaceTalksNoDiplomat".Translate(), caravan, MessageTypeDefOf.NegativeEvent, false);
				return;
			}
			float badOutcomeWeightFactor = PeaceTalks.GetBadOutcomeWeightFactor(pawn, caravan);
			float num = 1f / badOutcomeWeightFactor;
			PeaceTalks.tmpPossibleOutcomes.Clear();
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_Disaster(caravan);
			}, 0.05f * badOutcomeWeightFactor));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_Backfire(caravan);
			}, 0.1f * badOutcomeWeightFactor));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_TalksFlounder(caravan);
			}, 0.2f));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_Success(caravan);
			}, 0.55f * num));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_Triumph(caravan);
			}, 0.1f * num));
			PeaceTalks.tmpPossibleOutcomes.RandomElementByWeight((Pair<Action, float> x) => x.Second).First();
			pawn.skills.Learn(SkillDefOf.Social, 6000f, true, false);
			QuestUtility.SendQuestTargetSignals(this.questTags, "Resolved", this.Named("SUBJECT"));
			this.Destroy();
		}

		// Token: 0x0600E391 RID: 58257 RVA: 0x00549DEC File Offset: 0x00547FEC
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			PeaceTalks.<GetFloatMenuOptions>d__12 <GetFloatMenuOptions>d__ = new PeaceTalks.<GetFloatMenuOptions>d__12(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__caravan = caravan;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600E392 RID: 58258 RVA: 0x00549E03 File Offset: 0x00548003
		private void Outcome_Disaster(Caravan caravan)
		{
			LongEventHandler.QueueLongEvent(delegate()
			{
				FactionRelationKind playerRelationKind = this.Faction.PlayerRelationKind;
				int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksDisasterRange.RandomInRange;
				Faction.OfPlayer.TryAffectGoodwillWith(this.Faction, Mathf.Min(randomInRange, Faction.OfPlayer.GoodwillToMakeHostile(this.Faction)), false, false, HistoryEventDefOf.PeaceTalksDisaster, null);
				IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, caravan);
				incidentParms.faction = this.Faction;
				PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms, true);
				defaultPawnGroupMakerParms.generateFightersOnly = true;
				List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
				Map map = CaravanIncidentUtility.SetupCaravanAttackMap(caravan, list, false);
				if (list.Any<Pawn>())
				{
					LordMaker.MakeNewLord(incidentParms.faction, new LordJob_AssaultColony(this.Faction, true, true, false, false, true, false, false), map, list);
				}
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				GlobalTargetInfo target = list.Any<Pawn>() ? new GlobalTargetInfo(list[0].Position, map, false) : GlobalTargetInfo.Invalid;
				TaggedString label = "LetterLabelPeaceTalks_Disaster".Translate();
				TaggedString text = this.GetLetterText("LetterPeaceTalks_Disaster".Translate(this.Faction.def.pawnsPlural.CapitalizeFirst(), this.Faction.NameColored, Mathf.RoundToInt((float)randomInRange)), caravan, playerRelationKind, 0);
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref label, ref text, "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, target, this.Faction, null, null, null, 0, true);
			}, "GeneratingMapForNewEncounter", false, null, true, false, null);
		}

		// Token: 0x0600E393 RID: 58259 RVA: 0x00549E34 File Offset: 0x00548034
		private void Outcome_Backfire(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksBackfireRange.RandomInRange;
			Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, randomInRange, false, false, HistoryEventDefOf.PeaceTalksBackfire, null);
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Backfire".Translate(), this.GetLetterText("LetterPeaceTalks_Backfire".Translate(base.Faction.NameColored, randomInRange), caravan, playerRelationKind, 0), LetterDefOf.NegativeEvent, caravan, base.Faction, null, null, null, 0, true);
		}

		// Token: 0x0600E394 RID: 58260 RVA: 0x00549EDC File Offset: 0x005480DC
		private void Outcome_TalksFlounder(Caravan caravan)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_TalksFlounder".Translate(), this.GetLetterText("LetterPeaceTalks_TalksFlounder".Translate(base.Faction.NameColored), caravan, base.Faction.PlayerRelationKind, 0), LetterDefOf.NeutralEvent, caravan, base.Faction, null, null, null, 0, true);
		}

		// Token: 0x0600E395 RID: 58261 RVA: 0x00549F4C File Offset: 0x0054814C
		private void Outcome_Success(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksSuccessRange.RandomInRange;
			Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, randomInRange, false, false, HistoryEventDefOf.PeaceTalksSuccess, null);
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Success".Translate(), this.GetLetterText("LetterPeaceTalks_Success".Translate(base.Faction.NameColored, randomInRange), caravan, playerRelationKind, this.TryGainRoyalFavor(caravan)), LetterDefOf.PositiveEvent, caravan, base.Faction, null, null, null, 0, true);
		}

		// Token: 0x0600E396 RID: 58262 RVA: 0x00549FF8 File Offset: 0x005481F8
		private void Outcome_Triumph(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksTriumphRange.RandomInRange;
			Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, randomInRange, false, false, HistoryEventDefOf.PeaceTalksTriumph, null);
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.makingFaction = base.Faction;
			parms.techLevel = new TechLevel?(base.Faction.def.techLevel);
			parms.maxTotalMass = new float?((float)20);
			parms.totalMarketValueRange = new FloatRange?(new FloatRange(500f, 1200f));
			parms.tile = new PlanetTile?(base.Tile);
			List<Thing> list = ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms);
			for (int i = 0; i < list.Count; i++)
			{
				caravan.AddPawnOrItem(list[i], true);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Triumph".Translate(), this.GetLetterText("LetterPeaceTalks_Triumph".Translate(base.Faction.NameColored, randomInRange, GenLabel.ThingsLabel(list, "  - ")), caravan, playerRelationKind, this.TryGainRoyalFavor(caravan)), LetterDefOf.PositiveEvent, caravan, base.Faction, null, null, null, 0, true);
		}

		// Token: 0x0600E397 RID: 58263 RVA: 0x0054A158 File Offset: 0x00548358
		private int TryGainRoyalFavor(Caravan caravan)
		{
			int num = 0;
			if (base.Faction.def.HasRoyalTitles)
			{
				num = DiplomacyTuning.RoyalFavor_PeaceTalksSuccessRange.RandomInRange;
				Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
				if (pawn != null)
				{
					pawn.royalty.GainFavor(base.Faction, num);
				}
			}
			return num;
		}

		// Token: 0x0600E398 RID: 58264 RVA: 0x0054A1A4 File Offset: 0x005483A4
		private string GetLetterText(string baseText, Caravan caravan, FactionRelationKind previousRelationKind, int royalFavorGained = 0)
		{
			TaggedString taggedString = baseText;
			Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
			if (pawn != null)
			{
				taggedString += "\n\n" + "PeaceTalksSocialXPGain".Translate(pawn.LabelShort, 6000f.ToString("F0"), pawn.Named("PAWN"));
				if (royalFavorGained > 0)
				{
					taggedString += "\n\n" + "PeaceTalksRoyalFavorGain".Translate(pawn.LabelShort, royalFavorGained.ToString(), base.Faction.Named("FACTION"), pawn.Named("PAWN"));
				}
			}
			base.Faction.TryAppendRelationKindChangedInfo(ref taggedString, previousRelationKind, base.Faction.PlayerRelationKind, null);
			return taggedString;
		}

		// Token: 0x0600E399 RID: 58265 RVA: 0x0054A280 File Offset: 0x00548480
		private static float GetBadOutcomeWeightFactor(Pawn diplomat, Caravan caravan)
		{
			float statValue = diplomat.GetStatValue(StatDefOf.NegotiationAbility, true, -1);
			float num = 0f;
			if (ModsConfig.IdeologyActive)
			{
				bool flag = false;
				using (List<Pawn>.Enumerator enumerator = caravan.pawns.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == caravan.Faction.leader)
						{
							flag = true;
							break;
						}
					}
				}
				num = (flag ? -0.05f : 0.05f);
			}
			return PeaceTalks.GetBadOutcomeWeightFactor(statValue) * (1f + num);
		}

		// Token: 0x0600E39A RID: 58266 RVA: 0x0054A318 File Offset: 0x00548518
		private static float GetBadOutcomeWeightFactor(float negotationAbility)
		{
			return PeaceTalks.BadOutcomeChanceFactorByNegotiationAbility.Evaluate(negotationAbility);
		}

		// Token: 0x0600E39B RID: 58267 RVA: 0x0054A325 File Offset: 0x00548525
		[DebugOutput("Incidents", false)]
		private static void PeaceTalksChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			PeaceTalks.AppendDebugChances(stringBuilder, 0f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1.5f);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600E39C RID: 58268 RVA: 0x0054A358 File Offset: 0x00548558
		private static void AppendDebugChances(StringBuilder sb, float negotiationAbility)
		{
			if (sb.Length > 0)
			{
				sb.AppendLine();
			}
			sb.AppendLine("--- NegotiationAbility = " + negotiationAbility.ToStringPercent() + " ---");
			float badOutcomeWeightFactor = PeaceTalks.GetBadOutcomeWeightFactor(negotiationAbility);
			float num = 1f / badOutcomeWeightFactor;
			sb.AppendLine("Bad outcome weight factor: " + badOutcomeWeightFactor.ToString("0.##"));
			float num2 = 0.05f * badOutcomeWeightFactor;
			float num3 = 0.1f * badOutcomeWeightFactor;
			float num4 = 0.2f;
			float num5 = 0.55f * num;
			float num6 = 0.1f * num;
			float num7 = num2 + num3 + num4 + num5 + num6;
			sb.AppendLine("Disaster: " + (num2 / num7).ToStringPercent());
			sb.AppendLine("Backfire: " + (num3 / num7).ToStringPercent());
			sb.AppendLine("Talks flounder: " + (num4 / num7).ToStringPercent());
			sb.AppendLine("Success: " + (num5 / num7).ToStringPercent());
			sb.AppendLine("Triumph: " + (num6 / num7).ToStringPercent());
		}

		// Token: 0x04009109 RID: 37129
		private Material cachedMat;

		// Token: 0x0400910A RID: 37130
		private static readonly SimpleCurve BadOutcomeChanceFactorByNegotiationAbility = new SimpleCurve
		{
			{
				new CurvePoint(0f, 4f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(1.5f, 0.4f),
				true
			}
		};

		// Token: 0x0400910B RID: 37131
		private const float BaseWeight_Disaster = 0.05f;

		// Token: 0x0400910C RID: 37132
		private const float BaseWeight_Backfire = 0.1f;

		// Token: 0x0400910D RID: 37133
		private const float BaseWeight_TalksFlounder = 0.2f;

		// Token: 0x0400910E RID: 37134
		private const float BaseWeight_Success = 0.55f;

		// Token: 0x0400910F RID: 37135
		private const float BaseWeight_Triumph = 0.1f;

		// Token: 0x04009110 RID: 37136
		public const float LeaderOffset = 0.05f;

		// Token: 0x04009111 RID: 37137
		private static List<Pair<Action, float>> tmpPossibleOutcomes = new List<Pair<Action, float>>();
	}
}
