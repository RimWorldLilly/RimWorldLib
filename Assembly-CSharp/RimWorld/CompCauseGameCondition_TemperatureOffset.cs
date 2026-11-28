using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020017FE RID: 6142
	public class CompCauseGameCondition_TemperatureOffset : CompCauseGameCondition
	{
		// Token: 0x17001A10 RID: 6672
		// (get) Token: 0x06009950 RID: 39248 RVA: 0x003776C7 File Offset: 0x003758C7
		public new CompProperties_CausesGameCondition_ClimateAdjuster Props
		{
			get
			{
				return (CompProperties_CausesGameCondition_ClimateAdjuster)this.props;
			}
		}

		// Token: 0x06009951 RID: 39249 RVA: 0x003776D4 File Offset: 0x003758D4
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.temperatureOffset = this.Props.temperatureOffsetRange.min;
		}

		// Token: 0x06009952 RID: 39250 RVA: 0x003776F3 File Offset: 0x003758F3
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.temperatureOffset, "temperatureOffset", 0f, false);
		}

		// Token: 0x06009953 RID: 39251 RVA: 0x00377711 File Offset: 0x00375911
		private string GetFloatStringWithSign(float val)
		{
			if (val < 0f)
			{
				return val.ToString("0");
			}
			return "+" + val.ToString("0");
		}

		// Token: 0x06009954 RID: 39252 RVA: 0x0037773E File Offset: 0x0037593E
		public void SetTemperatureOffset(float offset)
		{
			this.temperatureOffset = this.Props.temperatureOffsetRange.ClampToRange(offset);
			base.ReSetupAllConditions();
		}

		// Token: 0x06009955 RID: 39253 RVA: 0x0037775D File Offset: 0x0037595D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompCauseGameCondition_TemperatureOffset.<CompGetGizmosExtra>d__7 <CompGetGizmosExtra>d__ = new CompCauseGameCondition_TemperatureOffset.<CompGetGizmosExtra>d__7(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009956 RID: 39254 RVA: 0x00377770 File Offset: 0x00375970
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("Temperature".Translate() + ": " + this.GetFloatStringWithSign(this.temperatureOffset));
		}

		// Token: 0x06009957 RID: 39255 RVA: 0x003777CA File Offset: 0x003759CA
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_TemperatureOffset)condition).tempOffset = this.temperatureOffset;
		}

		// Token: 0x06009958 RID: 39256 RVA: 0x003777E8 File Offset: 0x003759E8
		public override void RandomizeSettings(Site site)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
			{
				Settlement settlement = worldObject as Settlement;
				if (settlement != null && settlement.Faction == Faction.OfPlayer)
				{
					if (settlement.Map != null)
					{
						bool flag3 = false;
						foreach (GameCondition gameCondition in settlement.Map.GameConditionManager.ActiveConditions)
						{
							if (gameCondition is GameCondition_TemperatureOffset)
							{
								float num = gameCondition.TemperatureOffset();
								if (num > 0f)
								{
									flag3 = true;
									flag = true;
									flag2 = false;
								}
								else if (num < 0f)
								{
									flag3 = true;
									flag2 = true;
									flag = false;
								}
								if (flag3)
								{
									break;
								}
							}
						}
						if (flag3)
						{
							break;
						}
					}
					PlanetTile tile = worldObject.Tile;
					if ((float)Find.WorldGrid.TraversalDistanceBetween(site.Tile, tile, true, this.Props.worldRange + 1, false) <= (float)this.Props.worldRange)
					{
						float num2 = GenTemperature.MinTemperatureAtTile(tile);
						float num3 = GenTemperature.MaxTemperatureAtTile(tile);
						if (num2 < -5f)
						{
							flag2 = true;
						}
						if (num3 > 20f)
						{
							flag = true;
						}
					}
				}
			}
			if (flag2 == flag)
			{
				this.temperatureOffset = (Rand.Bool ? this.Props.temperatureOffsetRange.min : this.Props.temperatureOffsetRange.max);
				return;
			}
			if (flag2)
			{
				this.temperatureOffset = this.Props.temperatureOffsetRange.min;
				return;
			}
			if (flag)
			{
				this.temperatureOffset = this.Props.temperatureOffsetRange.max;
			}
		}

		// Token: 0x04005B85 RID: 23429
		public float temperatureOffset;

		// Token: 0x04005B86 RID: 23430
		private const float MaxTempForMinOffset = -5f;

		// Token: 0x04005B87 RID: 23431
		private const float MinTempForMaxOffset = 20f;
	}
}
