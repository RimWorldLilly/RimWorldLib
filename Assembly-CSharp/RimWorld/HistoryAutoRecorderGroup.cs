using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001030 RID: 4144
	public class HistoryAutoRecorderGroup : IExposable
	{
		// Token: 0x0600686F RID: 26735 RVA: 0x0024E6F4 File Offset: 0x0024C8F4
		public float GetMaxDay()
		{
			float num = 0f;
			foreach (HistoryAutoRecorder historyAutoRecorder in this.recorders)
			{
				int count = historyAutoRecorder.records.Count;
				if (count != 0)
				{
					float num2 = (float)((count - 1) * historyAutoRecorder.def.recordTicksFrequency) / 60000f;
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		// Token: 0x06006870 RID: 26736 RVA: 0x0024E778 File Offset: 0x0024C978
		public void Tick()
		{
			for (int i = 0; i < this.recorders.Count; i++)
			{
				this.recorders[i].Tick();
			}
		}

		// Token: 0x06006871 RID: 26737 RVA: 0x0024E7AC File Offset: 0x0024C9AC
		public void DrawGraph(Rect graphRect, Rect legendRect, FloatRange section, List<CurveMark> marks)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame != this.cachedGraphTickCount)
			{
				this.cachedGraphTickCount = ticksGame;
				this.curves.Clear();
				for (int i = 0; i < this.recorders.Count; i++)
				{
					HistoryAutoRecorder historyAutoRecorder = this.recorders[i];
					SimpleCurveDrawInfo simpleCurveDrawInfo = new SimpleCurveDrawInfo();
					simpleCurveDrawInfo.color = historyAutoRecorder.def.graphColor;
					simpleCurveDrawInfo.label = historyAutoRecorder.def.LabelCap;
					simpleCurveDrawInfo.valueFormat = historyAutoRecorder.def.valueFormat;
					simpleCurveDrawInfo.curve = new SimpleCurve();
					for (int j = 0; j < historyAutoRecorder.records.Count; j++)
					{
						simpleCurveDrawInfo.curve.Add(new CurvePoint((float)j * (float)historyAutoRecorder.def.recordTicksFrequency / 60000f, historyAutoRecorder.records[j]), false);
					}
					simpleCurveDrawInfo.curve.SortPoints();
					if (historyAutoRecorder.records.Count == 1)
					{
						simpleCurveDrawInfo.curve.Add(new CurvePoint(1.6666667E-05f, historyAutoRecorder.records[0]), true);
					}
					this.curves.Add(simpleCurveDrawInfo);
				}
			}
			if (Mathf.Approximately(section.min, section.max))
			{
				section.max += 1.6666667E-05f;
			}
			SimpleCurveDrawerStyle curveDrawerStyle = Find.History.curveDrawerStyle;
			curveDrawerStyle.FixedSection = section;
			curveDrawerStyle.UseFixedScale = this.def.useFixedScale;
			curveDrawerStyle.FixedScale = this.def.fixedScale;
			curveDrawerStyle.YIntegersOnly = this.def.integersOnly;
			curveDrawerStyle.OnlyPositiveValues = this.def.onlyPositiveValues;
			SimpleCurveDrawer.DrawCurves(graphRect, this.curves, curveDrawerStyle, marks, legendRect);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06006872 RID: 26738 RVA: 0x0024E981 File Offset: 0x0024CB81
		public void ExposeData()
		{
			Scribe_Defs.Look<HistoryAutoRecorderGroupDef>(ref this.def, "def");
			Scribe_Collections.Look<HistoryAutoRecorder>(ref this.recorders, "recorders", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.AddOrRemoveHistoryRecorders();
			}
		}

		// Token: 0x06006873 RID: 26739 RVA: 0x0024E9B8 File Offset: 0x0024CBB8
		public void AddOrRemoveHistoryRecorders()
		{
			if (this.recorders.RemoveAll((HistoryAutoRecorder x) => x == null) != 0)
			{
				Log.Warning("Some history auto recorders were null.");
			}
			using (List<HistoryAutoRecorderDef>.Enumerator enumerator = this.def.historyAutoRecorderDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HistoryAutoRecorderDef recorderDef = enumerator.Current;
					if (!this.recorders.Any((HistoryAutoRecorder x) => x.def == recorderDef))
					{
						HistoryAutoRecorder historyAutoRecorder = new HistoryAutoRecorder();
						historyAutoRecorder.def = recorderDef;
						this.recorders.Add(historyAutoRecorder);
					}
				}
			}
			this.recorders.RemoveAll((HistoryAutoRecorder x) => x.def == null);
		}

		// Token: 0x04004283 RID: 17027
		public HistoryAutoRecorderGroupDef def;

		// Token: 0x04004284 RID: 17028
		public List<HistoryAutoRecorder> recorders = new List<HistoryAutoRecorder>();

		// Token: 0x04004285 RID: 17029
		private List<SimpleCurveDrawInfo> curves = new List<SimpleCurveDrawInfo>();

		// Token: 0x04004286 RID: 17030
		private int cachedGraphTickCount = -1;
	}
}
