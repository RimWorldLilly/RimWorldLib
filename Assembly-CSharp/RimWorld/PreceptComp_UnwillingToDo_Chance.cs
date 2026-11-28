using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200163D RID: 5693
	public class PreceptComp_UnwillingToDo_Chance : PreceptComp_UnwillingToDo
	{
		// Token: 0x06008EA0 RID: 36512 RVA: 0x00346CB5 File Offset: 0x00344EB5
		public override bool MemberWillingToDo(HistoryEvent ev)
		{
			return Rand.Value >= this.chance || base.MemberWillingToDo(ev);
		}

		// Token: 0x06008EA1 RID: 36513 RVA: 0x00346CCD File Offset: 0x00344ECD
		public override string GetProhibitionText()
		{
			return null;
		}

		// Token: 0x0400576A RID: 22378
		public float chance;
	}
}
