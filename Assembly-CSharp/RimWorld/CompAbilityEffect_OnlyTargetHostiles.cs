using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001425 RID: 5157
	public class CompAbilityEffect_OnlyTargetHostiles : CompAbilityEffect
	{
		// Token: 0x06007F5F RID: 32607 RVA: 0x002D8146 File Offset: 0x002D6346
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			return this.parent.pawn != null && this.parent.pawn.HostileTo(target.Thing);
		}

		// Token: 0x06007F60 RID: 32608 RVA: 0x002D816E File Offset: 0x002D636E
		public override bool Valid(GlobalTargetInfo target, bool throwMessages = false)
		{
			return this.parent.pawn != null && this.parent.pawn.HostileTo(target.Thing);
		}

		// Token: 0x06007F61 RID: 32609 RVA: 0x002D8196 File Offset: 0x002D6396
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return this.parent.pawn != null && this.parent.pawn.HostileTo(target.Thing);
		}
	}
}
