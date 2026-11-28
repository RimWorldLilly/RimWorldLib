using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001AA4 RID: 6820
	public class Building_CommsConsole : Building
	{
		// Token: 0x17001EAB RID: 7851
		// (get) Token: 0x0600AC42 RID: 44098 RVA: 0x003CC9E6 File Offset: 0x003CABE6
		public bool CanUseCommsNow
		{
			get
			{
				return (!base.Spawned || !base.Map.gameConditionManager.ElectricityDisabled(base.Map)) && (this.powerComp == null || this.powerComp.PowerOn);
			}
		}

		// Token: 0x0600AC43 RID: 44099 RVA: 0x003CCA20 File Offset: 0x003CAC20
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.GoodToKnow);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.OpeningComms, OpportunityType.GoodToKnow);
			if (this.CanUseCommsNow)
			{
				LongEventHandler.ExecuteWhenFinished(new Action(this.AnnounceTradeShips));
			}
		}

		// Token: 0x0600AC44 RID: 44100 RVA: 0x003CCA70 File Offset: 0x003CAC70
		private void UseAct(Pawn myPawn, ICommunicable commTarget)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseCommsConsole, this);
			job.commTarget = commTarget;
			myPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}

		// Token: 0x0600AC45 RID: 44101 RVA: 0x003CCAB4 File Offset: 0x003CACB4
		private FloatMenuOption GetFailureReason(Pawn myPawn)
		{
			if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (base.Spawned && base.Map.gameConditionManager.ElectricityDisabled(base.Map))
			{
				return new FloatMenuOption("CannotUseSolarFlare".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (this.powerComp != null && !this.powerComp.PowerOn)
			{
				return new FloatMenuOption("CannotUseNoPower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
			{
				return new FloatMenuOption("CannotUseReason".Translate("IncapableOfCapacity".Translate(PawnCapacityDefOf.Talking.label, myPawn.Named("PAWN"))), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!this.GetCommTargets(myPawn).Any<ICommunicable>())
			{
				return new FloatMenuOption("CannotUseReason".Translate("NoCommsTarget".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!this.CanUseCommsNow)
			{
				Log.Error(((myPawn != null) ? myPawn.ToString() : null) + " could not use comm console for unknown reason.");
				return new FloatMenuOption("Cannot use now", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return null;
		}

		// Token: 0x0600AC46 RID: 44102 RVA: 0x003CCC50 File Offset: 0x003CAE50
		public IEnumerable<ICommunicable> GetCommTargets(Pawn myPawn)
		{
			return myPawn.Map.passingShipManager.passingShips.Cast<ICommunicable>().Concat((from f in Find.FactionManager.AllFactionsVisibleInViewOrder
			where !f.temporary && !f.IsPlayer
			select f).Cast<ICommunicable>());
		}

		// Token: 0x0600AC47 RID: 44103 RVA: 0x003CCCAA File Offset: 0x003CAEAA
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			Building_CommsConsole.<GetFloatMenuOptions>d__7 <GetFloatMenuOptions>d__ = new Building_CommsConsole.<GetFloatMenuOptions>d__7(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__myPawn = myPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600AC48 RID: 44104 RVA: 0x003CCCC4 File Offset: 0x003CAEC4
		public void GiveUseCommsJob(Pawn negotiator, ICommunicable target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseCommsConsole, this);
			job.commTarget = target;
			negotiator.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}

		// Token: 0x0600AC49 RID: 44105 RVA: 0x003CCD08 File Offset: 0x003CAF08
		private void AnnounceTradeShips()
		{
			foreach (TradeShip tradeShip in from s in base.Map.passingShipManager.passingShips.OfType<TradeShip>()
			where !s.WasAnnounced
			select s)
			{
				TaggedString baseLetterText = "TraderArrival".Translate(tradeShip.name, tradeShip.def.label, (tradeShip.Faction == null) ? "TraderArrivalNoFaction".Translate() : "TraderArrivalFromFaction".Translate(tradeShip.Faction.Named("FACTION")));
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = base.Map;
				incidentParms.traderKind = tradeShip.TraderKind;
				IncidentWorker.SendIncidentLetter(tradeShip.def.LabelCap, baseLetterText, LetterDefOf.PositiveEvent, incidentParms, LookTargets.Invalid, null, Array.Empty<NamedArgument>());
				tradeShip.WasAnnounced = true;
			}
		}

		// Token: 0x0400641E RID: 25630
		private CompPowerTrader powerComp;
	}
}
