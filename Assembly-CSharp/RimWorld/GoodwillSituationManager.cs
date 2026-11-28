using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001609 RID: 5641
	public class GoodwillSituationManager
	{
		// Token: 0x06008C9E RID: 35998 RVA: 0x00331D14 File Offset: 0x0032FF14
		public List<GoodwillSituationManager.CachedSituation> GetSituations(Faction other)
		{
			if (other == null || other.IsPlayer)
			{
				Log.Error("Called GetSituations() for faction " + ((other != null) ? other.ToString() : null));
				return null;
			}
			List<GoodwillSituationManager.CachedSituation> result;
			if (this.cachedData.TryGetValue(other, out result))
			{
				return result;
			}
			this.Recalculate(other, true);
			return this.cachedData[other];
		}

		// Token: 0x06008C9F RID: 35999 RVA: 0x00331D70 File Offset: 0x0032FF70
		public int GetMaxGoodwill(Faction other)
		{
			List<GoodwillSituationManager.CachedSituation> situations = this.GetSituations(other);
			int num = 100;
			for (int i = 0; i < situations.Count; i++)
			{
				num = Mathf.Min(num, situations[i].maxGoodwill);
			}
			return num;
		}

		// Token: 0x06008CA0 RID: 36000 RVA: 0x00331DB0 File Offset: 0x0032FFB0
		public int GetNaturalGoodwill(Faction other)
		{
			List<GoodwillSituationManager.CachedSituation> situations = this.GetSituations(other);
			int num = 0;
			for (int i = 0; i < situations.Count; i++)
			{
				num += situations[i].naturalGoodwillOffset;
			}
			return num;
		}

		// Token: 0x06008CA1 RID: 36001 RVA: 0x00331DE8 File Offset: 0x0032FFE8
		public string GetExplanation(Faction other)
		{
			if (other == null || other == Faction.OfPlayer)
			{
				Log.Error("Tried to get CachedGoodwillData explanation for faction " + ((other != null) ? other.ToString() : null));
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			List<GoodwillSituationManager.CachedSituation> situations = this.GetSituations(other);
			for (int i = 0; i < situations.Count; i++)
			{
				stringBuilder.AppendInNewLine(situations[i].def.LabelCap);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06008CA2 RID: 36002 RVA: 0x00331E60 File Offset: 0x00330060
		public void GoodwillManagerTick()
		{
			if (Find.TickManager.TicksGame % 1000 == 0)
			{
				this.RecalculateAll(true);
			}
		}

		// Token: 0x06008CA3 RID: 36003 RVA: 0x00331E7C File Offset: 0x0033007C
		public void RecalculateAll(bool canSendHostilityChangedLetter)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i] != Faction.OfPlayer && allFactionsListForReading[i].HasGoodwill)
				{
					this.Recalculate(allFactionsListForReading[i], canSendHostilityChangedLetter);
				}
			}
		}

		// Token: 0x06008CA4 RID: 36004 RVA: 0x00331ED0 File Offset: 0x003300D0
		private void Recalculate(Faction other, bool canSendHostilityChangedLetter)
		{
			List<GoodwillSituationManager.CachedSituation> outSituations;
			if (this.cachedData.TryGetValue(other, out outSituations))
			{
				this.Recalculate(other, outSituations);
			}
			else
			{
				List<GoodwillSituationManager.CachedSituation> list = new List<GoodwillSituationManager.CachedSituation>();
				this.Recalculate(other, list);
				this.cachedData.Add(other, list);
			}
			this.CheckHostilityChanged(other, canSendHostilityChangedLetter);
		}

		// Token: 0x06008CA5 RID: 36005 RVA: 0x00331F1C File Offset: 0x0033011C
		private void Recalculate(Faction other, List<GoodwillSituationManager.CachedSituation> outSituations)
		{
			outSituations.Clear();
			if (!other.HasGoodwill)
			{
				return;
			}
			List<GoodwillSituationDef> allDefsListForReading = DefDatabase<GoodwillSituationDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				int maxGoodwill = allDefsListForReading[i].Worker.GetMaxGoodwill(other);
				int naturalGoodwillOffset = allDefsListForReading[i].Worker.GetNaturalGoodwillOffset(other);
				if (maxGoodwill < 100 || naturalGoodwillOffset != 0)
				{
					outSituations.Add(new GoodwillSituationManager.CachedSituation
					{
						def = allDefsListForReading[i],
						maxGoodwill = maxGoodwill,
						naturalGoodwillOffset = naturalGoodwillOffset
					});
				}
			}
		}

		// Token: 0x06008CA6 RID: 36006 RVA: 0x00331FAC File Offset: 0x003301AC
		private void CheckHostilityChanged(Faction other, bool canSendHostilityChangedLetter)
		{
			if (Current.ProgramState == ProgramState.Entry)
			{
				return;
			}
			if (!other.HasGoodwill)
			{
				return;
			}
			Faction.OfPlayer.Notify_GoodwillSituationsChanged(other, canSendHostilityChangedLetter, null, null);
		}

		// Token: 0x04005639 RID: 22073
		private Dictionary<Faction, List<GoodwillSituationManager.CachedSituation>> cachedData = new Dictionary<Faction, List<GoodwillSituationManager.CachedSituation>>();

		// Token: 0x0400563A RID: 22074
		private const int RecacheEveryTicks = 1000;

		// Token: 0x0200340D RID: 13325
		public struct CachedSituation
		{
			// Token: 0x0400D0D0 RID: 53456
			public GoodwillSituationDef def;

			// Token: 0x0400D0D1 RID: 53457
			public int maxGoodwill;

			// Token: 0x0400D0D2 RID: 53458
			public int naturalGoodwillOffset;
		}
	}
}
