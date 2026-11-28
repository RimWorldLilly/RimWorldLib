using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000703 RID: 1795
	public static class LifeStageUtility
	{
		// Token: 0x060038E8 RID: 14568 RVA: 0x0016D534 File Offset: 0x0016B734
		public static void PlayNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> lifestageGetter, Func<GeneDef, SoundDef> geneGetter, Func<MutantDef, SoundDef> mutantGetter, float volumeFactor = 1f)
		{
			SoundDef soundDef;
			float voxPitch;
			float voxVolume;
			LifeStageUtility.GetNearestLifestageSound(pawn, lifestageGetter, out soundDef, out voxPitch, out voxVolume);
			if (ModsConfig.BiotechActive && pawn.genes != null && geneGetter != null)
			{
				soundDef = pawn.genes.GetSoundOverrideFromGenes(geneGetter, soundDef);
				voxPitch = pawn.ageTracker.CurLifeStage.voxPitch;
				voxVolume = pawn.ageTracker.CurLifeStage.voxVolume;
			}
			if (pawn.IsMutant && mutantGetter != null)
			{
				soundDef = mutantGetter(pawn.mutant.Def);
				voxPitch = pawn.ageTracker.CurLifeStage.voxPitch;
				voxVolume = pawn.ageTracker.CurLifeStage.voxVolume;
			}
			if (soundDef == null)
			{
				return;
			}
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.PositionHeld, pawn.MapHeld, false), MaintenanceType.None);
			info.pitchFactor = voxPitch;
			info.volumeFactor = voxVolume * volumeFactor;
			soundDef.PlayOneShot(info);
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x0016D610 File Offset: 0x0016B810
		private static void GetNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> getter, out SoundDef def, out float pitch, out float volume)
		{
			int num = pawn.ageTracker.CurLifeStageIndex;
			LifeStageAge lifeStageAge;
			for (;;)
			{
				lifeStageAge = pawn.RaceProps.lifeStageAges[num];
				def = getter(lifeStageAge);
				if (def != null)
				{
					break;
				}
				num++;
				if (num < 0 || num >= pawn.RaceProps.lifeStageAges.Count)
				{
					goto IL_84;
				}
			}
			pitch = pawn.ageTracker.CurLifeStage.voxPitch / lifeStageAge.def.voxPitch;
			volume = pawn.ageTracker.CurLifeStage.voxVolume / lifeStageAge.def.voxVolume;
			return;
			IL_84:
			def = null;
			pitch = (volume = 1f);
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x0016D6B4 File Offset: 0x0016B8B4
		private static LifeStageAge GetLifeStageAgeForYears(Pawn pawn, float years)
		{
			List<LifeStageAge> lifeStageAges = pawn.RaceProps.lifeStageAges;
			int num = 0;
			int num2 = 1;
			while (num2 < lifeStageAges.Count && lifeStageAges[num2].minAge < years)
			{
				num++;
				num2++;
			}
			return lifeStageAges[num];
		}

		// Token: 0x060038EB RID: 14571 RVA: 0x0016D6FA File Offset: 0x0016B8FA
		public static DevelopmentalStage CalculateDevelopmentalStage(Pawn pawn, float years)
		{
			return LifeStageUtility.GetLifeStageAgeForYears(pawn, years).def.developmentalStage;
		}

		// Token: 0x060038EC RID: 14572 RVA: 0x0016D710 File Offset: 0x0016B910
		public static float GetMaxBabyAge(RaceProperties raceProps)
		{
			foreach (LifeStageAge lifeStageAge in raceProps.lifeStageAges)
			{
				if (!lifeStageAge.def.developmentalStage.Baby())
				{
					return lifeStageAge.minAge;
				}
			}
			return 0f;
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x0016D780 File Offset: 0x0016B980
		public static bool AlwaysDowned(Pawn pawn)
		{
			Pawn_AgeTracker ageTracker = pawn.ageTracker;
			bool? flag;
			if (ageTracker == null)
			{
				flag = null;
			}
			else
			{
				LifeStageDef curLifeStage = ageTracker.CurLifeStage;
				flag = ((curLifeStage != null) ? new bool?(curLifeStage.alwaysDowned) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}
	}
}
