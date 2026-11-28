using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200180E RID: 6158
	public class CompProperties_ApparelReloadable : CompProperties_ApparelVerbOwnerCharged
	{
		// Token: 0x17001A26 RID: 6694
		// (get) Token: 0x060099CD RID: 39373 RVA: 0x00379ADA File Offset: 0x00377CDA
		public NamedArgument CooldownVerbArgument
		{
			get
			{
				return this.cooldownGerund.CapitalizeFirst().Named("COOLDOWNGERUND");
			}
		}

		// Token: 0x060099CE RID: 39374 RVA: 0x00379AF1 File Offset: 0x00377CF1
		public CompProperties_ApparelReloadable()
		{
			this.compClass = typeof(CompApparelReloadable);
		}

		// Token: 0x060099CF RID: 39375 RVA: 0x00379B1C File Offset: 0x00377D1C
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_ApparelReloadable.<ConfigErrors>d__10 <ConfigErrors>d__ = new CompProperties_ApparelReloadable.<ConfigErrors>d__10(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x060099D0 RID: 39376 RVA: 0x00379B33 File Offset: 0x00377D33
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			CompProperties_ApparelReloadable.<SpecialDisplayStats>d__11 <SpecialDisplayStats>d__ = new CompProperties_ApparelReloadable.<SpecialDisplayStats>d__11(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x04005BAF RID: 23471
		public ThingDef ammoDef;

		// Token: 0x04005BB0 RID: 23472
		public int ammoCountToRefill;

		// Token: 0x04005BB1 RID: 23473
		public int ammoCountPerCharge;

		// Token: 0x04005BB2 RID: 23474
		public int baseReloadTicks = 60;

		// Token: 0x04005BB3 RID: 23475
		public bool replenishAfterCooldown;

		// Token: 0x04005BB4 RID: 23476
		public SoundDef soundReload;

		// Token: 0x04005BB5 RID: 23477
		[MustTranslate]
		public string cooldownGerund = "on cooldown";
	}
}
