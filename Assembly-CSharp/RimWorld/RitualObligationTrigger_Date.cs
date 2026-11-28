using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016B6 RID: 5814
	public class RitualObligationTrigger_Date : RitualObligationTrigger
	{
		// Token: 0x17001909 RID: 6409
		// (get) Token: 0x0600918D RID: 37261 RVA: 0x00353EBD File Offset: 0x003520BD
		public string DateString
		{
			get
			{
				return GenDate.QuadrumDateStringAt((long)(this.triggerDaysSinceStartOfYear * 60000), 0f);
			}
		}

		// Token: 0x0600918E RID: 37262 RVA: 0x00353ED6 File Offset: 0x003520D6
		public override void Init(RitualObligationTriggerProperties props)
		{
			base.Init(props);
			this.triggerDaysSinceStartOfYear = this.RandomDate();
		}

		// Token: 0x0600918F RID: 37263 RVA: 0x00353EEC File Offset: 0x003520EC
		public int RandomDate()
		{
			List<int> list = new List<int>();
			foreach (Precept precept in this.ritual.ideo.PreceptsListForReading)
			{
				if (precept != this.ritual)
				{
					Precept_Ritual precept_Ritual = precept as Precept_Ritual;
					if (precept_Ritual != null)
					{
						foreach (RitualObligationTrigger ritualObligationTrigger in precept_Ritual.obligationTriggers)
						{
							RitualObligationTrigger_Date ritualObligationTrigger_Date = ritualObligationTrigger as RitualObligationTrigger_Date;
							if (ritualObligationTrigger_Date != null)
							{
								list.Add(ritualObligationTrigger_Date.triggerDaysSinceStartOfYear);
							}
						}
					}
				}
			}
			List<int> source = Enumerable.Range(0, 60).Except(list).ToList<int>();
			int num = 20;
			bool flag = false;
			int num2 = 0;
			while (num >= 5 && !flag)
			{
				for (int i = 0; i < 10; i++)
				{
					num2 = source.RandomElement<int>();
					bool flag2 = false;
					for (int j = 0; j < list.Count; j++)
					{
						int num3 = list[j];
						int num4 = Mathf.Abs(num2 - num3);
						if (Mathf.Min(num4, 59 - num4) < num)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						flag = true;
						break;
					}
				}
				num -= 5;
			}
			return num2;
		}

		// Token: 0x06009190 RID: 37264 RVA: 0x00354044 File Offset: 0x00352244
		public override void Tick()
		{
			if (this.ritual.isAnytime)
			{
				return;
			}
			int num = this.CurrentTickRelative();
			int num2 = this.OccursOnTick();
			if ((!this.mustBePlayerIdeo || Faction.OfPlayer.ideos.Has(this.ritual.ideo)) && num == num2)
			{
				this.ritual.AddObligation(new RitualObligation(this.ritual, true));
			}
		}

		// Token: 0x06009191 RID: 37265 RVA: 0x003540AC File Offset: 0x003522AC
		public int CurrentTickRelative()
		{
			Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
			long num = GenDate.LocalTicksOffsetFromLongitude((anyPlayerHomeMap != null) ? Find.WorldGrid.LongLatOf(anyPlayerHomeMap.Tile).x : 0f);
			return (int)((long)(GenTicks.TicksAbs % 3600000) + num);
		}

		// Token: 0x06009192 RID: 37266 RVA: 0x003540F4 File Offset: 0x003522F4
		public int OccursOnTick()
		{
			int num = this.triggerDaysSinceStartOfYear * 60000;
			Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
			float longitude = (anyPlayerHomeMap != null) ? Find.WorldGrid.LongLatOf(anyPlayerHomeMap.Tile).x : 0f;
			return num + (int)GenDate.LocalTicksOffsetFromLongitude(longitude);
		}

		// Token: 0x06009193 RID: 37267 RVA: 0x0035413B File Offset: 0x0035233B
		public override void CopyTo(RitualObligationTrigger other)
		{
			base.CopyTo(other);
			((RitualObligationTrigger_Date)other).triggerDaysSinceStartOfYear = this.triggerDaysSinceStartOfYear;
		}

		// Token: 0x06009194 RID: 37268 RVA: 0x00354155 File Offset: 0x00352355
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.triggerDaysSinceStartOfYear, "triggerDaysSinceStartOfYear", 0, false);
		}

		// Token: 0x04005863 RID: 22627
		public int triggerDaysSinceStartOfYear;
	}
}
