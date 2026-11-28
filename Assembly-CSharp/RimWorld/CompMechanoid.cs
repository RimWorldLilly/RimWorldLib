using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020018E8 RID: 6376
	public class CompMechanoid : CompCanBeDormant
	{
		// Token: 0x17001B87 RID: 7047
		// (get) Token: 0x06009FC5 RID: 40901 RVA: 0x00394184 File Offset: 0x00392384
		public bool Deactivated
		{
			get
			{
				if (this.deactivated)
				{
					return true;
				}
				Faction ofMechanoids = Faction.OfMechanoids;
				return ofMechanoids != null && ofMechanoids.deactivated && (this.parent.Faction == Faction.OfMechanoids || this.parent.Faction == null);
			}
		}

		// Token: 0x17001B88 RID: 7048
		// (get) Token: 0x06009FC6 RID: 40902 RVA: 0x003941D2 File Offset: 0x003923D2
		protected override bool ShowZs
		{
			get
			{
				return !this.Deactivated && base.Props.showSleepingZs && (base.Props.delayedWakeUpDoesZs || this.wakeUpOnTick == int.MinValue);
			}
		}

		// Token: 0x17001B89 RID: 7049
		// (get) Token: 0x06009FC7 RID: 40903 RVA: 0x00394208 File Offset: 0x00392408
		protected override JobDef SleepJob
		{
			get
			{
				Faction faction = this.parent.Faction;
				if (faction == null || !faction.deactivated)
				{
					return base.SleepJob;
				}
				return JobDefOf.Deactivated;
			}
		}

		// Token: 0x17001B8A RID: 7050
		// (get) Token: 0x06009FC8 RID: 40904 RVA: 0x00394238 File Offset: 0x00392438
		private Pawn Pawn
		{
			get
			{
				return this.parent as Pawn;
			}
		}

		// Token: 0x17001B8B RID: 7051
		// (get) Token: 0x06009FC9 RID: 40905 RVA: 0x00394245 File Offset: 0x00392445
		private Building Building
		{
			get
			{
				return this.parent as Building;
			}
		}

		// Token: 0x06009FCA RID: 40906 RVA: 0x00394252 File Offset: 0x00392452
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", true, false);
			Scribe_Values.Look<bool>(ref this.deactivated, "deactivated", false, false);
			Scribe_References.Look<Thing>(ref this.chargerBeforeTakeoff, "chargerBeforeTakeoff", false);
		}

		// Token: 0x06009FCB RID: 40907 RVA: 0x0039428F File Offset: 0x0039248F
		public override void CompTick()
		{
			base.CompTick();
			if (this.Deactivated && this.active)
			{
				this.ToSleep();
			}
		}

		// Token: 0x06009FCC RID: 40908 RVA: 0x003942B0 File Offset: 0x003924B0
		public void Deactivate()
		{
			this.deactivated = true;
			this.ToSleep();
			this.Pawn.health.CheckForStateChange(null, null);
		}

		// Token: 0x06009FCD RID: 40909 RVA: 0x003942E4 File Offset: 0x003924E4
		public override void ToSleep()
		{
			base.ToSleep();
			Pawn pawn = this.Pawn;
			if (pawn != null)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			Pawn pawn2 = this.Pawn;
			if (pawn2 != null)
			{
				Lord lord = pawn2.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.Pawn, PawnLostCondition.Incapped, null);
				}
			}
			Building building = this.Building;
			if (building != null)
			{
				Lord lord2 = building.GetLord();
				if (lord2 != null)
				{
					lord2.Notify_BuildingLost(this.Building, null);
				}
			}
			this.active = false;
		}

		// Token: 0x06009FCE RID: 40910 RVA: 0x0039436F File Offset: 0x0039256F
		public override void WakeUp()
		{
			if (this.Deactivated)
			{
				return;
			}
			base.WakeUp();
			this.active = true;
		}

		// Token: 0x06009FCF RID: 40911 RVA: 0x00394387 File Offset: 0x00392587
		public override void PreSwapMap()
		{
			Pawn pawn = this.Pawn;
			Thing thing;
			if (pawn == null)
			{
				thing = null;
			}
			else
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs == null)
				{
					thing = null;
				}
				else
				{
					Need_MechEnergy energy = needs.energy;
					thing = ((energy != null) ? energy.currentCharger : null);
				}
			}
			this.chargerBeforeTakeoff = thing;
		}

		// Token: 0x06009FD0 RID: 40912 RVA: 0x003943BC File Offset: 0x003925BC
		public override void PostSwapMap()
		{
			if (this.chargerBeforeTakeoff != null)
			{
				Job newJob = JobMaker.MakeJob(JobDefOf.MechCharge, this.chargerBeforeTakeoff);
				this.Pawn.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
				this.chargerBeforeTakeoff = null;
			}
		}

		// Token: 0x06009FD1 RID: 40913 RVA: 0x0039441C File Offset: 0x0039261C
		public override string CompInspectStringExtra()
		{
			if (this.Deactivated)
			{
				return "Deactivated".Translate() + ".";
			}
			return null;
		}

		// Token: 0x04005E7A RID: 24186
		private bool active = true;

		// Token: 0x04005E7B RID: 24187
		private bool deactivated;

		// Token: 0x04005E7C RID: 24188
		private Thing chargerBeforeTakeoff;
	}
}
