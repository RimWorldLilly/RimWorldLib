using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001145 RID: 4421
	public class GameCondition_HateChantDrone : GameCondition
	{
		// Token: 0x17001304 RID: 4868
		// (get) Token: 0x06006EFD RID: 28413 RVA: 0x0026E19D File Offset: 0x0026C39D
		public override string Label
		{
			get
			{
				return this.def.label + ": " + this.level.GetLabel().CapitalizeFirst();
			}
		}

		// Token: 0x17001305 RID: 4869
		// (get) Token: 0x06006EFE RID: 28414 RVA: 0x0026E1C4 File Offset: 0x0026C3C4
		public override string LetterText
		{
			get
			{
				return this.def.letterText.Formatted(this.level.GetLabel());
			}
		}

		// Token: 0x17001306 RID: 4870
		// (get) Token: 0x06006EFF RID: 28415 RVA: 0x0026E1EB File Offset: 0x0026C3EB
		public override int TransitionTicks
		{
			get
			{
				return 180;
			}
		}

		// Token: 0x06006F00 RID: 28416 RVA: 0x0026E1F2 File Offset: 0x0026C3F2
		public override void PostMake()
		{
			base.PostMake();
			base.Permanent = true;
		}

		// Token: 0x06006F01 RID: 28417 RVA: 0x0026E201 File Offset: 0x0026C401
		public override void Init()
		{
			if (!ModLister.CheckAnomaly("Hate chant drone"))
			{
				this.End();
				return;
			}
			base.Init();
		}

		// Token: 0x06006F02 RID: 28418 RVA: 0x0026E21C File Offset: 0x0026C41C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<HateChantDroneLevel>(ref this.level, "level", HateChantDroneLevel.None, false);
		}

		// Token: 0x06006F03 RID: 28419 RVA: 0x0026E236 File Offset: 0x0026C436
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x06006F04 RID: 28420 RVA: 0x0026E24C File Offset: 0x0026C44C
		public override void GameConditionTick()
		{
			base.GameConditionTick();
			List<Map> affectedMaps = base.AffectedMaps;
			for (int i = 0; i < this.overlays.Count; i++)
			{
				for (int j = 0; j < affectedMaps.Count; j++)
				{
					this.overlays[i].TickOverlay(affectedMaps[j], 1f);
				}
			}
		}

		// Token: 0x06006F05 RID: 28421 RVA: 0x0026E2AC File Offset: 0x0026C4AC
		public override void GameConditionDraw(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x06006F06 RID: 28422 RVA: 0x0026E2E1 File Offset: 0x0026C4E1
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}

		// Token: 0x04004704 RID: 18180
		public HateChantDroneLevel level = HateChantDroneLevel.VeryLow;

		// Token: 0x04004705 RID: 18181
		private List<SkyOverlay> overlays = new List<SkyOverlay>();
	}
}
