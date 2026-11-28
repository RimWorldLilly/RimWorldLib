using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023A8 RID: 9128
	public class Caravan_ForageTracker : IExposable
	{
		// Token: 0x170027E7 RID: 10215
		// (get) Token: 0x0600E329 RID: 58153 RVA: 0x00547C51 File Offset: 0x00545E51
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		public ValueTuple<ThingDef, float> ForagedFoodPerDay
		{
			[return: TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})]
			get
			{
				return ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, null);
			}
		}

		// Token: 0x170027E8 RID: 10216
		// (get) Token: 0x0600E32A RID: 58154 RVA: 0x00547C60 File Offset: 0x00545E60
		public string ForagedFoodPerDayExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600E32B RID: 58155 RVA: 0x00547C86 File Offset: 0x00545E86
		public Caravan_ForageTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600E32C RID: 58156 RVA: 0x00547C95 File Offset: 0x00545E95
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
		}

		// Token: 0x0600E32D RID: 58157 RVA: 0x00547CAD File Offset: 0x00545EAD
		public void ForageTrackerTickInterval(int delta)
		{
			if (this.caravan.IsHashIntervalTick(15, delta))
			{
				this.UpdateProgressInterval();
			}
		}

		// Token: 0x0600E32E RID: 58158 RVA: 0x00547CC5 File Offset: 0x00545EC5
		public IEnumerable<Gizmo> GetGizmos()
		{
			Caravan_ForageTracker.<GetGizmos>d__10 <GetGizmos>d__ = new Caravan_ForageTracker.<GetGizmos>d__10(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600E32F RID: 58159 RVA: 0x00547CD8 File Offset: 0x00545ED8
		private void UpdateProgressInterval()
		{
			float num = 15f * ForagedFoodPerDayCalculator.GetProgressPerTick(this.caravan, null);
			this.progress += num;
			if (this.progress >= 1f)
			{
				this.Forage();
				this.progress = 0f;
			}
		}

		// Token: 0x0600E330 RID: 58160 RVA: 0x00547D24 File Offset: 0x00545F24
		private void Forage()
		{
			ThingDef foragedFood = this.caravan.Biome.foragedFood;
			if (foragedFood == null)
			{
				return;
			}
			int i = GenMath.RoundRandom(ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(this.caravan, null));
			int b = Mathf.FloorToInt((this.caravan.MassCapacity - this.caravan.MassUsage) / foragedFood.GetStatValueAbstract(StatDefOf.Mass, null));
			i = Mathf.Min(i, b);
			while (i > 0)
			{
				Thing thing = ThingMaker.MakeThing(foragedFood, null);
				thing.stackCount = Mathf.Min(i, foragedFood.stackLimit);
				i -= thing.stackCount;
				CaravanInventoryUtility.GiveThing(this.caravan, thing);
			}
		}

		// Token: 0x040090ED RID: 37101
		private Caravan caravan;

		// Token: 0x040090EE RID: 37102
		private float progress;

		// Token: 0x040090EF RID: 37103
		private const int UpdateProgressIntervalTicks = 15;
	}
}
