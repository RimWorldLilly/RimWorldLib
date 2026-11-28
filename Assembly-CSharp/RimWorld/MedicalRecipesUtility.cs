using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200149A RID: 5274
	public class MedicalRecipesUtility
	{
		// Token: 0x06008146 RID: 33094 RVA: 0x002E2464 File Offset: 0x002E0664
		public static bool IsCleanAndDroppable(Pawn pawn, BodyPartRecord part)
		{
			return !pawn.Dead && !pawn.RaceProps.Animal && (!pawn.IsMutant || pawn.mutant.Def.partsCleanAndDroppable) && part.def.spawnThingOnRemoved != null && MedicalRecipesUtility.IsClean(pawn, part);
		}

		// Token: 0x06008147 RID: 33095 RVA: 0x002E24BC File Offset: 0x002E06BC
		public static bool IsClean(Pawn pawn, BodyPartRecord part)
		{
			return !pawn.Dead && !(from x in pawn.health.hediffSet.hediffs
			where x.Part == part
			select x).Any<Hediff>();
		}

		// Token: 0x06008148 RID: 33096 RVA: 0x002E2509 File Offset: 0x002E0709
		public static void RestorePartAndSpawnAllPreviousParts(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, pos, map);
			MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, pos, map);
			pawn.health.RestorePart(part, null, true);
		}

		// Token: 0x06008149 RID: 33097 RVA: 0x002E252C File Offset: 0x002E072C
		public static Thing SpawnNaturalPartIfClean(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, part))
			{
				return GenSpawn.Spawn(part.def.spawnThingOnRemoved, pos, map, WipeMode.Vanish);
			}
			return null;
		}

		// Token: 0x0600814A RID: 33098 RVA: 0x002E254C File Offset: 0x002E074C
		public static void SpawnThingsFromHediffs(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			if (!pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(part))
			{
				return;
			}
			foreach (Hediff hediff in from x in pawn.health.hediffSet.hediffs
			where x.Part == part
			select x)
			{
				if (hediff.def.spawnThingOnRemoved != null)
				{
					GenSpawn.Spawn(hediff.def.spawnThingOnRemoved, pos, map, WipeMode.Vanish);
				}
			}
			for (int i = 0; i < part.parts.Count; i++)
			{
				MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part.parts[i], pos, map);
			}
		}

		// Token: 0x0600814B RID: 33099 RVA: 0x002E2634 File Offset: 0x002E0834
		public static IEnumerable<BodyPartRecord> GetFixedPartsToApplyOn(RecipeDef recipe, Pawn pawn, Func<BodyPartRecord, bool> validator = null)
		{
			MedicalRecipesUtility.<GetFixedPartsToApplyOn>d__5 <GetFixedPartsToApplyOn>d__ = new MedicalRecipesUtility.<GetFixedPartsToApplyOn>d__5(-2);
			<GetFixedPartsToApplyOn>d__.<>3__recipe = recipe;
			<GetFixedPartsToApplyOn>d__.<>3__pawn = pawn;
			<GetFixedPartsToApplyOn>d__.<>3__validator = validator;
			return <GetFixedPartsToApplyOn>d__;
		}

		// Token: 0x0600814C RID: 33100 RVA: 0x002E2652 File Offset: 0x002E0852
		public static IEnumerable<StatDrawEntry> GetMedicalStatsFromRecipeDefs(IEnumerable<RecipeDef> recipes)
		{
			MedicalRecipesUtility.<GetMedicalStatsFromRecipeDefs>d__6 <GetMedicalStatsFromRecipeDefs>d__ = new MedicalRecipesUtility.<GetMedicalStatsFromRecipeDefs>d__6(-2);
			<GetMedicalStatsFromRecipeDefs>d__.<>3__recipes = recipes;
			return <GetMedicalStatsFromRecipeDefs>d__;
		}

		// Token: 0x0600814E RID: 33102 RVA: 0x002E266A File Offset: 0x002E086A
		[CompilerGenerated]
		internal static IEnumerable<BodyPartRecord> <GetMedicalStatsFromRecipeDefs>g__PartAndAllPartsUnder|6_0(List<BodyPartDef> bpDefs)
		{
			MedicalRecipesUtility.<<GetMedicalStatsFromRecipeDefs>g__PartAndAllPartsUnder|6_0>d <<GetMedicalStatsFromRecipeDefs>g__PartAndAllPartsUnder|6_0>d = new MedicalRecipesUtility.<<GetMedicalStatsFromRecipeDefs>g__PartAndAllPartsUnder|6_0>d(-2);
			<<GetMedicalStatsFromRecipeDefs>g__PartAndAllPartsUnder|6_0>d.<>3__bpDefs = bpDefs;
			return <<GetMedicalStatsFromRecipeDefs>g__PartAndAllPartsUnder|6_0>d;
		}
	}
}
