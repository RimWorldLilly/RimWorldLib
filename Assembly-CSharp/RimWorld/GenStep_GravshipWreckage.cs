using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020012F0 RID: 4848
	public class GenStep_GravshipWreckage : GenStep
	{
		// Token: 0x17001495 RID: 5269
		// (get) Token: 0x0600784F RID: 30799 RVA: 0x002A8765 File Offset: 0x002A6965
		public override int SeedPart
		{
			get
			{
				return 26145111;
			}
		}

		// Token: 0x06007850 RID: 30800 RVA: 0x002A876C File Offset: 0x002A696C
		public override void Generate(Map map, GenStepParams parms)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass11_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass11_0();
			CS$<>8__locals1.map = map;
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			CellRect cellRect;
			bool flag = MapGenUtility.TryGetRandomClearRect(20, 20, out cellRect, -1, -1, new Predicate<CellRect>(CS$<>8__locals1.<Generate>g__RectValidator|0), -1f, 0.7f, MapGenUtility.SearchWeightMode.Largest, 1);
			IntVec3 intVec = flag ? (cellRect.CenterCell + IntVec3.SouthEast) : CS$<>8__locals1.map.Center;
			GenStep_GravshipWreckage.SpawnCraters(CS$<>8__locals1.map, intVec);
			GenStep_GravshipWreckage.SpawnFilth(CS$<>8__locals1.map, intVec);
			GenStep_GravshipWreckage.SpawnShipChunks(CS$<>8__locals1.map, intVec);
			GenSpawn.Spawn(ThingDefOf.GravEngine, intVec, CS$<>8__locals1.map, WipeMode.Vanish).questTags = parms.sitePart.site.questTags;
			this.SpawnCorpses(CS$<>8__locals1.map, parms, intVec);
			GenStep_GravshipWreckage.SpawnPanels(CS$<>8__locals1.map, intVec);
			this.SpawnDefenders(CS$<>8__locals1.map, parms, intVec);
			if (flag)
			{
				MapGenerator.UsedRects.Add(cellRect.ExpandedBy(8));
			}
		}

		// Token: 0x06007851 RID: 30801 RVA: 0x002A885C File Offset: 0x002A6A5C
		private static void SpawnCraters(Map map, IntVec3 root)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass12_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass12_0();
			CS$<>8__locals1.map = map;
			int randomInRange = GenStep_GravshipWreckage.NumCratersRange.RandomInRange;
			List<ThingDef> source = new List<ThingDef>
			{
				ThingDefOf.CraterSmall,
				ThingDefOf.CraterMedium,
				ThingDefOf.CraterLarge
			};
			for (int i = 0; i < randomInRange; i++)
			{
				ThingDef def = source.RandomElement<ThingDef>();
				IntVec3 loc;
				if (!RCellFinder.TryFindRandomCellNearWith(root, (IntVec3 c) => CS$<>8__locals1.<SpawnCraters>g__Validator|0(c, def), CS$<>8__locals1.map, out loc, 7, 40))
				{
					break;
				}
				GenSpawn.Spawn(def, loc, CS$<>8__locals1.map, WipeMode.Vanish);
			}
		}

		// Token: 0x06007852 RID: 30802 RVA: 0x002A891C File Offset: 0x002A6B1C
		private static void SpawnFilth(Map map, IntVec3 root)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass13_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass13_0();
			CS$<>8__locals1.map = map;
			int randomInRange = GenStep_GravshipWreckage.NumFilthRange.RandomInRange;
			List<ThingDef> source = new List<ThingDef>
			{
				ThingDefOf.Filth_BlastMark,
				ThingDefOf.Filth_OilSmear
			};
			for (int i = 0; i < randomInRange; i++)
			{
				ThingDef def = source.RandomElement<ThingDef>();
				IntVec3 c2;
				if (!RCellFinder.TryFindRandomCellNearWith(root, (IntVec3 c) => CS$<>8__locals1.<SpawnFilth>g__Validator|0(c, def), CS$<>8__locals1.map, out c2, 7, 40))
				{
					break;
				}
				FilthMaker.TryMakeFilth(c2, CS$<>8__locals1.map, def, 1, FilthSourceFlags.None, true);
			}
		}

		// Token: 0x06007853 RID: 30803 RVA: 0x002A89D0 File Offset: 0x002A6BD0
		private static void SpawnPanels(Map map, IntVec3 root)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass14_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass14_0();
			CS$<>8__locals1.map = map;
			int num = GenStep_GravshipWreckage.Panels.RandomInRange;
			CS$<>8__locals1.def = ThingDefOf.GravlitePanel;
			IntVec3 loc;
			while (num > 0 && RCellFinder.TryFindRandomCellNearWith(root, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnPanels>g__Validator|0), CS$<>8__locals1.map, out loc, 5, 40))
			{
				Thing thing = GenSpawn.Spawn(CS$<>8__locals1.def, loc, CS$<>8__locals1.map, WipeMode.Vanish);
				thing.stackCount = Mathf.Min(num, CS$<>8__locals1.def.stackLimit);
				thing.TrySetForbidden(true);
				num -= thing.stackCount;
			}
		}

		// Token: 0x06007854 RID: 30804 RVA: 0x002A8A68 File Offset: 0x002A6C68
		private static void SpawnShipChunks(Map map, IntVec3 root)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass15_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass15_0();
			CS$<>8__locals1.map = map;
			int randomInRange = GenStep_GravshipWreckage.ChunkGroups.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 pos;
				if (RCellFinder.TryFindRandomCellNearWith(root, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnShipChunks>g__Validator|0), CS$<>8__locals1.map, out pos, 5, 40))
				{
					GenSpawn.SpawnIrregularLump(ThingDefOf.ShipChunk_Mech, pos, CS$<>8__locals1.map, GenStep_GravshipWreckage.ChunksPerGroup, GenStep_GravshipWreckage.ChunksDistRange, WipeMode.Vanish, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnShipChunks>g__Validator|0), null, null, null, null);
				}
			}
		}

		// Token: 0x06007855 RID: 30805 RVA: 0x002A8AE8 File Offset: 0x002A6CE8
		private void SpawnCorpses(Map map, GenStepParams parms, IntVec3 root)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass16_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass16_0();
			CS$<>8__locals1.map = map;
			using (IEnumerator<Pawn> enumerator = this.GeneratePawns(parms, CS$<>8__locals1.map, GenStep_GravshipWreckage.DeadPointsRange.RandomInRange).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn = enumerator.Current;
					IntVec3 loc;
					if (!RCellFinder.TryFindRandomCellNearWith(root, (IntVec3 c) => CS$<>8__locals1.<SpawnCorpses>g__Validator|0(c, pawn.def), CS$<>8__locals1.map, out loc, 5, 40))
					{
						pawn.Destroy(DestroyMode.Vanish);
						break;
					}
					HealthUtility.SimulateKilled(pawn, DamageDefOf.Blunt, null, null, null);
					GenSpawn.Spawn(pawn.Corpse, loc, CS$<>8__locals1.map, WipeMode.Vanish);
					pawn.Corpse.TrySetForbidden(true);
				}
			}
		}

		// Token: 0x06007856 RID: 30806 RVA: 0x002A8BDC File Offset: 0x002A6DDC
		private void SpawnDefenders(Map map, GenStepParams parms, IntVec3 root)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass17_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass17_0();
			CS$<>8__locals1.map = map;
			List<Pawn> list = new List<Pawn>();
			float points = (parms.sitePart != null) ? parms.sitePart.parms.points : GenStep_GravshipWreckage.DefaultPointsRange.RandomInRange;
			using (IEnumerator<Pawn> enumerator = this.GeneratePawns(parms, CS$<>8__locals1.map, points).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn = enumerator.Current;
					IntVec3 loc;
					if (!RCellFinder.TryFindRandomCellNearWith(root, (IntVec3 c) => CS$<>8__locals1.<SpawnDefenders>g__Validator|0(c, pawn.def), CS$<>8__locals1.map, out loc, 5, 40))
					{
						pawn.Destroy(DestroyMode.Vanish);
						break;
					}
					GenSpawn.Spawn(pawn, loc, CS$<>8__locals1.map, WipeMode.Vanish);
					list.Add(pawn);
				}
			}
			if (!list.Any<Pawn>())
			{
				return;
			}
			LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_DefendPoint(root, new float?(11.9f), null, false, true), CS$<>8__locals1.map, list);
		}

		// Token: 0x06007857 RID: 30807 RVA: 0x002A8D18 File Offset: 0x002A6F18
		private IEnumerable<Pawn> GeneratePawns(GenStepParams parms, Map map, float points)
		{
			float b = Faction.OfMechanoids.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null);
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Combat,
				tile = map.Tile,
				faction = Faction.OfMechanoids,
				points = Mathf.Max(points, b)
			};
			if (parms.sitePart != null)
			{
				pawnGroupMakerParms.seed = new int?(Gen.HashCombineInt(SleepingMechanoidsSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms), (int)points));
			}
			return PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, true);
		}

		// Token: 0x04004B69 RID: 19305
		private static readonly FloatRange DefaultPointsRange = new FloatRange(340f, 1000f);

		// Token: 0x04004B6A RID: 19306
		private static readonly FloatRange DeadPointsRange = new FloatRange(340f, 600f);

		// Token: 0x04004B6B RID: 19307
		private const float WanderRadius = 11.9f;

		// Token: 0x04004B6C RID: 19308
		private static readonly IntRange ChunkGroups = new IntRange(2, 3);

		// Token: 0x04004B6D RID: 19309
		private static readonly IntRange ChunksPerGroup = new IntRange(1, 2);

		// Token: 0x04004B6E RID: 19310
		private static readonly IntRange ChunksDistRange = new IntRange(2, 4);

		// Token: 0x04004B6F RID: 19311
		private static readonly IntRange Panels = new IntRange(100, 100);

		// Token: 0x04004B70 RID: 19312
		private static readonly IntRange NumCratersRange = new IntRange(3, 6);

		// Token: 0x04004B71 RID: 19313
		private static readonly IntRange NumFilthRange = new IntRange(10, 15);
	}
}
