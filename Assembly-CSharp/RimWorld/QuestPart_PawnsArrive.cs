using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001117 RID: 4375
	public class QuestPart_PawnsArrive : QuestPart
	{
		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x06006D6F RID: 28015 RVA: 0x00265464 File Offset: 0x00263664
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_PawnsArrive.<get_QuestLookTargets>d__12 <get_QuestLookTargets>d__ = new QuestPart_PawnsArrive.<get_QuestLookTargets>d__12(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x06006D70 RID: 28016 RVA: 0x00265474 File Offset: 0x00263674
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, false);
			}
		}

		// Token: 0x06006D71 RID: 28017 RVA: 0x00265488 File Offset: 0x00263688
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag != this.inSignal)
			{
				return;
			}
			this.pawns.RemoveAll((Pawn x) => x.Destroyed);
			if (this.mapParent == null || !this.mapParent.HasMap || !this.quest.IsParentSuitableForQuest(this.mapParent))
			{
				this.mapParent = this.quest.TryFindNewSuitableMapParentForRetarget();
				if (this.spawnNear.IsValid)
				{
					this.spawnNear = IntVec3.Invalid;
				}
			}
			if (this.mapParent == null || !this.mapParent.HasMap || !this.pawns.Any<Pawn>())
			{
				return;
			}
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.joinPlayer && this.pawns[i].Faction != Faction.OfPlayer)
				{
					this.pawns[i].SetFaction(Faction.OfPlayer, null);
				}
			}
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = this.mapParent.Map;
			incidentParms.spawnCenter = this.spawnNear;
			PawnsArrivalModeDef pawnsArrivalModeDef = this.arrivalMode ?? PawnsArrivalModeDefOf.EdgeWalkIn;
			if (!pawnsArrivalModeDef.Worker.CanUseOnMap(this.mapParent.Map))
			{
				foreach (PawnsArrivalModeDef pawnsArrivalModeDef2 in DefDatabase<PawnsArrivalModeDef>.AllDefsListForReading.InRandomOrder(null))
				{
					if (pawnsArrivalModeDef2.canBeBackup && pawnsArrivalModeDef2.Worker.CanUseOnMap(this.mapParent.Map))
					{
						pawnsArrivalModeDef = pawnsArrivalModeDef2;
						break;
					}
				}
			}
			if (!pawnsArrivalModeDef.Worker.CanUseOnMap(this.mapParent.Map))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(90, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to do pawns arrive on map ");
				defaultInterpolatedStringHandler.AppendFormatted<Map>(this.mapParent.Map);
				defaultInterpolatedStringHandler.AppendLiteral(" but could not find a legal arrival mode, current method: ");
				defaultInterpolatedStringHandler.AppendFormatted(pawnsArrivalModeDef.defName);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			pawnsArrivalModeDef.Worker.TryResolveRaidSpawnCenter(incidentParms);
			pawnsArrivalModeDef.Worker.Arrive(this.pawns, incidentParms);
			if (this.sendStandardLetter)
			{
				TaggedString taggedString;
				TaggedString taggedString2;
				if (this.joinPlayer && this.pawns.Count == 1 && this.pawns[0].RaceProps.Humanlike)
				{
					taggedString = "LetterRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
					taggedString2 = "LetterLabelRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
					PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref taggedString2, this.pawns[0]);
				}
				else
				{
					if (this.joinPlayer)
					{
						taggedString = "LetterPawnsArriveAndJoin".Translate(GenLabel.ThingsLabel(this.pawns.Cast<Thing>(), "  - "));
						taggedString2 = "LetterLabelPawnsArriveAndJoin".Translate();
					}
					else
					{
						taggedString = "LetterPawnsArrive".Translate(GenLabel.ThingsLabel(this.pawns.Cast<Thing>(), "  - "));
						taggedString2 = "LetterLabelPawnsArrive".Translate();
					}
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(this.pawns, ref taggedString2, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
				}
				taggedString2 = (this.customLetterLabel.NullOrEmpty() ? taggedString2 : this.customLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
				taggedString = (this.customLetterText.NullOrEmpty() ? taggedString : this.customLetterText.Formatted(taggedString.Named("BASETEXT")));
				Find.LetterStack.ReceiveLetter(taggedString2, taggedString, this.customLetterDef ?? LetterDefOf.PositiveEvent, this.pawns, null, this.quest, null, null, 0, true);
			}
		}

		// Token: 0x06006D72 RID: 28018 RVA: 0x002658A0 File Offset: 0x00263AA0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.spawnNear, "spawnNear", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.addPawnsToLookTargets, "addPawnsToLookTargets", true, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006D73 RID: 28019 RVA: 0x002659B4 File Offset: 0x00263BB4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				pawn.relations.everSeenByPlayer = true;
				if (!pawn.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
				this.pawns.Add(pawn);
				this.arrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.joinPlayer = true;
			}
		}

		// Token: 0x06006D74 RID: 28020 RVA: 0x00265AD1 File Offset: 0x00263CD1
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06006D75 RID: 28021 RVA: 0x00265ADF File Offset: 0x00263CDF
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040045B1 RID: 17841
		public string inSignal;

		// Token: 0x040045B2 RID: 17842
		public MapParent mapParent;

		// Token: 0x040045B3 RID: 17843
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040045B4 RID: 17844
		public PawnsArrivalModeDef arrivalMode;

		// Token: 0x040045B5 RID: 17845
		public IntVec3 spawnNear = IntVec3.Invalid;

		// Token: 0x040045B6 RID: 17846
		public bool joinPlayer;

		// Token: 0x040045B7 RID: 17847
		public bool addPawnsToLookTargets = true;

		// Token: 0x040045B8 RID: 17848
		public string customLetterText;

		// Token: 0x040045B9 RID: 17849
		public string customLetterLabel;

		// Token: 0x040045BA RID: 17850
		public LetterDef customLetterDef;

		// Token: 0x040045BB RID: 17851
		public bool sendStandardLetter = true;
	}
}
