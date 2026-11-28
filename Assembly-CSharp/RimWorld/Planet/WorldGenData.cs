using System;
using System.Collections.Generic;

namespace RimWorld.Planet
{
	// Token: 0x02002347 RID: 9031
	public class WorldGenData : WorldComponent
	{
		// Token: 0x0600DFA3 RID: 57251 RVA: 0x005325C3 File Offset: 0x005307C3
		public WorldGenData(World world) : base(world)
		{
		}

		// Token: 0x04008F84 RID: 36740
		public Dictionary<PlanetLayer, List<PlanetTile>> roadNodes = new Dictionary<PlanetLayer, List<PlanetTile>>();

		// Token: 0x04008F85 RID: 36741
		public Dictionary<PlanetLayer, List<PlanetTile>> ancientSites = new Dictionary<PlanetLayer, List<PlanetTile>>();
	}
}
