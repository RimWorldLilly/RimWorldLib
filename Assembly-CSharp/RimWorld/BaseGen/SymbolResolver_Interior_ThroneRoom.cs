using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200214C RID: 8524
	public class SymbolResolver_Interior_ThroneRoom : SymbolResolver
	{
		// Token: 0x0600D528 RID: 54568 RVA: 0x004F03FC File Offset: 0x004EE5FC
		public override void Resolve(ResolveParams rp)
		{
			Rot4 value;
			IntVec3 throneCell = this.GetThroneCell(rp.rect, out value);
			Pair<IntVec3, Rot4> pair;
			if (this.GetPossibleDrapeCells(throneCell, rp.rect).TryRandomElement(out pair))
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.Drape;
				resolveParams.rect = CellRect.SingleCell(pair.First);
				resolveParams.thingRot = new Rot4?(pair.Second);
				BaseGen.symbolStack.Push("thing", resolveParams, null);
			}
			foreach (IntVec3 intVec in rp.rect.Corners)
			{
				if (!(intVec == throneCell) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(intVec, BaseGen.globalSettings.map))
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.singleThingDef = ThingDefOf.Brazier;
					resolveParams2.rect = CellRect.SingleCell(intVec);
					resolveParams2.postThingSpawn = delegate(Thing x)
					{
						x.TryGetComp<CompRefuelable>().Refuel(9999f);
					};
					BaseGen.symbolStack.Push("thing", resolveParams2, null);
				}
			}
			ResolveParams resolveParams3 = rp;
			resolveParams3.singleThingDef = ThingDefOf.Throne;
			resolveParams3.thingRot = new Rot4?(value);
			resolveParams3.rect = CellRect.SingleCell(throneCell);
			BaseGen.symbolStack.Push("thing", resolveParams3, null);
		}

		// Token: 0x0600D529 RID: 54569 RVA: 0x004F0568 File Offset: 0x004EE768
		private IntVec3 GetThroneCell(CellRect rect, out Rot4 dir)
		{
			SymbolResolver_Interior_ThroneRoom.tmpCells.Clear();
			SymbolResolver_Interior_ThroneRoom.tmpCells.Add(new Pair<IntVec3, Rot4>(new IntVec3(rect.CenterCell.x, 0, rect.maxZ), Rot4.South));
			SymbolResolver_Interior_ThroneRoom.tmpCells.Add(new Pair<IntVec3, Rot4>(new IntVec3(rect.CenterCell.x, 0, rect.minZ), Rot4.North));
			SymbolResolver_Interior_ThroneRoom.tmpCells.Add(new Pair<IntVec3, Rot4>(new IntVec3(rect.minX, 0, rect.CenterCell.z), Rot4.East));
			SymbolResolver_Interior_ThroneRoom.tmpCells.Add(new Pair<IntVec3, Rot4>(new IntVec3(rect.maxX, 0, rect.CenterCell.z), Rot4.West));
			Pair<IntVec3, Rot4> pair;
			if (!(from x in SymbolResolver_Interior_ThroneRoom.tmpCells
			where !BaseGenUtility.AnyDoorAdjacentCardinalTo(x.First, BaseGen.globalSettings.map)
			select x).TryRandomElement(out pair))
			{
				SymbolResolver_Interior_ThroneRoom.tmpCells.TryRandomElement(out pair);
			}
			dir = pair.Second;
			return pair.First;
		}

		// Token: 0x0600D52A RID: 54570 RVA: 0x004F0682 File Offset: 0x004EE882
		private IEnumerable<Pair<IntVec3, Rot4>> GetPossibleDrapeCells(IntVec3 throneCell, CellRect rect)
		{
			SymbolResolver_Interior_ThroneRoom.<GetPossibleDrapeCells>d__3 <GetPossibleDrapeCells>d__ = new SymbolResolver_Interior_ThroneRoom.<GetPossibleDrapeCells>d__3(-2);
			<GetPossibleDrapeCells>d__.<>3__throneCell = throneCell;
			<GetPossibleDrapeCells>d__.<>3__rect = rect;
			return <GetPossibleDrapeCells>d__;
		}

		// Token: 0x04008724 RID: 34596
		private static List<Pair<IntVec3, Rot4>> tmpCells = new List<Pair<IntVec3, Rot4>>();
	}
}
