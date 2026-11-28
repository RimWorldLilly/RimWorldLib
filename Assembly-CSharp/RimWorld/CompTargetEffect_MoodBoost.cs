using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A17 RID: 6679
	public class CompTargetEffect_MoodBoost : CompTargetEffect
	{
		// Token: 0x0600A7EB RID: 42987 RVA: 0x003BBCD4 File Offset: 0x003B9ED4
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead || pawn.needs == null || pawn.needs.mood == null)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.ArtifactMoodBoost), null);
		}
	}
}
