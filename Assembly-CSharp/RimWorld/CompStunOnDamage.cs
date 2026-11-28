using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A5 RID: 6565
	public class CompStunOnDamage : ThingComp
	{
		// Token: 0x17001CAE RID: 7342
		// (get) Token: 0x0600A4D9 RID: 42201 RVA: 0x003AE38A File Offset: 0x003AC58A
		private CompProperties_StunOnDamage Props
		{
			get
			{
				return (CompProperties_StunOnDamage)this.props;
			}
		}

		// Token: 0x0600A4DA RID: 42202 RVA: 0x003AE398 File Offset: 0x003AC598
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (dinfo.Def == this.Props.damage)
			{
				Pawn_StanceTracker stances = ((Pawn)this.parent).stances;
				if (stances == null)
				{
					return;
				}
				StunHandler stunner = stances.stunner;
				if (stunner == null)
				{
					return;
				}
				stunner.StunFor(this.Props.delayTicks, dinfo.Instigator, false, true, false);
			}
		}
	}
}
