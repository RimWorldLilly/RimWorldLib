using System;

namespace Verse
{
	// Token: 0x02000317 RID: 791
	public static class TemperatureTuning
	{
		// Token: 0x04001445 RID: 5189
		public const float MinimumTemperature = -273.15f;

		// Token: 0x04001446 RID: 5190
		public const float MaximumTemperature = 1000f;

		// Token: 0x04001447 RID: 5191
		public const float DefaultTemperature = 21f;

		// Token: 0x04001448 RID: 5192
		public const float DeepUndergroundTemperature = 15f;

		// Token: 0x04001449 RID: 5193
		public static readonly SimpleCurve SeasonalTempVariationCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(0.1f, 4f),
				true
			},
			{
				new CurvePoint(1f, 28f),
				true
			}
		};

		// Token: 0x0400144A RID: 5194
		public const float DailyTempVariationAmplitude = 7f;

		// Token: 0x0400144B RID: 5195
		public const float DailySunEffect = 14f;

		// Token: 0x0400144C RID: 5196
		public const float FoodRefrigerationTemp = 10f;

		// Token: 0x0400144D RID: 5197
		public const float FoodFreezingTemp = 0f;

		// Token: 0x0400144E RID: 5198
		public const int RoomTempEqualizeInterval = 120;

		// Token: 0x0400144F RID: 5199
		public const int Door_TempEqualizeIntervalOpen = 34;

		// Token: 0x04001450 RID: 5200
		public const int Door_TempEqualizeIntervalClosed = 375;

		// Token: 0x04001451 RID: 5201
		public const float Door_TempEqualizeRate = 1f;

		// Token: 0x04001452 RID: 5202
		public const float Vent_TempEqualizeRate = 14f;

		// Token: 0x04001453 RID: 5203
		public const float InventoryTemperature = 14f;

		// Token: 0x04001454 RID: 5204
		public const float DropPodTemperature = 14f;

		// Token: 0x04001455 RID: 5205
		public const float TradeShipTemperature = 14f;

		// Token: 0x04001456 RID: 5206
		public const float TransporterTemperature = 14f;
	}
}
