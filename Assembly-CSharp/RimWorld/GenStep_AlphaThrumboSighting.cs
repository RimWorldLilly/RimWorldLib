using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E4 RID: 4836
	public class GenStep_AlphaThrumboSighting : GenStep
	{
		// Token: 0x1700146E RID: 5230
		// (get) Token: 0x060077FA RID: 30714 RVA: 0x002A7211 File Offset: 0x002A5411
		public override int SeedPart
		{
			get
			{
				return 792525399;
			}
		}

		// Token: 0x060077FB RID: 30715 RVA: 0x002A7218 File Offset: 0x002A5418
		public override void Generate(Map map, GenStepParams parms)
		{
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false).WithFenceblocked(true);
			List<CellRect> usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			IntVec3 root;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && map.reachability.CanReachMapEdge(x, traverseParams) && x.GetRoom(map).CellCount >= this.MinRoomCells && !usedRects.Any((CellRect ur) => ur.Contains(x)), map, out root))
			{
				List<Pawn> list = new List<Pawn>();
				PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.AlphaThrumbo, null, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)
				{
					ExcludeBiologicalAgeRange = new FloatRange?(GenStep_AlphaThrumboSighting.ExcludeBiologicalAgeRange)
				};
				Pawn item = PawnGenerator.GeneratePawn(request);
				list.Add(item);
				int randomInRange = this.normalThrumboCountRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					request = new PawnGenerationRequest(PawnKindDefOf.Thrumbo, null, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
					item = PawnGenerator.GeneratePawn(request);
					list.Add(item);
				}
				for (int j = 0; j < list.Count; j++)
				{
					IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(root, map, 10);
					GenSpawn.Spawn(list[j], loc, map, Rot4.Random, WipeMode.Vanish, false, false);
				}
			}
		}

		// Token: 0x04004B4C RID: 19276
		public IntRange normalThrumboCountRange = new IntRange(3, 5);

		// Token: 0x04004B4D RID: 19277
		private int MinRoomCells = 225;

		// Token: 0x04004B4E RID: 19278
		private static readonly FloatRange ExcludeBiologicalAgeRange = new FloatRange(0f, 250f);
	}
}
