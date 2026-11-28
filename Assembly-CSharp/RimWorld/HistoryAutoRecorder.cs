using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102F RID: 4143
	public class HistoryAutoRecorder : IExposable
	{
		// Token: 0x0600686A RID: 26730 RVA: 0x0024E584 File Offset: 0x0024C784
		public void Tick()
		{
			if (Find.TickManager.TicksGame % this.def.recordTicksFrequency == 0 || !this.records.Any<float>())
			{
				if (Find.AnyPlayerHomeMap == null)
				{
					this.records.Add(this.records.Last<float>());
					return;
				}
				float item = this.def.Worker.PullRecord();
				this.records.Add(item);
			}
		}

		// Token: 0x0600686B RID: 26731 RVA: 0x0024E5F4 File Offset: 0x0024C7F4
		public void ExposeData()
		{
			Scribe_Defs.Look<HistoryAutoRecorderDef>(ref this.def, "def");
			byte[] recordsFromBytes = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				recordsFromBytes = this.RecordsToBytes();
			}
			DataExposeUtility.LookByteArray(ref recordsFromBytes, "records");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SetRecordsFromBytes(recordsFromBytes);
			}
		}

		// Token: 0x0600686C RID: 26732 RVA: 0x0024E640 File Offset: 0x0024C840
		private byte[] RecordsToBytes()
		{
			byte[] array = new byte[this.records.Count * 4];
			for (int i = 0; i < this.records.Count; i++)
			{
				byte[] bytes = BitConverter.GetBytes(this.records[i]);
				for (int j = 0; j < 4; j++)
				{
					array[i * 4 + j] = bytes[j];
				}
			}
			return array;
		}

		// Token: 0x0600686D RID: 26733 RVA: 0x0024E6A0 File Offset: 0x0024C8A0
		private void SetRecordsFromBytes(byte[] bytes)
		{
			int num = bytes.Length / 4;
			this.records.Clear();
			for (int i = 0; i < num; i++)
			{
				float item = BitConverter.ToSingle(bytes, i * 4);
				this.records.Add(item);
			}
		}

		// Token: 0x04004281 RID: 17025
		public HistoryAutoRecorderDef def;

		// Token: 0x04004282 RID: 17026
		public List<float> records = new List<float>();
	}
}
