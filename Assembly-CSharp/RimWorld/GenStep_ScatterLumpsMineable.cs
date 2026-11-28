using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C5 RID: 4805
	public class GenStep_ScatterLumpsMineable : GenStep_Scatterer
	{
		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x06007739 RID: 30521 RVA: 0x002A0E34 File Offset: 0x0029F034
		public override int SeedPart
		{
			get
			{
				return 920906419;
			}
		}

		// Token: 0x0600773A RID: 30522 RVA: 0x002A0E3C File Offset: 0x0029F03C
		public override void Generate(Map map, GenStepParams parms)
		{
			this.minSpacing = 5f;
			this.warnOnFail = false;
			int num = this.CalculateFinalCount(map);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec;
				if (!this.TryFindScatterCell(map, out intVec))
				{
					return;
				}
				this.ScatterAt(intVec, map, parms, 1);
				this.usedSpots.Add(intVec);
			}
			this.usedSpots.Clear();
		}

		// Token: 0x0600773B RID: 30523 RVA: 0x002A0E9C File Offset: 0x0029F09C
		protected override int CalculateFinalCount(Map map)
		{
			if (this.count >= 0)
			{
				return Mathf.RoundToInt((float)this.count * this.GetPlacementFactor(map));
			}
			float num = this.countPer10kCellsRange.RandomInRange;
			if (!map.IsStartingMap && this.useNomadicMineables)
			{
				num *= Current.Game.storyteller.difficulty.nomadicMineableResourcesFactor;
			}
			return Mathf.RoundToInt((float)GenStep_Scatterer.CountFromPer10kCells(num, map, -1) * this.GetPlacementFactor(map));
		}

		// Token: 0x0600773C RID: 30524 RVA: 0x002A0F10 File Offset: 0x0029F110
		protected virtual ThingDef ChooseThingDef()
		{
			if (this.forcedDefToScatter != null)
			{
				return this.forcedDefToScatter;
			}
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeightWithFallback(delegate(ThingDef d)
			{
				if (d.building == null)
				{
					return 0f;
				}
				if (d.building.mineableThing != null && d.building.mineableThing.BaseMarketValue > this.maxValue)
				{
					return 0f;
				}
				return d.building.mineableScatterCommonality;
			}, null);
		}

		// Token: 0x0600773D RID: 30525 RVA: 0x002A0F38 File Offset: 0x0029F138
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (base.NearUsedSpot(c, this.CalculateFinalMinSpacing(map)))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building.isNaturalRock;
		}

		// Token: 0x0600773E RID: 30526 RVA: 0x002A0F78 File Offset: 0x0029F178
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			GenStep_ScatterLumpsMineable.<>c__DisplayClass11_0 CS$<>8__locals1 = new GenStep_ScatterLumpsMineable.<>c__DisplayClass11_0();
			if (Current.ProgramState == ProgramState.MapInitializing && Find.Storyteller.def.tutorialMode)
			{
				return;
			}
			ThingDef thingDef = this.ChooseThingDef();
			if (thingDef == null)
			{
				return;
			}
			int numCells = (this.forcedLumpSize > 0) ? this.forcedLumpSize : thingDef.building.mineableScatterLumpSizeRange.RandomInRange;
			this.recentLumpCells.Clear();
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			foreach (IntVec3 intVec in GridShapeMaker.IrregularLump(c, map, numCells, new Predicate<IntVec3>(CS$<>8__locals1.<ScatterAt>g__Validator|0)))
			{
				GenSpawn.Spawn(thingDef, intVec, map, WipeMode.Vanish);
				this.recentLumpCells.Add(intVec);
			}
		}

		// Token: 0x04004ACA RID: 19146
		public ThingDef forcedDefToScatter;

		// Token: 0x04004ACB RID: 19147
		public int forcedLumpSize;

		// Token: 0x04004ACC RID: 19148
		public float maxValue = float.MaxValue;

		// Token: 0x04004ACD RID: 19149
		public bool useNomadicMineables;

		// Token: 0x04004ACE RID: 19150
		[Unsaved(false)]
		protected readonly List<IntVec3> recentLumpCells = new List<IntVec3>();
	}
}
