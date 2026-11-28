using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001312 RID: 4882
	public abstract class TileMutatorWorker_AncientVent : TileMutatorWorker
	{
		// Token: 0x170014A8 RID: 5288
		// (get) Token: 0x0600792D RID: 31021
		protected abstract ThingDef AncientVentDef { get; }

		// Token: 0x0600792E RID: 31022 RVA: 0x002B1030 File Offset: 0x002AF230
		public TileMutatorWorker_AncientVent(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600792F RID: 31023 RVA: 0x002B1044 File Offset: 0x002AF244
		public override void GenerateNonCriticalStructures(Map map)
		{
			TileMutatorWorker_AncientVent.<>c__DisplayClass8_0 CS$<>8__locals1 = new TileMutatorWorker_AncientVent.<>c__DisplayClass8_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.<>4__this = this;
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.numToGenerate = TileMutatorWorker_AncientVent.numVentsRange.RandomInRange;
			this.positionsToGenerate.Clear();
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			while (this.positionsToGenerate.Count < this.numToGenerate)
			{
				IntVec3 item;
				if (!CellFinder.TryFindRandomCell(CS$<>8__locals1.map, new Predicate<IntVec3>(CS$<>8__locals1.<GenerateNonCriticalStructures>g__Validator|0), out item))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(92, 3);
					defaultInterpolatedStringHandler.AppendFormatted("TileMutatorWorker_AncientVent");
					defaultInterpolatedStringHandler.AppendLiteral(" failed to find any valid cells to place at least one vent. Placed ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.positionsToGenerate.Count);
					defaultInterpolatedStringHandler.AppendLiteral(" vents, wanted to place ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.numToGenerate);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
					break;
				}
				this.positionsToGenerate.Add(item);
			}
			TerrainGrid terrainGrid = CS$<>8__locals1.map.terrainGrid;
			IntVec3 c;
			this.positionsToGenerate.RemoveWhere(delegate(IntVec3 p)
			{
				IEnumerable<IntVec3> source = GenRadial.RadialCellsAround(p, 0f, 8.9f);
				Func<IntVec3, bool> predicate;
				if ((predicate = CS$<>8__locals1.<>9__4) == null)
				{
					predicate = (CS$<>8__locals1.<>9__4 = ((IntVec3 c) => CS$<>8__locals1.map.terrainGrid.TerrainAt(c).IsWater));
				}
				return source.Any(predicate);
			});
			foreach (IntVec3 center in this.positionsToGenerate)
			{
				using (IEnumerator<IntVec3> enumerator2 = GenRadial.RadialCellsAround(center, 0f, 8.9f).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						c = enumerator2.Current;
						if (c.InBounds(CS$<>8__locals1.map) && c.GetEdifice(CS$<>8__locals1.map) == null && !Rand.Chance(0f))
						{
							terrainGrid.SetTerrain(c, TerrainDefOf.AncientMegastructure);
						}
					}
				}
			}
			foreach (IntVec3 loc in this.positionsToGenerate)
			{
				Thing thing = ThingMaker.MakeThing(this.AncientVentDef, null);
				GenSpawn.Spawn(thing, loc, CS$<>8__locals1.map, WipeMode.Vanish);
				CS$<>8__locals1.usedRects.Add(thing.OccupiedRect());
			}
		}

		// Token: 0x04004C07 RID: 19463
		private const float RadiusToClear = 8.9f;

		// Token: 0x04004C08 RID: 19464
		private const float ChanceNoMegaStructure = 0f;

		// Token: 0x04004C09 RID: 19465
		private static readonly IntRange numVentsRange = new IntRange(5, 9);

		// Token: 0x04004C0A RID: 19466
		private int numToGenerate;

		// Token: 0x04004C0B RID: 19467
		private List<IntVec3> positionsToGenerate = new List<IntVec3>();
	}
}
