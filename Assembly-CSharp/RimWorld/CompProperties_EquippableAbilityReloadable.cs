using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001876 RID: 6262
	public class CompProperties_EquippableAbilityReloadable : CompProperties_EquippableAbility
	{
		// Token: 0x17001AC8 RID: 6856
		// (get) Token: 0x06009CA3 RID: 40099 RVA: 0x00386143 File Offset: 0x00384343
		public NamedArgument CooldownVerbArgument
		{
			get
			{
				return this.cooldownGerund.CapitalizeFirst().Named("COOLDOWNGERUND");
			}
		}

		// Token: 0x17001AC9 RID: 6857
		// (get) Token: 0x06009CA4 RID: 40100 RVA: 0x0038615A File Offset: 0x0038435A
		public NamedArgument ChargeNounArgument
		{
			get
			{
				return this.chargeNoun.Named("CHARGENOUN");
			}
		}

		// Token: 0x06009CA5 RID: 40101 RVA: 0x0038616C File Offset: 0x0038436C
		public CompProperties_EquippableAbilityReloadable()
		{
			this.compClass = typeof(CompEquippableAbilityReloadable);
		}

		// Token: 0x06009CA6 RID: 40102 RVA: 0x003861A2 File Offset: 0x003843A2
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_EquippableAbilityReloadable.<ConfigErrors>d__14 <ConfigErrors>d__ = new CompProperties_EquippableAbilityReloadable.<ConfigErrors>d__14(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x06009CA7 RID: 40103 RVA: 0x003861B9 File Offset: 0x003843B9
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			CompProperties_EquippableAbilityReloadable.<SpecialDisplayStats>d__15 <SpecialDisplayStats>d__ = new CompProperties_EquippableAbilityReloadable.<SpecialDisplayStats>d__15(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x04005CF9 RID: 23801
		public int maxCharges;

		// Token: 0x04005CFA RID: 23802
		public ThingDef ammoDef;

		// Token: 0x04005CFB RID: 23803
		public int ammoCountToRefill;

		// Token: 0x04005CFC RID: 23804
		public int ammoCountPerCharge;

		// Token: 0x04005CFD RID: 23805
		public int baseReloadTicks = 60;

		// Token: 0x04005CFE RID: 23806
		public bool replenishAfterCooldown;

		// Token: 0x04005CFF RID: 23807
		public SoundDef soundReload;

		// Token: 0x04005D00 RID: 23808
		[MustTranslate]
		public string chargeNoun = "charge";

		// Token: 0x04005D01 RID: 23809
		[MustTranslate]
		public string cooldownGerund = "on cooldown";
	}
}
