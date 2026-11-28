using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Utility;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001877 RID: 6263
	public class CompEquippableAbilityReloadable : CompEquippableAbility, IReloadableComp, ICompWithCharges
	{
		// Token: 0x17001ACA RID: 6858
		// (get) Token: 0x06009CAA RID: 40106 RVA: 0x003861E2 File Offset: 0x003843E2
		public CompProperties_EquippableAbilityReloadable Props
		{
			get
			{
				return this.props as CompProperties_EquippableAbilityReloadable;
			}
		}

		// Token: 0x17001ACB RID: 6859
		// (get) Token: 0x06009CAB RID: 40107 RVA: 0x003861EF File Offset: 0x003843EF
		public Thing ReloadableThing
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17001ACC RID: 6860
		// (get) Token: 0x06009CAC RID: 40108 RVA: 0x003861F7 File Offset: 0x003843F7
		public ThingDef AmmoDef
		{
			get
			{
				return this.Props.ammoDef;
			}
		}

		// Token: 0x17001ACD RID: 6861
		// (get) Token: 0x06009CAD RID: 40109 RVA: 0x00386204 File Offset: 0x00384404
		public int MaxCharges
		{
			get
			{
				return this.Props.maxCharges;
			}
		}

		// Token: 0x17001ACE RID: 6862
		// (get) Token: 0x06009CAE RID: 40110 RVA: 0x00386211 File Offset: 0x00384411
		public int BaseReloadTicks
		{
			get
			{
				return this.Props.baseReloadTicks;
			}
		}

		// Token: 0x17001ACF RID: 6863
		// (get) Token: 0x06009CAF RID: 40111 RVA: 0x00386220 File Offset: 0x00384420
		public string LabelRemaining
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.RemainingCharges);
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.MaxCharges);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17001AD0 RID: 6864
		// (get) Token: 0x06009CB0 RID: 40112 RVA: 0x00386263 File Offset: 0x00384463
		public bool AbilityUsesCharges
		{
			get
			{
				Ability abilityForReading = base.AbilityForReading;
				return abilityForReading != null && abilityForReading.UsesCharges;
			}
		}

		// Token: 0x17001AD1 RID: 6865
		// (get) Token: 0x06009CB1 RID: 40113 RVA: 0x00386276 File Offset: 0x00384476
		// (set) Token: 0x06009CB2 RID: 40114 RVA: 0x00386289 File Offset: 0x00384489
		public int RemainingCharges
		{
			get
			{
				Ability abilityForReading = base.AbilityForReading;
				if (abilityForReading == null)
				{
					return 0;
				}
				return abilityForReading.RemainingCharges;
			}
			set
			{
				if (base.AbilityForReading != null)
				{
					base.AbilityForReading.RemainingCharges = value;
				}
			}
		}

		// Token: 0x06009CB3 RID: 40115 RVA: 0x0038629F File Offset: 0x0038449F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.replenishInTicks, "replenishInTicks", -1, false);
		}

		// Token: 0x06009CB4 RID: 40116 RVA: 0x003862B9 File Offset: 0x003844B9
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.Notify_PropsChanged();
		}

		// Token: 0x06009CB5 RID: 40117 RVA: 0x003862C7 File Offset: 0x003844C7
		public void Notify_PropsChanged()
		{
			if (base.AbilityForReading == null)
			{
				return;
			}
			base.AbilityForReading.maxCharges = this.MaxCharges;
			this.RemainingCharges = this.MaxCharges;
		}

		// Token: 0x06009CB6 RID: 40118 RVA: 0x003862F0 File Offset: 0x003844F0
		public override void CompTick()
		{
			base.CompTick();
			if (base.AbilityForReading == null)
			{
				return;
			}
			if (this.Props.replenishAfterCooldown && this.RemainingCharges == 0)
			{
				if (this.replenishInTicks > 0)
				{
					this.replenishInTicks--;
					return;
				}
				this.RemainingCharges = this.MaxCharges;
			}
		}

		// Token: 0x06009CB7 RID: 40119 RVA: 0x00386348 File Offset: 0x00384548
		public override string CompInspectStringExtra()
		{
			if (base.AbilityForReading == null)
			{
				return null;
			}
			if (!this.AbilityUsesCharges)
			{
				return null;
			}
			return "ChargesRemaining".Translate(this.Props.ChargeNounArgument) + ": " + this.LabelRemaining;
		}

		// Token: 0x06009CB8 RID: 40120 RVA: 0x00386398 File Offset: 0x00384598
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompEquippableAbilityReloadable.<SpecialDisplayStats>d__23 <SpecialDisplayStats>d__ = new CompEquippableAbilityReloadable.<SpecialDisplayStats>d__23(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06009CB9 RID: 40121 RVA: 0x003863A8 File Offset: 0x003845A8
		public override IEnumerable<Gizmo> CompGetEquippedGizmosExtra()
		{
			CompEquippableAbilityReloadable.<CompGetEquippedGizmosExtra>d__24 <CompGetEquippedGizmosExtra>d__ = new CompEquippableAbilityReloadable.<CompGetEquippedGizmosExtra>d__24(-2);
			<CompGetEquippedGizmosExtra>d__.<>4__this = this;
			return <CompGetEquippedGizmosExtra>d__;
		}

		// Token: 0x06009CBA RID: 40122 RVA: 0x003863B8 File Offset: 0x003845B8
		public bool NeedsReload(bool allowForcedReload)
		{
			if (base.AbilityForReading == null)
			{
				return false;
			}
			if (this.Props.ammoDef == null)
			{
				return false;
			}
			if (this.Props.ammoCountToRefill == 0)
			{
				return this.RemainingCharges != this.MaxCharges;
			}
			if (!allowForcedReload)
			{
				return this.RemainingCharges == 0;
			}
			return this.RemainingCharges != this.MaxCharges;
		}

		// Token: 0x06009CBB RID: 40123 RVA: 0x0038641C File Offset: 0x0038461C
		public void ReloadFrom(Thing ammo)
		{
			if (!this.NeedsReload(true))
			{
				return;
			}
			if (this.Props.ammoCountToRefill != 0)
			{
				if (ammo.stackCount < this.Props.ammoCountToRefill)
				{
					return;
				}
				ammo.SplitOff(this.Props.ammoCountToRefill).Destroy(DestroyMode.Vanish);
				this.RemainingCharges = this.MaxCharges;
			}
			else
			{
				if (ammo.stackCount < this.Props.ammoCountPerCharge)
				{
					return;
				}
				int num = Mathf.Clamp(ammo.stackCount / this.Props.ammoCountPerCharge, 0, this.MaxCharges - this.RemainingCharges);
				ammo.SplitOff(num * this.Props.ammoCountPerCharge).Destroy(DestroyMode.Vanish);
				this.RemainingCharges += num;
			}
			if (this.Props.soundReload != null)
			{
				this.Props.soundReload.PlayOneShot(new TargetInfo(this.parent.PositionHeld, this.parent.MapHeld, false));
			}
		}

		// Token: 0x06009CBC RID: 40124 RVA: 0x00386518 File Offset: 0x00384718
		public int MinAmmoNeeded(bool allowForcedReload)
		{
			if (!this.NeedsReload(allowForcedReload))
			{
				return 0;
			}
			if (this.Props.ammoCountToRefill != 0)
			{
				return this.Props.ammoCountToRefill;
			}
			return this.Props.ammoCountPerCharge;
		}

		// Token: 0x06009CBD RID: 40125 RVA: 0x00386549 File Offset: 0x00384749
		public int MaxAmmoNeeded(bool allowForcedReload)
		{
			if (!this.NeedsReload(allowForcedReload))
			{
				return 0;
			}
			if (this.Props.ammoCountToRefill != 0)
			{
				return this.Props.ammoCountToRefill;
			}
			return this.Props.ammoCountPerCharge * (this.MaxCharges - this.RemainingCharges);
		}

		// Token: 0x06009CBE RID: 40126 RVA: 0x00386588 File Offset: 0x00384788
		public int MaxAmmoAmount()
		{
			if (base.AbilityForReading == null)
			{
				return 0;
			}
			if (this.Props.ammoDef == null)
			{
				return 0;
			}
			if (this.Props.ammoCountToRefill == 0)
			{
				return this.Props.ammoCountPerCharge * this.MaxCharges;
			}
			return this.Props.ammoCountToRefill;
		}

		// Token: 0x06009CBF RID: 40127 RVA: 0x003865D9 File Offset: 0x003847D9
		public override void UsedOnce()
		{
			if (base.AbilityForReading == null)
			{
				return;
			}
			if (this.Props.replenishAfterCooldown && this.RemainingCharges == 0)
			{
				this.replenishInTicks = this.Props.baseReloadTicks;
			}
		}

		// Token: 0x06009CC0 RID: 40128 RVA: 0x0038660A File Offset: 0x0038480A
		public bool CanBeUsed(out string reason)
		{
			reason = null;
			if (base.AbilityForReading == null)
			{
				return false;
			}
			if (this.RemainingCharges <= 0)
			{
				reason = this.DisabledReason(this.MinAmmoNeeded(false), this.MaxAmmoNeeded(false));
				return false;
			}
			return true;
		}

		// Token: 0x06009CC1 RID: 40129 RVA: 0x0038663C File Offset: 0x0038483C
		public string DisabledReason(int minNeeded, int maxNeeded)
		{
			if (base.AbilityForReading == null)
			{
				return null;
			}
			if (this.Props.replenishAfterCooldown)
			{
				return "CommandReload_Cooldown".Translate(this.Props.CooldownVerbArgument, this.replenishInTicks.ToStringTicksToPeriod(true, false, true, true, false).Named("TIME"));
			}
			if (this.Props.ammoDef == null)
			{
				return "CommandReload_NoCharges".Translate(this.Props.ChargeNounArgument);
			}
			string arg;
			if (this.Props.ammoCountToRefill != 0)
			{
				arg = this.Props.ammoCountToRefill.ToString();
			}
			else
			{
				string text;
				if (minNeeded != maxNeeded)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted<int>(minNeeded);
					defaultInterpolatedStringHandler.AppendLiteral("-");
					defaultInterpolatedStringHandler.AppendFormatted<int>(maxNeeded);
					text = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					text = minNeeded.ToString();
				}
				arg = text;
			}
			return "CommandReload_NoAmmo".Translate(this.Props.ChargeNounArgument, this.Props.ammoDef.Named("AMMO"), arg.Named("COUNT"));
		}

		// Token: 0x04005D02 RID: 23810
		private int replenishInTicks = -1;
	}
}
