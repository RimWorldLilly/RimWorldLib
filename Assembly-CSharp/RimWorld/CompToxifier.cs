using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019C4 RID: 6596
	public class CompToxifier : ThingComp
	{
		// Token: 0x17001CD9 RID: 7385
		// (get) Token: 0x0600A58B RID: 42379 RVA: 0x003B05D8 File Offset: 0x003AE7D8
		private CompProperties_Toxifier Props
		{
			get
			{
				return (CompProperties_Toxifier)this.props;
			}
		}

		// Token: 0x17001CDA RID: 7386
		// (get) Token: 0x0600A58C RID: 42380 RVA: 0x003B05E8 File Offset: 0x003AE7E8
		public bool CanPolluteNow
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				if (!this.nextPollutionCell.CanPollute(this.parent.Map))
				{
					this.UpdateNextPolluteCell();
				}
				return this.nextPollutionCell.CanPollute(this.parent.Map);
			}
		}

		// Token: 0x0600A58D RID: 42381 RVA: 0x003B0638 File Offset: 0x003AE838
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Toxifier"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.pollutingProgressTicks = this.Props.pollutionIntervalTicks;
			}
		}

		// Token: 0x0600A58E RID: 42382 RVA: 0x003B0688 File Offset: 0x003AE888
		public override string CompInspectStringExtra()
		{
			TaggedString t = base.CompInspectStringExtra();
			if (this.CanPolluteNow)
			{
				if (!t.NullOrEmpty())
				{
					t += "\n";
				}
				t += "PollutingTerrainProgress".Translate() + ": " + (this.Props.pollutionIntervalTicks - this.pollutingProgressTicks).ToStringTicksToPeriod(true, false, true, true, false) + " (" + this.Props.cellsToPollute.ToString() + " " + "TilesLower".Translate() + ")";
			}
			return t.Resolve();
		}

		// Token: 0x0600A58F RID: 42383 RVA: 0x003B0748 File Offset: 0x003AE948
		private void UpdateNextPolluteCell()
		{
			if (this.nextPollutionCell.CanPollute(this.parent.Map))
			{
				return;
			}
			this.nextPollutionCell = IntVec3.Invalid;
			int num = GenRadial.NumCellsInRadius(this.Props.radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = this.parent.Position + GenRadial.RadialPattern[i];
				if (this.<UpdateNextPolluteCell>g__NextPolluteCellValidator|8_0(c))
				{
					this.nextPollutionCell = c;
					return;
				}
			}
		}

		// Token: 0x0600A590 RID: 42384 RVA: 0x003B07C4 File Offset: 0x003AE9C4
		private void PolluteNextCell(bool silent = false)
		{
			if (this.CanPolluteNow)
			{
				int num = GenRadial.NumCellsInRadius(this.Props.radius);
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					IntVec3 c = this.parent.Position + GenRadial.RadialPattern[i];
					if (c.InBounds(this.parent.Map) && c.CanPollute(this.parent.Map))
					{
						c.Pollute(this.parent.Map, silent);
						num2++;
						if (num2 >= this.Props.cellsToPollute)
						{
							break;
						}
					}
				}
				if (!silent)
				{
					this.DoEffects();
				}
			}
		}

		// Token: 0x0600A591 RID: 42385 RVA: 0x003B086A File Offset: 0x003AEA6A
		private void DoEffects()
		{
			FleckMaker.Static(this.parent.TrueCenter(), this.parent.Map, FleckDefOf.Fleck_ToxifierPollutionSource, 1f);
			SoundDefOf.Toxifier_Pollute.PlayOneShot(this.parent);
		}

		// Token: 0x0600A592 RID: 42386 RVA: 0x003B08A6 File Offset: 0x003AEAA6
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompToxifier.<CompGetGizmosExtra>d__11 <CompGetGizmosExtra>d__ = new CompToxifier.<CompGetGizmosExtra>d__11(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A593 RID: 42387 RVA: 0x003B08B6 File Offset: 0x003AEAB6
		public override void CompTickInterval(int delta)
		{
			if (!this.CanPolluteNow)
			{
				return;
			}
			this.pollutingProgressTicks += delta;
			if (this.pollutingProgressTicks >= this.Props.pollutionIntervalTicks)
			{
				this.pollutingProgressTicks = 0;
				this.PolluteNextCell(false);
			}
		}

		// Token: 0x0600A594 RID: 42388 RVA: 0x003B08F0 File Offset: 0x003AEAF0
		public override void PostExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.nextPollutionCell, "nextPollutionCell", IntVec3.Invalid, false);
			Scribe_Values.Look<int>(ref this.pollutingProgressTicks, "pollutingProgressTicks", 0, false);
		}

		// Token: 0x0600A596 RID: 42390 RVA: 0x003B092D File Offset: 0x003AEB2D
		[CompilerGenerated]
		private bool <UpdateNextPolluteCell>g__NextPolluteCellValidator|8_0(IntVec3 c)
		{
			return c.InBounds(this.parent.Map) && c.CanPollute(this.parent.Map);
		}

		// Token: 0x040061BA RID: 25018
		private IntVec3 nextPollutionCell = IntVec3.Invalid;

		// Token: 0x040061BB RID: 25019
		private int pollutingProgressTicks;
	}
}
