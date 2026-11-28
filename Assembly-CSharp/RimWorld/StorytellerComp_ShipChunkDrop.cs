using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E9 RID: 4585
	public class StorytellerComp_ShipChunkDrop : StorytellerComp
	{
		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x0600721A RID: 29210 RVA: 0x0027ED34 File Offset: 0x0027CF34
		private float ShipChunkDropMTBDays
		{
			get
			{
				float x = (float)Find.TickManager.TicksGame / 3600000f;
				return StorytellerComp_ShipChunkDrop.ShipChunkDropMTBDaysCurve.Evaluate(x);
			}
		}

		// Token: 0x0600721B RID: 29211 RVA: 0x0027ED5E File Offset: 0x0027CF5E
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_ShipChunkDrop.<MakeIntervalIncidents>d__3 <MakeIntervalIncidents>d__ = new StorytellerComp_ShipChunkDrop.<MakeIntervalIncidents>d__3(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x0400482A RID: 18474
		private static readonly SimpleCurve ShipChunkDropMTBDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 20f),
				true
			},
			{
				new CurvePoint(1f, 40f),
				true
			},
			{
				new CurvePoint(2f, 80f),
				true
			},
			{
				new CurvePoint(2.75f, 135f),
				true
			}
		};
	}
}
