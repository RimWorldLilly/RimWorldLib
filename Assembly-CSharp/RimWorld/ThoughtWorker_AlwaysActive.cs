using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E43 RID: 3651
	public class ThoughtWorker_AlwaysActive : ThoughtWorker
	{
		// Token: 0x0600617C RID: 24956 RVA: 0x002331D2 File Offset: 0x002313D2
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return true;
		}

		// Token: 0x0600617D RID: 24957 RVA: 0x002331DA File Offset: 0x002313DA
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			return true;
		}
	}
}
