using System;
using System.Runtime.CompilerServices;
using RimWorld.Utility;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200180F RID: 6159
	public class CompApparelReloadable : CompApparelVerbOwner_Charged, IReloadableComp, ICompWithCharges
	{
		// Token: 0x17001A27 RID: 6695
		// (get) Token: 0x060099D3 RID: 39379 RVA: 0x00379B5C File Offset: 0x00377D5C
		public new CompProperties_ApparelReloadable Props
		{
			get
			{
				return this.props as CompProperties_ApparelReloadable;
			}
		}

		// Token: 0x17001A28 RID: 6696
		// (get) Token: 0x060099D4 RID: 39380 RVA: 0x00379B69 File Offset: 0x00377D69
		public Thing ReloadableThing
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17001A29 RID: 6697
		// (get) Token: 0x060099D5 RID: 39381 RVA: 0x00379B71 File Offset: 0x00377D71
		public ThingDef AmmoDef
		{
			get
			{
				return this.Props.ammoDef;
			}
		}

		// Token: 0x17001A2A RID: 6698
		// (get) Token: 0x060099D6 RID: 39382 RVA: 0x00379B7E File Offset: 0x00377D7E
		public int BaseReloadTicks
		{
			get
			{
				return this.Props.baseReloadTicks;
			}
		}

		// Token: 0x060099D7 RID: 39383 RVA: 0x00379B8B File Offset: 0x00377D8B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.replenishInTicks, "replenishInTicks", -1, false);
		}

		// Token: 0x060099D8 RID: 39384 RVA: 0x00379BA8 File Offset: 0x00377DA8
		public override void CompTick()
		{
			base.CompTick();
			if (this.Props.replenishAfterCooldown && base.RemainingCharges == 0)
			{
				if (this.replenishInTicks > 0)
				{
					this.replenishInTicks--;
					return;
				}
				this.remainingCharges = base.MaxCharges;
			}
		}

		// Token: 0x060099D9 RID: 39385 RVA: 0x00379BF4 File Offset: 0x00377DF4
		public string DisabledReason(int minNeeded, int maxNeeded)
		{
			if (this.Props.replenishAfterCooldown)
			{
				return "CommandReload_Cooldown".Translate(this.Props.CooldownVerbArgument, this.replenishInTicks.ToStringTicksToPeriod(true, false, true, true, false).Named("TIME"));
			}
			if (this.AmmoDef == null)
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
			return "CommandReload_NoAmmo".Translate(this.Props.ChargeNounArgument, this.AmmoDef.Named("AMMO"), arg.Named("COUNT"));
		}

		// Token: 0x060099DA RID: 39386 RVA: 0x00379CF8 File Offset: 0x00377EF8
		public bool NeedsReload(bool allowForcedReload)
		{
			if (this.AmmoDef == null)
			{
				return false;
			}
			if (this.Props.ammoCountToRefill == 0)
			{
				return base.RemainingCharges != base.MaxCharges;
			}
			if (!allowForcedReload)
			{
				return this.remainingCharges == 0;
			}
			return base.RemainingCharges != base.MaxCharges;
		}

		// Token: 0x060099DB RID: 39387 RVA: 0x00379D4C File Offset: 0x00377F4C
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
				this.remainingCharges = base.MaxCharges;
			}
			else
			{
				if (ammo.stackCount < this.Props.ammoCountPerCharge)
				{
					return;
				}
				int num = Mathf.Clamp(ammo.stackCount / this.Props.ammoCountPerCharge, 0, base.MaxCharges - base.RemainingCharges);
				ammo.SplitOff(num * this.Props.ammoCountPerCharge).Destroy(DestroyMode.Vanish);
				this.remainingCharges += num;
			}
			if (this.Props.soundReload != null)
			{
				this.Props.soundReload.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
			}
		}

		// Token: 0x060099DC RID: 39388 RVA: 0x00379E48 File Offset: 0x00378048
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

		// Token: 0x060099DD RID: 39389 RVA: 0x00379E79 File Offset: 0x00378079
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
			return this.Props.ammoCountPerCharge * (base.MaxCharges - base.RemainingCharges);
		}

		// Token: 0x060099DE RID: 39390 RVA: 0x00379EB8 File Offset: 0x003780B8
		public int MaxAmmoAmount()
		{
			if (this.AmmoDef == null)
			{
				return 0;
			}
			if (this.Props.ammoCountToRefill == 0)
			{
				return this.Props.ammoCountPerCharge * base.MaxCharges;
			}
			return this.Props.ammoCountToRefill;
		}

		// Token: 0x060099DF RID: 39391 RVA: 0x00379EEF File Offset: 0x003780EF
		public override void UsedOnce()
		{
			base.UsedOnce();
			if (this.Props.replenishAfterCooldown && this.remainingCharges == 0)
			{
				this.replenishInTicks = this.Props.baseReloadTicks;
			}
		}

		// Token: 0x060099E0 RID: 39392 RVA: 0x00379F1D File Offset: 0x0037811D
		public override bool CanBeUsed(out string reason)
		{
			reason = "";
			if (this.remainingCharges <= 0)
			{
				reason = this.DisabledReason(this.MinAmmoNeeded(false), this.MaxAmmoNeeded(false));
				return false;
			}
			return base.CanBeUsed(out reason);
		}

		// Token: 0x04005BB6 RID: 23478
		private int replenishInTicks = -1;
	}
}
