using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200222D RID: 8749
	public class QuestNode_RemoveMemoryThought : QuestNode
	{
		// Token: 0x0600D8B9 RID: 55481 RVA: 0x005052E2 File Offset: 0x005034E2
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8BA RID: 55482 RVA: 0x005052E8 File Offset: 0x005034E8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_RemoveMemoryThought questPart_RemoveMemoryThought = new QuestPart_RemoveMemoryThought();
			questPart_RemoveMemoryThought.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_RemoveMemoryThought.def = this.def.GetValue(slate);
			questPart_RemoveMemoryThought.pawn = this.pawn.GetValue(slate);
			questPart_RemoveMemoryThought.count = this.count.GetValue(slate);
			questPart_RemoveMemoryThought.otherPawn = this.otherPawn.GetValue(slate);
			questPart_RemoveMemoryThought.addToLookTargets = this.addToLookTargets.GetValue(slate).GetValueOrDefault(true);
			QuestGen.quest.AddPart(questPart_RemoveMemoryThought);
		}

		// Token: 0x04008A65 RID: 35429
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A66 RID: 35430
		public SlateRef<ThoughtDef> def;

		// Token: 0x04008A67 RID: 35431
		public SlateRef<Pawn> pawn;

		// Token: 0x04008A68 RID: 35432
		public SlateRef<Pawn> otherPawn;

		// Token: 0x04008A69 RID: 35433
		public SlateRef<int?> count;

		// Token: 0x04008A6A RID: 35434
		public SlateRef<bool?> addToLookTargets;
	}
}
