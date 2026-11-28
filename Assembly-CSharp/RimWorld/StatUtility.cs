using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FB7 RID: 8119
	public static class StatUtility
	{
		// Token: 0x0600CE20 RID: 52768 RVA: 0x004BAF74 File Offset: 0x004B9174
		public static void SetStatValueInList(ref List<StatModifier> statList, StatDef stat, float value)
		{
			if (statList == null)
			{
				statList = new List<StatModifier>();
			}
			for (int i = 0; i < statList.Count; i++)
			{
				if (statList[i].stat == stat)
				{
					statList[i].value = value;
					return;
				}
			}
			StatModifier statModifier = new StatModifier();
			statModifier.stat = stat;
			statModifier.value = value;
			statList.Add(statModifier);
		}

		// Token: 0x0600CE21 RID: 52769 RVA: 0x004BAFD9 File Offset: 0x004B91D9
		public static float GetStatFactorFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 1f);
		}

		// Token: 0x0600CE22 RID: 52770 RVA: 0x004BAFE7 File Offset: 0x004B91E7
		public static float GetStatFactorFromList(this List<StatModifierQuality> modList, StatDef stat, QualityCategory qc)
		{
			return modList.GetStatValueFromList(stat, qc, 1f);
		}

		// Token: 0x0600CE23 RID: 52771 RVA: 0x004BAFF6 File Offset: 0x004B91F6
		public static bool TryGetStatFactorRangeFromList(this List<StatModifierQuality> modList, StatDef stat, out FloatRange range)
		{
			return modList.TryGetStatValueRangeFromList(stat, out range);
		}

		// Token: 0x0600CE24 RID: 52772 RVA: 0x004BB000 File Offset: 0x004B9200
		public static float GetStatOffsetFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 0f);
		}

		// Token: 0x0600CE25 RID: 52773 RVA: 0x004BB00E File Offset: 0x004B920E
		public static float GetStatOffsetFromList(this List<StatModifierQuality> modList, StatDef stat, QualityCategory qc)
		{
			return modList.GetStatValueFromList(stat, qc, 0f);
		}

		// Token: 0x0600CE26 RID: 52774 RVA: 0x004BB01D File Offset: 0x004B921D
		public static bool TryGetStatOffsetRangeFromList(this List<StatModifierQuality> modList, StatDef stat, out FloatRange range)
		{
			return modList.TryGetStatValueRangeFromList(stat, out range);
		}

		// Token: 0x0600CE27 RID: 52775 RVA: 0x004BB028 File Offset: 0x004B9228
		public static float GetStatValueFromList(this List<StatModifier> modList, StatDef stat, float defaultValue)
		{
			if (modList != null)
			{
				for (int i = 0; i < modList.Count; i++)
				{
					if (modList[i].stat == stat)
					{
						return modList[i].value;
					}
				}
			}
			return defaultValue;
		}

		// Token: 0x0600CE28 RID: 52776 RVA: 0x004BB068 File Offset: 0x004B9268
		public static float GetStatValueFromList(this List<StatModifierQuality> modList, StatDef stat, QualityCategory qc, float defaultValue)
		{
			if (modList != null)
			{
				foreach (StatModifierQuality statModifierQuality in modList)
				{
					if (statModifierQuality.stat == stat)
					{
						return statModifierQuality.GetValue(qc);
					}
				}
				return defaultValue;
			}
			return defaultValue;
		}

		// Token: 0x0600CE29 RID: 52777 RVA: 0x004BB0C8 File Offset: 0x004B92C8
		public static bool TryGetStatValueRangeFromList(this List<StatModifierQuality> modList, StatDef stat, out FloatRange range)
		{
			if (modList != null)
			{
				foreach (StatModifierQuality statModifierQuality in modList)
				{
					if (statModifierQuality.stat == stat)
					{
						range = new FloatRange(statModifierQuality.GetValue(QualityCategory.Awful), statModifierQuality.GetValue(QualityCategory.Legendary));
						return true;
					}
				}
			}
			range = default(FloatRange);
			return false;
		}

		// Token: 0x0600CE2A RID: 52778 RVA: 0x004BB144 File Offset: 0x004B9344
		public static bool StatListContains(this List<StatModifier> modList, StatDef stat)
		{
			if (modList != null)
			{
				for (int i = 0; i < modList.Count; i++)
				{
					if (modList[i].stat == stat)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600CE2B RID: 52779 RVA: 0x004BB178 File Offset: 0x004B9378
		public static string GetOffsetsAndFactorsFor(StatDef stat, Thing thing)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StatRequest statRequest = StatRequest.For(thing);
			float baseValueFor = stat.Worker.GetBaseValueFor(statRequest);
			ToStringNumberSense toStringNumberSense = stat.toStringNumberSense;
			stat.Worker.GetOffsetsAndFactorsExplanation(statRequest, stringBuilder, baseValueFor, "    ");
			stat.Worker.GetAdditionalOffsetsAndFactorsExplanation(statRequest, toStringNumberSense, stringBuilder, "    ");
			return stringBuilder.ToString();
		}
	}
}
