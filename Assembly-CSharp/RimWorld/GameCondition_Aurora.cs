using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200113A RID: 4410
	public class GameCondition_Aurora : GameCondition
	{
		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x06006EB3 RID: 28339 RVA: 0x0026D10A File Offset: 0x0026B30A
		public Color CurrentColor
		{
			get
			{
				return Color.Lerp(GameCondition_Aurora.Colors[this.prevColorIndex], GameCondition_Aurora.Colors[this.curColorIndex], this.curColorTransition);
			}
		}

		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x06006EB4 RID: 28340 RVA: 0x0026D137 File Offset: 0x0026B337
		private int TransitionDurationTicks
		{
			get
			{
				if (!base.Permanent)
				{
					return 280;
				}
				return 3750;
			}
		}

		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x06006EB5 RID: 28341 RVA: 0x0026D14C File Offset: 0x0026B34C
		private bool BrightInAllMaps
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (GenCelestial.CurCelestialSunGlow(maps[i]) <= 0.5f)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x06006EB6 RID: 28342 RVA: 0x0026D188 File Offset: 0x0026B388
		public bool AlwaysDarkInAllMaps
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (!maps[i].GameConditionManager.IsAlwaysDarkOutside)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x06006EB7 RID: 28343 RVA: 0x0026D1C2 File Offset: 0x0026B3C2
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06006EB8 RID: 28344 RVA: 0x0026D1CC File Offset: 0x0026B3CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.curColorIndex, "curColorIndex", 0, false);
			Scribe_Values.Look<int>(ref this.prevColorIndex, "prevColorIndex", 0, false);
			Scribe_Values.Look<float>(ref this.curColorTransition, "curColorTransition", 0f, false);
		}

		// Token: 0x06006EB9 RID: 28345 RVA: 0x0026D219 File Offset: 0x0026B419
		public override void Init()
		{
			base.Init();
			this.curColorIndex = Rand.Range(0, GameCondition_Aurora.Colors.Length);
			this.prevColorIndex = this.curColorIndex;
			this.curColorTransition = 1f;
		}

		// Token: 0x06006EBA RID: 28346 RVA: 0x0026D24B File Offset: 0x0026B44B
		public override float SkyGazeChanceFactor(Map map)
		{
			if (map.GameConditionManager.IsAlwaysDarkOutside)
			{
				return 0f;
			}
			return 8f;
		}

		// Token: 0x06006EBB RID: 28347 RVA: 0x0026D265 File Offset: 0x0026B465
		public override float SkyGazeJoyGainFactor(Map map)
		{
			if (map.GameConditionManager.IsAlwaysDarkOutside)
			{
				return 0f;
			}
			return 5f;
		}

		// Token: 0x06006EBC RID: 28348 RVA: 0x0026D27F File Offset: 0x0026B47F
		public override float SkyTargetLerpFactor(Map map)
		{
			if (map.GameConditionManager.IsAlwaysDarkOutside)
			{
				return 0f;
			}
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x06006EBD RID: 28349 RVA: 0x0026D2A8 File Offset: 0x0026B4A8
		public override SkyTarget? SkyTarget(Map map)
		{
			if (map.GameConditionManager.IsAlwaysDarkOutside)
			{
				return null;
			}
			Color currentColor = this.CurrentColor;
			SkyColorSet colorSet = new SkyColorSet(Color.Lerp(Color.white, currentColor, 0.075f) * this.Brightness(map), new Color(0.92f, 0.92f, 0.92f), Color.Lerp(Color.white, currentColor, 0.025f) * this.Brightness(map), 1f);
			return new SkyTarget?(new SkyTarget(Mathf.Max(GenCelestial.CurCelestialSunGlow(map), 0.25f), colorSet, 1f, 1f));
		}

		// Token: 0x06006EBE RID: 28350 RVA: 0x0026D350 File Offset: 0x0026B550
		private float Brightness(Map map)
		{
			return Mathf.Max(0.73f, GenCelestial.CurCelestialSunGlow(map));
		}

		// Token: 0x06006EBF RID: 28351 RVA: 0x0026D364 File Offset: 0x0026B564
		public override void GameConditionTick()
		{
			this.curColorTransition += 1f / (float)this.TransitionDurationTicks;
			if (this.curColorTransition >= 1f)
			{
				this.prevColorIndex = this.curColorIndex;
				this.curColorIndex = this.GetNewColorIndex();
				this.curColorTransition = 0f;
			}
			if (!base.Permanent && base.TicksLeft > this.TransitionTicks)
			{
				if (this.BrightInAllMaps)
				{
					base.TicksLeft = this.TransitionTicks;
				}
				if (this.AlwaysDarkInAllMaps)
				{
					base.TicksLeft = this.TransitionTicks;
				}
			}
		}

		// Token: 0x06006EC0 RID: 28352 RVA: 0x0026D3F9 File Offset: 0x0026B5F9
		private int GetNewColorIndex()
		{
			return (from x in Enumerable.Range(0, GameCondition_Aurora.Colors.Length)
			where x != this.curColorIndex
			select x).RandomElement<int>();
		}

		// Token: 0x040046D2 RID: 18130
		private int curColorIndex = -1;

		// Token: 0x040046D3 RID: 18131
		private int prevColorIndex = -1;

		// Token: 0x040046D4 RID: 18132
		private float curColorTransition;

		// Token: 0x040046D5 RID: 18133
		public const float MaxSunGlow = 0.5f;

		// Token: 0x040046D6 RID: 18134
		private const float Glow = 0.25f;

		// Token: 0x040046D7 RID: 18135
		private const float SkyColorStrength = 0.075f;

		// Token: 0x040046D8 RID: 18136
		private const float OverlayColorStrength = 0.025f;

		// Token: 0x040046D9 RID: 18137
		private const float BaseBrightness = 0.73f;

		// Token: 0x040046DA RID: 18138
		private const int TransitionDurationTicks_NotPermanent = 280;

		// Token: 0x040046DB RID: 18139
		private const int TransitionDurationTicks_Permanent = 3750;

		// Token: 0x040046DC RID: 18140
		private static readonly Color[] Colors = new Color[]
		{
			new Color(0f, 1f, 0f),
			new Color(0.3f, 1f, 0f),
			new Color(0f, 1f, 0.7f),
			new Color(0.3f, 1f, 0.7f),
			new Color(0f, 0.5f, 1f),
			new Color(0f, 0f, 1f),
			new Color(0.87f, 0f, 1f),
			new Color(0.75f, 0f, 1f)
		};
	}
}
