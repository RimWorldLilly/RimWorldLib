using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200150A RID: 5386
	public class RoyalTitlePermitWorker_CallLaborers : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x060083ED RID: 33773 RVA: 0x002F6D40 File Offset: 0x002F4F40
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.CanHitTarget(target))
			{
				if (target.IsValid && showMessages)
				{
					Messages.Message(this.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			AcceptanceReport acceptanceReport = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, this.map, null, null);
			if (!acceptanceReport.Accepted)
			{
				Messages.Message(acceptanceReport.Reason, new LookTargets(target.Cell, this.map), MessageTypeDefOf.RejectInput, false);
			}
			return acceptanceReport.Accepted;
		}

		// Token: 0x060083EE RID: 33774 RVA: 0x002F6DE2 File Offset: 0x002F4FE2
		public override void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, base.RangeClamped, Color.white, null);
			RoyalTitlePermitWorker_CallShuttle.DrawShuttleGhost(target, this.map, ThingDefOf.Shuttle, ThingDefOf.Shuttle.defaultPlacingRot);
		}

		// Token: 0x060083EF RID: 33775 RVA: 0x002F6E1B File Offset: 0x002F501B
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallLaborers(target.Cell);
		}

		// Token: 0x060083F0 RID: 33776 RVA: 0x002F6E2C File Offset: 0x002F502C
		public override void OnGUI(LocalTargetInfo target)
		{
			if (!target.IsValid || !RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, this.map, null, null).Accepted)
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
		}

		// Token: 0x060083F1 RID: 33777 RVA: 0x002F6E6C File Offset: 0x002F506C
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			RoyalTitlePermitWorker_CallLaborers.<GetRoyalAidOptions>d__5 <GetRoyalAidOptions>d__ = new RoyalTitlePermitWorker_CallLaborers.<GetRoyalAidOptions>d__5(-2);
			<GetRoyalAidOptions>d__.<>4__this = this;
			<GetRoyalAidOptions>d__.<>3__map = map;
			<GetRoyalAidOptions>d__.<>3__pawn = pawn;
			<GetRoyalAidOptions>d__.<>3__faction = faction;
			return <GetRoyalAidOptions>d__;
		}

		// Token: 0x060083F2 RID: 33778 RVA: 0x002F6E94 File Offset: 0x002F5094
		private void BeginCallLaborers(Pawn pawn, Map map, Faction faction, bool free)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				return;
			}
			this.targetingParameters = new TargetingParameters();
			this.targetingParameters.canTargetLocations = true;
			this.targetingParameters.canTargetSelf = false;
			this.targetingParameters.canTargetPawns = false;
			this.targetingParameters.canTargetFires = false;
			this.targetingParameters.canTargetBuildings = true;
			this.targetingParameters.canTargetItems = true;
			this.caller = pawn;
			this.map = map;
			this.calledFaction = faction;
			this.free = free;
			float rangeActual = base.RangeClamped;
			this.targetingParameters.validator = ((TargetInfo target) => rangeActual <= 0f || target.Cell.DistanceTo(this.caller.Position) <= rangeActual);
			Find.Targeter.BeginTargeting(this, null, false, null, null, true);
		}

		// Token: 0x060083F3 RID: 33779 RVA: 0x002F6F60 File Offset: 0x002F5160
		private void CallLaborers(IntVec3 landingCell)
		{
			QuestScriptDef permit_CallLaborers = QuestScriptDefOf.Permit_CallLaborers;
			Slate slate = new Slate();
			slate.Set<Map>("map", this.map, false);
			slate.Set<int>("laborersCount", this.def.royalAid.pawnCount, false);
			slate.Set<Faction>("permitFaction", this.calledFaction, false);
			slate.Set<PawnKindDef>("laborersPawnKind", this.def.royalAid.pawnKindDef, false);
			slate.Set<float>("laborersDurationDays", this.def.royalAid.aidDurationDays, false);
			slate.Set<IntVec3>("landingCell", landingCell, false);
			QuestUtility.GenerateQuestAndMakeAvailable(permit_CallLaborers, slate);
			this.caller.royalty.GetPermit(this.def, this.calledFaction).Notify_Used();
			if (!this.free)
			{
				this.caller.royalty.TryRemoveFavor(this.calledFaction, this.def.royalAid.favorCost);
			}
		}

		// Token: 0x04005181 RID: 20865
		private Faction calledFaction;
	}
}
