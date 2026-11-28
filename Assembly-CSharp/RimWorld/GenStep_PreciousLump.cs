using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F8 RID: 4856
	public class GenStep_PreciousLump : GenStep_ScatterLumpsMineable
	{
		// Token: 0x1700149D RID: 5277
		// (get) Token: 0x06007873 RID: 30835 RVA: 0x002A9E6A File Offset: 0x002A806A
		public override int SeedPart
		{
			get
			{
				return 1634184421;
			}
		}

		// Token: 0x06007874 RID: 30836 RVA: 0x002A9E74 File Offset: 0x002A8074
		public override void Generate(Map map, GenStepParams parms)
		{
			if (parms.sitePart != null && parms.sitePart.parms.preciousLumpResources != null)
			{
				this.forcedDefToScatter = parms.sitePart.parms.preciousLumpResources;
			}
			else
			{
				this.forcedDefToScatter = this.mineables.RandomElement<ThingDef>();
			}
			this.count = 1;
			float randomInRange = this.totalValueRange.RandomInRange;
			float baseMarketValue = this.forcedDefToScatter.building.mineableThing.BaseMarketValue;
			this.forcedLumpSize = Mathf.Max(Mathf.RoundToInt(randomInRange / ((float)this.forcedDefToScatter.building.mineableYield * baseMarketValue)), 1);
			base.Generate(map, parms);
		}

		// Token: 0x06007875 RID: 30837 RVA: 0x002A9F1C File Offset: 0x002A811C
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			List<CellRect> list;
			return (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) || !list.Any((CellRect x) => x.Contains(c))) && (!map.wasSpawnedViaGravShipLanding || !CellRect.CenteredOn(MapGenerator.PlayerStartSpot, Find.CurrentGravship.Bounds.Size).ExpandedBy(10).Contains(c)) && map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false));
		}

		// Token: 0x06007876 RID: 30838 RVA: 0x002A9FB8 File Offset: 0x002A81B8
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			base.ScatterAt(c, map, parms, stackCount);
			int minX = this.recentLumpCells.Min((IntVec3 x) => x.x);
			int minZ = this.recentLumpCells.Min((IntVec3 x) => x.z);
			int maxX = this.recentLumpCells.Max((IntVec3 x) => x.x);
			int maxZ = this.recentLumpCells.Max((IntVec3 x) => x.z);
			CellRect var = CellRect.FromLimits(minX, minZ, maxX, maxZ);
			MapGenerator.SetVar<CellRect>("RectOfInterest", var);
		}

		// Token: 0x04004B88 RID: 19336
		public List<ThingDef> mineables;

		// Token: 0x04004B89 RID: 19337
		public FloatRange totalValueRange = new FloatRange(1000f, 2000f);
	}
}
