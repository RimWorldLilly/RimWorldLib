using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001200 RID: 4608
	public static class TechLevelUtility
	{
		// Token: 0x0600727D RID: 29309 RVA: 0x002815C8 File Offset: 0x0027F7C8
		public static string ToStringHuman(this TechLevel tl)
		{
			switch (tl)
			{
			case TechLevel.Undefined:
				return "Undefined".Translate();
			case TechLevel.Animal:
				return "TechLevel_Animal".Translate();
			case TechLevel.Neolithic:
				return "TechLevel_Neolithic".Translate();
			case TechLevel.Medieval:
				return "TechLevel_Medieval".Translate();
			case TechLevel.Industrial:
				return "TechLevel_Industrial".Translate();
			case TechLevel.Spacer:
				return "TechLevel_Spacer".Translate();
			case TechLevel.Ultra:
				return "TechLevel_Ultra".Translate();
			case TechLevel.Archotech:
				return "TechLevel_Archotech".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600727E RID: 29310 RVA: 0x00281688 File Offset: 0x0027F888
		public static bool CanSpawnWithEquipmentFrom(this TechLevel pawnLevel, TechLevel gearLevel)
		{
			if (gearLevel == TechLevel.Undefined)
			{
				return false;
			}
			switch (pawnLevel)
			{
			case TechLevel.Undefined:
				return false;
			case TechLevel.Neolithic:
				return gearLevel <= TechLevel.Neolithic;
			case TechLevel.Medieval:
				return gearLevel <= TechLevel.Medieval;
			case TechLevel.Industrial:
				return gearLevel == TechLevel.Industrial;
			case TechLevel.Spacer:
				return gearLevel == TechLevel.Spacer || gearLevel == TechLevel.Industrial;
			case TechLevel.Ultra:
				return gearLevel == TechLevel.Ultra || gearLevel == TechLevel.Spacer;
			case TechLevel.Archotech:
				return gearLevel == TechLevel.Archotech;
			}
			Log.Error("Unknown tech levels " + pawnLevel.ToString() + ", " + gearLevel.ToString());
			return true;
		}

		// Token: 0x0600727F RID: 29311 RVA: 0x00281723 File Offset: 0x0027F923
		public static bool IsNeolithicOrWorse(this TechLevel techLevel)
		{
			return techLevel != TechLevel.Undefined && techLevel <= TechLevel.Neolithic;
		}
	}
}
