using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020004A3 RID: 1187
	public class Hediff_BandNode : Hediff
	{
		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060020D7 RID: 8407 RVA: 0x000C4407 File Offset: 0x000C2607
		public int AdditionalBandwidth
		{
			get
			{
				return this.cachedTunedBandNodesCount;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060020D8 RID: 8408 RVA: 0x000C440F File Offset: 0x000C260F
		public override bool ShouldRemove
		{
			get
			{
				return this.cachedTunedBandNodesCount == 0;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060020D9 RID: 8409 RVA: 0x000C441C File Offset: 0x000C261C
		public override HediffStage CurStage
		{
			get
			{
				if (this.curStage == null && this.cachedTunedBandNodesCount > 0)
				{
					StatModifier statModifier = new StatModifier();
					statModifier.stat = StatDefOf.MechBandwidth;
					statModifier.value = (float)this.cachedTunedBandNodesCount;
					this.curStage = new HediffStage();
					this.curStage.statOffsets = new List<StatModifier>
					{
						statModifier
					};
				}
				return this.curStage;
			}
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x000C4480 File Offset: 0x000C2680
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			if (this.pawn.IsHashIntervalTick(60, delta))
			{
				this.RecacheBandNodes();
			}
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000C449F File Offset: 0x000C269F
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.RecacheBandNodes();
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000C44B0 File Offset: 0x000C26B0
		public void RecacheBandNodes()
		{
			int num = this.cachedTunedBandNodesCount;
			this.cachedTunedBandNodesCount = 0;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Building thing in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.BandNode))
				{
					if (thing.TryGetComp<CompBandNode>().tunedTo == this.pawn && thing.TryGetComp<CompPowerTrader>().PowerOn)
					{
						this.cachedTunedBandNodesCount++;
					}
				}
			}
			if (num != this.cachedTunedBandNodesCount)
			{
				this.curStage = null;
				Pawn_MechanitorTracker mechanitor = this.pawn.mechanitor;
				if (mechanitor == null)
				{
					return;
				}
				mechanitor.Notify_BandwidthChanged();
			}
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000C4588 File Offset: 0x000C2788
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.cachedTunedBandNodesCount, "cachedTunedBandNodesCount", 0, false);
		}

		// Token: 0x04001932 RID: 6450
		private const int BandNodeCheckInterval = 60;

		// Token: 0x04001933 RID: 6451
		private int cachedTunedBandNodesCount;

		// Token: 0x04001934 RID: 6452
		private HediffStage curStage;
	}
}
