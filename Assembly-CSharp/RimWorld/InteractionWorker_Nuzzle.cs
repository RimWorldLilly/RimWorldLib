using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200151F RID: 5407
	public class InteractionWorker_Nuzzle : InteractionWorker
	{
		// Token: 0x0600845A RID: 33882 RVA: 0x002FA2CC File Offset: 0x002F84CC
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			this.AddNuzzledThought(initiator, recipient);
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x0600845B RID: 33883 RVA: 0x002FA2E8 File Offset: 0x002F84E8
		private void AddNuzzledThought(Pawn initiator, Pawn recipient)
		{
			Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.Nuzzled);
			thought_Memory.SetForcedStage(NuzzleUtility.GetNuzzleStageIndex(initiator));
			if (recipient.needs.mood != null)
			{
				recipient.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
			}
		}
	}
}
