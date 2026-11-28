using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF0 RID: 8176
	public class Verb_ShootOneUse : Verb_Shoot
	{
		// Token: 0x0600CF44 RID: 53060 RVA: 0x004C578F File Offset: 0x004C398F
		protected override bool TryCastShot()
		{
			if (base.TryCastShot())
			{
				if (this.burstShotsLeft <= 1)
				{
					this.SelfConsume();
				}
				return true;
			}
			if (this.burstShotsLeft < base.BurstShotCount)
			{
				this.SelfConsume();
			}
			return false;
		}

		// Token: 0x0600CF45 RID: 53061 RVA: 0x004C57BF File Offset: 0x004C39BF
		public override void Notify_EquipmentLost()
		{
			base.Notify_EquipmentLost();
			if (this.state == VerbState.Bursting && this.burstShotsLeft < base.BurstShotCount)
			{
				this.SelfConsume();
			}
		}

		// Token: 0x0600CF46 RID: 53062 RVA: 0x004C57E4 File Offset: 0x004C39E4
		private void SelfConsume()
		{
			if (base.EquipmentSource != null && !base.EquipmentSource.Destroyed)
			{
				base.EquipmentSource.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
