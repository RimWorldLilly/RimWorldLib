using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200209F RID: 8351
	public struct ThingStuffPair : IEquatable<ThingStuffPair>
	{
		// Token: 0x170025CD RID: 9677
		// (get) Token: 0x0600D275 RID: 53877 RVA: 0x004D5150 File Offset: 0x004D3350
		public float Price
		{
			get
			{
				return this.cachedPrice;
			}
		}

		// Token: 0x170025CE RID: 9678
		// (get) Token: 0x0600D276 RID: 53878 RVA: 0x004D5158 File Offset: 0x004D3358
		public float InsulationCold
		{
			get
			{
				return this.cachedInsulationCold;
			}
		}

		// Token: 0x170025CF RID: 9679
		// (get) Token: 0x0600D277 RID: 53879 RVA: 0x004D5160 File Offset: 0x004D3360
		public float VacuumResistance
		{
			get
			{
				return this.cachedVacuumResistance;
			}
		}

		// Token: 0x170025D0 RID: 9680
		// (get) Token: 0x0600D278 RID: 53880 RVA: 0x004D5168 File Offset: 0x004D3368
		public float InsulationHeat
		{
			get
			{
				return this.cachedInsulationHeat;
			}
		}

		// Token: 0x170025D1 RID: 9681
		// (get) Token: 0x0600D279 RID: 53881 RVA: 0x004D5170 File Offset: 0x004D3370
		public float ToxicEnvironmentResistance
		{
			get
			{
				return this.cachedToxicEnvironmentResistance;
			}
		}

		// Token: 0x170025D2 RID: 9682
		// (get) Token: 0x0600D27A RID: 53882 RVA: 0x004D5178 File Offset: 0x004D3378
		public float Commonality
		{
			get
			{
				float num = this.commonalityMultiplier;
				num *= this.thing.generateCommonality;
				if (this.stuff != null)
				{
					num *= this.stuff.stuffProps.commonality;
				}
				if (PawnWeaponGenerator.IsDerpWeapon(this.thing, this.stuff) || PawnApparelGenerator.IsDerpApparel(this.thing, this.stuff))
				{
					num *= 0.01f;
				}
				return num;
			}
		}

		// Token: 0x0600D27B RID: 53883 RVA: 0x004D51E4 File Offset: 0x004D33E4
		public ThingStuffPair(ThingDef thing, ThingDef stuff, float commonalityMultiplier = 1f)
		{
			this.thing = thing;
			this.stuff = stuff;
			this.commonalityMultiplier = commonalityMultiplier;
			if (stuff != null && !thing.MadeFromStuff)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Created ThingStuffPairWithQuality with stuff ");
				defaultInterpolatedStringHandler.AppendFormatted<ThingDef>(stuff);
				defaultInterpolatedStringHandler.AppendLiteral(" but ");
				defaultInterpolatedStringHandler.AppendFormatted<ThingDef>(thing);
				defaultInterpolatedStringHandler.AppendLiteral(" is not made from stuff.");
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				stuff = null;
			}
			this.cachedPrice = thing.GetStatValueAbstract(StatDefOf.MarketValue, stuff);
			this.cachedInsulationCold = thing.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff);
			this.cachedInsulationHeat = thing.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff);
			this.cachedToxicEnvironmentResistance = (ModsConfig.BiotechActive ? thing.equippedStatOffsets.GetStatOffsetFromList(StatDefOf.ToxicEnvironmentResistance) : 0f);
			this.cachedVacuumResistance = (ModsConfig.OdysseyActive ? thing.equippedStatOffsets.GetStatOffsetFromList(StatDefOf.VacuumResistance) : 0f);
		}

		// Token: 0x0600D27C RID: 53884 RVA: 0x004D52DC File Offset: 0x004D34DC
		public static List<ThingStuffPair> AllWith(Predicate<ThingDef> thingValidator)
		{
			List<ThingStuffPair> list = new List<ThingStuffPair>();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingDef thingDef = allDefsListForReading[i];
				if (thingValidator(thingDef))
				{
					if (!thingDef.MadeFromStuff)
					{
						list.Add(new ThingStuffPair(thingDef, null, 1f));
					}
					else
					{
						ThingStuffPair.tmpUsableStuffs.Clear();
						ThingStuffPair.tmpUsableStuffs.AddRange(from st in DefDatabase<ThingDef>.AllDefs
						where st.IsStuff && st.stuffProps.CanMake(thingDef)
						select st);
						int count = ThingStuffPair.tmpUsableStuffs.Count;
						float num = ThingStuffPair.tmpUsableStuffs.Average((ThingDef st) => st.stuffProps.commonality);
						float num2;
						if (Mathf.Approximately(num, 0f))
						{
							num2 = 1f;
						}
						else
						{
							num2 = 1f / (float)count / num;
						}
						foreach (ThingDef thingDef2 in ThingStuffPair.tmpUsableStuffs)
						{
							list.Add(new ThingStuffPair(thingDef, thingDef2, num2));
						}
					}
				}
			}
			return (from p in list
			orderby p.Price descending
			select p).ToList<ThingStuffPair>();
		}

		// Token: 0x0600D27D RID: 53885 RVA: 0x004D5460 File Offset: 0x004D3660
		public override string ToString()
		{
			if (this.thing == null)
			{
				return "(null)";
			}
			string text;
			if (this.stuff == null)
			{
				text = this.thing.label;
			}
			else
			{
				text = this.thing.label + " " + this.stuff.LabelAsStuff;
			}
			return string.Concat(new string[]
			{
				text,
				" $",
				this.Price.ToString("F0"),
				" c=",
				this.Commonality.ToString("F4")
			});
		}

		// Token: 0x0600D27E RID: 53886 RVA: 0x004D54FD File Offset: 0x004D36FD
		public static bool operator ==(ThingStuffPair a, ThingStuffPair b)
		{
			return a.thing == b.thing && a.stuff == b.stuff && Mathf.Approximately(a.commonalityMultiplier, b.commonalityMultiplier);
		}

		// Token: 0x0600D27F RID: 53887 RVA: 0x004D552E File Offset: 0x004D372E
		public static bool operator !=(ThingStuffPair a, ThingStuffPair b)
		{
			return !(a == b);
		}

		// Token: 0x0600D280 RID: 53888 RVA: 0x004D553A File Offset: 0x004D373A
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPair && this.Equals((ThingStuffPair)obj);
		}

		// Token: 0x0600D281 RID: 53889 RVA: 0x004D5552 File Offset: 0x004D3752
		public bool Equals(ThingStuffPair other)
		{
			return this == other;
		}

		// Token: 0x0600D282 RID: 53890 RVA: 0x004D5560 File Offset: 0x004D3760
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(Gen.HashCombine<ThingDef>(Gen.HashCombine<ThingDef>(0, this.thing), this.stuff), this.commonalityMultiplier);
		}

		// Token: 0x0600D283 RID: 53891 RVA: 0x004D5584 File Offset: 0x004D3784
		public static explicit operator ThingStuffPair(ThingStuffPairWithQuality p)
		{
			return new ThingStuffPair(p.thing, p.stuff, 1f);
		}

		// Token: 0x04008552 RID: 34130
		public ThingDef thing;

		// Token: 0x04008553 RID: 34131
		public ThingDef stuff;

		// Token: 0x04008554 RID: 34132
		public float commonalityMultiplier;

		// Token: 0x04008555 RID: 34133
		private float cachedPrice;

		// Token: 0x04008556 RID: 34134
		private float cachedInsulationCold;

		// Token: 0x04008557 RID: 34135
		private float cachedVacuumResistance;

		// Token: 0x04008558 RID: 34136
		private float cachedInsulationHeat;

		// Token: 0x04008559 RID: 34137
		private float cachedToxicEnvironmentResistance;

		// Token: 0x0400855A RID: 34138
		private static readonly List<ThingDef> tmpUsableStuffs = new List<ThingDef>();
	}
}
