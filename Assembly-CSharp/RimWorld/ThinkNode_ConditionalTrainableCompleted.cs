using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D4A RID: 3402
	public class ThinkNode_ConditionalTrainableCompleted : ThinkNode_Conditional
	{
		// Token: 0x06005F11 RID: 24337 RVA: 0x0022B8C5 File Offset: 0x00229AC5
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalTrainableCompleted thinkNode_ConditionalTrainableCompleted = (ThinkNode_ConditionalTrainableCompleted)base.DeepCopy(resolve);
			thinkNode_ConditionalTrainableCompleted.trainable = this.trainable;
			return thinkNode_ConditionalTrainableCompleted;
		}

		// Token: 0x06005F12 RID: 24338 RVA: 0x0022B8DF File Offset: 0x00229ADF
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.training != null && pawn.training.HasLearned(this.trainable);
		}

		// Token: 0x040035BB RID: 13755
		private TrainableDef trainable;
	}
}
