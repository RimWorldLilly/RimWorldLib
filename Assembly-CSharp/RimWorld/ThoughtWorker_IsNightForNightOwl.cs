using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3A RID: 3642
	public class ThoughtWorker_IsNightForNightOwl : ThoughtWorker
	{
		// Token: 0x0600616A RID: 24938 RVA: 0x00232B81 File Offset: 0x00230D81
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (GenLocalDate.HourInteger(p) >= 23 || GenLocalDate.HourInteger(p) <= 5);
		}
	}
}
