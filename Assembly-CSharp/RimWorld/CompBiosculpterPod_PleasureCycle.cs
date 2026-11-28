using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001839 RID: 6201
	public class CompBiosculpterPod_PleasureCycle : CompBiosculpterPod_Cycle
	{
		// Token: 0x06009B43 RID: 39747 RVA: 0x0037FCA8 File Offset: 0x0037DEA8
		public override void CycleCompleted(Pawn pawn)
		{
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BiosculpterPleasure, null, null);
			Messages.Message("BiosculpterPleasureCompletedMessage".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
