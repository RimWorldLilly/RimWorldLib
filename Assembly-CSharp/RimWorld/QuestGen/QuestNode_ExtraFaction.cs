using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002210 RID: 8720
	public class QuestNode_ExtraFaction : QuestNode
	{
		// Token: 0x0600D857 RID: 55383 RVA: 0x005035A0 File Offset: 0x005017A0
		protected override void RunInt()
		{
			Faction value = this.faction.GetValue(QuestGen.slate);
			if (value == null)
			{
				Thing value2 = this.factionOf.GetValue(QuestGen.slate);
				if (value2 != null)
				{
					value = value2.Faction;
				}
				if (value == null)
				{
					return;
				}
			}
			QuestGen.quest.AddPart(new QuestPart_ExtraFaction
			{
				affectedPawns = this.pawns.GetValue(QuestGen.slate).ToList<Pawn>(),
				extraFaction = new ExtraFaction(value, this.factionType.GetValue(QuestGen.slate)),
				areHelpers = this.areHelpers.GetValue(QuestGen.slate),
				inSignalRemovePawn = this.inSignalRemovePawn.GetValue(QuestGen.slate),
				inSignalsRemovePawn = this.inSignalsRemovePawn.GetValue(QuestGen.slate).ToList<string>()
			});
		}

		// Token: 0x0600D858 RID: 55384 RVA: 0x0050366D File Offset: 0x0050186D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x040089C8 RID: 35272
		public SlateRef<Thing> factionOf;

		// Token: 0x040089C9 RID: 35273
		public SlateRef<Faction> faction;

		// Token: 0x040089CA RID: 35274
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x040089CB RID: 35275
		public SlateRef<ExtraFactionType> factionType;

		// Token: 0x040089CC RID: 35276
		public SlateRef<bool> areHelpers;

		// Token: 0x040089CD RID: 35277
		[NoTranslate]
		public SlateRef<string> inSignalRemovePawn;

		// Token: 0x040089CE RID: 35278
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignalsRemovePawn;
	}
}
