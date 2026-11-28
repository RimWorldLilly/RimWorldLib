using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017A3 RID: 6051
	internal class ScenPart_CreateIncident : ScenPart_IncidentBase
	{
		// Token: 0x170019AD RID: 6573
		// (get) Token: 0x060096DC RID: 38620 RVA: 0x0036BBC7 File Offset: 0x00369DC7
		protected override string IncidentTag
		{
			get
			{
				return "CreateIncident";
			}
		}

		// Token: 0x170019AE RID: 6574
		// (get) Token: 0x060096DD RID: 38621 RVA: 0x0036BBD0 File Offset: 0x00369DD0
		private float RandomInterval
		{
			get
			{
				float maxInclusive = (this.maxDays > 0f && this.maxDays > this.minDays) ? this.maxDays : this.minDays;
				return Rand.Range(this.minDays, maxInclusive);
			}
		}

		// Token: 0x060096DE RID: 38622 RVA: 0x0036BC14 File Offset: 0x00369E14
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalDays, "intervalDays", 0f, false);
			Scribe_Values.Look<float>(ref this.minDays, "minDays", this.intervalDays, false);
			Scribe_Values.Look<float>(ref this.maxDays, "maxDays", 0f, false);
			Scribe_Values.Look<bool>(ref this.repeat, "repeat", false, false);
			Scribe_Values.Look<float>(ref this.occurTick, "occurTick", 0f, false);
			Scribe_Values.Look<bool>(ref this.isFinished, "isFinished", false, false);
		}

		// Token: 0x060096DF RID: 38623 RVA: 0x0036BCA4 File Offset: 0x00369EA4
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.intervalDays.GetHashCode() ^ ((this.repeat > false) ? 1 : 0) ^ this.occurTick.GetHashCode() ^ ((this.isFinished > false) ? 1 : 0);
		}

		// Token: 0x060096E0 RID: 38624 RVA: 0x0036BCD8 File Offset: 0x00369ED8
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
			Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect2 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height / 3f, scenPartRect.width / 2f, scenPartRect.height / 3f);
			Rect rect3 = new Rect(scenPartRect.x + scenPartRect.width / 2f, scenPartRect.y + scenPartRect.height / 3f, scenPartRect.width / 2f, scenPartRect.height / 3f);
			Rect rect4 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 2f / 3f, scenPartRect.width, scenPartRect.height / 3f);
			base.DoIncidentEditInterface(rect);
			Widgets.TextFieldNumericLabeled<float>(rect2, "minDays".Translate(), ref this.minDays, ref this.minDaysBuffer, 0f, 1E+09f);
			Widgets.TextFieldNumericLabeled<float>(rect3, "maxDays".Translate(), ref this.maxDays, ref this.maxDaysBuffer, 0f, 1E+09f);
			Widgets.CheckboxLabeled(rect4, "repeat".Translate(), ref this.repeat, false, null, null, false, false);
		}

		// Token: 0x060096E1 RID: 38625 RVA: 0x0036BE60 File Offset: 0x0036A060
		public override void Randomize()
		{
			base.Randomize();
			this.intervalDays = 15f * Rand.Gaussian(0f, 1f) + 30f;
			if (this.intervalDays < 0f)
			{
				this.intervalDays = 0f;
			}
			this.minDays = this.intervalDays;
			this.repeat = (Rand.Range(0, 100) < 50);
		}

		// Token: 0x060096E2 RID: 38626 RVA: 0x0036BECA File Offset: 0x0036A0CA
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return new ScenPart_CreateIncident.<RandomizableIncidents>d__19(-2);
		}

		// Token: 0x060096E3 RID: 38627 RVA: 0x0036BED3 File Offset: 0x0036A0D3
		public override void PostGameStart()
		{
			base.PostGameStart();
			this.occurTick = (float)Find.TickManager.TicksGame + 60000f * this.RandomInterval;
		}

		// Token: 0x060096E4 RID: 38628 RVA: 0x0036BEFC File Offset: 0x0036A0FC
		public override void Tick()
		{
			base.Tick();
			if (Find.AnyPlayerHomeMap == null)
			{
				return;
			}
			if (this.isFinished)
			{
				return;
			}
			if (this.incident == null)
			{
				Log.Error("Trying to tick ScenPart_CreateIncident but the incident is null");
				this.isFinished = true;
				return;
			}
			if ((float)Find.TickManager.TicksGame >= this.occurTick)
			{
				IncidentParms parms = StorytellerUtility.DefaultParmsNow(this.incident.category, (from x in Find.Maps
				where x.IsPlayerHome
				select x).RandomElement<Map>());
				if (!this.incident.Worker.TryExecute(parms))
				{
					this.isFinished = true;
					return;
				}
				if (this.repeat && (this.minDays > 0f || this.maxDays > this.minDays))
				{
					this.occurTick += 60000f * this.RandomInterval;
					return;
				}
				this.isFinished = true;
			}
		}

		// Token: 0x04005A6B RID: 23147
		private const float IntervalMidpoint = 30f;

		// Token: 0x04005A6C RID: 23148
		private const float IntervalDeviation = 15f;

		// Token: 0x04005A6D RID: 23149
		private float intervalDays;

		// Token: 0x04005A6E RID: 23150
		private float minDays;

		// Token: 0x04005A6F RID: 23151
		private float maxDays;

		// Token: 0x04005A70 RID: 23152
		private bool repeat;

		// Token: 0x04005A71 RID: 23153
		private string intervalDaysBuffer;

		// Token: 0x04005A72 RID: 23154
		private string minDaysBuffer;

		// Token: 0x04005A73 RID: 23155
		private string maxDaysBuffer;

		// Token: 0x04005A74 RID: 23156
		private float occurTick;

		// Token: 0x04005A75 RID: 23157
		private bool isFinished;
	}
}
