using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103A RID: 4154
	public class HistoryAutoRecorderWorker_Prisoners : HistoryAutoRecorderWorker
	{
		// Token: 0x06006895 RID: 26773 RVA: 0x0024F61F File Offset: 0x0024D81F
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony.Count<Pawn>();
		}
	}
}
