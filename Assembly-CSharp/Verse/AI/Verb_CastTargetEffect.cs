using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000810 RID: 2064
	public class Verb_CastTargetEffect : Verb_CastBase
	{
		// Token: 0x06004105 RID: 16645 RVA: 0x0019D170 File Offset: 0x0019B370
		protected override bool TryCastShot()
		{
			Pawn casterPawn = this.CasterPawn;
			Thing thing = this.currentTarget.Thing;
			if (casterPawn == null || thing == null)
			{
				return false;
			}
			foreach (CompTargetEffect compTargetEffect in base.EquipmentSource.GetComps<CompTargetEffect>())
			{
				compTargetEffect.DoEffectOn(casterPawn, thing);
			}
			CompApparelReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}
	}
}
