using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021E5 RID: 8677
	public class QuestNode_AssaultColony : QuestNode
	{
		// Token: 0x0600D7C9 RID: 55241 RVA: 0x00501798 File Offset: 0x004FF998
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D7CA RID: 55242 RVA: 0x0050179C File Offset: 0x004FF99C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate).EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_AssaultColony questPart_AssaultColony = new QuestPart_AssaultColony();
			questPart_AssaultColony.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_AssaultColony.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_AssaultColony.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_AssaultColony.faction = this.faction.GetValue(slate);
			questPart_AssaultColony.canTimeoutOrFlee = this.canTimeoutOrFlee.GetValue(slate).GetValueOrDefault(true);
			QuestGen.quest.AddPart(questPart_AssaultColony);
		}

		// Token: 0x0400893C RID: 35132
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400893D RID: 35133
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400893E RID: 35134
		public SlateRef<Faction> faction;

		// Token: 0x0400893F RID: 35135
		public SlateRef<bool?> canTimeoutOrFlee;
	}
}
