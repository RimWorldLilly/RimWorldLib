using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013A7 RID: 5031
	[StaticConstructorOnStartup]
	public class WeatherEvent_LightningStrike : WeatherEvent_LightningFlash
	{
		// Token: 0x06007CCE RID: 31950 RVA: 0x002C9AE0 File Offset: 0x002C7CE0
		public WeatherEvent_LightningStrike(Map map) : base(map)
		{
		}

		// Token: 0x06007CCF RID: 31951 RVA: 0x002C9AF4 File Offset: 0x002C7CF4
		public WeatherEvent_LightningStrike(Map map, IntVec3 forcedStrikeLoc) : base(map)
		{
			this.strikeLoc = forcedStrikeLoc;
		}

		// Token: 0x06007CD0 RID: 31952 RVA: 0x002C9B0F File Offset: 0x002C7D0F
		public override void FireEvent()
		{
			WeatherEvent_LightningStrike.DoStrike(this.strikeLoc, this.map, ref this.boltMesh);
		}

		// Token: 0x06007CD1 RID: 31953 RVA: 0x002C9B28 File Offset: 0x002C7D28
		public static void DoStrike(IntVec3 strikeLoc, Map map, ref Mesh boltMesh)
		{
			SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(map);
			if (!strikeLoc.IsValid)
			{
				strikeLoc = CellFinderLoose.RandomCellWith((IntVec3 sq) => sq.Standable(map) && !map.roofGrid.Roofed(sq), map, 1000);
			}
			boltMesh = LightningBoltMeshPool.RandomBoltMesh;
			if (!strikeLoc.Fogged(map))
			{
				GenExplosion.DoExplosion(strikeLoc, map, 1.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
				Vector3 loc = strikeLoc.ToVector3Shifted();
				for (int i = 0; i < 4; i++)
				{
					FleckMaker.ThrowSmoke(loc, map, 1.5f);
					FleckMaker.ThrowMicroSparks(loc, map);
					FleckMaker.ThrowLightningGlow(loc, map, 1.5f);
				}
			}
			SoundInfo info = SoundInfo.InMap(new TargetInfo(strikeLoc, map, false), MaintenanceType.None);
			SoundDefOf.Thunder_OnMap.PlayOneShot(info);
		}

		// Token: 0x06007CD2 RID: 31954 RVA: 0x002C9C6F File Offset: 0x002C7E6F
		public override void WeatherEventDraw()
		{
			Graphics.DrawMesh(this.boltMesh, this.strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity, FadedMaterialPool.FadedVersionOf(WeatherEvent_LightningStrike.LightningMat, this.LightningBrightness), 0);
		}

		// Token: 0x04004E96 RID: 20118
		private IntVec3 strikeLoc = IntVec3.Invalid;

		// Token: 0x04004E97 RID: 20119
		private Mesh boltMesh;

		// Token: 0x04004E98 RID: 20120
		private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt", -1);
	}
}
