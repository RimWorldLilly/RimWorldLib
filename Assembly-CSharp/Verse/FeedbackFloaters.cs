using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000600 RID: 1536
	public class FeedbackFloaters
	{
		// Token: 0x0600302C RID: 12332 RVA: 0x0013530C File Offset: 0x0013350C
		public void AddFeedback(FeedbackItem newFeedback)
		{
			this.feeders.Add(newFeedback);
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x0013531C File Offset: 0x0013351C
		public void FeedbackUpdate()
		{
			for (int i = this.feeders.Count - 1; i >= 0; i--)
			{
				this.feeders[i].Update();
				if (this.feeders[i].TimeLeft <= 0f)
				{
					this.feeders.Remove(this.feeders[i]);
				}
			}
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x00135384 File Offset: 0x00133584
		public void FeedbackOnGUI()
		{
			foreach (FeedbackItem feedbackItem in this.feeders)
			{
				feedbackItem.FeedbackOnGUI();
			}
		}

		// Token: 0x04001ED8 RID: 7896
		protected List<FeedbackItem> feeders = new List<FeedbackItem>();
	}
}
