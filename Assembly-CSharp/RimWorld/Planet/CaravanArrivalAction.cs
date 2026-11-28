using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002372 RID: 9074
	public abstract class CaravanArrivalAction : IExposable
	{
		// Token: 0x170027A0 RID: 10144
		// (get) Token: 0x0600E13E RID: 57662
		public abstract string Label { get; }

		// Token: 0x170027A1 RID: 10145
		// (get) Token: 0x0600E13F RID: 57663
		public abstract string ReportString { get; }

		// Token: 0x0600E140 RID: 57664 RVA: 0x00539F65 File Offset: 0x00538165
		public virtual FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			return true;
		}

		// Token: 0x0600E141 RID: 57665
		public abstract void Arrived(Caravan caravan);

		// Token: 0x0600E142 RID: 57666 RVA: 0x00539F6D File Offset: 0x0053816D
		public virtual void ExposeData()
		{
		}
	}
}
