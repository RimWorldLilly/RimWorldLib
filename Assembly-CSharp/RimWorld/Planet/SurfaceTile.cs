using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002331 RID: 9009
	public class SurfaceTile : Tile
	{
		// Token: 0x17002709 RID: 9993
		// (get) Token: 0x0600DE8E RID: 56974 RVA: 0x0052C5A9 File Offset: 0x0052A7A9
		public override bool WaterCovered
		{
			get
			{
				return this.elevation <= 0f;
			}
		}

		// Token: 0x1700270A RID: 9994
		// (get) Token: 0x0600DE8F RID: 56975 RVA: 0x0052C5BB File Offset: 0x0052A7BB
		public List<SurfaceTile.RoadLink> Roads
		{
			get
			{
				if (!base.PrimaryBiome.allowRoads)
				{
					return null;
				}
				return this.potentialRoads;
			}
		}

		// Token: 0x1700270B RID: 9995
		// (get) Token: 0x0600DE90 RID: 56976 RVA: 0x0052C5D2 File Offset: 0x0052A7D2
		public List<SurfaceTile.RiverLink> Rivers
		{
			get
			{
				if (!base.PrimaryBiome.allowRivers)
				{
					return null;
				}
				return this.potentialRivers;
			}
		}

		// Token: 0x0600DE91 RID: 56977 RVA: 0x0052C5E9 File Offset: 0x0052A7E9
		public SurfaceTile()
		{
		}

		// Token: 0x0600DE92 RID: 56978 RVA: 0x0052C5F1 File Offset: 0x0052A7F1
		public SurfaceTile(PlanetTile tile) : base(tile)
		{
		}

		// Token: 0x0600DE93 RID: 56979 RVA: 0x0052C5FC File Offset: 0x0052A7FC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(98, 10);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted<BiomeDef>(base.PrimaryBiome);
			defaultInterpolatedStringHandler.AppendLiteral(" elev=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.elevation);
			defaultInterpolatedStringHandler.AppendLiteral("m hill=");
			defaultInterpolatedStringHandler.AppendFormatted<Hilliness>(this.hilliness);
			defaultInterpolatedStringHandler.AppendLiteral(" temp=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.temperature);
			defaultInterpolatedStringHandler.AppendLiteral("°C rain=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.rainfall);
			defaultInterpolatedStringHandler.AppendLiteral("mm");
			defaultInterpolatedStringHandler.AppendLiteral(" swampiness=");
			defaultInterpolatedStringHandler.AppendFormatted(this.swampiness.ToStringPercent());
			defaultInterpolatedStringHandler.AppendLiteral(" potentialRoads=");
			List<SurfaceTile.RoadLink> list = this.potentialRoads;
			defaultInterpolatedStringHandler.AppendFormatted<int>((list != null) ? list.Count : 0);
			defaultInterpolatedStringHandler.AppendLiteral(" (allowed=");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(base.PrimaryBiome.allowRoads);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			defaultInterpolatedStringHandler.AppendLiteral(" potentialRivers=");
			List<SurfaceTile.RiverLink> list2 = this.potentialRivers;
			defaultInterpolatedStringHandler.AppendFormatted<int>((list2 != null) ? list2.Count : 0);
			defaultInterpolatedStringHandler.AppendLiteral(" (allowed=");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(base.PrimaryBiome.allowRivers);
			defaultInterpolatedStringHandler.AppendLiteral("))");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600DE94 RID: 56980 RVA: 0x0052C760 File Offset: 0x0052A960
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.riverDist, "riverDist", 0, false);
		}

		// Token: 0x04008EE8 RID: 36584
		public List<SurfaceTile.RoadLink> potentialRoads;

		// Token: 0x04008EE9 RID: 36585
		public List<SurfaceTile.RiverLink> potentialRivers;

		// Token: 0x04008EEA RID: 36586
		public int riverDist;

		// Token: 0x02003E0C RID: 15884
		public struct RoadLink
		{
			// Token: 0x0400F4DF RID: 62687
			public PlanetTile neighbor;

			// Token: 0x0400F4E0 RID: 62688
			public RoadDef road;
		}

		// Token: 0x02003E0D RID: 15885
		public struct RiverLink
		{
			// Token: 0x0400F4E1 RID: 62689
			public PlanetTile neighbor;

			// Token: 0x0400F4E2 RID: 62690
			public RiverDef river;
		}
	}
}
