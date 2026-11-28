using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CEB RID: 7403
	public static class PawnNeedsUIUtility
	{
		// Token: 0x0600BE87 RID: 48775 RVA: 0x0044D3AF File Offset: 0x0044B5AF
		public static void SortInDisplayOrder(List<Need> needs)
		{
			needs.Sort((Need a, Need b) => b.def.listPriority.CompareTo(a.def.listPriority));
		}

		// Token: 0x0600BE88 RID: 48776 RVA: 0x0044D3D8 File Offset: 0x0044B5D8
		public static Thought GetLeadingThoughtInGroup(List<Thought> thoughtsInGroup)
		{
			Thought result = null;
			int num = -1;
			for (int i = 0; i < thoughtsInGroup.Count; i++)
			{
				if (thoughtsInGroup[i].CurStageIndex > num)
				{
					num = thoughtsInGroup[i].CurStageIndex;
					result = thoughtsInGroup[i];
				}
			}
			return result;
		}

		// Token: 0x0600BE89 RID: 48777 RVA: 0x0044D420 File Offset: 0x0044B620
		public static void GetThoughtGroupsInDisplayOrder(Need_Mood mood, List<Thought> outThoughtGroupsPresent)
		{
			mood.thoughts.GetDistinctMoodThoughtGroups(outThoughtGroupsPresent);
			for (int i = outThoughtGroupsPresent.Count - 1; i >= 0; i--)
			{
				if (!outThoughtGroupsPresent[i].VisibleInNeedsTab)
				{
					outThoughtGroupsPresent.RemoveAt(i);
				}
			}
			foreach (Thought thought in outThoughtGroupsPresent)
			{
				thought.cachedMoodOffsetOfGroup = mood.thoughts.MoodOffsetOfGroup(thought);
			}
			outThoughtGroupsPresent.SortByDescending((Thought t) => t.cachedMoodOffsetOfGroup, (Thought t) => t.GetHashCode());
		}
	}
}
