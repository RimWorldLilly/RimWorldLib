using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200167E RID: 5758
	public class RitualStage_WaitingForParticipants : RitualStage
	{
		// Token: 0x0600905A RID: 36954 RVA: 0x0034FD54 File Offset: 0x0034DF54
		public override float ProgressPerTick(LordJob_Ritual ritual)
		{
			foreach (Pawn p in ritual.assignments.SpectatorsForReading)
			{
				if (!ritual.IsParticipating(p))
				{
					return 0f;
				}
			}
			return base.ProgressPerTick(ritual);
		}
	}
}
