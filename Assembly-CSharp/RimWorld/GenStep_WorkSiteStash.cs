using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020012FF RID: 4863
	public class GenStep_WorkSiteStash : GenStep_Scatterer
	{
		// Token: 0x170014A4 RID: 5284
		// (get) Token: 0x06007895 RID: 30869 RVA: 0x002AAF9E File Offset: 0x002A919E
		public override int SeedPart
		{
			get
			{
				return 913432591;
			}
		}

		// Token: 0x06007896 RID: 30870 RVA: 0x002AAFA8 File Offset: 0x002A91A8
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
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)))
			{
				return false;
			}
			CellRect rect = CellRect.CenteredOn(c, 10, 10);
			List<CellRect> list;
			if (MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) && list.Any((CellRect x) => x.Overlaps(rect)))
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

		// Token: 0x06007897 RID: 30871 RVA: 0x002AB084 File Offset: 0x002A9284
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			CellRect cellRect = CellRect.CenteredOn(loc, 10, 10).ClipInsideMap(map);
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = map.ParentFaction;
			resolveParams.workSitePoints = parms.sitePart.site.desiredThreatPoints;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				resolveParams.stockpileConcreteContents = parms.sitePart.things;
			}
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push(this.symbolName, resolveParams, null);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
			orGenerateVar.Add(cellRect);
		}

		// Token: 0x04004B97 RID: 19351
		public string symbolName;

		// Token: 0x04004B98 RID: 19352
		private const int Size = 10;
	}
}
