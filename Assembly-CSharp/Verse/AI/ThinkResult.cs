using System;

namespace Verse.AI
{
	// Token: 0x020008F0 RID: 2288
	public struct ThinkResult : IEquatable<ThinkResult>
	{
		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06004612 RID: 17938 RVA: 0x001B8446 File Offset: 0x001B6646
		public Job Job
		{
			get
			{
				return this.jobInt;
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06004613 RID: 17939 RVA: 0x001B844E File Offset: 0x001B664E
		public ThinkNode SourceNode
		{
			get
			{
				return this.sourceNodeInt;
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06004614 RID: 17940 RVA: 0x001B8456 File Offset: 0x001B6656
		public JobTag? Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06004615 RID: 17941 RVA: 0x001B845E File Offset: 0x001B665E
		public bool FromQueue
		{
			get
			{
				return this.fromQueue;
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06004616 RID: 17942 RVA: 0x001B8466 File Offset: 0x001B6666
		public bool IsValid
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06004617 RID: 17943 RVA: 0x001B8474 File Offset: 0x001B6674
		public static ThinkResult NoJob
		{
			get
			{
				return new ThinkResult(null, null, null, false);
			}
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x001B8492 File Offset: 0x001B6692
		public ThinkResult(Job job, ThinkNode sourceNode, JobTag? tag = null, bool fromQueue = false)
		{
			this.jobInt = job;
			this.sourceNodeInt = sourceNode;
			this.tag = tag;
			this.fromQueue = fromQueue;
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x001B84B4 File Offset: 0x001B66B4
		public override string ToString()
		{
			string text = (this.Job != null) ? this.Job.ToString() : "null";
			string text2 = (this.SourceNode != null) ? this.SourceNode.ToString() : "null";
			return string.Concat(new string[]
			{
				"(job=",
				text,
				" sourceNode=",
				text2,
				")"
			});
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x001B8522 File Offset: 0x001B6722
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<bool>(Gen.HashCombine<JobTag?>(Gen.HashCombine<ThinkNode>(Gen.HashCombine<Job>(0, this.jobInt), this.sourceNodeInt), this.tag), this.fromQueue);
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x001B8551 File Offset: 0x001B6751
		public override bool Equals(object obj)
		{
			return obj is ThinkResult && this.Equals((ThinkResult)obj);
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x001B856C File Offset: 0x001B676C
		public bool Equals(ThinkResult other)
		{
			if (this.jobInt == other.jobInt && this.sourceNodeInt == other.sourceNodeInt)
			{
				JobTag? jobTag = this.tag;
				JobTag? jobTag2 = other.tag;
				if (jobTag.GetValueOrDefault() == jobTag2.GetValueOrDefault() & jobTag != null == (jobTag2 != null))
				{
					return this.fromQueue == other.fromQueue;
				}
			}
			return false;
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x001B85D6 File Offset: 0x001B67D6
		public static bool operator ==(ThinkResult lhs, ThinkResult rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x001B85E0 File Offset: 0x001B67E0
		public static bool operator !=(ThinkResult lhs, ThinkResult rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04002D80 RID: 11648
		private Job jobInt;

		// Token: 0x04002D81 RID: 11649
		private ThinkNode sourceNodeInt;

		// Token: 0x04002D82 RID: 11650
		private JobTag? tag;

		// Token: 0x04002D83 RID: 11651
		private bool fromQueue;
	}
}
