using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F8 RID: 4600
	public class StatsRecord : IExposable
	{
		// Token: 0x0600725D RID: 29277 RVA: 0x00280D48 File Offset: 0x0027EF48
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.numRaidsEnemy, "numRaidsEnemy", 0, false);
			Scribe_Values.Look<int>(ref this.numThreatBigs, "numThreatsQueued", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsKilled, "colonistsKilled", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsLaunched, "colonistsLaunched", 0, false);
			Scribe_Values.Look<int>(ref this.greatestPopulation, "greatestPopulation", 3, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.UpdateGreatestPopulation();
			}
		}

		// Token: 0x0600725E RID: 29278 RVA: 0x00280DBD File Offset: 0x0027EFBD
		public void Notify_ColonistKilled()
		{
			this.colonistsKilled++;
		}

		// Token: 0x0600725F RID: 29279 RVA: 0x00280DD0 File Offset: 0x0027EFD0
		public void UpdateGreatestPopulation()
		{
			int a = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists.Count<Pawn>();
			this.greatestPopulation = Mathf.Max(a, this.greatestPopulation);
		}

		// Token: 0x0400485C RID: 18524
		public int numRaidsEnemy;

		// Token: 0x0400485D RID: 18525
		public int numThreatBigs;

		// Token: 0x0400485E RID: 18526
		public int colonistsKilled;

		// Token: 0x0400485F RID: 18527
		public int colonistsLaunched;

		// Token: 0x04004860 RID: 18528
		public int greatestPopulation;
	}
}
