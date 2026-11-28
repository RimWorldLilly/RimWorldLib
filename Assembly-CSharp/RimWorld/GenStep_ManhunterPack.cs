using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F3 RID: 4851
	public class GenStep_ManhunterPack : GenStep
	{
		// Token: 0x17001498 RID: 5272
		// (get) Token: 0x06007861 RID: 30817 RVA: 0x002A901C File Offset: 0x002A721C
		public override int SeedPart
		{
			get
			{
				return 457293335;
			}
		}

		// Token: 0x06007862 RID: 30818 RVA: 0x002A9024 File Offset: 0x002A7224
		public override void Generate(Map map, GenStepParams parms)
		{
			GenStep_ManhunterPack.<>c__DisplayClass4_0 CS$<>8__locals1 = new GenStep_ManhunterPack.<>c__DisplayClass4_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false).WithFenceblocked(true);
			IntVec3 root;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => base.<Generate>g__CellValidator|0(x) && !x.Fogged(CS$<>8__locals1.map) && CS$<>8__locals1.map.reachability.CanReachMapEdge(x, CS$<>8__locals1.traverseParams) && x.GetRoom(CS$<>8__locals1.map).CellCount >= CS$<>8__locals1.<>4__this.MinRoomCells, CS$<>8__locals1.map, out root))
			{
				float points = (parms.sitePart != null) ? parms.sitePart.parms.threatPoints : this.defaultPointsRange.RandomInRange;
				PawnKindDef animalKind;
				if (parms.sitePart != null && parms.sitePart.parms.animalKind != null)
				{
					animalKind = parms.sitePart.parms.animalKind;
				}
				else if (!ManhunterPackGenStepUtility.TryGetAnimalsKind(points, CS$<>8__locals1.map.Tile, out animalKind))
				{
					return;
				}
				List<Pawn> list = AggressiveAnimalIncidentUtility.GenerateAnimals(animalKind, CS$<>8__locals1.map.Tile, points, 0);
				for (int i = 0; i < list.Count; i++)
				{
					IntVec3 loc;
					CellFinder.TryFindRandomSpawnCellForPawnNear(root, CS$<>8__locals1.map, out loc, 10, new Predicate<IntVec3>(CS$<>8__locals1.<Generate>g__CellValidator|0));
					GenSpawn.Spawn(list[i], loc, CS$<>8__locals1.map, Rot4.Random, WipeMode.Vanish, false, false);
					list[i].health.AddHediff(HediffDefOf.Scaria, null, null, null);
					list[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, false, null, false, false, false);
				}
			}
		}

		// Token: 0x04004B74 RID: 19316
		public FloatRange defaultPointsRange = new FloatRange(300f, 500f);

		// Token: 0x04004B75 RID: 19317
		private int MinRoomCells = 225;
	}
}
