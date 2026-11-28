using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200212D RID: 8493
	public class SymbolResolver_LandingPad : SymbolResolver
	{
		// Token: 0x0600D4CC RID: 54476 RVA: 0x004EC870 File Offset: 0x004EAA70
		public override void Resolve(ResolveParams rp)
		{
			TerrainDef floorDef = rp.floorDef ?? Rand.Element<TerrainDef>(TerrainDefOf.PavedTile, TerrainDefOf.Concrete);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = ThingDefOf.Shuttle;
			resolveParams.rect = CellRect.SingleCell(rp.rect.CenterCell);
			resolveParams.postThingSpawn = delegate(Thing x)
			{
				TransportShip transportShip = TransportShipMaker.MakeTransportShip(TransportShipDefOf.Ship_Shuttle, null, x);
				ShipJob_WaitTime shipJob_WaitTime = (ShipJob_WaitTime)ShipJobMaker.MakeShipJob(ShipJobDefOf.WaitTime);
				shipJob_WaitTime.duration = SymbolResolver_LandingPad.ShuttleLeaveAfterTicksRange.RandomInRange;
				shipJob_WaitTime.showGizmos = false;
				transportShip.AddJob(shipJob_WaitTime);
				transportShip.AddJob(ShipJobDefOf.FlyAway);
				transportShip.Start();
			};
			BaseGen.symbolStack.Push("thing", resolveParams, null);
			foreach (IntVec3 c in rp.rect.Corners)
			{
				ResolveParams resolveParams2 = rp;
				resolveParams2.singleThingDef = ThingDefOf.ShipLandingBeacon;
				resolveParams2.rect = CellRect.SingleCell(c);
				BaseGen.symbolStack.Push("thing", resolveParams2, null);
			}
			ResolveParams resolveParams3 = rp;
			resolveParams3.floorDef = floorDef;
			BaseGen.symbolStack.Push("floor", resolveParams3, null);
			BaseGen.symbolStack.Push("clear", rp, null);
		}

		// Token: 0x040086FA RID: 34554
		private static readonly IntRange ShuttleLeaveAfterTicksRange = new IntRange(300, 3600);
	}
}
