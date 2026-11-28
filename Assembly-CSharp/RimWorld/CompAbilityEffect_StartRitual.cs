using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001451 RID: 5201
	public class CompAbilityEffect_StartRitual : CompAbilityEffect
	{
		// Token: 0x170015E6 RID: 5606
		// (get) Token: 0x06007FE7 RID: 32743 RVA: 0x002DA169 File Offset: 0x002D8369
		public new CompProperties_AbilityStartRitual Props
		{
			get
			{
				return (CompProperties_AbilityStartRitual)this.props;
			}
		}

		// Token: 0x170015E7 RID: 5607
		// (get) Token: 0x06007FE8 RID: 32744 RVA: 0x002DA178 File Offset: 0x002D8378
		public Precept_Ritual Ritual
		{
			get
			{
				if (this.ritualCached == null && this.parent.pawn.Ideo != null)
				{
					for (int i = 0; i < this.parent.pawn.Ideo.PreceptsListForReading.Count; i++)
					{
						if (this.parent.pawn.Ideo.PreceptsListForReading[i].def == this.Props.ritualDef)
						{
							this.ritualCached = (Precept_Ritual)this.parent.pawn.Ideo.PreceptsListForReading[i];
							break;
						}
					}
				}
				return this.ritualCached;
			}
		}

		// Token: 0x06007FE9 RID: 32745 RVA: 0x002DA222 File Offset: 0x002D8422
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
		}

		// Token: 0x06007FEA RID: 32746 RVA: 0x002DA224 File Offset: 0x002D8424
		public override bool GizmoDisabled(out string reason)
		{
			Pawn initiator = this.GetInitiator();
			if (initiator == null)
			{
				reason = "AbilityStartRitualNoMembersOfIdeo".Translate(this.Ritual.ideo.memberName);
				return true;
			}
			if (ModsConfig.BiotechActive && !this.Props.allowedForChild && initiator.DevelopmentalStage.Juvenile())
			{
				reason = "AbilityStartRitualDisabledChild".Translate();
				return true;
			}
			if (initiator.Spawned && GatheringsUtility.AnyLordJobPreventsNewRituals(initiator.Map))
			{
				reason = "AbilitySpeechDisabledAnotherGatheringInProgress".Translate();
				return true;
			}
			string text;
			if (this.Ritual != null && this.Ritual.targetFilter != null && !this.Ritual.targetFilter.CanStart(initiator, TargetInfo.Invalid, out text))
			{
				reason = text;
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x06007FEB RID: 32747 RVA: 0x002DA2FC File Offset: 0x002D84FC
		public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			TargetInfo targetInfo = TargetInfo.Invalid;
			Pawn initiator = this.GetInitiator();
			if (this.Ritual.targetFilter != null)
			{
				targetInfo = this.Ritual.targetFilter.BestTarget(initiator, target.ToTargetInfo(initiator.Map));
			}
			return this.Ritual.GetRitualBeginWindow(targetInfo, null, confirmAction, this.parent.pawn, null, null);
		}

		// Token: 0x06007FEC RID: 32748 RVA: 0x002DA364 File Offset: 0x002D8564
		public Pawn GetInitiator()
		{
			if (this.parent.pawn != null && this.parent.pawn.Map == Find.CurrentMap)
			{
				return this.parent.pawn;
			}
			return Find.CurrentMap.mapPawns.FreeColonistsSpawned.FirstOrDefault((Pawn c) => c.Ideo == this.Ritual.ideo);
		}

		// Token: 0x04004FFA RID: 20474
		private Precept_Ritual ritualCached;
	}
}
