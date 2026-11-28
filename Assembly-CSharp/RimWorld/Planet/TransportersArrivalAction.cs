using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023D4 RID: 9172
	public abstract class TransportersArrivalAction : IExposable
	{
		// Token: 0x1700283D RID: 10301
		// (get) Token: 0x0600E494 RID: 58516
		public abstract bool GeneratesMap { get; }

		// Token: 0x0600E495 RID: 58517 RVA: 0x0054EC12 File Offset: 0x0054CE12
		public virtual FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			return true;
		}

		// Token: 0x0600E496 RID: 58518 RVA: 0x0054EC1A File Offset: 0x0054CE1A
		public virtual bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
		{
			return false;
		}

		// Token: 0x0600E497 RID: 58519
		public abstract void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile);

		// Token: 0x0600E498 RID: 58520 RVA: 0x0054EC1D File Offset: 0x0054CE1D
		public virtual void ExposeData()
		{
		}
	}
}
