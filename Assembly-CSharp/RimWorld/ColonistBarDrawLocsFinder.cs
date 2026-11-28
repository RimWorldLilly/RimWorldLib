using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C2B RID: 7211
	public class ColonistBarDrawLocsFinder
	{
		// Token: 0x17002134 RID: 8500
		// (get) Token: 0x0600B7F1 RID: 47089 RVA: 0x0040EBF2 File Offset: 0x0040CDF2
		private ColonistBar ColonistBar
		{
			get
			{
				return Find.ColonistBar;
			}
		}

		// Token: 0x17002135 RID: 8501
		// (get) Token: 0x0600B7F2 RID: 47090 RVA: 0x0040EBF9 File Offset: 0x0040CDF9
		private static float MaxColonistBarWidth
		{
			get
			{
				return (float)UI.screenWidth - 520f;
			}
		}

		// Token: 0x0600B7F3 RID: 47091 RVA: 0x0040EC08 File Offset: 0x0040CE08
		public void CalculateDrawLocs(List<Vector2> outDrawLocs, out float scale, int groupsCount)
		{
			if (this.ColonistBar.Entries.Count == 0)
			{
				outDrawLocs.Clear();
				scale = 1f;
				return;
			}
			this.CalculateColonistsInGroup(groupsCount);
			bool onlyOneRow;
			int maxPerGlobalRow;
			scale = this.FindBestScale(out onlyOneRow, out maxPerGlobalRow, groupsCount);
			this.CalculateDrawLocs(outDrawLocs, scale, onlyOneRow, maxPerGlobalRow, groupsCount);
		}

		// Token: 0x0600B7F4 RID: 47092 RVA: 0x0040EC58 File Offset: 0x0040CE58
		private void CalculateColonistsInGroup(int groupsCount)
		{
			this.entriesInGroup.Clear();
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			for (int i = 0; i < groupsCount; i++)
			{
				this.entriesInGroup.Add(0);
			}
			for (int j = 0; j < entries.Count; j++)
			{
				List<int> list = this.entriesInGroup;
				int group = entries[j].group;
				int num = list[group];
				list[group] = num + 1;
			}
		}

		// Token: 0x0600B7F5 RID: 47093 RVA: 0x0040ECCC File Offset: 0x0040CECC
		private int CalculateGroupsCount()
		{
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < entries.Count; i++)
			{
				if (num != entries[i].group)
				{
					num2++;
					num = entries[i].group;
				}
			}
			return num2;
		}

		// Token: 0x0600B7F6 RID: 47094 RVA: 0x0040ED1C File Offset: 0x0040CF1C
		private float FindBestScale(out bool onlyOneRow, out int maxPerGlobalRow, int groupsCount)
		{
			float num = 1f;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			for (;;)
			{
				float num2 = (ColonistBar.BaseSize.x + 24f) * num;
				float num3 = ColonistBarDrawLocsFinder.MaxColonistBarWidth - (float)(groupsCount - 1) * 25f * num;
				maxPerGlobalRow = Mathf.FloorToInt(num3 / num2);
				onlyOneRow = true;
				if (this.TryDistributeHorizontalSlotsBetweenGroups(maxPerGlobalRow, groupsCount))
				{
					int allowedRowsCountForScale = ColonistBarDrawLocsFinder.GetAllowedRowsCountForScale(num);
					bool flag = true;
					int num4 = -1;
					for (int i = 0; i < entries.Count; i++)
					{
						if (num4 != entries[i].group)
						{
							num4 = entries[i].group;
							int num5 = Mathf.CeilToInt((float)this.entriesInGroup[entries[i].group] / (float)this.horizontalSlotsPerGroup[entries[i].group]);
							if (num5 > 1)
							{
								onlyOneRow = false;
							}
							if (num5 > allowedRowsCountForScale)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
				num *= 0.95f;
			}
			return num;
		}

		// Token: 0x0600B7F7 RID: 47095 RVA: 0x0040EE1C File Offset: 0x0040D01C
		private bool TryDistributeHorizontalSlotsBetweenGroups(int maxPerGlobalRow, int groupsCount)
		{
			this.horizontalSlotsPerGroup.Clear();
			for (int k = 0; k < groupsCount; k++)
			{
				this.horizontalSlotsPerGroup.Add(0);
			}
			GenMath.DHondtDistribution(this.horizontalSlotsPerGroup, (int i) => (float)this.entriesInGroup[i], maxPerGlobalRow);
			for (int j = 0; j < this.horizontalSlotsPerGroup.Count; j++)
			{
				if (this.horizontalSlotsPerGroup[j] == 0)
				{
					int num = this.horizontalSlotsPerGroup.Max();
					if (num <= 1)
					{
						return false;
					}
					int num2 = this.horizontalSlotsPerGroup.IndexOf(num);
					List<int> list = this.horizontalSlotsPerGroup;
					int num3 = num2;
					int num4 = list[num3];
					list[num3] = num4 - 1;
					List<int> list2 = this.horizontalSlotsPerGroup;
					num4 = j;
					num3 = list2[num4];
					list2[num4] = num3 + 1;
				}
			}
			return true;
		}

		// Token: 0x0600B7F8 RID: 47096 RVA: 0x0040EEE4 File Offset: 0x0040D0E4
		private static int GetAllowedRowsCountForScale(float scale)
		{
			if (scale > 0.58f)
			{
				return 1;
			}
			if (scale > 0.42f)
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x0600B7F9 RID: 47097 RVA: 0x0040EEFC File Offset: 0x0040D0FC
		private void CalculateDrawLocs(List<Vector2> outDrawLocs, float scale, bool onlyOneRow, int maxPerGlobalRow, int groupsCount)
		{
			outDrawLocs.Clear();
			int num = maxPerGlobalRow;
			if (onlyOneRow)
			{
				for (int i = 0; i < this.horizontalSlotsPerGroup.Count; i++)
				{
					this.horizontalSlotsPerGroup[i] = Mathf.Min(this.horizontalSlotsPerGroup[i], this.entriesInGroup[i]);
				}
				num = this.ColonistBar.Entries.Count;
			}
			float num2 = (ColonistBar.BaseSize.x + 24f) * scale;
			float num3 = (float)num * num2 + (float)(groupsCount - 1) * 25f * scale;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			int num4 = -1;
			int num5 = -1;
			float num6 = ((float)UI.screenWidth - num3) / 2f;
			for (int j = 0; j < entries.Count; j++)
			{
				if (num4 != entries[j].group)
				{
					if (num4 >= 0)
					{
						num6 += 25f * scale;
						num6 += (float)this.horizontalSlotsPerGroup[num4] * scale * (ColonistBar.BaseSize.x + 24f);
					}
					num5 = 0;
					num4 = entries[j].group;
				}
				else
				{
					num5++;
				}
				Vector2 drawLoc = this.GetDrawLoc(num6, 21f, entries[j].group, num5, scale);
				outDrawLocs.Add(drawLoc);
			}
		}

		// Token: 0x0600B7FA RID: 47098 RVA: 0x0040F058 File Offset: 0x0040D258
		private Vector2 GetDrawLoc(float groupStartX, float groupStartY, int group, int numInGroup, float scale)
		{
			float num = groupStartX + (float)(numInGroup % this.horizontalSlotsPerGroup[group]) * scale * (ColonistBar.BaseSize.x + 24f);
			float y = groupStartY + (float)(numInGroup / this.horizontalSlotsPerGroup[group]) * scale * (ColonistBar.BaseSize.y + 32f);
			if (numInGroup >= this.entriesInGroup[group] - this.entriesInGroup[group] % this.horizontalSlotsPerGroup[group])
			{
				int num2 = this.horizontalSlotsPerGroup[group] - this.entriesInGroup[group] % this.horizontalSlotsPerGroup[group];
				num += (float)num2 * scale * (ColonistBar.BaseSize.x + 24f) * 0.5f;
			}
			return new Vector2(num, y);
		}

		// Token: 0x04006992 RID: 27026
		private List<int> entriesInGroup = new List<int>();

		// Token: 0x04006993 RID: 27027
		private List<int> horizontalSlotsPerGroup = new List<int>();

		// Token: 0x04006994 RID: 27028
		private const float MarginTop = 21f;
	}
}
