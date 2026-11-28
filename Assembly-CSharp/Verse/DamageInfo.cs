using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006A0 RID: 1696
	public struct DamageInfo
	{
		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06003563 RID: 13667 RVA: 0x0015B06C File Offset: 0x0015926C
		// (set) Token: 0x06003564 RID: 13668 RVA: 0x0015B074 File Offset: 0x00159274
		public DamageDef Def
		{
			get
			{
				return this.defInt;
			}
			set
			{
				this.defInt = value;
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06003565 RID: 13669 RVA: 0x0015B07D File Offset: 0x0015927D
		public float Amount
		{
			get
			{
				if (!DebugSettings.enableDamage)
				{
					return 0f;
				}
				return this.amountInt;
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06003566 RID: 13670 RVA: 0x0015B092 File Offset: 0x00159292
		public float ArmorPenetrationInt
		{
			get
			{
				return this.armorPenetrationInt;
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06003567 RID: 13671 RVA: 0x0015B09A File Offset: 0x0015929A
		public Thing Instigator
		{
			get
			{
				return this.instigatorInt;
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06003568 RID: 13672 RVA: 0x0015B0A2 File Offset: 0x001592A2
		public DamageInfo.SourceCategory Category
		{
			get
			{
				return this.categoryInt;
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06003569 RID: 13673 RVA: 0x0015B0AA File Offset: 0x001592AA
		public Thing IntendedTarget
		{
			get
			{
				return this.intendedTargetInt;
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x0600356A RID: 13674 RVA: 0x0015B0B2 File Offset: 0x001592B2
		public float Angle
		{
			get
			{
				return this.angleInt;
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x0600356B RID: 13675 RVA: 0x0015B0BA File Offset: 0x001592BA
		public BodyPartRecord HitPart
		{
			get
			{
				return this.hitPartInt;
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x0600356C RID: 13676 RVA: 0x0015B0C2 File Offset: 0x001592C2
		public BodyPartHeight Height
		{
			get
			{
				return this.heightInt;
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x0600356D RID: 13677 RVA: 0x0015B0CA File Offset: 0x001592CA
		public BodyPartDepth Depth
		{
			get
			{
				return this.depthInt;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x0600356E RID: 13678 RVA: 0x0015B0D2 File Offset: 0x001592D2
		public ThingDef Weapon
		{
			get
			{
				return this.weaponInt;
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x0600356F RID: 13679 RVA: 0x0015B0DA File Offset: 0x001592DA
		public BodyPartGroupDef WeaponBodyPartGroup
		{
			get
			{
				return this.weaponBodyPartGroupInt;
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06003570 RID: 13680 RVA: 0x0015B0E2 File Offset: 0x001592E2
		public HediffDef WeaponLinkedHediff
		{
			get
			{
				return this.weaponHediffInt;
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06003571 RID: 13681 RVA: 0x0015B0EA File Offset: 0x001592EA
		public Tool Tool
		{
			get
			{
				return this.tool;
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06003572 RID: 13682 RVA: 0x0015B0F2 File Offset: 0x001592F2
		public bool InstantPermanentInjury
		{
			get
			{
				return this.instantPermanentInjuryInt;
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06003573 RID: 13683 RVA: 0x0015B0FA File Offset: 0x001592FA
		public bool InstigatorGuilty
		{
			get
			{
				return this.instigatorGuilty;
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x06003574 RID: 13684 RVA: 0x0015B102 File Offset: 0x00159302
		public bool SpawnFilth
		{
			get
			{
				return this.spawnFilth;
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06003575 RID: 13685 RVA: 0x0015B10A File Offset: 0x0015930A
		public bool CheckForJobOverride
		{
			get
			{
				return this.checkForJobOverride;
			}
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06003576 RID: 13686 RVA: 0x0015B112 File Offset: 0x00159312
		public QualityCategory WeaponQuality
		{
			get
			{
				return this.weaponQuality;
			}
		}

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x06003577 RID: 13687 RVA: 0x0015B11A File Offset: 0x0015931A
		public IntRange DamagePropagationPartsRange
		{
			get
			{
				return this.damagePropagationPartsRange;
			}
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x06003578 RID: 13688 RVA: 0x0015B122 File Offset: 0x00159322
		public bool AllowDamagePropagation
		{
			get
			{
				return !this.InstantPermanentInjury && this.allowDamagePropagationInt;
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06003579 RID: 13689 RVA: 0x0015B134 File Offset: 0x00159334
		public bool PreventCascade
		{
			get
			{
				return this.preventCascadeInt;
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x0600357A RID: 13690 RVA: 0x0015B13C File Offset: 0x0015933C
		public bool ApplyAllDamage
		{
			get
			{
				return this.applyAllDamage;
			}
		}

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x0600357B RID: 13691 RVA: 0x0015B144 File Offset: 0x00159344
		public bool IgnoreArmor
		{
			get
			{
				return this.ignoreArmorInt;
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x0600357C RID: 13692 RVA: 0x0015B14C File Offset: 0x0015934C
		public bool IgnoreInstantKillProtection
		{
			get
			{
				return this.ignoreInstantKillProtectionInt;
			}
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x0015B154 File Offset: 0x00159354
		public DamageInfo(DamageDef def, float amount, float armorPenetration = 0f, float angle = -1f, Thing instigator = null, BodyPartRecord hitPart = null, ThingDef weapon = null, DamageInfo.SourceCategory category = DamageInfo.SourceCategory.ThingOrUnknown, Thing intendedTarget = null, bool instigatorGuilty = true, bool spawnFilth = true, QualityCategory weaponQuality = QualityCategory.Normal, bool checkForJobOverride = true, bool preventCascade = false)
		{
			this.defInt = def;
			this.amountInt = amount;
			this.armorPenetrationInt = armorPenetration;
			if (angle < 0f)
			{
				this.angleInt = (float)Rand.RangeInclusive(0, 359);
			}
			else
			{
				this.angleInt = angle;
			}
			this.instigatorInt = instigator;
			this.categoryInt = category;
			this.hitPartInt = hitPart;
			this.heightInt = BodyPartHeight.Undefined;
			this.depthInt = BodyPartDepth.Undefined;
			this.weaponInt = weapon;
			this.weaponBodyPartGroupInt = null;
			this.weaponHediffInt = null;
			this.tool = null;
			this.weaponQuality = weaponQuality;
			this.instantPermanentInjuryInt = false;
			this.allowDamagePropagationInt = true;
			this.preventCascadeInt = preventCascade;
			this.ignoreArmorInt = false;
			this.ignoreInstantKillProtectionInt = false;
			this.instigatorGuilty = instigatorGuilty;
			this.intendedTargetInt = intendedTarget;
			this.spawnFilth = spawnFilth;
			this.checkForJobOverride = checkForJobOverride;
			this.damagePropagationPartsRange = new IntRange(2, 4);
			this.applyAllDamage = false;
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x0015B240 File Offset: 0x00159440
		public DamageInfo(DamageInfo cloneSource)
		{
			this.defInt = cloneSource.defInt;
			this.amountInt = cloneSource.amountInt;
			this.armorPenetrationInt = cloneSource.armorPenetrationInt;
			this.angleInt = cloneSource.angleInt;
			this.instigatorInt = cloneSource.instigatorInt;
			this.categoryInt = cloneSource.categoryInt;
			this.hitPartInt = cloneSource.hitPartInt;
			this.heightInt = cloneSource.heightInt;
			this.depthInt = cloneSource.depthInt;
			this.weaponInt = cloneSource.weaponInt;
			this.weaponBodyPartGroupInt = cloneSource.weaponBodyPartGroupInt;
			this.weaponHediffInt = cloneSource.weaponHediffInt;
			this.tool = cloneSource.tool;
			this.weaponQuality = cloneSource.weaponQuality;
			this.instantPermanentInjuryInt = cloneSource.instantPermanentInjuryInt;
			this.allowDamagePropagationInt = cloneSource.allowDamagePropagationInt;
			this.preventCascadeInt = cloneSource.preventCascadeInt;
			this.intendedTargetInt = cloneSource.intendedTargetInt;
			this.ignoreArmorInt = cloneSource.ignoreArmorInt;
			this.ignoreInstantKillProtectionInt = cloneSource.ignoreInstantKillProtectionInt;
			this.instigatorGuilty = cloneSource.instigatorGuilty;
			this.spawnFilth = cloneSource.spawnFilth;
			this.checkForJobOverride = cloneSource.checkForJobOverride;
			this.damagePropagationPartsRange = cloneSource.damagePropagationPartsRange;
			this.applyAllDamage = cloneSource.applyAllDamage;
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x0015B379 File Offset: 0x00159579
		public void SetAmount(float newAmount)
		{
			this.amountInt = newAmount;
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x0015B382 File Offset: 0x00159582
		public void SetIgnoreArmor(bool ignoreArmor)
		{
			this.ignoreArmorInt = ignoreArmor;
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x0015B38B File Offset: 0x0015958B
		public void SetIgnoreInstantKillProtection(bool ignore)
		{
			this.ignoreInstantKillProtectionInt = ignore;
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x0015B394 File Offset: 0x00159594
		public void SetBodyRegion(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			this.heightInt = height;
			this.depthInt = depth;
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x0015B3A4 File Offset: 0x001595A4
		public void SetHitPart(BodyPartRecord forceHitPart)
		{
			this.hitPartInt = forceHitPart;
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x0015B3AD File Offset: 0x001595AD
		public void SetInstantPermanentInjury(bool val)
		{
			this.instantPermanentInjuryInt = val;
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x0015B3B6 File Offset: 0x001595B6
		public void SetWeaponBodyPartGroup(BodyPartGroupDef gr)
		{
			this.weaponBodyPartGroupInt = gr;
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x0015B3BF File Offset: 0x001595BF
		public void SetWeaponHediff(HediffDef hd)
		{
			this.weaponHediffInt = hd;
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x0015B3C8 File Offset: 0x001595C8
		public void SetTool(Tool tool)
		{
			this.tool = tool;
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x0015B3D1 File Offset: 0x001595D1
		public void SetWeaponQuality(QualityCategory weaponQuality)
		{
			this.weaponQuality = weaponQuality;
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x0015B3DA File Offset: 0x001595DA
		public void SetAllowDamagePropagation(bool val)
		{
			this.allowDamagePropagationInt = val;
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x0015B3E3 File Offset: 0x001595E3
		public void SetAllowDamagePropagation(bool val, IntRange partsRange)
		{
			this.allowDamagePropagationInt = val;
			this.damagePropagationPartsRange = partsRange;
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x0015B3F3 File Offset: 0x001595F3
		public void SetApplyAllDamage(bool value)
		{
			this.allowDamagePropagationInt = value;
			this.applyAllDamage = value;
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x0015B404 File Offset: 0x00159604
		public void SetAngle(Vector3 vec)
		{
			if (vec.x != 0f || vec.z != 0f)
			{
				this.angleInt = Quaternion.LookRotation(vec).eulerAngles.y;
				return;
			}
			this.angleInt = (float)Rand.RangeInclusive(0, 359);
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x0015B458 File Offset: 0x00159658
		public override string ToString()
		{
			string[] array = new string[9];
			array[0] = "(def=";
			int num = 1;
			DamageDef damageDef = this.defInt;
			array[num] = ((damageDef != null) ? damageDef.ToString() : null);
			array[2] = ", amount= ";
			array[3] = this.amountInt.ToString();
			array[4] = ", instigator=";
			array[5] = ((this.instigatorInt != null) ? this.instigatorInt.ToString() : this.categoryInt.ToString());
			array[6] = ", angle=";
			array[7] = this.angleInt.ToString("F1");
			array[8] = ")";
			return string.Concat(array);
		}

		// Token: 0x040022E8 RID: 8936
		private DamageDef defInt;

		// Token: 0x040022E9 RID: 8937
		private float amountInt;

		// Token: 0x040022EA RID: 8938
		private float armorPenetrationInt;

		// Token: 0x040022EB RID: 8939
		private float angleInt;

		// Token: 0x040022EC RID: 8940
		private Thing instigatorInt;

		// Token: 0x040022ED RID: 8941
		private DamageInfo.SourceCategory categoryInt;

		// Token: 0x040022EE RID: 8942
		public Thing intendedTargetInt;

		// Token: 0x040022EF RID: 8943
		private bool ignoreArmorInt;

		// Token: 0x040022F0 RID: 8944
		private bool ignoreInstantKillProtectionInt;

		// Token: 0x040022F1 RID: 8945
		private BodyPartRecord hitPartInt;

		// Token: 0x040022F2 RID: 8946
		private BodyPartHeight heightInt;

		// Token: 0x040022F3 RID: 8947
		private BodyPartDepth depthInt;

		// Token: 0x040022F4 RID: 8948
		private ThingDef weaponInt;

		// Token: 0x040022F5 RID: 8949
		private BodyPartGroupDef weaponBodyPartGroupInt;

		// Token: 0x040022F6 RID: 8950
		private HediffDef weaponHediffInt;

		// Token: 0x040022F7 RID: 8951
		private Tool tool;

		// Token: 0x040022F8 RID: 8952
		private QualityCategory weaponQuality;

		// Token: 0x040022F9 RID: 8953
		private bool instantPermanentInjuryInt;

		// Token: 0x040022FA RID: 8954
		private bool allowDamagePropagationInt;

		// Token: 0x040022FB RID: 8955
		private bool preventCascadeInt;

		// Token: 0x040022FC RID: 8956
		private bool instigatorGuilty;

		// Token: 0x040022FD RID: 8957
		private bool spawnFilth;

		// Token: 0x040022FE RID: 8958
		private bool checkForJobOverride;

		// Token: 0x040022FF RID: 8959
		private bool applyAllDamage;

		// Token: 0x04002300 RID: 8960
		private IntRange damagePropagationPartsRange;

		// Token: 0x02002A18 RID: 10776
		public enum SourceCategory
		{
			// Token: 0x0400AC7A RID: 44154
			ThingOrUnknown,
			// Token: 0x0400AC7B RID: 44155
			Collapse
		}
	}
}
