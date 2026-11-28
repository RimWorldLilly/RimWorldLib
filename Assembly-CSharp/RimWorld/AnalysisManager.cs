using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101C RID: 4124
	public class AnalysisManager : IExposable
	{
		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x060067DA RID: 26586 RVA: 0x0024AC39 File Offset: 0x00248E39
		public IEnumerable<AnalysisDetails> AnalysisDetailsForReading
		{
			get
			{
				return this.analysisDetails.Values;
			}
		}

		// Token: 0x060067DB RID: 26587 RVA: 0x0024AC46 File Offset: 0x00248E46
		public bool TryGetAnalysisProgress(int id, out AnalysisDetails details)
		{
			return this.analysisDetails.TryGetValue(id, out details);
		}

		// Token: 0x060067DC RID: 26588 RVA: 0x0024AC55 File Offset: 0x00248E55
		public bool TryIncrementAnalysisProgress(int id, out AnalysisDetails details)
		{
			if (!this.analysisDetails.TryGetValue(id, out details))
			{
				return false;
			}
			if (details.timesDone >= details.required)
			{
				return false;
			}
			details.timesDone++;
			return true;
		}

		// Token: 0x060067DD RID: 26589 RVA: 0x0024AC8C File Offset: 0x00248E8C
		public bool IsAnalysisComplete(int id)
		{
			AnalysisDetails analysisDetails;
			return !this.analysisDetails.TryGetValue(id, out analysisDetails) || analysisDetails.Satisfied;
		}

		// Token: 0x060067DE RID: 26590 RVA: 0x0024ACB4 File Offset: 0x00248EB4
		public void ForceCompleteAnalysisProgress(int id)
		{
			if (!this.HasAnalysisWithID(id))
			{
				this.AddAnalysisTask(id, 1);
			}
			AnalysisDetails analysisDetails;
			if (!this.analysisDetails.TryGetValue(id, out analysisDetails))
			{
				return;
			}
			analysisDetails.timesDone = analysisDetails.required;
		}

		// Token: 0x060067DF RID: 26591 RVA: 0x0024ACEF File Offset: 0x00248EEF
		public void AddAnalysisTask(int id, int requiredTimes)
		{
			if (this.analysisDetails.ContainsKey(id))
			{
				return;
			}
			this.analysisDetails[id] = new AnalysisDetails
			{
				id = id,
				required = requiredTimes
			};
		}

		// Token: 0x060067E0 RID: 26592 RVA: 0x0024AD1F File Offset: 0x00248F1F
		public void RemoveAnalysisDetails(int id)
		{
			if (this.analysisDetails.ContainsKey(id))
			{
				this.analysisDetails.Remove(id);
			}
		}

		// Token: 0x060067E1 RID: 26593 RVA: 0x0024AD3C File Offset: 0x00248F3C
		public bool HasAnalysisWithID(int id)
		{
			return this.analysisDetails.ContainsKey(id);
		}

		// Token: 0x060067E2 RID: 26594 RVA: 0x0024AD4A File Offset: 0x00248F4A
		public void ExposeData()
		{
			Scribe_Collections.Look<int, AnalysisDetails>(ref this.analysisDetails, "analysisDetails", LookMode.Value, LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.analysisDetails == null)
			{
				this.analysisDetails = new Dictionary<int, AnalysisDetails>();
			}
		}

		// Token: 0x04004244 RID: 16964
		private Dictionary<int, AnalysisDetails> analysisDetails = new Dictionary<int, AnalysisDetails>();
	}
}
