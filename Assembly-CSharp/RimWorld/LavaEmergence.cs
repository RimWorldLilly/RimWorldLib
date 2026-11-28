using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AF7 RID: 6903
	public class LavaEmergence : Thing
	{
		// Token: 0x17001FD1 RID: 8145
		// (get) Token: 0x0600B06B RID: 45163 RVA: 0x003E24CB File Offset: 0x003E06CB
		protected virtual IntRange PoolSizeRange
		{
			get
			{
				return new IntRange(450, 600);
			}
		}

		// Token: 0x17001FD2 RID: 8146
		// (get) Token: 0x0600B06C RID: 45164 RVA: 0x003E24DC File Offset: 0x003E06DC
		protected virtual bool FireLetter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600B06D RID: 45165 RVA: 0x003E24E0 File Offset: 0x003E06E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.openCells, "openCells", LookMode.Value);
			Scribe_Collections.Look<IntVec3>(ref this.lavaCells, "lavaCells", LookMode.Value);
			Scribe_Values.Look<int>(ref this.poolSize, "poolSize", 0, false);
			Scribe_Values.Look<int>(ref this.coolDelay, "coolDelay", 0, false);
		}

		// Token: 0x0600B06E RID: 45166 RVA: 0x003E253C File Offset: 0x003E073C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			this.openCells.AddRange(this.GetInitialCells(map));
			if (this.openCells.Count == 0)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.poolSize = ((this.forcePoolSize > 0) ? this.forcePoolSize : this.PoolSizeRange.RandomInRange);
			this.coolDelay = ((this.forceCoolDelay > 0) ? this.forceCoolDelay : LavaEmergence.LavaBeginCoolingDelay.RandomInRange);
			if (this.FireLetter)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelLavaEmergence".Translate(), "LetterLavaEmergence".Translate(), LetterDefOf.ThreatSmall, new LookTargets(this.openCells.First<IntVec3>(), base.Map), null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600B06F RID: 45167 RVA: 0x003E260C File Offset: 0x003E080C
		protected virtual IEnumerable<IntVec3> GetInitialCells(Map map)
		{
			LavaEmergence.<GetInitialCells>d__16 <GetInitialCells>d__ = new LavaEmergence.<GetInitialCells>d__16(-2);
			<GetInitialCells>d__.<>3__map = map;
			return <GetInitialCells>d__;
		}

		// Token: 0x0600B070 RID: 45168 RVA: 0x003E261C File Offset: 0x003E081C
		protected override void Tick()
		{
			if (!this.IsHashIntervalTick(480))
			{
				return;
			}
			this.SpreadLava();
			if (this.lavaCells.Count >= this.poolSize || this.openCells.Count == 0)
			{
				this.BeginCooling();
			}
		}

		// Token: 0x0600B071 RID: 45169 RVA: 0x003E2658 File Offset: 0x003E0858
		protected void SpreadLava()
		{
			if (this.openCells.Count == 0)
			{
				return;
			}
			IntVec3 intVec;
			if (!this.openCells.TryRandomElementByWeight((IntVec3 c) => (float)(Mathf.Max(this.AdjacentLavaCells(c), 1) * 4), out intVec))
			{
				intVec = this.openCells.RandomElement<IntVec3>();
			}
			this.openCells.Remove(intVec);
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				IntVec3 intVec2 = intVec + b;
				if (!this.openCells.Contains(intVec2) && !this.lavaCells.Contains(intVec2) && this.CanLavaSpreadInto(intVec2))
				{
					this.openCells.Add(intVec2);
				}
			}
			base.Map.terrainGrid.SetTempTerrain(intVec, TerrainDefOf.LavaShallow);
			this.lavaCells.Add(intVec);
		}

		// Token: 0x0600B072 RID: 45170 RVA: 0x003E2728 File Offset: 0x003E0928
		protected void BeginCooling()
		{
			List<IntVec3> cells = this.lavaCells.ToList<IntVec3>();
			int num = 0;
			Func<IntVec3, float> <>9__0;
			while (!cells.Empty<IntVec3>())
			{
				IEnumerable<IntVec3> cells2 = cells;
				Func<IntVec3, float> weightSelector;
				if ((weightSelector = <>9__0) == null)
				{
					weightSelector = (<>9__0 = delegate(IntVec3 c)
					{
						int num2 = 0;
						foreach (IntVec3 b in GenAdj.CardinalDirections)
						{
							IntVec3 intVec2 = c + b;
							if (intVec2.InBounds(this.Map) && (cells.Contains(intVec2) || intVec2.GetTerrain(this.Map) == TerrainDefOf.LavaDeep))
							{
								num2++;
							}
						}
						return (float)(16 - num2 * 4);
					});
				}
				IntVec3 intVec;
				if (!cells2.TryRandomElementByWeight(weightSelector, out intVec))
				{
					intVec = cells.RandomElement<IntVec3>();
				}
				base.Map.tempTerrain.QueueRemoveTerrain(intVec, Find.TickManager.TicksGame + this.coolDelay + 400 * num);
				cells.Remove(intVec);
				num++;
			}
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0600B073 RID: 45171 RVA: 0x003E27E4 File Offset: 0x003E09E4
		private bool CanLavaSpreadInto(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && !edifice.IsClearableFreeBuilding)
			{
				return false;
			}
			TerrainDef terrain = c.GetTerrain(base.Map);
			return terrain.natural && terrain.affordances.Contains(TerrainAffordanceDefOf.Walkable) && base.Map.terrainGrid.FoundationAt(c) == null && terrain != TerrainDefOf.LavaDeep;
		}

		// Token: 0x0600B074 RID: 45172 RVA: 0x003E2868 File Offset: 0x003E0A68
		private int AdjacentLavaCells(IntVec3 c)
		{
			int num = 0;
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				IntVec3 c2 = c + b;
				if (c2.InBounds(base.Map) && c2.GetTerrain(base.Map) == TerrainDefOf.LavaShallow)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x040065D4 RID: 26068
		private static readonly IntRange EmergencePointsCountRange = new IntRange(2, 4);

		// Token: 0x040065D5 RID: 26069
		private static readonly IntRange LavaBeginCoolingDelay = new IntRange(300000, 360000);

		// Token: 0x040065D6 RID: 26070
		private const int LavaCoolInterval = 400;

		// Token: 0x040065D7 RID: 26071
		private const int ExpandIntervalTicks = 480;

		// Token: 0x040065D8 RID: 26072
		[Unsaved(false)]
		public int forcePoolSize = -1;

		// Token: 0x040065D9 RID: 26073
		[Unsaved(false)]
		public int forceCoolDelay = -1;

		// Token: 0x040065DA RID: 26074
		protected HashSet<IntVec3> openCells = new HashSet<IntVec3>();

		// Token: 0x040065DB RID: 26075
		protected HashSet<IntVec3> lavaCells = new HashSet<IntVec3>();

		// Token: 0x040065DC RID: 26076
		protected int poolSize;

		// Token: 0x040065DD RID: 26077
		private int coolDelay;
	}
}
