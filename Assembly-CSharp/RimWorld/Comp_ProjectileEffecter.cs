using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE2 RID: 3810
	public class Comp_ProjectileEffecter : ThingComp
	{
		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x060063B3 RID: 25523 RVA: 0x0023C2BE File Offset: 0x0023A4BE
		public CompProperties_ProjectileEffecter Props
		{
			get
			{
				return (CompProperties_ProjectileEffecter)this.props;
			}
		}

		// Token: 0x060063B4 RID: 25524 RVA: 0x0023C2CC File Offset: 0x0023A4CC
		public override void CompTick()
		{
			base.CompTick();
			if (!this.parent.Spawned)
			{
				Effecter effecter = this.effecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effecter = null;
				return;
			}
			Projectile projectile = this.parent as Projectile;
			if (this.effecter == null)
			{
				this.effecter = this.Props.effecterDef.Spawn(projectile.Position, projectile.intendedTarget.Cell, this.parent.MapHeld, 1f);
			}
			Effecter effecter2 = this.effecter;
			if (effecter2 == null)
			{
				return;
			}
			effecter2.EffectTick(this.parent, this.parent);
		}

		// Token: 0x04003813 RID: 14355
		private Effecter effecter;
	}
}
