using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013FC RID: 5116
	public class CompAbilityEffect_FireBurst : CompAbilityEffect
	{
		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x06007EDD RID: 32477 RVA: 0x002D59D3 File Offset: 0x002D3BD3
		private new CompProperties_AbilityFireBurst Props
		{
			get
			{
				return (CompProperties_AbilityFireBurst)this.props;
			}
		}

		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x06007EDE RID: 32478 RVA: 0x002D59E0 File Offset: 0x002D3BE0
		private Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x06007EDF RID: 32479 RVA: 0x002D59F0 File Offset: 0x002D3BF0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			GenExplosion.DoExplosion(this.Pawn.Position, this.Pawn.MapHeld, this.Props.radius, DamageDefOf.Flame, this.Pawn, -1, -1f, null, null, null, null, ThingDefOf.Filth_Fuel, 1f, 1, null, null, 255, false, null, 0f, 1, 1f, false, null, null, null, false, 0.6f, 0f, true, null, 1f, null, null, null, null);
			base.Apply(target, dest);
		}

		// Token: 0x06007EE0 RID: 32480 RVA: 0x002D5A99 File Offset: 0x002D3C99
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			CompAbilityEffect_FireBurst.<GetPreCastActions>d__5 <GetPreCastActions>d__ = new CompAbilityEffect_FireBurst.<GetPreCastActions>d__5(-2);
			<GetPreCastActions>d__.<>4__this = this;
			return <GetPreCastActions>d__;
		}

		// Token: 0x06007EE1 RID: 32481 RVA: 0x002D5AAC File Offset: 0x002D3CAC
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			if (this.Pawn.Faction == Faction.OfPlayer)
			{
				return false;
			}
			if (target.HasThing)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					return pawn.TargetCurrentlyAimingAt == this.Pawn;
				}
			}
			return false;
		}

		// Token: 0x06007EE2 RID: 32482 RVA: 0x002D5AFE File Offset: 0x002D3CFE
		public override void CompTickInterval(int delta)
		{
			if (this.parent.Casting)
			{
				FireBurstUtility.ThrowFuelTick(this.Pawn.Position, this.Props.radius, this.Pawn.Map);
			}
		}
	}
}
