using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D92 RID: 3474
	public abstract class ThoughtWorker_Precept_Social : ThoughtWorker
	{
		// Token: 0x06005FC5 RID: 24517
		protected abstract ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn);

		// Token: 0x06005FC6 RID: 24518 RVA: 0x0022D464 File Offset: 0x0022B664
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			Ideo ideo = p.Ideo;
			if (ideo == null)
			{
				return ThoughtState.Inactive;
			}
			if (!ideo.cachedPossibleSituationalThoughts.Contains(this.def))
			{
				return ThoughtState.Inactive;
			}
			if (this.def.gender != Gender.None && otherPawn.gender != this.def.gender)
			{
				return ThoughtState.Inactive;
			}
			return this.ShouldHaveThought(p, otherPawn);
		}

		// Token: 0x06005FC7 RID: 24519 RVA: 0x0022D4C8 File Offset: 0x0022B6C8
		public override string PostProcessLabel(Pawn p, string label)
		{
			return base.PostProcessLabel(p, label) + " (" + "Ideo".Translate() + ")";
		}

		// Token: 0x06005FC8 RID: 24520 RVA: 0x0022D4FC File Offset: 0x0022B6FC
		public override string PostProcessDescription(Pawn p, string description)
		{
			return base.PostProcessDescription(p, description) + "\n\n" + "ComesFromIdeo".Translate() + ": " + p.Ideo.name;
		}
	}
}
