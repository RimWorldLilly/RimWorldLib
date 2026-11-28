using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015CF RID: 5583
	public sealed class ThoughtHandler : IExposable
	{
		// Token: 0x06008AAD RID: 35501 RVA: 0x00325184 File Offset: 0x00323384
		public ThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
			this.memories = new MemoryThoughtHandler(pawn);
			this.situational = new SituationalThoughtHandler(pawn);
		}

		// Token: 0x06008AAE RID: 35502 RVA: 0x003251AB File Offset: 0x003233AB
		public void ExposeData()
		{
			Scribe_Deep.Look<MemoryThoughtHandler>(ref this.memories, "memories", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x06008AAF RID: 35503 RVA: 0x003251CC File Offset: 0x003233CC
		public void ThoughtInterval()
		{
			this.situational.SituationalThoughtInterval();
			this.memories.MemoryThoughtInterval();
		}

		// Token: 0x06008AB0 RID: 35504 RVA: 0x003251E4 File Offset: 0x003233E4
		public void GetMoodThoughtsFor(ThoughtDef def, List<Thought> thoughts)
		{
			thoughts.Clear();
			for (int i = 0; i < this.memories.Memories.Count; i++)
			{
				Thought_Memory thought_Memory = this.memories.Memories[i];
				if (thought_Memory.def == def && thought_Memory.MoodOffset() != 0f)
				{
					thoughts.Add(thought_Memory);
				}
			}
			this.situational.AppendMoodThoughts(def, thoughts);
		}

		// Token: 0x06008AB1 RID: 35505 RVA: 0x00325250 File Offset: 0x00323450
		public void GetAllMoodThoughts(List<Thought> outThoughts)
		{
			outThoughts.Clear();
			List<Thought_Memory> list = this.memories.Memories;
			for (int i = 0; i < list.Count; i++)
			{
				Thought_Memory thought_Memory = list[i];
				if (thought_Memory.MoodOffset() != 0f)
				{
					outThoughts.Add(thought_Memory);
				}
			}
			this.situational.AppendMoodThoughts(outThoughts);
		}

		// Token: 0x06008AB2 RID: 35506 RVA: 0x003252A8 File Offset: 0x003234A8
		public void GetMoodThoughts(Thought group, List<Thought> outThoughts)
		{
			this.GetAllMoodThoughts(outThoughts);
			for (int i = outThoughts.Count - 1; i >= 0; i--)
			{
				if (!outThoughts[i].GroupsWith(group))
				{
					outThoughts.RemoveAt(i);
				}
			}
		}

		// Token: 0x06008AB3 RID: 35507 RVA: 0x003252E8 File Offset: 0x003234E8
		public float MoodOffsetOfGroup(Thought group)
		{
			this.GetMoodThoughts(group, ThoughtHandler.tmpThoughts);
			if (!ThoughtHandler.tmpThoughts.Any<Thought>())
			{
				return 0f;
			}
			float num = 0f;
			float num2 = 1f;
			float num3 = 0f;
			for (int i = 0; i < ThoughtHandler.tmpThoughts.Count; i++)
			{
				Thought thought = ThoughtHandler.tmpThoughts[i];
				num += thought.MoodOffset();
				num3 += num2;
				num2 *= thought.def.stackedEffectMultiplier;
			}
			float num4 = num / (float)ThoughtHandler.tmpThoughts.Count;
			ThoughtHandler.tmpThoughts.Clear();
			return num4 * num3;
		}

		// Token: 0x06008AB4 RID: 35508 RVA: 0x00325380 File Offset: 0x00323580
		public void GetDistinctMoodThoughtGroups(List<Thought> outThoughts)
		{
			this.GetAllMoodThoughts(outThoughts);
			for (int i = outThoughts.Count - 1; i >= 0; i--)
			{
				Thought other = outThoughts[i];
				for (int j = 0; j < i; j++)
				{
					if (outThoughts[j].GroupsWith(other))
					{
						outThoughts.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x06008AB5 RID: 35509 RVA: 0x003253D4 File Offset: 0x003235D4
		public float TotalMoodOffset()
		{
			this.GetDistinctMoodThoughtGroups(ThoughtHandler.tmpTotalMoodOffsetThoughts);
			float num = 0f;
			for (int i = 0; i < ThoughtHandler.tmpTotalMoodOffsetThoughts.Count; i++)
			{
				num += this.MoodOffsetOfGroup(ThoughtHandler.tmpTotalMoodOffsetThoughts[i]);
			}
			ThoughtHandler.tmpTotalMoodOffsetThoughts.Clear();
			return num;
		}

		// Token: 0x06008AB6 RID: 35510 RVA: 0x00325428 File Offset: 0x00323628
		public void GetSocialThoughts(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			outThoughts.Clear();
			List<Thought_Memory> list = this.memories.Memories;
			for (int i = 0; i < list.Count; i++)
			{
				ISocialThought socialThought = list[i] as ISocialThought;
				if (socialThought != null && socialThought.OtherPawn() == otherPawn)
				{
					outThoughts.Add(socialThought);
				}
			}
			this.situational.AppendSocialThoughts(otherPawn, outThoughts);
		}

		// Token: 0x06008AB7 RID: 35511 RVA: 0x00325488 File Offset: 0x00323688
		public void GetSocialThoughts(Pawn otherPawn, ISocialThought group, List<ISocialThought> outThoughts)
		{
			this.GetSocialThoughts(otherPawn, outThoughts);
			for (int i = outThoughts.Count - 1; i >= 0; i--)
			{
				if (!((Thought)outThoughts[i]).GroupsWith((Thought)group))
				{
					outThoughts.RemoveAt(i);
				}
			}
		}

		// Token: 0x06008AB8 RID: 35512 RVA: 0x003254D0 File Offset: 0x003236D0
		public int OpinionOffsetOfGroup(ISocialThought group, Pawn otherPawn)
		{
			this.GetSocialThoughts(otherPawn, group, ThoughtHandler.tmpSocialThoughts);
			for (int i = ThoughtHandler.tmpSocialThoughts.Count - 1; i >= 0; i--)
			{
				if (ThoughtHandler.tmpSocialThoughts[i].OpinionOffset() == 0f)
				{
					ThoughtHandler.tmpSocialThoughts.RemoveAt(i);
				}
			}
			if (!ThoughtHandler.tmpSocialThoughts.Any<ISocialThought>())
			{
				return 0;
			}
			ThoughtDef def = ((Thought)group).def;
			if (def.IsMemory && def.stackedEffectMultiplier != 1f)
			{
				ThoughtHandler.tmpSocialThoughts.Sort((ISocialThought a, ISocialThought b) => ((Thought_Memory)a).age.CompareTo(((Thought_Memory)b).age));
			}
			float num = 0f;
			float num2 = 1f;
			for (int j = 0; j < ThoughtHandler.tmpSocialThoughts.Count; j++)
			{
				num += ThoughtHandler.tmpSocialThoughts[j].OpinionOffset() * num2;
				num2 *= ((Thought)ThoughtHandler.tmpSocialThoughts[j]).def.stackedEffectMultiplier;
			}
			ThoughtHandler.tmpSocialThoughts.Clear();
			if (num == 0f)
			{
				return 0;
			}
			if (num > 0f)
			{
				return Mathf.Max(Mathf.RoundToInt(num), 1);
			}
			return Mathf.Min(Mathf.RoundToInt(num), -1);
		}

		// Token: 0x06008AB9 RID: 35513 RVA: 0x0032560C File Offset: 0x0032380C
		public void GetDistinctSocialThoughtGroups(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			this.GetSocialThoughts(otherPawn, outThoughts);
			for (int i = outThoughts.Count - 1; i >= 0; i--)
			{
				ISocialThought socialThought = outThoughts[i];
				for (int j = 0; j < i; j++)
				{
					if (((Thought)outThoughts[j]).GroupsWith((Thought)socialThought))
					{
						outThoughts.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x06008ABA RID: 35514 RVA: 0x0032566C File Offset: 0x0032386C
		public int TotalOpinionOffset(Pawn otherPawn)
		{
			this.GetDistinctSocialThoughtGroups(otherPawn, ThoughtHandler.tmpTotalOpinionOffsetThoughts);
			int num = 0;
			for (int i = 0; i < ThoughtHandler.tmpTotalOpinionOffsetThoughts.Count; i++)
			{
				num += this.OpinionOffsetOfGroup(ThoughtHandler.tmpTotalOpinionOffsetThoughts[i], otherPawn);
			}
			ThoughtHandler.tmpTotalOpinionOffsetThoughts.Clear();
			return num;
		}

		// Token: 0x04005569 RID: 21865
		public Pawn pawn;

		// Token: 0x0400556A RID: 21866
		public MemoryThoughtHandler memories;

		// Token: 0x0400556B RID: 21867
		public SituationalThoughtHandler situational;

		// Token: 0x0400556C RID: 21868
		private static List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x0400556D RID: 21869
		private static List<Thought> tmpTotalMoodOffsetThoughts = new List<Thought>();

		// Token: 0x0400556E RID: 21870
		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();

		// Token: 0x0400556F RID: 21871
		private static List<ISocialThought> tmpTotalOpinionOffsetThoughts = new List<ISocialThought>();
	}
}
