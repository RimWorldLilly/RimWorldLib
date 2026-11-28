using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DD6 RID: 7638
	public class PawnColumnWorker_Timetable : PawnColumnWorker
	{
		// Token: 0x0600C681 RID: 50817 RVA: 0x0048B848 File Offset: 0x00489A48
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable == null)
			{
				return;
			}
			if (pawn.IsSubhuman)
			{
				return;
			}
			float num = rect.x;
			float num2 = rect.width / 24f;
			for (int i = 0; i < 24; i++)
			{
				Rect rect2 = new Rect(num, rect.y, num2, rect.height);
				this.DoTimeAssignment(rect2, pawn, i);
				num += num2;
			}
			GUI.color = Color.white;
			if (TimeAssignmentSelector.selectedAssignment != null)
			{
				UIHighlighter.HighlightOpportunity(rect, "TimeAssignmentTableRow-If" + TimeAssignmentSelector.selectedAssignment.defName + "Selected");
			}
		}

		// Token: 0x0600C682 RID: 50818 RVA: 0x0048B8E0 File Offset: 0x00489AE0
		public override void DoHeader(Rect rect, PawnTable table)
		{
			float num = rect.x;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.LowerCenter;
			float num2 = rect.width / 24f;
			for (int i = 0; i < 24; i++)
			{
				Widgets.Label(new Rect(num, rect.y, num2, rect.height + 3f), i.ToString());
				num += num2;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600C683 RID: 50819 RVA: 0x0048B953 File Offset: 0x00489B53
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 360);
		}

		// Token: 0x0600C684 RID: 50820 RVA: 0x0048B966 File Offset: 0x00489B66
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(504, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600C685 RID: 50821 RVA: 0x0048B980 File Offset: 0x00489B80
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), 600);
		}

		// Token: 0x0600C686 RID: 50822 RVA: 0x0048B993 File Offset: 0x00489B93
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 15);
		}

		// Token: 0x0600C687 RID: 50823 RVA: 0x0048B9A4 File Offset: 0x00489BA4
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C688 RID: 50824 RVA: 0x0048B9C7 File Offset: 0x00489BC7
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.timetable == null)
			{
				return int.MinValue;
			}
			return pawn.timetable.times.FirstIndexOf((TimeAssignmentDef x) => x == TimeAssignmentDefOf.Work);
		}

		// Token: 0x0600C689 RID: 50825 RVA: 0x0048BA08 File Offset: 0x00489C08
		private void DoTimeAssignment(Rect rect, Pawn p, int hour)
		{
			rect = rect.ContractedBy(1f);
			bool mouseButton = Input.GetMouseButton(0);
			TimeAssignmentDef assignment = p.timetable.GetAssignment(hour);
			GUI.DrawTexture(rect, assignment.ColorTexture);
			if (!mouseButton)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawBox(rect, 2, null);
				if (mouseButton && assignment != TimeAssignmentSelector.selectedAssignment && TimeAssignmentSelector.selectedAssignment != null)
				{
					SoundDefOf.Designate_DragStandard_Changed_NoCam.PlayOneShotOnCamera(null);
					p.timetable.SetAssignment(hour, TimeAssignmentSelector.selectedAssignment);
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeAssignments, KnowledgeAmount.SmallInteraction);
					if (TimeAssignmentSelector.selectedAssignment == TimeAssignmentDefOf.Meditate)
					{
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.MeditationSchedule, KnowledgeAmount.Total);
					}
				}
			}
		}
	}
}
