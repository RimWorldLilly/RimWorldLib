using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001683 RID: 5763
	public class RitualBehaviorWorker_Speech : RitualBehaviorWorker
	{
		// Token: 0x06009087 RID: 36999 RVA: 0x00350BD0 File Offset: 0x0034EDD0
		public RitualBehaviorWorker_Speech()
		{
		}

		// Token: 0x06009088 RID: 37000 RVA: 0x00350BD8 File Offset: 0x0034EDD8
		public RitualBehaviorWorker_Speech(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x06009089 RID: 37001 RVA: 0x00350BE4 File Offset: 0x0034EDE4
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Pawn organizer2 = assignments.AssignedPawns("speaker").First<Pawn>();
			return new LordJob_Joinable_Speech(target, organizer2, ritual, this.def.stages, assignments, false);
		}

		// Token: 0x0600908A RID: 37002 RVA: 0x00350C1C File Offset: 0x0034EE1C
		protected override void PostExecute(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Pawn arg = assignments.AssignedPawns("speaker").First<Pawn>();
			Find.LetterStack.ReceiveLetter(this.def.letterTitle.Formatted(ritual.Named("RITUAL")), this.def.letterText.Formatted(arg.Named("SPEAKER"), ritual.Named("RITUAL"), ritual.ideo.MemberNamePlural.Named("IDEOMEMBERS")) + "\n\n" + ritual.outcomeEffect.ExtraAlertParagraph(ritual), LetterDefOf.PositiveEvent, target, null, null, null, null, 0, true);
		}

		// Token: 0x0600908B RID: 37003 RVA: 0x00350CC8 File Offset: 0x0034EEC8
		public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			Precept_Role precept_Role = ritual.ideo.RolesListForReading.FirstOrDefault((Precept_Role r) => r.def == PreceptDefOf.IdeoRole_Leader);
			if (precept_Role == null)
			{
				return null;
			}
			if (precept_Role.ChosenPawnSingle() == null)
			{
				return "CantStartRitualRoleNotAssigned".Translate(precept_Role.LabelCap);
			}
			return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
		}
	}
}
