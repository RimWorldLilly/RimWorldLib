using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021FC RID: 8700
	public class QuestNode_AddMemoryThought : QuestNode
	{
		// Token: 0x0600D81A RID: 55322 RVA: 0x005026BB File Offset: 0x005008BB
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D81B RID: 55323 RVA: 0x005026C0 File Offset: 0x005008C0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				foreach (Pawn pawn in this.pawns.GetValue(slate))
				{
					QuestPart_AddMemoryThought questPart_AddMemoryThought = new QuestPart_AddMemoryThought();
					questPart_AddMemoryThought.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
					questPart_AddMemoryThought.def = this.def.GetValue(slate);
					questPart_AddMemoryThought.pawn = pawn;
					questPart_AddMemoryThought.otherPawn = this.otherPawn.GetValue(slate);
					questPart_AddMemoryThought.addToLookTargets = this.addToLookTargets.GetValue(slate).GetValueOrDefault(true);
					QuestGen.quest.AddPart(questPart_AddMemoryThought);
				}
			}
		}

		// Token: 0x04008979 RID: 35193
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400897A RID: 35194
		public SlateRef<ThoughtDef> def;

		// Token: 0x0400897B RID: 35195
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400897C RID: 35196
		public SlateRef<Pawn> otherPawn;

		// Token: 0x0400897D RID: 35197
		public SlateRef<bool?> addToLookTargets;
	}
}
