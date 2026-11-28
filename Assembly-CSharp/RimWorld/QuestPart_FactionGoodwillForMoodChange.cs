using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200107D RID: 4221
	public class QuestPart_FactionGoodwillForMoodChange : QuestPartActivable
	{
		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x060069C4 RID: 27076 RVA: 0x002543B8 File Offset: 0x002525B8
		public override string ExpiryInfoPart
		{
			get
			{
				return "QuestAveragePawnMood".Translate(120000.ToStringTicksToPeriodVerbose(true, true), this.cachedMovingAverage.ToStringPercent());
			}
		}

		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x060069C5 RID: 27077 RVA: 0x002543EC File Offset: 0x002525EC
		public override string ExpiryInfoPartTip
		{
			get
			{
				return "QuestAveragePawnMoodTargets".Translate((from p in this.pawns
				select p.LabelShort).ToCommaList(true, false), 120000.ToStringTicksToPeriodVerbose(true, true));
			}
		}

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x060069C6 RID: 27078 RVA: 0x00254450 File Offset: 0x00252650
		private float AveragePawnMoodPercent
		{
			get
			{
				float num = 0f;
				int num2 = 0;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].needs != null && this.pawns[i].needs.mood != null)
					{
						num += this.pawns[i].needs.mood.CurLevelPercentage;
						num2++;
					}
				}
				if (num2 == 0)
				{
					return 0f;
				}
				return num / (float)num2;
			}
		}

		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x060069C7 RID: 27079 RVA: 0x002544D8 File Offset: 0x002526D8
		private float MovingAveragePawnMoodPercent
		{
			get
			{
				if (this.movingAverage.Count == 0)
				{
					return this.AveragePawnMoodPercent;
				}
				float num = 0f;
				for (int i = 0; i < this.movingAverage.Count; i++)
				{
					num += this.movingAverage[i];
				}
				return num / (float)this.movingAverage.Count;
			}
		}

		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x060069C8 RID: 27080 RVA: 0x00254532 File Offset: 0x00252732
		public int SampleSize
		{
			get
			{
				return Mathf.FloorToInt(48f);
			}
		}

		// Token: 0x060069C9 RID: 27081 RVA: 0x00254540 File Offset: 0x00252740
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			this.currentInterval++;
			if (this.currentInterval >= 2500)
			{
				this.currentInterval = 0;
				while (this.movingAverage.Count >= this.SampleSize)
				{
					this.movingAverage.RemoveLast<float>();
				}
				this.movingAverage.Insert(0, this.AveragePawnMoodPercent);
				this.cachedMovingAverage = this.MovingAveragePawnMoodPercent;
			}
		}

		// Token: 0x060069CA RID: 27082 RVA: 0x002545B4 File Offset: 0x002527B4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (!this.inSignal.NullOrEmpty() && signal.tag == this.inSignal)
			{
				float movingAveragePawnMoodPercent = this.MovingAveragePawnMoodPercent;
				int num = Mathf.RoundToInt(QuestPart_FactionGoodwillForMoodChange.GoodwillFromAverageMoodCurve.Evaluate(movingAveragePawnMoodPercent));
				SignalArgs args = new SignalArgs(num.Named("GOODWILL"), movingAveragePawnMoodPercent.ToStringPercent().Named("AVERAGEMOOD"));
				if (num > 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalSuccess, args, false));
					return;
				}
				Find.SignalManager.SendSignal(new Signal(this.outSignalFailed, args, false));
			}
		}

		// Token: 0x060069CB RID: 27083 RVA: 0x00254660 File Offset: 0x00252860
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalSuccess, "outSignalSuccess", null, false);
			Scribe_Values.Look<string>(ref this.outSignalFailed, "outSignalFailed", null, false);
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<float>(ref this.movingAverage, "movingAverage", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
				if (this.movingAverage == null)
				{
					this.movingAverage = new List<float>();
				}
				this.cachedMovingAverage = this.MovingAveragePawnMoodPercent;
			}
		}

		// Token: 0x0400433A RID: 17210
		public string inSignal;

		// Token: 0x0400433B RID: 17211
		public string outSignalSuccess;

		// Token: 0x0400433C RID: 17212
		public string outSignalFailed;

		// Token: 0x0400433D RID: 17213
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400433E RID: 17214
		private int currentInterval = 2500;

		// Token: 0x0400433F RID: 17215
		private List<float> movingAverage = new List<float>();

		// Token: 0x04004340 RID: 17216
		private float cachedMovingAverage;

		// Token: 0x04004341 RID: 17217
		private static readonly SimpleCurve GoodwillFromAverageMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 20f),
				true
			}
		};

		// Token: 0x04004342 RID: 17218
		public const int Interval = 2500;

		// Token: 0x04004343 RID: 17219
		public const int Range = 120000;
	}
}
