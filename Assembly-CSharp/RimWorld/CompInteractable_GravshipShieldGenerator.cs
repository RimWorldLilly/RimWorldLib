using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AB RID: 6315
	public class CompInteractable_GravshipShieldGenerator : CompInteractable
	{
		// Token: 0x17001B1D RID: 6941
		// (get) Token: 0x06009E25 RID: 40485 RVA: 0x0038C594 File Offset: 0x0038A794
		private CompProjectileInterceptor Shield
		{
			get
			{
				return this.shield ?? this.parent.TryGetComp<CompProjectileInterceptor>();
			}
		}

		// Token: 0x17001B1E RID: 6942
		// (get) Token: 0x06009E26 RID: 40486 RVA: 0x0038C5AB File Offset: 0x0038A7AB
		private CompGravshipFacility Facility
		{
			get
			{
				return this.facility ?? this.parent.TryGetComp<CompGravshipFacility>();
			}
		}

		// Token: 0x17001B1F RID: 6943
		// (get) Token: 0x06009E27 RID: 40487 RVA: 0x0038C5C2 File Offset: 0x0038A7C2
		protected override string ActivateOptionLabel
		{
			get
			{
				return this.Shield.Active ? "Deactivate".Translate() : "Activate".Translate();
			}
		}

		// Token: 0x06009E28 RID: 40488 RVA: 0x0038C5EC File Offset: 0x0038A7EC
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (this.Facility.LinkedBuildings.NullOrEmpty<Thing>())
			{
				return "ShieldNotConnectedToGravship".Translate();
			}
			if (this.Shield.Charging)
			{
				return "ShieldOnCooldown".Translate();
			}
			if (!this.Shield.Active && this.Shield.currentHitPoints < 100)
			{
				return "ShieldBelowMinHitpoints".Translate();
			}
			return base.CanInteract(activateBy, checkOptionalItems);
		}

		// Token: 0x06009E29 RID: 40489 RVA: 0x0038C67A File Offset: 0x0038A87A
		protected override void OnInteracted(Pawn caster)
		{
			if (!this.Shield.Active)
			{
				this.Shield.Activate();
				return;
			}
			this.Shield.Deactivate();
		}

		// Token: 0x06009E2A RID: 40490 RVA: 0x0038C6A0 File Offset: 0x0038A8A0
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompInteractable_GravshipShieldGenerator.<CompGetGizmosExtra>d__11 <CompGetGizmosExtra>d__ = new CompInteractable_GravshipShieldGenerator.<CompGetGizmosExtra>d__11(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04005D8C RID: 23948
		private const int MinHitPointsToActivate = 100;

		// Token: 0x04005D8D RID: 23949
		private CompProjectileInterceptor shield;

		// Token: 0x04005D8E RID: 23950
		private CompGravshipFacility facility;
	}
}
