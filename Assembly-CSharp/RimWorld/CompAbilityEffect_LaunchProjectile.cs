using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200141C RID: 5148
	public class CompAbilityEffect_LaunchProjectile : CompAbilityEffect
	{
		// Token: 0x170015C5 RID: 5573
		// (get) Token: 0x06007F3F RID: 32575 RVA: 0x002D7648 File Offset: 0x002D5848
		public new CompProperties_AbilityLaunchProjectile Props
		{
			get
			{
				return (CompProperties_AbilityLaunchProjectile)this.props;
			}
		}

		// Token: 0x06007F40 RID: 32576 RVA: 0x002D7655 File Offset: 0x002D5855
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			this.LaunchProjectile(target);
		}

		// Token: 0x06007F41 RID: 32577 RVA: 0x002D7668 File Offset: 0x002D5868
		private void LaunchProjectile(LocalTargetInfo target)
		{
			if (this.Props.projectileDef != null)
			{
				Pawn pawn = this.parent.pawn;
				((Projectile)GenSpawn.Spawn(this.Props.projectileDef, pawn.Position, pawn.Map, WipeMode.Vanish)).Launch(pawn, pawn.DrawPos, target, target, ProjectileHitFlags.IntendedTarget, this.parent.verb.preventFriendlyFire, null, null);
			}
		}

		// Token: 0x06007F42 RID: 32578 RVA: 0x002D76D1 File Offset: 0x002D58D1
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return target.Pawn != null;
		}
	}
}
