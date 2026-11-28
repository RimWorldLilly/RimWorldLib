using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000321 RID: 801
	public class WindManager
	{
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001894 RID: 6292 RVA: 0x00093F3C File Offset: 0x0009213C
		public float WindSpeed
		{
			get
			{
				return this.cachedWindSpeed;
			}
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x00093F44 File Offset: 0x00092144
		public WindManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x00093F60 File Offset: 0x00092160
		public void WindManagerTick()
		{
			this.cachedWindSpeed = this.BaseWindSpeedAt(Find.TickManager.TicksAbs) * this.map.weatherManager.CurWindSpeedFactor;
			float num = this.map.weatherManager.CurWindSpeedOffset;
			this.tempAllGameConditionsAffectingMap.Clear();
			this.map.gameConditionManager.GetAllGameConditionsAffectingMap(this.map, this.tempAllGameConditionsAffectingMap);
			for (int i = 0; i < this.tempAllGameConditionsAffectingMap.Count; i++)
			{
				float num2 = this.tempAllGameConditionsAffectingMap[i].MinWindSpeed();
				if (num2 > num)
				{
					num = num2;
				}
			}
			if (num > 0f)
			{
				FloatRange floatRange = WindManager.WindSpeedRange * this.map.weatherManager.CurWindSpeedFactor;
				float num3 = (this.cachedWindSpeed - floatRange.min) / (floatRange.max - floatRange.min) * (floatRange.max - num);
				this.cachedWindSpeed = num + num3;
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.WindSource);
			for (int j = 0; j < list.Count; j++)
			{
				CompWindSource compWindSource = list[j].TryGetComp<CompWindSource>();
				this.cachedWindSpeed = Mathf.Max(this.cachedWindSpeed, compWindSource.wind);
			}
			if (Prefs.PlantWindSway)
			{
				this.plantSwayHead += Mathf.Min(this.WindSpeed, 1f);
			}
			else
			{
				this.plantSwayHead = 0f;
			}
			if (Find.CurrentMap == this.map)
			{
				for (int k = 0; k < WindManager.plantMaterials.Count; k++)
				{
					WindManager.plantMaterials[k].SetFloat(ShaderPropertyIDs.SwayHead, this.plantSwayHead);
				}
			}
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x00094114 File Offset: 0x00092314
		public static void Notify_PlantMaterialCreated(Material newMat)
		{
			WindManager.plantMaterials.Add(newMat);
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x00094124 File Offset: 0x00092324
		private float BaseWindSpeedAt(int ticksAbs)
		{
			if (this.windNoise == null)
			{
				int seed = Gen.HashCombineInt(this.map.GetHashCode(), 122049541) ^ Find.World.info.Seed;
				this.windNoise = new Perlin(3.9999998989515007E-05, 2.0, 0.5, 4, seed, QualityMode.Medium);
				this.windNoise = new ScaleBias(1.5, 0.5, this.windNoise);
				this.windNoise = new Clamp((double)WindManager.WindSpeedRange.min, (double)WindManager.WindSpeedRange.max, this.windNoise);
			}
			return (float)this.windNoise.GetValue((double)ticksAbs, 0.0, 0.0);
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x000941F8 File Offset: 0x000923F8
		public string DebugString()
		{
			return "WindSpeed: " + this.WindSpeed.ToString() + "\nplantSwayHead: " + this.plantSwayHead.ToString();
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x00094230 File Offset: 0x00092430
		public void LogWindSpeeds()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Upcoming wind speeds:");
			for (int i = 0; i < 72; i++)
			{
				stringBuilder.AppendLine("Hour " + i.ToString() + " - " + this.BaseWindSpeedAt(Find.TickManager.TicksAbs + 2500 * i).ToString("F2"));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04001478 RID: 5240
		private static readonly FloatRange WindSpeedRange = new FloatRange(0.04f, 2f);

		// Token: 0x04001479 RID: 5241
		private Map map;

		// Token: 0x0400147A RID: 5242
		private static List<Material> plantMaterials = new List<Material>();

		// Token: 0x0400147B RID: 5243
		private float cachedWindSpeed;

		// Token: 0x0400147C RID: 5244
		private ModuleBase windNoise;

		// Token: 0x0400147D RID: 5245
		private float plantSwayHead;

		// Token: 0x0400147E RID: 5246
		private List<GameCondition> tempAllGameConditionsAffectingMap = new List<GameCondition>();
	}
}
