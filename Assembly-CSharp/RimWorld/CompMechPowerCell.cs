using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F1 RID: 6385
	public class CompMechPowerCell : ThingComp
	{
		// Token: 0x17001B98 RID: 7064
		// (get) Token: 0x0600A014 RID: 40980 RVA: 0x003958CF File Offset: 0x00393ACF
		public CompProperties_MechPowerCell Props
		{
			get
			{
				return (CompProperties_MechPowerCell)this.props;
			}
		}

		// Token: 0x17001B99 RID: 7065
		// (get) Token: 0x0600A015 RID: 40981 RVA: 0x003958DC File Offset: 0x00393ADC
		public float PercentFull
		{
			get
			{
				return (float)this.powerTicksLeft / (float)this.Props.totalPowerTicks;
			}
		}

		// Token: 0x17001B9A RID: 7066
		// (get) Token: 0x0600A016 RID: 40982 RVA: 0x003958F2 File Offset: 0x00393AF2
		public int PowerTicksLeft
		{
			get
			{
				return this.powerTicksLeft;
			}
		}

		// Token: 0x0600A018 RID: 40984 RVA: 0x00395904 File Offset: 0x00393B04
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotechOrAnomalyOrOdyssey("Mech power cell"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.powerTicksLeft = this.Props.totalPowerTicks;
			}
		}

		// Token: 0x0600A019 RID: 40985 RVA: 0x00395952 File Offset: 0x00393B52
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompMechPowerCell.<CompGetGizmosExtra>d__11 <CompGetGizmosExtra>d__ = new CompMechPowerCell.<CompGetGizmosExtra>d__11(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A01A RID: 40986 RVA: 0x00395964 File Offset: 0x00393B64
		public override void CompTick()
		{
			base.CompTick();
			if (this.depleted)
			{
				return;
			}
			this.powerTicksLeft--;
			if (this.powerTicksLeft <= 0)
			{
				if (this.Props.killWhenDepleted)
				{
					this.KillPowerProcessor();
					return;
				}
				this.powerTicksLeft = 0;
				this.depleted = true;
			}
		}

		// Token: 0x0600A01B RID: 40987 RVA: 0x003959BC File Offset: 0x00393BBC
		private void KillPowerProcessor()
		{
			Pawn pawn = (Pawn)this.parent;
			List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				BodyPartRecord bodyPartRecord = allParts[i];
				if (bodyPartRecord.def.tags.Contains(BodyPartTagDefOf.BloodPumpingSource))
				{
					pawn.health.AddHediff(HediffDefOf.MissingBodyPart, bodyPartRecord, null, null);
				}
			}
			if (!pawn.Dead)
			{
				pawn.Kill(null, null);
			}
		}

		// Token: 0x0600A01C RID: 40988 RVA: 0x00395A51 File Offset: 0x00393C51
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.powerTicksLeft, "powerTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.depleted, "depleted", false, false);
		}

		// Token: 0x04005EB0 RID: 24240
		private int powerTicksLeft;

		// Token: 0x04005EB1 RID: 24241
		public bool depleted;

		// Token: 0x04005EB2 RID: 24242
		private MechPowerCellGizmo gizmo;
	}
}
