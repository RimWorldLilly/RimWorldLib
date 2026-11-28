using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021D0 RID: 8656
	public class QuestNode_GetPawnsWithRoyalTitle : QuestNode
	{
		// Token: 0x0600D772 RID: 55154 RVA: 0x004FF9DC File Offset: 0x004FDBDC
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D773 RID: 55155 RVA: 0x004FF9E0 File Offset: 0x004FDBE0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				IEnumerable<Pawn> filteredPawns = this.GetFilteredPawns(this.pawns.GetValue(slate));
				slate.Set<IEnumerable<Pawn>>(this.storeAs.GetValue(slate), filteredPawns, false);
				if (this.storeCountAs.GetValue(slate) != null)
				{
					slate.Set<int>(this.storeCountAs.GetValue(slate), filteredPawns.Count<Pawn>(), false);
				}
				if (this.storePawnsLabelAs.GetValue(slate) != null)
				{
					slate.Set<string>(this.storePawnsLabelAs.GetValue(slate), (from p in filteredPawns
					select p.LabelNoCountColored.Resolve()).ToCommaList(true, false), false);
				}
			}
		}

		// Token: 0x0600D774 RID: 55156 RVA: 0x004FFA9F File Offset: 0x004FDC9F
		private IEnumerable<Pawn> GetFilteredPawns(List<Pawn> pawns)
		{
			QuestNode_GetPawnsWithRoyalTitle.<GetFilteredPawns>d__6 <GetFilteredPawns>d__ = new QuestNode_GetPawnsWithRoyalTitle.<GetFilteredPawns>d__6(-2);
			<GetFilteredPawns>d__.<>3__pawns = pawns;
			return <GetFilteredPawns>d__;
		}

		// Token: 0x040088D9 RID: 35033
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x040088DA RID: 35034
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088DB RID: 35035
		[NoTranslate]
		public SlateRef<string> storeCountAs;

		// Token: 0x040088DC RID: 35036
		[NoTranslate]
		public SlateRef<string> storePawnsLabelAs;
	}
}
