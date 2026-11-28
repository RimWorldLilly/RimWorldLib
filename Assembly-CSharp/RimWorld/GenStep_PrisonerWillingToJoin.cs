using System;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F9 RID: 4857
	public class GenStep_PrisonerWillingToJoin : GenStep_Scatterer
	{
		// Token: 0x1700149E RID: 5278
		// (get) Token: 0x06007878 RID: 30840 RVA: 0x002AA0AC File Offset: 0x002A82AC
		public override int SeedPart
		{
			get
			{
				return 69356099;
			}
		}

		// Token: 0x06007879 RID: 30841 RVA: 0x002AA0B4 File Offset: 0x002A82B4
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			if (c.Fogged(map))
			{
				return false;
			}
			CellRect rect = CellRect.CenteredOn(c, 8, 8);
			if (MapGenerator.UsedRects.Any((CellRect ur) => ur.Overlaps(rect)))
			{
				return false;
			}
			foreach (IntVec3 c2 in rect)
			{
				if (!c2.InBounds(map) || c2.GetEdifice(map) != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600787A RID: 30842 RVA: 0x002AA170 File Offset: 0x002A8370
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			CellRect cellRect = CellRect.CenteredOn(loc, 8, 8).ClipInsideMap(map);
			Pawn singlePawnToSpawn;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				singlePawnToSpawn = (Pawn)parms.sitePart.things.Take(parms.sitePart.things[0]);
			}
			else
			{
				PrisonerWillingToJoinComp component = map.Parent.GetComponent<PrisonerWillingToJoinComp>();
				if (component != null && component.pawn.Any)
				{
					singlePawnToSpawn = component.pawn.Take(component.pawn[0]);
				}
				else
				{
					singlePawnToSpawn = PrisonerWillingToJoinQuestUtility.GeneratePrisoner(map.Tile, faction);
				}
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("prisonCell", resolveParams, null);
			BaseGen.Generate();
			ResolveParams resolveParams2 = default(ResolveParams);
			resolveParams2.rect = cellRect;
			resolveParams2.faction = faction;
			resolveParams2.singlePawnToSpawn = singlePawnToSpawn;
			resolveParams2.singlePawnSpawnCellExtraPredicate = ((IntVec3 x) => x.GetDoor(map) == null);
			resolveParams2.postThingSpawn = delegate(Thing x)
			{
				MapGenerator.rootsToUnfog.Add(x.Position);
				((Pawn)x).mindState.WillJoinColonyIfRescued = true;
			};
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("pawn", resolveParams2, null);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
			map.fogGrid.Refog(cellRect.ContractedBy(1));
		}

		// Token: 0x04004B8A RID: 19338
		private const int Size = 8;
	}
}
