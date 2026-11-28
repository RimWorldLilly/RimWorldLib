using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200208B RID: 8331
	public class HediffComp_ExplodeOnDeath : HediffComp
	{
		// Token: 0x170025B8 RID: 9656
		// (get) Token: 0x0600D213 RID: 53779 RVA: 0x004D2D4D File Offset: 0x004D0F4D
		public HediffCompProperties_ExplodeOnDeath Props
		{
			get
			{
				return (HediffCompProperties_ExplodeOnDeath)this.props;
			}
		}

		// Token: 0x0600D214 RID: 53780 RVA: 0x004D2D5C File Offset: 0x004D0F5C
		public override void Notify_PawnKilled()
		{
			GenExplosion.DoExplosion(base.Pawn.Position, base.Pawn.Map, this.Props.explosionRadius, this.Props.damageDef, base.Pawn, this.Props.damageAmount, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			if (this.Props.destroyGear)
			{
				base.Pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				base.Pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600D215 RID: 53781 RVA: 0x004D2E38 File Offset: 0x004D1038
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			if (this.Props.destroyBody)
			{
				base.Pawn.Corpse.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
