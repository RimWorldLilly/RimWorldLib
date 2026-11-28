using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002343 RID: 9027
	public class PlanetLayerConnection : IExposable
	{
		// Token: 0x0600DF22 RID: 57122 RVA: 0x005302ED File Offset: 0x0052E4ED
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.fuelCost, "fuelCost", 0f, false);
			Scribe_References.Look<PlanetLayer>(ref this.origin, "origin", false);
			Scribe_References.Look<PlanetLayer>(ref this.target, "target", false);
		}

		// Token: 0x04008F48 RID: 36680
		public PlanetLayer origin;

		// Token: 0x04008F49 RID: 36681
		public PlanetLayer target;

		// Token: 0x04008F4A RID: 36682
		public float fuelCost;
	}
}
