using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101A RID: 4122
	public class GeneratedLocationDef : Def
	{
		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x060067D5 RID: 26581 RVA: 0x0024AB8C File Offset: 0x00248D8C
		public List<PlanetLayerDef> LayerDefs
		{
			get
			{
				List<PlanetLayerDef> result;
				if ((result = this.layerDefs) == null && (result = GeneratedLocationDef.SurfaceList) == null)
				{
					List<PlanetLayerDef> list = new List<PlanetLayerDef>();
					list.Add(PlanetLayerDefOf.Surface);
					result = list;
					GeneratedLocationDef.SurfaceList = list;
				}
				return result;
			}
		}

		// Token: 0x0400423A RID: 16954
		private List<PlanetLayerDef> layerDefs;

		// Token: 0x0400423B RID: 16955
		public int layerMaximum = -1;

		// Token: 0x0400423C RID: 16956
		public WorldObjectDef worldObjectDef;

		// Token: 0x0400423D RID: 16957
		public List<ThingDef> preciousResources;

		// Token: 0x0400423E RID: 16958
		public FloatRange TimeoutRangeDays = new FloatRange(20f, 40f);

		// Token: 0x0400423F RID: 16959
		public float weight = 1f;

		// Token: 0x04004240 RID: 16960
		private static List<PlanetLayerDef> SurfaceList;
	}
}
