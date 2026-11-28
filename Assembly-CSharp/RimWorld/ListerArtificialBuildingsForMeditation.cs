using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200127B RID: 4731
	public class ListerArtificialBuildingsForMeditation
	{
		// Token: 0x0600752E RID: 29998 RVA: 0x002924BB File Offset: 0x002906BB
		public ListerArtificialBuildingsForMeditation(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600752F RID: 29999 RVA: 0x002924D5 File Offset: 0x002906D5
		public void Notify_BuildingSpawned(Building b)
		{
			if (MeditationUtility.CountsAsArtificialBuilding(b))
			{
				this.artificialBuildingsPerCell.Clear();
			}
		}

		// Token: 0x06007530 RID: 30000 RVA: 0x002924EA File Offset: 0x002906EA
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (MeditationUtility.CountsAsArtificialBuilding(b))
			{
				this.artificialBuildingsPerCell.Clear();
			}
		}

		// Token: 0x06007531 RID: 30001 RVA: 0x00292500 File Offset: 0x00290700
		public List<Thing> GetForCell(IntVec3 cell, float radius)
		{
			CellWithRadius key = new CellWithRadius(cell, radius);
			List<Thing> list;
			if (!this.artificialBuildingsPerCell.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false))
				{
					if (MeditationUtility.CountsAsArtificialBuilding(thing))
					{
						list.Add(thing);
					}
				}
				this.artificialBuildingsPerCell[key] = list;
			}
			return list;
		}

		// Token: 0x04004972 RID: 18802
		private Map map;

		// Token: 0x04004973 RID: 18803
		private Dictionary<CellWithRadius, List<Thing>> artificialBuildingsPerCell = new Dictionary<CellWithRadius, List<Thing>>();
	}
}
