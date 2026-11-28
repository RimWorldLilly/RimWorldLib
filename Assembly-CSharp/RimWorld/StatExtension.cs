using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F69 RID: 8041
	public static class StatExtension
	{
		// Token: 0x0600CCC3 RID: 52419 RVA: 0x004B6205 File Offset: 0x004B4405
		public static float GetStatValue(this Thing thing, StatDef stat, bool applyPostProcess = true, int cacheStaleAfterTicks = -1)
		{
			return stat.Worker.GetValue(thing, applyPostProcess, cacheStaleAfterTicks);
		}

		// Token: 0x0600CCC4 RID: 52420 RVA: 0x004B6215 File Offset: 0x004B4415
		public static float GetStatValueForPawn(this Thing thing, StatDef stat, Pawn pawn, bool applyPostProcess = true)
		{
			return stat.Worker.GetValue(thing, pawn, applyPostProcess);
		}

		// Token: 0x0600CCC5 RID: 52421 RVA: 0x004B6225 File Offset: 0x004B4425
		public static float GetStatValueAbstract(this BuildableDef def, StatDef stat, ThingDef stuff = null)
		{
			return stat.Worker.GetValueAbstract(def, stuff);
		}

		// Token: 0x0600CCC6 RID: 52422 RVA: 0x004B6234 File Offset: 0x004B4434
		public static float GetStatValueAbstract(this AbilityDef def, StatDef stat, Pawn forPawn = null)
		{
			return stat.Worker.GetValueAbstract(def, forPawn);
		}

		// Token: 0x0600CCC7 RID: 52423 RVA: 0x004B6244 File Offset: 0x004B4444
		public static bool StatBaseDefined(this BuildableDef def, StatDef stat)
		{
			if (def.statBases == null)
			{
				return false;
			}
			for (int i = 0; i < def.statBases.Count; i++)
			{
				if (def.statBases[i].stat == stat)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CCC8 RID: 52424 RVA: 0x004B6288 File Offset: 0x004B4488
		public static void SetStatBaseValue(this BuildableDef def, StatDef stat, float newBaseValue)
		{
			StatUtility.SetStatValueInList(ref def.statBases, stat, newBaseValue);
		}
	}
}
