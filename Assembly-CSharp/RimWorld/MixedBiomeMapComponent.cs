using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001285 RID: 4741
	public class MixedBiomeMapComponent : MapComponent
	{
		// Token: 0x170013DF RID: 5087
		// (get) Token: 0x06007583 RID: 30083 RVA: 0x00293DD2 File Offset: 0x00291FD2
		public bool IsMixedBiome
		{
			get
			{
				return this.biomeGrid != null;
			}
		}

		// Token: 0x06007584 RID: 30084 RVA: 0x00293DDD File Offset: 0x00291FDD
		public MixedBiomeMapComponent(Map map) : base(map)
		{
		}

		// Token: 0x06007585 RID: 30085 RVA: 0x00293DE8 File Offset: 0x00291FE8
		public BiomeDef GetBiomeAt(IntVec3 cell)
		{
			if (this.biomeGrid == null || !ModsConfig.OdysseyActive)
			{
				return this.map.Biome;
			}
			if (!cell.InBounds(this.map))
			{
				return null;
			}
			int num = this.map.cellIndices.CellToIndex(cell);
			if (!this.biomeGrid[num])
			{
				return this.map.Biome;
			}
			return this.secondaryBiome;
		}

		// Token: 0x06007586 RID: 30086 RVA: 0x00293E50 File Offset: 0x00292050
		public override void ExposeData()
		{
			base.ExposeData();
			bool[] array = this.biomeGrid;
			List<bool> list = (array != null) ? array.ToList<bool>() : null;
			Scribe_Collections.Look<bool>(ref list, "biomeGrid", LookMode.Undefined, Array.Empty<object>());
			this.biomeGrid = ((list != null) ? list.ToArray() : null);
			Scribe_Defs.Look<BiomeDef>(ref this.secondaryBiome, "secondaryBiome");
		}

		// Token: 0x04004993 RID: 18835
		public bool[] biomeGrid;

		// Token: 0x04004994 RID: 18836
		public BiomeDef secondaryBiome;
	}
}
