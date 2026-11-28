using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AC3 RID: 6851
	public class Building_OrbitalTradeBeacon : Building
	{
		// Token: 0x17001F4D RID: 8013
		// (get) Token: 0x0600AE53 RID: 44627 RVA: 0x003D6ACF File Offset: 0x003D4CCF
		public IEnumerable<IntVec3> TradeableCells
		{
			get
			{
				return Building_OrbitalTradeBeacon.TradeableCellsAround(base.Position, base.Map);
			}
		}

		// Token: 0x0600AE54 RID: 44628 RVA: 0x003D6AE2 File Offset: 0x003D4CE2
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_OrbitalTradeBeacon.<GetGizmos>d__3 <GetGizmos>d__ = new Building_OrbitalTradeBeacon.<GetGizmos>d__3(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AE55 RID: 44629 RVA: 0x003D6AF4 File Offset: 0x003D4CF4
		private void MakeMatchingStockpile()
		{
			Designator_ZoneAddStockpile_Resources des = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>();
			des.DesignateMultiCell(from c in this.TradeableCells
			where des.CanDesignateCell(c).Accepted
			select c);
		}

		// Token: 0x0600AE56 RID: 44630 RVA: 0x003D6B34 File Offset: 0x003D4D34
		public static List<IntVec3> TradeableCellsAround(IntVec3 pos, Map map)
		{
			Building_OrbitalTradeBeacon.tradeableCells.Clear();
			if (!pos.InBounds(map))
			{
				return Building_OrbitalTradeBeacon.tradeableCells;
			}
			Region region = pos.GetRegion(map, RegionType.Set_Passable);
			if (region == null)
			{
				return Building_OrbitalTradeBeacon.tradeableCells;
			}
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.door == null, delegate(Region r)
			{
				foreach (IntVec3 item in r.Cells)
				{
					if (item.InHorDistOf(pos, 7.9f))
					{
						Building_OrbitalTradeBeacon.tradeableCells.Add(item);
					}
				}
				return false;
			}, 16, RegionType.Set_Passable);
			return Building_OrbitalTradeBeacon.tradeableCells;
		}

		// Token: 0x0600AE57 RID: 44631 RVA: 0x003D6BBE File Offset: 0x003D4DBE
		public static IEnumerable<Building_OrbitalTradeBeacon> AllPowered(Map map)
		{
			Building_OrbitalTradeBeacon.<AllPowered>d__7 <AllPowered>d__ = new Building_OrbitalTradeBeacon.<AllPowered>d__7(-2);
			<AllPowered>d__.<>3__map = map;
			return <AllPowered>d__;
		}

		// Token: 0x040064E4 RID: 25828
		private const float TradeRadius = 7.9f;

		// Token: 0x040064E5 RID: 25829
		private static List<IntVec3> tradeableCells = new List<IntVec3>();
	}
}
