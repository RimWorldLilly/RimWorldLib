using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C60 RID: 3168
	public class Command_BestowerCeremony : Command
	{
		// Token: 0x060059EA RID: 23018 RVA: 0x00211738 File Offset: 0x0020F938
		public Command_BestowerCeremony(LordJob_BestowingCeremony job, Pawn bestower, Pawn forPawn, Action<List<Pawn>> action)
		{
			this.bestower = bestower;
			this.forPawn = forPawn;
			this.action = action;
			this.job = job;
			this.defaultLabel = "BeginCeremony".Translate(this.forPawn);
			this.icon = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/BestowCeremony", true);
		}

		// Token: 0x060059EB RID: 23019 RVA: 0x0021179C File Offset: 0x0020F99C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!JobDriver_BestowingCeremony.AnalyzeThroneRoom(this.bestower, this.forPawn))
			{
				this.disabledReason = "BestowingCeremonyThroneroomRequirementsNotSatisfiedShort".Translate(this.forPawn.Named("PAWN"), this.forPawn.royalty.GetTitleAwardedWhenUpdating(this.bestower.Faction, this.forPawn.royalty.GetFavor(this.bestower.Faction)).label.Named("TITLE"));
				this.disabled = true;
			}
			else if (!this.job.GetSpot().IsValid)
			{
				this.disabledReason = "MessageBestowerUnreachable".Translate();
				this.disabled = true;
			}
			else
			{
				Lord lord = this.forPawn.GetLord();
				if (lord != null)
				{
					if (lord.LordJob is LordJob_Ritual)
					{
						this.disabledReason = "CantStartRitualTargetIsAlreadyInRitual".Translate(this.forPawn.LabelShort);
						this.disabled = true;
					}
					else
					{
						this.disabledReason = "MessageBestowingTargetIsBusy".Translate(this.forPawn.LabelShort);
						this.disabled = true;
					}
				}
			}
			return base.GizmoOnGUI(topLeft, maxWidth, parms);
		}

		// Token: 0x060059EC RID: 23020 RVA: 0x002118E4 File Offset: 0x0020FAE4
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			Find.WindowStack.Add(new Dialog_BeginRitual("RitualBestowingCeremony".Translate(), null, this.job.targetSpot.ToTargetInfo(this.bestower.Map), this.bestower.Map, delegate(RitualRoleAssignments assignments)
			{
				this.action((from p in assignments.Participants
				where p != this.bestower
				select p).ToList<Pawn>());
				return true;
			}, this.bestower, null, delegate(Pawn pawn, bool voluntary, bool allowOtherIdeos)
			{
				Lord lord = pawn.GetLord();
				return !(((lord != null) ? lord.LordJob : null) is LordJob_Ritual) && !pawn.IsSubhuman && !pawn.IsPrisonerOfColony && !pawn.RaceProps.Animal;
			}, "Begin".Translate(), new List<Pawn>
			{
				this.bestower,
				this.forPawn
			}, null, RitualOutcomeEffectDefOf.BestowingCeremony, null, null));
		}

		// Token: 0x0400338E RID: 13198
		private Pawn bestower;

		// Token: 0x0400338F RID: 13199
		private Pawn forPawn;

		// Token: 0x04003390 RID: 13200
		private Action<List<Pawn>> action;

		// Token: 0x04003391 RID: 13201
		private LordJob_BestowingCeremony job;
	}
}
