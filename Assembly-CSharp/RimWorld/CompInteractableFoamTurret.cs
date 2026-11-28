using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CF RID: 6351
	public class CompInteractableFoamTurret : CompInteractable
	{
		// Token: 0x17001B5E RID: 7006
		// (get) Token: 0x06009F44 RID: 40772 RVA: 0x00391E8C File Offset: 0x0039008C
		private Building_TurretGun ParentGun
		{
			get
			{
				return (Building_TurretGun)this.parent;
			}
		}

		// Token: 0x17001B5F RID: 7007
		// (get) Token: 0x06009F45 RID: 40773 RVA: 0x00391E99 File Offset: 0x00390099
		public override bool CanCooldown
		{
			get
			{
				return this.power == null || this.power.PowerOn;
			}
		}

		// Token: 0x06009F46 RID: 40774 RVA: 0x00391EB0 File Offset: 0x003900B0
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
			if (!result.Accepted)
			{
				return result;
			}
			if (!this.ParentGun.TryFindNewTarget().IsValid)
			{
				return "NoNearbyFire".Translate();
			}
			return true;
		}

		// Token: 0x06009F47 RID: 40775 RVA: 0x00391EFC File Offset: 0x003900FC
		protected override void SendDeactivateMessage()
		{
			Messages.Message("MessageActivationCanceled".Translate(this.parent) + ": " + "NoNearbyFireNoFuelUsed".Translate(), this.parent, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06009F48 RID: 40776 RVA: 0x00391F52 File Offset: 0x00390152
		protected override bool ShouldDeactivate()
		{
			return !this.CanInteract(null, true);
		}

		// Token: 0x06009F49 RID: 40777 RVA: 0x00391F64 File Offset: 0x00390164
		protected override bool TryInteractTick()
		{
			this.ParentGun.TryActivateBurst();
			if (this.ParentGun.CurrentTarget.IsValid)
			{
				this.ParentGun.Top.ForceFaceTarget(this.ParentGun.CurrentTarget);
				return true;
			}
			return false;
		}
	}
}
