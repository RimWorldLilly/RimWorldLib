using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D90 RID: 3472
	public abstract class ThoughtWorker
	{
		// Token: 0x06005FB8 RID: 24504 RVA: 0x0022D2B6 File Offset: 0x0022B4B6
		public virtual string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.Named("PAWN"));
		}

		// Token: 0x06005FB9 RID: 24505 RVA: 0x0022D2CE File Offset: 0x0022B4CE
		public virtual string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.Named("PAWN"));
		}

		// Token: 0x06005FBA RID: 24506 RVA: 0x0022D2E8 File Offset: 0x0022B4E8
		private ThoughtState? FailState(Pawn p)
		{
			if (!ThoughtUtility.CanGetThought(p, this.def, false))
			{
				return new ThoughtState?(ThoughtState.Inactive);
			}
			return null;
		}

		// Token: 0x06005FBB RID: 24507 RVA: 0x0022D318 File Offset: 0x0022B518
		public ThoughtState CurrentState(Pawn p)
		{
			return this.PostProcessedState(this.FailState(p) ?? this.CurrentStateInternal(p));
		}

		// Token: 0x06005FBC RID: 24508 RVA: 0x0022D34C File Offset: 0x0022B54C
		public ThoughtState CurrentSocialState(Pawn p, Pawn otherPawn)
		{
			return this.PostProcessedState(this.FailState(p) ?? this.CurrentSocialStateInternal(p, otherPawn));
		}

		// Token: 0x06005FBD RID: 24509 RVA: 0x0022D381 File Offset: 0x0022B581
		private ThoughtState PostProcessedState(ThoughtState state)
		{
			if (this.def.invert)
			{
				if (state.Active)
				{
					state = ThoughtState.Inactive;
				}
				else
				{
					state = ThoughtState.ActiveAtStage(0);
				}
			}
			return state;
		}

		// Token: 0x06005FBE RID: 24510 RVA: 0x0022D3AB File Offset: 0x0022B5AB
		protected virtual ThoughtState CurrentStateInternal(Pawn p)
		{
			throw new NotImplementedException(this.def.defName + " (normal)");
		}

		// Token: 0x06005FBF RID: 24511 RVA: 0x0022D3C7 File Offset: 0x0022B5C7
		protected virtual ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			throw new NotImplementedException(this.def.defName + " (social)");
		}

		// Token: 0x06005FC0 RID: 24512 RVA: 0x0022D3E3 File Offset: 0x0022B5E3
		public virtual float MoodMultiplier(Pawn p)
		{
			return 1f;
		}

		// Token: 0x040035DA RID: 13786
		public ThoughtDef def;
	}
}
