using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E0 RID: 4832
	public class GenStep_Settlement : GenStep_Scatterer
	{
		// Token: 0x1700146A RID: 5226
		// (get) Token: 0x060077E7 RID: 30695 RVA: 0x002A6BC4 File Offset: 0x002A4DC4
		private bool WillPostProcess
		{
			get
			{
				return this.postProcessSettlementParams != null;
			}
		}

		// Token: 0x1700146B RID: 5227
		// (get) Token: 0x060077E8 RID: 30696 RVA: 0x002A6BCF File Offset: 0x002A4DCF
		public override int SeedPart
		{
			get
			{
				return 1806208471;
			}
		}

		// Token: 0x060077E9 RID: 30697 RVA: 0x002A6BD8 File Offset: 0x002A4DD8
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)))
			{
				return false;
			}
			int min = GenStep_Settlement.SettlementSizeRange.min;
			CellRect cellRect = new CellRect(c.x - min / 2, c.z - min / 2, min, min);
			CellRect within = map.BoundsRect(12);
			return cellRect.FullyContainedWithin(within);
		}

		// Token: 0x060077EA RID: 30698 RVA: 0x002A6C64 File Offset: 0x002A4E64
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			int randomInRange = GenStep_Settlement.SettlementSizeRange.RandomInRange;
			int randomInRange2 = GenStep_Settlement.SettlementSizeRange.RandomInRange;
			CellRect cellRect = new CellRect(c.x - randomInRange / 2, c.z - randomInRange2 / 2, randomInRange, randomInRange2);
			Faction faction;
			if (this.overrideFaction != null)
			{
				faction = this.overrideFaction;
			}
			else if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			cellRect.ClipInsideMap(map);
			ResolveParams resolveParams = new ResolveParams
			{
				sitePart = parms.sitePart,
				rect = cellRect,
				faction = faction,
				settlementDontGeneratePawns = new bool?(!this.generatePawns),
				thingSetMakerDef = this.lootThingSetMaker,
				lootMarketValue = this.lootMarketValue
			};
			if (this.postProcessSettlementParams != null)
			{
				this.postProcessSettlementParams.faction = faction;
			}
			MapGenerator.SetVar<CellRect>("SettlementRect", cellRect);
			BaseGen.globalSettings.map = map;
			BaseGen.globalSettings.minBuildings = 1 + this.requiredGravcoreRooms;
			BaseGen.globalSettings.minBarracks = 1;
			BaseGen.globalSettings.requiredGravcoreRooms = this.requiredGravcoreRooms;
			BaseGen.symbolStack.Push("settlement", resolveParams, null);
			if (faction != null && faction == Faction.OfEmpire)
			{
				BaseGen.globalSettings.minThroneRooms = 1;
				BaseGen.globalSettings.minLandingPads = 1;
			}
			List<Building> previous = null;
			if (this.WillPostProcess)
			{
				previous = new List<Building>(map.listerThings.GetThingsOfType<Building>());
			}
			BaseGen.Generate();
			if (faction != null && faction == Faction.OfEmpire && BaseGen.globalSettings.landingPadsGenerated == 0)
			{
				CellRect cellRect2;
				GenStep_Settlement.GenerateLandingPadNearby(resolveParams.rect, map, faction, out cellRect2);
			}
			if (this.WillPostProcess)
			{
				List<Building> placed = (from b in map.listerThings.GetThingsOfType<Building>()
				where !previous.Contains(b)
				select b).ToList<Building>();
				previous.Clear();
				MapGenUtility.PostProcessSettlement(map, placed, this.postProcessSettlementParams);
			}
		}

		// Token: 0x060077EB RID: 30699 RVA: 0x002A6E7C File Offset: 0x002A507C
		public static void GenerateLandingPadNearby(CellRect rect, Map map, Faction faction, out CellRect usedRect)
		{
			ResolveParams resolveParams = default(ResolveParams);
			List<CellRect> usedRects;
			MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out usedRects);
			GenStep_Settlement.tmpCandidates.Clear();
			int size = 9;
			GenStep_Settlement.tmpCandidates.Add(new IntVec3(rect.maxX + 1, 0, rect.CenterCell.z));
			GenStep_Settlement.tmpCandidates.Add(new IntVec3(rect.minX - size, 0, rect.CenterCell.z));
			GenStep_Settlement.tmpCandidates.Add(new IntVec3(rect.CenterCell.x, 0, rect.maxZ + 1));
			GenStep_Settlement.tmpCandidates.Add(new IntVec3(rect.CenterCell.x, 0, rect.minZ - size));
			IntVec3 intVec;
			if (!GenStep_Settlement.tmpCandidates.Where(delegate(IntVec3 x)
			{
				CellRect r = new CellRect(x.x, x.z, size, size);
				return r.InBounds(map) && (usedRects == null || !usedRects.Any((CellRect y) => y.Overlaps(r)));
			}).TryRandomElement(out intVec))
			{
				usedRect = CellRect.Empty;
				return;
			}
			resolveParams.rect = new CellRect(intVec.x, intVec.z, size, size);
			resolveParams.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("landingPad", resolveParams, null);
			BaseGen.Generate();
			usedRect = resolveParams.rect;
		}

		// Token: 0x04004B42 RID: 19266
		private static readonly IntRange SettlementSizeRange = new IntRange(34, 38);

		// Token: 0x04004B43 RID: 19267
		public const string RectVarName = "SettlementRect";

		// Token: 0x04004B44 RID: 19268
		public bool generatePawns = true;

		// Token: 0x04004B45 RID: 19269
		public MapGenUtility.PostProcessSettlementParams postProcessSettlementParams;

		// Token: 0x04004B46 RID: 19270
		public int requiredGravcoreRooms;

		// Token: 0x04004B47 RID: 19271
		public ThingSetMakerDef lootThingSetMaker;

		// Token: 0x04004B48 RID: 19272
		public float? lootMarketValue;

		// Token: 0x04004B49 RID: 19273
		public Faction overrideFaction;

		// Token: 0x04004B4A RID: 19274
		private static readonly List<IntVec3> tmpCandidates = new List<IntVec3>();
	}
}
