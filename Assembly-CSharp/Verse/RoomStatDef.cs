using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001B3 RID: 435
	public class RoomStatDef : Def
	{
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x00040399 File Offset: 0x0003E599
		public RoomStatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomStatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x000403CB File Offset: 0x0003E5CB
		public RoomStatScoreStage GetScoreStage(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				return null;
			}
			return this.scoreStages[this.GetScoreStageIndex(score)];
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x000403F0 File Offset: 0x0003E5F0
		public int GetScoreStageIndex(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				throw new InvalidOperationException("No score stages available.");
			}
			int result = 0;
			int num = 0;
			while (num < this.scoreStages.Count && score >= this.scoreStages[num].minScore)
			{
				result = num;
				num++;
			}
			return result;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00040444 File Offset: 0x0003E644
		public string ScoreToString(float score)
		{
			if (this.displayRounded)
			{
				return Mathf.RoundToInt(score).ToString();
			}
			return score.ToString("F2");
		}

		// Token: 0x04000B6E RID: 2926
		public Type workerClass;

		// Token: 0x04000B6F RID: 2927
		public float updatePriority;

		// Token: 0x04000B70 RID: 2928
		public bool displayRounded;

		// Token: 0x04000B71 RID: 2929
		public bool isHidden;

		// Token: 0x04000B72 RID: 2930
		public float roomlessScore;

		// Token: 0x04000B73 RID: 2931
		public List<RoomStatScoreStage> scoreStages;

		// Token: 0x04000B74 RID: 2932
		public RoomStatDef inputStat;

		// Token: 0x04000B75 RID: 2933
		public SimpleCurve curve;

		// Token: 0x04000B76 RID: 2934
		[Unsaved(false)]
		private RoomStatWorker workerInt;
	}
}
