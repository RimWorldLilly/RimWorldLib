using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E39 RID: 3641
	public class ThoughtWorker_IsDayForNightOwl : ThoughtWorker
	{
		// Token: 0x06006168 RID: 24936 RVA: 0x00232B50 File Offset: 0x00230D50
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && GenLocalDate.HourInteger(p) >= 11 && GenLocalDate.HourInteger(p) <= 17;
		}
	}
}
