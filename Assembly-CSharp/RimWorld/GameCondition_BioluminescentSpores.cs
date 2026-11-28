using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200113B RID: 4411
	public class GameCondition_BioluminescentSpores : GameCondition
	{
		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x06006EC4 RID: 28356 RVA: 0x0026D532 File Offset: 0x0026B732
		public Color CurrentColor
		{
			get
			{
				return Color.Lerp(GameCondition_BioluminescentSpores.Colors[this.prevColorIndex], GameCondition_BioluminescentSpores.Colors[this.curColorIndex], this.curColorTransition);
			}
		}

		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x06006EC5 RID: 28357 RVA: 0x0026D55F File Offset: 0x0026B75F
		private int TransitionDurationTicks
		{
			get
			{
				return 280;
			}
		}

		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x06006EC6 RID: 28358 RVA: 0x0026D566 File Offset: 0x0026B766
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06006EC7 RID: 28359 RVA: 0x0026D570 File Offset: 0x0026B770
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.curColorIndex, "curColorIndex", 0, false);
			Scribe_Values.Look<int>(ref this.prevColorIndex, "prevColorIndex", 0, false);
			Scribe_Values.Look<float>(ref this.curColorTransition, "curColorTransition", 0f, false);
		}

		// Token: 0x06006EC8 RID: 28360 RVA: 0x0026D5BD File Offset: 0x0026B7BD
		public override void Init()
		{
			base.Init();
			this.curColorIndex = Rand.Range(0, GameCondition_BioluminescentSpores.Colors.Length);
			this.prevColorIndex = this.curColorIndex;
			this.curColorTransition = 1f;
		}

		// Token: 0x06006EC9 RID: 28361 RVA: 0x0026D5EF File Offset: 0x0026B7EF
		public override float SkyGazeChanceFactor(Map map)
		{
			return 8f;
		}

		// Token: 0x06006ECA RID: 28362 RVA: 0x0026D5F6 File Offset: 0x0026B7F6
		public override float SkyGazeJoyGainFactor(Map map)
		{
			return 5f;
		}

		// Token: 0x06006ECB RID: 28363 RVA: 0x0026D5FD File Offset: 0x0026B7FD
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x06006ECC RID: 28364 RVA: 0x0026D614 File Offset: 0x0026B814
		public override SkyTarget? SkyTarget(Map map)
		{
			Color currentColor = this.CurrentColor;
			SkyColorSet colorSet = new SkyColorSet(Color.Lerp(Color.white, currentColor, 0.075f) * this.Brightness(map), new Color(0.92f, 0.92f, 0.92f), Color.Lerp(Color.white, currentColor, 0.025f) * this.Brightness(map), 1f);
			return new SkyTarget?(new SkyTarget(0f, colorSet, 1f, 1f));
		}

		// Token: 0x06006ECD RID: 28365 RVA: 0x0026D69A File Offset: 0x0026B89A
		private float Brightness(Map map)
		{
			return Mathf.Max(0.73f, GenCelestial.CurCelestialSunGlow(map));
		}

		// Token: 0x06006ECE RID: 28366 RVA: 0x0026D6AC File Offset: 0x0026B8AC
		public override void GameConditionDraw(Map map)
		{
			if (!base.HiddenByOtherCondition(map))
			{
				GameCondition_BioluminescentSpores.glowSporeOverlay.DrawOverlay(map);
			}
		}

		// Token: 0x06006ECF RID: 28367 RVA: 0x0026D6C4 File Offset: 0x0026B8C4
		public override void GameConditionTick()
		{
			this.curColorTransition += 1f / (float)this.TransitionDurationTicks;
			if (this.curColorTransition >= 1f)
			{
				this.prevColorIndex = this.curColorIndex;
				this.curColorIndex = this.GetNewColorIndex();
				this.curColorTransition = 0f;
			}
			foreach (Map map in base.AffectedMaps)
			{
				if (!base.HiddenByOtherCondition(map))
				{
					GameCondition_BioluminescentSpores.glowSporeOverlay.TickOverlay(map, 1f);
				}
			}
		}

		// Token: 0x06006ED0 RID: 28368 RVA: 0x0026D774 File Offset: 0x0026B974
		private int GetNewColorIndex()
		{
			return (from x in Enumerable.Range(0, GameCondition_BioluminescentSpores.Colors.Length)
			where x != this.curColorIndex
			select x).RandomElement<int>();
		}

		// Token: 0x06006ED1 RID: 28369 RVA: 0x0026D799 File Offset: 0x0026B999
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return GameCondition_BioluminescentSpores.overlays;
		}

		// Token: 0x040046DD RID: 18141
		private int curColorIndex = -1;

		// Token: 0x040046DE RID: 18142
		private int prevColorIndex = -1;

		// Token: 0x040046DF RID: 18143
		private float curColorTransition;

		// Token: 0x040046E0 RID: 18144
		private const float SkyColorStrength = 0.075f;

		// Token: 0x040046E1 RID: 18145
		private const float OverlayColorStrength = 0.025f;

		// Token: 0x040046E2 RID: 18146
		private const float BaseBrightness = 0.73f;

		// Token: 0x040046E3 RID: 18147
		private const int TransitionDurationTicks_NotPermanent = 280;

		// Token: 0x040046E4 RID: 18148
		private static readonly GameCondition_BioluminescentSpores.GlowSporeOverlay glowSporeOverlay = new GameCondition_BioluminescentSpores.GlowSporeOverlay();

		// Token: 0x040046E5 RID: 18149
		private static readonly List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			GameCondition_BioluminescentSpores.glowSporeOverlay
		};

		// Token: 0x040046E6 RID: 18150
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

		// Token: 0x02003083 RID: 12419
		[StaticConstructorOnStartup]
		private class GlowSporeOverlay : SkyOverlay
		{
			// Token: 0x06012545 RID: 75077 RVA: 0x00630A2C File Offset: 0x0062EC2C
			public override void SetOverlayColor(Color color)
			{
				GameCondition_BioluminescentSpores.GlowSporeOverlay.GlowSporeOverlayWorld.color = color;
			}

			// Token: 0x06012546 RID: 75078 RVA: 0x00630A39 File Offset: 0x0062EC39
			public override void DrawOverlay(Map map)
			{
				SkyOverlay.DrawWorldOverlay(map, GameCondition_BioluminescentSpores.GlowSporeOverlay.GlowSporeOverlayWorld, 0);
			}

			// Token: 0x06012547 RID: 75079 RVA: 0x00630A47 File Offset: 0x0062EC47
			public override void TickOverlay(Map map, float lerpFactor)
			{
				this.panner0.Tick();
				this.panner1.Tick();
			}

			// Token: 0x0400C511 RID: 50449
			private static readonly Material GlowSporeOverlayWorld = MatLoader.LoadMat("Weather/GlowSporeOverlayWorld", -1);

			// Token: 0x0400C512 RID: 50450
			private static readonly ComplexCurve speedCurve = new ComplexCurve(new UnityEngine.Keyframe[]
			{
				new UnityEngine.Keyframe(0f, 0f),
				new UnityEngine.Keyframe(1f, 1f),
				new UnityEngine.Keyframe(2f, 1.1f),
				new UnityEngine.Keyframe(3f, 1.2f)
			});

			// Token: 0x0400C513 RID: 50451
			private TexturePannerSpeedCurve panner0 = new TexturePannerSpeedCurve(GameCondition_BioluminescentSpores.GlowSporeOverlay.GlowSporeOverlayWorld, "_MainTex", GameCondition_BioluminescentSpores.GlowSporeOverlay.speedCurve, new Vector2(-1f, -0.2f), 0.0001f);

			// Token: 0x0400C514 RID: 50452
			private TexturePannerSpeedCurve panner1 = new TexturePannerSpeedCurve(GameCondition_BioluminescentSpores.GlowSporeOverlay.GlowSporeOverlayWorld, "_MainTex2", GameCondition_BioluminescentSpores.GlowSporeOverlay.speedCurve, new Vector2(0.35f, -1f), 5E-05f);
		}
	}
}
