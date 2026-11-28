using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020005A9 RID: 1449
	public class Tool
	{
		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06002BE8 RID: 11240 RVA: 0x0010A2DA File Offset: 0x001084DA
		public string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null)
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06002BE9 RID: 11241 RVA: 0x0010A2FB File Offset: 0x001084FB
		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where this.capacities.Contains(x.requiredCapacity)
				select x;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06002BEA RID: 11242 RVA: 0x0010A313 File Offset: 0x00108513
		public IEnumerable<VerbProperties> VerbsProperties
		{
			get
			{
				return from x in this.Maneuvers
				select x.verb;
			}
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x0010A340 File Offset: 0x00108540
		public float AdjustedBaseMeleeDamageAmount(Thing ownerEquipment, DamageDef damageDef)
		{
			float num = this.power;
			if (ownerEquipment != null)
			{
				num *= ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, true, -1);
				if (ownerEquipment.Stuff != null && damageDef != null)
				{
					num *= ownerEquipment.Stuff.GetStatValueAbstract(damageDef.armorCategory.multStat, null);
				}
			}
			return num;
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x0010A390 File Offset: 0x00108590
		public float AdjustedBaseMeleeDamageAmount(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff, DamageDef damageDef)
		{
			float num = this.power;
			if (ownerEquipmentStuff != null)
			{
				num *= ownerEquipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_DamageMultiplier, ownerEquipmentStuff);
				if (ownerEquipmentStuff != null && damageDef != null)
				{
					num *= ownerEquipmentStuff.GetStatValueAbstract(damageDef.armorCategory.multStat, null);
				}
			}
			return num;
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x0010A3D2 File Offset: 0x001085D2
		public float AdjustedCooldown(Thing ownerEquipment)
		{
			return this.cooldownTime * ((ownerEquipment == null) ? 1f : ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true, -1));
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x0010A3F2 File Offset: 0x001085F2
		public float AdjustedCooldown(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff)
		{
			return this.cooldownTime * ((ownerEquipment == null) ? 1f : ownerEquipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_CooldownMultiplier, ownerEquipmentStuff));
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x0010A411 File Offset: 0x00108611
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x0010A419 File Offset: 0x00108619
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x0010A427 File Offset: 0x00108627
		public IEnumerable<string> ConfigErrors()
		{
			Tool.<ConfigErrors>d__31 <ConfigErrors>d__ = new Tool.<ConfigErrors>d__31(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04001D9A RID: 7578
		[Unsaved(false)]
		public string id;

		// Token: 0x04001D9B RID: 7579
		[MustTranslate]
		public string label;

		// Token: 0x04001D9C RID: 7580
		[MustTranslate]
		public string labelNoLocation;

		// Token: 0x04001D9D RID: 7581
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x04001D9E RID: 7582
		public bool labelUsedInLogging = true;

		// Token: 0x04001D9F RID: 7583
		public List<ToolCapacityDef> capacities = new List<ToolCapacityDef>();

		// Token: 0x04001DA0 RID: 7584
		public float power;

		// Token: 0x04001DA1 RID: 7585
		public float armorPenetration = -1f;

		// Token: 0x04001DA2 RID: 7586
		public float cooldownTime;

		// Token: 0x04001DA3 RID: 7587
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x04001DA4 RID: 7588
		public HediffDef hediff;

		// Token: 0x04001DA5 RID: 7589
		public float chanceFactor = 1f;

		// Token: 0x04001DA6 RID: 7590
		public bool alwaysTreatAsWeapon;

		// Token: 0x04001DA7 RID: 7591
		public List<ExtraDamage> extraMeleeDamages;

		// Token: 0x04001DA8 RID: 7592
		public SoundDef soundMeleeHit;

		// Token: 0x04001DA9 RID: 7593
		public SoundDef soundMeleeMiss;

		// Token: 0x04001DAA RID: 7594
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x04001DAB RID: 7595
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x04001DAC RID: 7596
		[Unsaved(false)]
		private string cachedLabelCap;
	}
}
