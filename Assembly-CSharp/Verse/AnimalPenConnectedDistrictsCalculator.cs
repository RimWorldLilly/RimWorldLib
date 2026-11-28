using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200022F RID: 559
	public class AnimalPenConnectedDistrictsCalculator : AnimalPenEnclosureCalculator
	{
		// Token: 0x0600103A RID: 4154 RVA: 0x0005D851 File Offset: 0x0005BA51
		protected override void VisitDirectlyConnectedRegion(Region r)
		{
			this.AddDistrict(r);
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0005D85A File Offset: 0x0005BA5A
		protected override void VisitIndirectlyDirectlyConnectedRegion(Region r)
		{
			this.AddDistrict(r);
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0005D863 File Offset: 0x0005BA63
		protected override void VisitPassableDoorway(Region r)
		{
			this.AddDistrict(r);
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0005D86C File Offset: 0x0005BA6C
		private void AddDistrict(Region r)
		{
			District district = r.District;
			if (!this.districtsTmp.Contains(district))
			{
				this.districtsTmp.Add(district);
			}
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0005D89A File Offset: 0x0005BA9A
		public static void InvalidateDistrictCache(District district)
		{
			AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached.Remove(district);
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0005D8A8 File Offset: 0x0005BAA8
		public List<District> CalculateConnectedDistricts(IntVec3 position, Map map)
		{
			District district = position.GetDistrict(map, RegionType.Set_Passable);
			if (Find.TickManager.TicksGame == AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCachedTick)
			{
				if (AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached.ContainsKey(district))
				{
					return AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached[district];
				}
			}
			else
			{
				AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached.Clear();
				AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCachedTick = Find.TickManager.TicksGame;
			}
			this.districtsTmp.Clear();
			if (!base.VisitPen(position, map))
			{
				this.districtsTmp.Clear();
			}
			AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached[district] = this.districtsTmp.ToList<District>();
			return this.districtsTmp;
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0005D93E File Offset: 0x0005BB3E
		public void Reset()
		{
			this.districtsTmp.Clear();
		}

		// Token: 0x04000F38 RID: 3896
		private readonly List<District> districtsTmp = new List<District>();

		// Token: 0x04000F39 RID: 3897
		private static readonly Dictionary<District, List<District>> connectedDistrictsForRootCached = new Dictionary<District, List<District>>();

		// Token: 0x04000F3A RID: 3898
		private static int connectedDistrictsForRootCachedTick = -1;
	}
}
