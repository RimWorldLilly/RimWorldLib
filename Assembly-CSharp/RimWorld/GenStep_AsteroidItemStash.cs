using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001299 RID: 4761
	public class GenStep_AsteroidItemStash : GenStep
	{
		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x06007613 RID: 30227 RVA: 0x002989CC File Offset: 0x00296BCC
		public override int SeedPart
		{
			get
			{
				return 234098237;
			}
		}

		// Token: 0x06007614 RID: 30228 RVA: 0x002989D4 File Offset: 0x00296BD4
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Asteroid Item Stash"))
			{
				return;
			}
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			IntVec2 size = new IntVec2(GenStep_AsteroidItemStash.StructureSizeRange.RandomInRange, GenStep_AsteroidItemStash.StructureSizeRange.RandomInRange);
			CellRect item;
			if (!this.FindStashRect(map, size, out item))
			{
				return;
			}
			ModuleBase moduleBase = new DistFromPointRects(new List<CellRect>
			{
				item.ExpandedBy(3)
			});
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.03f, 5f, 4, -1);
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (moduleBase.GetValue(intVec) > 0f)
				{
					Building edifice = intVec.GetEdifice(map);
					if (edifice != null)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
				}
			}
			StructureGenParams parms2 = new StructureGenParams
			{
				size = size
			};
			LayoutWorker worker = LayoutDefOf.OrbitalItemStash.Worker;
			LayoutStructureSketch layoutStructureSketch = worker.GenerateStructureSketch(parms2);
			List<Thing> list = new List<Thing>();
			LayoutStructureSketch layoutStructureSketch2 = layoutStructureSketch;
			IntVec3 min = item.Min;
			List<Thing> allSpawnedThings = list;
			worker.Spawn(layoutStructureSketch2, map, min, null, allSpawnedThings, true, false, null);
			orGenerateVar.Add(item);
			foreach (Thing thing in list)
			{
				if (thing.def.IsDoor)
				{
					this.ClearAreaAroundDoor(thing.Position, map);
				}
			}
			map.OrbitalDebris = OrbitalDebrisDefOf.Asteroid;
		}

		// Token: 0x06007615 RID: 30229 RVA: 0x00298B6C File Offset: 0x00296D6C
		private void ClearAreaAroundDoor(IntVec3 thingPosition, Map map)
		{
			foreach (IntVec3 c in GridShapeMaker.IrregularLump(thingPosition, map, 100, null))
			{
				Building edifice = c.GetEdifice(map);
				if (edifice != null && edifice.def.building.isNaturalRock)
				{
					edifice.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06007616 RID: 30230 RVA: 0x00298BE0 File Offset: 0x00296DE0
		private bool FindStashRect(Map map, IntVec2 size, out CellRect rect)
		{
			rect = CellRect.Empty;
			int num = 0;
			Predicate<IntVec3> <>9__0;
			Func<IntVec3, bool> <>9__1;
			for (int i = 0; i < 100; i++)
			{
				IntVec3 center = map.Center;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => c.GetEdifice(map) != null));
				}
				IntVec3 center2;
				if (RCellFinder.TryFindRandomCellNearWith(center, validator, map, out center2, 5, 2147483647))
				{
					CellRect cellRect = CellRect.CenteredOn(center2, size);
					IEnumerable<IntVec3> cells = cellRect.Cells;
					Func<IntVec3, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((IntVec3 c) => c.GetEdifice(map) != null));
					}
					int num2 = cells.Count(predicate);
					if (num2 > num)
					{
						num = num2;
						rect = cellRect;
					}
				}
			}
			return (float)num > (float)rect.Area / 2f;
		}

		// Token: 0x04004A07 RID: 18951
		private const float ClearNoiseFreq = 0.03f;

		// Token: 0x04004A08 RID: 18952
		private const float ClearNoiseStrength = 5f;

		// Token: 0x04004A09 RID: 18953
		private static readonly IntRange StructureSizeRange = new IntRange(30, 40);

		// Token: 0x04004A0A RID: 18954
		private const int DoorClearArea = 100;
	}
}
