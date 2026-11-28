using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001039 RID: 4153
	public class HistoryAutoRecorderWorker_FreeColonists : HistoryAutoRecorderWorker
	{
		// Token: 0x06006893 RID: 26771 RVA: 0x0024F60A File Offset: 0x0024D80A
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers.Count<Pawn>();
		}
	}
}
