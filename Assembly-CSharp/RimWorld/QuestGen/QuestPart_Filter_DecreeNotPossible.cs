using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022CC RID: 8908
	public class QuestPart_Filter_DecreeNotPossible : QuestPart_Filter
	{
		// Token: 0x0600DBBB RID: 56251 RVA: 0x0051E140 File Offset: 0x0051C340
		protected override bool Pass(SignalArgs args)
		{
			Pawn pawn;
			return args.TryGetArg<Pawn>("SUBJECT", out pawn) && (pawn.royalty == null || !pawn.royalty.PossibleDecreeQuests.Contains(this.quest.root));
		}
	}
}
