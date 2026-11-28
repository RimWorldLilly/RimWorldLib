using System;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001DE8 RID: 7656
	[StaticConstructorOnStartup]
	public class PawnPsychicRitualRoleSelectionWidget : PawnRoleSelectionWidgetBase<PsychicRitualRoleDef>
	{
		// Token: 0x0600C70A RID: 50954 RVA: 0x0048E6F8 File Offset: 0x0048C8F8
		public PawnPsychicRitualRoleSelectionWidget(PsychicRitualDef ritualDef, PsychicRitualCandidatePool candidatePool, PsychicRitualRoleAssignments assignments) : base(candidatePool, assignments)
		{
			this.ritualDef = ritualDef;
			this.psychicRitualAssignments = assignments;
		}

		// Token: 0x0600C70B RID: 50955 RVA: 0x0048E71C File Offset: 0x0048C91C
		public override bool ShouldDrawHighlight(PsychicRitualRoleDef highlightedRole, Pawn pawn)
		{
			PsychicRitualRoleDef.Reason reason;
			return highlightedRole != null && !DragAndDropWidget.Dragging && !this.assignments.AssignedPawns(highlightedRole).Contains(pawn) && highlightedRole.PawnCanDo(PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, pawn, this.psychicRitualAssignments.Target, out reason);
		}

		// Token: 0x0600C70C RID: 50956 RVA: 0x0048E760 File Offset: 0x0048C960
		protected override string ExtraTipContents(Pawn pawn)
		{
			this.sb.Clear();
			this.sb.AppendLine("  - " + StatDefOf.PsychicSensitivity.LabelCap + ": " + StatDefOf.PsychicSensitivity.Worker.ValueToStringFor(pawn));
			if (ModsConfig.IdeologyActive && !Find.IdeoManager.classicMode && pawn.Ideo != null)
			{
				this.sb.AppendLineTagged("  - " + "Ideo".Translate().CapitalizeFirst() + ": " + pawn.Ideo.name.Colorize(pawn.Ideo.Color));
			}
			foreach (string str in this.ritualDef.GetPawnTooltipExtras(pawn))
			{
				this.sb.AppendLine("  - " + str);
			}
			return this.sb.ToString().TrimEndNewlines();
		}

		// Token: 0x04007134 RID: 28980
		protected PsychicRitualRoleAssignments psychicRitualAssignments;

		// Token: 0x04007135 RID: 28981
		protected PsychicRitualDef ritualDef;

		// Token: 0x04007136 RID: 28982
		private StringBuilder sb = new StringBuilder();
	}
}
