using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000828 RID: 2088
	public class JobDriver_CastAbilityMelee : JobDriver_CastAbility
	{
		// Token: 0x06004188 RID: 16776 RVA: 0x0019F92E File Offset: 0x0019DB2E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CastAbilityMelee.<MakeNewToils>d__0 <MakeNewToils>d__ = new JobDriver_CastAbilityMelee.<MakeNewToils>d__0(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x0019F93E File Offset: 0x0019DB3E
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			Ability ability = this.job.ability;
			if (ability == null)
			{
				return;
			}
			ability.Notify_StartedCasting();
		}
	}
}
