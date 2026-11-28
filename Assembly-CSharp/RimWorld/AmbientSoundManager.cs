using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020017C2 RID: 6082
	public static class AmbientSoundManager
	{
		// Token: 0x170019BD RID: 6589
		// (get) Token: 0x060097AE RID: 38830 RVA: 0x003704E4 File Offset: 0x0036E6E4
		private static bool WorldAmbientSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_Space);
			}
		}

		// Token: 0x170019BE RID: 6590
		// (get) Token: 0x060097AF RID: 38831 RVA: 0x003704FA File Offset: 0x0036E6FA
		private static bool AltitudeWindSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_AltitudeWind);
			}
		}

		// Token: 0x060097B0 RID: 38832 RVA: 0x00370510 File Offset: 0x0036E710
		public static void EnsureWorldAmbientSoundCreated()
		{
			if (!AmbientSoundManager.WorldAmbientSoundCreated)
			{
				SoundDefOf.Ambient_Space.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
			}
		}

		// Token: 0x060097B1 RID: 38833 RVA: 0x0037052A File Offset: 0x0036E72A
		public static void Notify_SwitchedMap()
		{
			LongEventHandler.ExecuteWhenFinished(new Action(AmbientSoundManager.RecreateMapSustainers));
		}

		// Token: 0x060097B2 RID: 38834 RVA: 0x00370540 File Offset: 0x0036E740
		private static void RecreateMapSustainers()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			BiomeDef biome = Find.CurrentMap.Biome;
			if (!AmbientSoundManager.AltitudeWindSoundCreated && !biome.noAmbientWind)
			{
				AmbientSoundManager.altitudeWindSustainer = SoundDefOf.Ambient_AltitudeWind.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
			}
			else if (AmbientSoundManager.altitudeWindSustainer != null && biome.noAmbientWind)
			{
				AmbientSoundManager.altitudeWindSustainer.End();
				AmbientSoundManager.altitudeWindSustainer = null;
			}
			SustainerManager sustainerManager = Find.SoundRoot.sustainerManager;
			for (int i = 0; i < AmbientSoundManager.biomeAmbientSustainers.Count; i++)
			{
				Sustainer sustainer = AmbientSoundManager.biomeAmbientSustainers[i];
				if (sustainerManager.AllSustainers.Contains(sustainer) && !sustainer.Ended)
				{
					sustainer.End();
				}
			}
			AmbientSoundManager.biomeAmbientSustainers.Clear();
			if (Find.CurrentMap != null)
			{
				List<SoundDef> soundsAmbient = Find.CurrentMap.Biome.soundsAmbient;
				for (int j = 0; j < soundsAmbient.Count; j++)
				{
					Sustainer item = soundsAmbient[j].TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
					AmbientSoundManager.biomeAmbientSustainers.Add(item);
				}
			}
		}

		// Token: 0x04005AC8 RID: 23240
		private static readonly List<Sustainer> biomeAmbientSustainers = new List<Sustainer>();

		// Token: 0x04005AC9 RID: 23241
		private static Sustainer altitudeWindSustainer;
	}
}
