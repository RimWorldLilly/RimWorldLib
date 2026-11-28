using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE9 RID: 7657
	[StaticConstructorOnStartup]
	public class PawnRitualRoleSelectionWidget : PawnRoleSelectionWidgetBase<RitualRole>
	{
		// Token: 0x0600C70D RID: 50957 RVA: 0x0048E890 File Offset: 0x0048CA90
		public override string SpectatorsLabel()
		{
			Precept_Ritual precept_Ritual = this.ritual;
			string text;
			if (precept_Ritual == null)
			{
				text = null;
			}
			else
			{
				RitualBehaviorWorker behavior = precept_Ritual.behavior;
				text = ((behavior != null) ? behavior.def.spectatorsLabel : null);
			}
			return text ?? base.SpectatorsLabel();
		}

		// Token: 0x0600C70E RID: 50958 RVA: 0x0048E8C0 File Offset: 0x0048CAC0
		public PawnRitualRoleSelectionWidget(RitualRoleAssignments assignments, Precept_Ritual ritual, TargetInfo target, RitualOutcomeEffectDef outcome) : base(assignments, assignments)
		{
			this.ritualAssignments = assignments;
			this.ritual = ritual;
			this.target = target;
			this.outcome = outcome;
			this.showIdeoIcon = (ritual != null && ritual.showIdeoIconsInDialog);
		}

		// Token: 0x0600C70F RID: 50959 RVA: 0x0048E91C File Offset: 0x0048CB1C
		public override bool ShouldDrawHighlight(RitualRole highlightedRole, Pawn pawn)
		{
			string text;
			return highlightedRole != null && !DragAndDropWidget.Dragging && !this.assignments.AssignedPawns(highlightedRole).Contains(pawn) && highlightedRole.AppliesToPawn(pawn, out text, this.target, null, this.ritualAssignments, this.ritual, true);
		}

		// Token: 0x0600C710 RID: 50960 RVA: 0x0048E968 File Offset: 0x0048CB68
		public override void Notify_AssignmentsChanged()
		{
			if (this.outcome != null)
			{
				foreach (RitualOutcomeComp ritualOutcomeComp in this.outcome.comps)
				{
					RitualOutcomeComp ritualOutcomeComp2 = ritualOutcomeComp;
					RitualRoleAssignments assignments = this.ritualAssignments;
					Precept_Ritual precept_Ritual = this.ritual;
					RitualOutcomeComp_Data data;
					if (precept_Ritual == null)
					{
						data = null;
					}
					else
					{
						RitualOutcomeEffectWorker outcomeEffect = precept_Ritual.outcomeEffect;
						data = ((outcomeEffect != null) ? outcomeEffect.DataForComp(ritualOutcomeComp) : null);
					}
					ritualOutcomeComp2.Notify_AssignmentsChanged(assignments, data);
				}
			}
		}

		// Token: 0x0600C711 RID: 50961 RVA: 0x0048E9EC File Offset: 0x0048CBEC
		public override string ExtraInfoForRole(RitualRole role, Pawn pawnToBeAssigned, IEnumerable<Pawn> currentlyAssigned)
		{
			string text = (pawnToBeAssigned == null) ? role.ExtraInfoForDialog(currentlyAssigned) : null;
			Pawn pawn = pawnToBeAssigned ?? currentlyAssigned.FirstOrDefault<Pawn>();
			PreceptDef preceptDef;
			if (pawn == null)
			{
				preceptDef = null;
			}
			else
			{
				Ideo ideo = pawn.Ideo;
				if (ideo == null)
				{
					preceptDef = null;
				}
				else
				{
					Precept_Role role2 = ideo.GetRole(pawn);
					preceptDef = ((role2 != null) ? role2.def : null);
				}
			}
			PreceptDef preceptDef2 = preceptDef;
			if (pawn != null && preceptDef2 != role.precept && role.substitutable && role.precept != null)
			{
				if (text != null)
				{
					text += "\n\n";
				}
				text += this.ExtraRoleInfo(role);
				if (!Find.IdeoManager.classicMode)
				{
					text = text + ": " + this.BonusExplanationInfo(role).CapitalizeFirst().Resolve();
				}
			}
			return text;
		}

		// Token: 0x0600C712 RID: 50962 RVA: 0x0048EAA2 File Offset: 0x0048CCA2
		public override bool ShouldGrayOut(Pawn pawn, out TaggedString reason)
		{
			if (base.ShouldGrayOut(pawn, out reason))
			{
				return true;
			}
			Precept_Ritual precept_Ritual = this.ritual;
			return ((precept_Ritual != null) ? precept_Ritual.obligationTargetFilter : null) != null && this.ritual.obligationTargetFilter.ShouldGrayOut(pawn, this.assignments, out reason);
		}

		// Token: 0x0600C713 RID: 50963 RVA: 0x0048EAE0 File Offset: 0x0048CCE0
		protected override string ExtraTipContents(Pawn pawn)
		{
			this.sb.Clear();
			this.tmpRelevantStats.Clear();
			foreach (RitualRole ritualRole in this.ritualAssignments.AllRolesForReading)
			{
				RitualRoleColonist ritualRoleColonist = ritualRole as RitualRoleColonist;
				if (ritualRoleColonist != null && ritualRoleColonist.usedStat != null)
				{
					this.tmpRelevantStats.Add(ritualRoleColonist.usedStat);
				}
			}
			if (this.tmpRelevantStats.Count == 0)
			{
				return null;
			}
			foreach (StatDef statDef in this.tmpRelevantStats)
			{
				if (statDef.Worker.IsDisabledFor(pawn))
				{
					this.sb.AppendLine("  - " + (statDef.LabelCap + ": " + "Disabled".Translate()).Colorize(ColorLibrary.RedReadable));
				}
				else
				{
					this.sb.AppendLine("  - " + statDef.LabelCap + ": " + statDef.Worker.ValueToStringFor(pawn));
				}
			}
			return this.sb.ToString();
		}

		// Token: 0x0600C714 RID: 50964 RVA: 0x0048EC50 File Offset: 0x0048CE50
		public string ExtraRoleInfo(RitualRole role)
		{
			Precept precept = this.ritual.ideo.PreceptsListForReading.FirstOrDefault((Precept p) => p.def == role.precept);
			if (precept == null)
			{
				return null;
			}
			return "RitualRoleRequiresSocialRole".Translate(precept.Label);
		}

		// Token: 0x0600C715 RID: 50965 RVA: 0x0048ECAC File Offset: 0x0048CEAC
		public TaggedString BonusExplanationInfo(RitualRole role)
		{
			TaggedString taggedString = "";
			bool flag = false;
			if (this.ritual.outcomeEffect != null && !this.ritual.outcomeEffect.def.comps.NullOrEmpty<RitualOutcomeComp>())
			{
				foreach (RitualOutcomeComp ritualOutcomeComp in this.ritual.outcomeEffect.def.comps)
				{
					if (ritualOutcomeComp is RitualOutcomeComp_RolePresentNotSubstituted)
					{
						if (flag)
						{
							taggedString += ", ";
						}
						taggedString += ritualOutcomeComp.GetBonusDescShort();
						flag = true;
					}
				}
			}
			if (flag)
			{
				return taggedString;
			}
			return "None".Translate();
		}

		// Token: 0x0600C716 RID: 50966 RVA: 0x0048ED78 File Offset: 0x0048CF78
		public override string SpectatorFilterReason(Pawn pawn)
		{
			if (this.ritual != null && this.ritual.behavior.def.spectatorFilter != null && !this.ritual.behavior.def.spectatorFilter.Allowed(pawn))
			{
				return this.ritual.behavior.def.spectatorFilter.description;
			}
			return null;
		}

		// Token: 0x04007137 RID: 28983
		protected RitualRoleAssignments ritualAssignments;

		// Token: 0x04007138 RID: 28984
		protected Precept_Ritual ritual;

		// Token: 0x04007139 RID: 28985
		protected TargetInfo target;

		// Token: 0x0400713A RID: 28986
		protected RitualOutcomeEffectDef outcome;

		// Token: 0x0400713B RID: 28987
		private StringBuilder sb = new StringBuilder();

		// Token: 0x0400713C RID: 28988
		private HashSet<StatDef> tmpRelevantStats = new HashSet<StatDef>();
	}
}
