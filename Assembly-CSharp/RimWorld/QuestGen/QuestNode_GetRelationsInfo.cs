using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021DC RID: 8668
	public class QuestNode_GetRelationsInfo : QuestNode
	{
		// Token: 0x0600D7A1 RID: 55201 RVA: 0x00500539 File Offset: 0x004FE739
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D7A2 RID: 55202 RVA: 0x00500543 File Offset: 0x004FE743
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D7A3 RID: 55203 RVA: 0x00500550 File Offset: 0x004FE750
		private void SetVars(Slate slate)
		{
			if (this.pawn.GetValue(slate) == null)
			{
				return;
			}
			if (this.otherPawns.GetValue(slate) == null)
			{
				return;
			}
			QuestNode_GetRelationsInfo.tmpRelations.Clear();
			int num = 0;
			foreach (Pawn other in this.otherPawns.GetValue(slate))
			{
				PawnRelationDef mostImportantRelation = this.pawn.GetValue(slate).GetMostImportantRelation(other);
				if (mostImportantRelation != null)
				{
					QuestNode_GetRelationsInfo.tmpRelations.Add(mostImportantRelation.GetGenderSpecificLabel(other));
				}
				else
				{
					num++;
				}
			}
			if (num == 1)
			{
				QuestNode_GetRelationsInfo.tmpRelations.Add(this.nonRelatedLabel.GetValue(slate));
			}
			else if (num >= 2)
			{
				QuestNode_GetRelationsInfo.tmpRelations.Add(num.ToString() + " " + this.nonRelatedLabelPlural.GetValue(slate));
			}
			if (!QuestNode_GetRelationsInfo.tmpRelations.Any<string>())
			{
				return;
			}
			slate.Set<string>(this.storeAs.GetValue(slate), QuestNode_GetRelationsInfo.tmpRelations.ToCommaList(true, false), false);
			QuestNode_GetRelationsInfo.tmpRelations.Clear();
		}

		// Token: 0x04008900 RID: 35072
		public SlateRef<Pawn> pawn;

		// Token: 0x04008901 RID: 35073
		public SlateRef<IEnumerable<Pawn>> otherPawns;

		// Token: 0x04008902 RID: 35074
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008903 RID: 35075
		public SlateRef<string> nonRelatedLabel;

		// Token: 0x04008904 RID: 35076
		public SlateRef<string> nonRelatedLabelPlural;

		// Token: 0x04008905 RID: 35077
		private static List<string> tmpRelations = new List<string>();
	}
}
