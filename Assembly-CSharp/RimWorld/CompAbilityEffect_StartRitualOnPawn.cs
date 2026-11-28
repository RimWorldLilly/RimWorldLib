using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001453 RID: 5203
	public class CompAbilityEffect_StartRitualOnPawn : CompAbilityEffect_StartRitual
	{
		// Token: 0x170015E8 RID: 5608
		// (get) Token: 0x06007FF0 RID: 32752 RVA: 0x002DA3F6 File Offset: 0x002D85F6
		public new CompProperties_AbilityStartRitualOnPawn Props
		{
			get
			{
				return (CompProperties_AbilityStartRitualOnPawn)this.props;
			}
		}

		// Token: 0x06007FF1 RID: 32753 RVA: 0x002DA403 File Offset: 0x002D8603
		protected virtual Precept_Ritual RitualForTarget(Pawn pawn)
		{
			return base.Ritual;
		}

		// Token: 0x06007FF2 RID: 32754 RVA: 0x002DA40C File Offset: 0x002D860C
		public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			Pawn pawn = target.Pawn;
			Precept_Ritual precept_Ritual = this.RitualForTarget(pawn);
			TargetInfo targetInfo = TargetInfo.Invalid;
			if (precept_Ritual.targetFilter != null)
			{
				targetInfo = precept_Ritual.targetFilter.BestTarget(this.parent.pawn, target.ToTargetInfo(this.parent.pawn.MapHeld));
			}
			return precept_Ritual.GetRitualBeginWindow(targetInfo, null, confirmAction, this.parent.pawn, new Dictionary<string, Pawn>
			{
				{
					this.Props.targetRoleId,
					pawn
				}
			}, null);
		}
	}
}
