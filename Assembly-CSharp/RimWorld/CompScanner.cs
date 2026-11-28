using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019F7 RID: 6647
	public abstract class CompScanner : ThingComp
	{
		// Token: 0x17001D4D RID: 7501
		// (get) Token: 0x0600A764 RID: 42852 RVA: 0x003BA08F File Offset: 0x003B828F
		public CompProperties_Scanner Props
		{
			get
			{
				return (CompProperties_Scanner)this.props;
			}
		}

		// Token: 0x17001D4E RID: 7502
		// (get) Token: 0x0600A765 RID: 42853 RVA: 0x003BA09C File Offset: 0x003B829C
		public virtual AcceptanceReport CanUseNow
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				if (this.powerComp != null && !this.powerComp.PowerOn)
				{
					return false;
				}
				if (RoofUtility.IsAnyCellUnderRoof(this.parent))
				{
					return "CannotUseScannerRoofed".Translate();
				}
				if (this.forbiddable != null && this.forbiddable.Forbidden)
				{
					return false;
				}
				return this.parent.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x0600A766 RID: 42854 RVA: 0x003BA12C File Offset: 0x003B832C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.daysWorkingSinceLastFinding, "daysWorkingSinceLastFinding", 0f, false);
			Scribe_Values.Look<float>(ref this.lastUserSpeed, "lastUserSpeed", 0f, false);
			Scribe_Values.Look<float>(ref this.lastScanTick, "lastScanTick", 0f, false);
		}

		// Token: 0x0600A767 RID: 42855 RVA: 0x003BA181 File Offset: 0x003B8381
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.forbiddable = this.parent.GetComp<CompForbiddable>();
		}

		// Token: 0x0600A768 RID: 42856 RVA: 0x003BA1AC File Offset: 0x003B83AC
		public void Used(Pawn worker)
		{
			if (!this.CanUseNow)
			{
				Log.Error("Used while CanUseNow is false.");
			}
			this.lastScanTick = (float)Find.TickManager.TicksGame;
			this.lastUserSpeed = 1f;
			if (this.Props.scanSpeedStat != null)
			{
				this.lastUserSpeed = worker.GetStatValue(this.Props.scanSpeedStat, true, -1);
			}
			this.daysWorkingSinceLastFinding += this.lastUserSpeed / 60000f;
			if (this.TickDoesFind(this.lastUserSpeed))
			{
				this.DoFind(worker);
				this.daysWorkingSinceLastFinding = 0f;
			}
		}

		// Token: 0x0600A769 RID: 42857 RVA: 0x003BA24C File Offset: 0x003B844C
		protected virtual bool TickDoesFind(float scanSpeed)
		{
			return this.parent.IsHashIntervalTick(59) && (Rand.MTBEventOccurs(this.Props.scanFindMtbDays / scanSpeed, 60000f, 59f) || (this.Props.scanFindGuaranteedDays > 0f && this.daysWorkingSinceLastFinding >= this.Props.scanFindGuaranteedDays));
		}

		// Token: 0x0600A76A RID: 42858 RVA: 0x003BA2B0 File Offset: 0x003B84B0
		public override string CompInspectStringExtra()
		{
			string t = "";
			if (this.lastScanTick > (float)(Find.TickManager.TicksGame - 30))
			{
				t += "UserScanAbility".Translate() + ": " + this.lastUserSpeed.ToStringPercent() + "\n" + "ScanAverageInterval".Translate() + ": " + "PeriodDays".Translate((this.Props.scanFindMtbDays / this.lastUserSpeed).ToString("F1")) + "\n";
			}
			return t + "ScanningProgressToGuaranteedFind".Translate() + ": " + (this.daysWorkingSinceLastFinding / this.Props.scanFindGuaranteedDays).ToStringPercent();
		}

		// Token: 0x0600A76B RID: 42859
		protected abstract void DoFind(Pawn worker);

		// Token: 0x0600A76C RID: 42860 RVA: 0x003BA3A7 File Offset: 0x003B85A7
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompScanner.<CompGetGizmosExtra>d__15 <CompGetGizmosExtra>d__ = new CompScanner.<CompGetGizmosExtra>d__15(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0400628E RID: 25230
		protected float daysWorkingSinceLastFinding;

		// Token: 0x0400628F RID: 25231
		protected float lastUserSpeed = 1f;

		// Token: 0x04006290 RID: 25232
		protected float lastScanTick = -1f;

		// Token: 0x04006291 RID: 25233
		protected CompPowerTrader powerComp;

		// Token: 0x04006292 RID: 25234
		protected CompForbiddable forbiddable;
	}
}
