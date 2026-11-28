using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200113C RID: 4412
	public class GameCondition_BloodRain : GameCondition_ForceWeather
	{
		// Token: 0x17001300 RID: 4864
		// (get) Token: 0x06006ED5 RID: 28373 RVA: 0x0026D8D5 File Offset: 0x0026BAD5
		public override int TransitionTicks
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x06006ED6 RID: 28374 RVA: 0x0026D8DC File Offset: 0x0026BADC
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x06006ED7 RID: 28375 RVA: 0x0026D8F0 File Offset: 0x0026BAF0
		public override void Init()
		{
			if (!ModLister.CheckAnomaly("Blood rain"))
			{
				this.End();
				return;
			}
			base.Init();
		}

		// Token: 0x06006ED8 RID: 28376 RVA: 0x0026D90C File Offset: 0x0026BB0C
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

		// Token: 0x06006ED9 RID: 28377 RVA: 0x0026D96C File Offset: 0x0026BB6C
		public override void GameConditionDraw(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x06006EDA RID: 28378 RVA: 0x0026D9A1 File Offset: 0x0026BBA1
		public override void End()
		{
			base.End();
			base.SingleMap.weatherDecider.StartNextWeather();
		}

		// Token: 0x040046E7 RID: 18151
		private List<SkyOverlay> overlays = new List<SkyOverlay>();
	}
}
