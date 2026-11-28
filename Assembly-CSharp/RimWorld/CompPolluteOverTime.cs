using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001939 RID: 6457
	public class CompPolluteOverTime : ThingComp
	{
		// Token: 0x17001BFC RID: 7164
		// (get) Token: 0x0600A1EF RID: 41455 RVA: 0x0039DF28 File Offset: 0x0039C128
		private CompProperties_PolluteOverTime Props
		{
			get
			{
				return (CompProperties_PolluteOverTime)this.props;
			}
		}

		// Token: 0x17001BFD RID: 7165
		// (get) Token: 0x0600A1F0 RID: 41456 RVA: 0x0039DF35 File Offset: 0x0039C135
		private int TicksToPolluteCell
		{
			get
			{
				return 60000 / this.Props.cellsToPollutePerDay;
			}
		}

		// Token: 0x0600A1F1 RID: 41457 RVA: 0x0039DF48 File Offset: 0x0039C148
		public override void CompTick()
		{
			if (this.parent.Spawned && this.parent.IsHashIntervalTick(this.TicksToPolluteCell))
			{
				this.Pollute();
			}
		}

		// Token: 0x0600A1F2 RID: 41458 RVA: 0x0039DF70 File Offset: 0x0039C170
		private void Pollute()
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			int num = GenRadial.NumCellsInRadius(GenRadial.MaxRadialPatternRadius - 1f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
				if (!intVec.IsPolluted(this.parent.Map) && intVec.CanPollute(this.parent.Map))
				{
					intVec.Pollute(this.parent.Map, false);
					this.parent.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(intVec, this.parent.Map, Vector3.zero, 1f), intVec, 45);
					return;
				}
			}
		}

		// Token: 0x0600A1F3 RID: 41459 RVA: 0x0039E035 File Offset: 0x0039C235
		public override string CompInspectStringExtra()
		{
			return "TilePollution".Translate() + ": " + "CellsPerDay".Translate(this.Props.cellsToPollutePerDay);
		}

		// Token: 0x0600A1F4 RID: 41460 RVA: 0x0039E06F File Offset: 0x0039C26F
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompPolluteOverTime.<CompGetGizmosExtra>d__7 <CompGetGizmosExtra>d__ = new CompPolluteOverTime.<CompGetGizmosExtra>d__7(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}
	}
}
