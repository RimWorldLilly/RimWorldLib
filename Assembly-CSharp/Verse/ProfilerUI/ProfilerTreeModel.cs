using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x020007A3 RID: 1955
	public sealed class ProfilerTreeModel
	{
		// Token: 0x06003EA7 RID: 16039 RVA: 0x0018F10B File Offset: 0x0018D30B
		public static ulong CombineKey(ulong parentKey, ProfilerLabelId labelId)
		{
			return (parentKey ^ (ulong)(labelId.Value + 2)) * 1099511628211UL;
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06003EA8 RID: 16040 RVA: 0x0018F123 File Offset: 0x0018D323
		// (set) Token: 0x06003EA9 RID: 16041 RVA: 0x0018F12B File Offset: 0x0018D32B
		public ProfilerTreeModel.SortColumn ActiveSortColumn { get; set; }

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06003EAA RID: 16042 RVA: 0x0018F134 File Offset: 0x0018D334
		// (set) Token: 0x06003EAB RID: 16043 RVA: 0x0018F13C File Offset: 0x0018D33C
		public bool SortAscending { get; set; }

		// Token: 0x06003EAC RID: 16044 RVA: 0x0018F148 File Offset: 0x0018D348
		public ProfilerTreeModel()
		{
			this.rowComparison = new Comparison<ProfilerTreeModel.Row>(this.CompareRows);
			this.emitTarget = this.DisplayRows;
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x0018F27C File Offset: 0x0018D47C
		private int CompareRows(ProfilerTreeModel.Row lhs, ProfilerTreeModel.Row rhs)
		{
			int num;
			switch (this.ActiveSortColumn)
			{
			case ProfilerTreeModel.SortColumn.Self:
				num = lhs.selfMicroseconds.CompareTo(rhs.selfMicroseconds);
				break;
			case ProfilerTreeModel.SortColumn.Calls:
				num = lhs.calls.CompareTo(rhs.calls);
				break;
			case ProfilerTreeModel.SortColumn.Alloc:
				num = lhs.allocBytes.CompareTo(rhs.allocBytes);
				break;
			case ProfilerTreeModel.SortColumn.Label:
				num = string.CompareOrdinal(ProfilerTreeModel.LabelForRow(lhs), ProfilerTreeModel.LabelForRow(rhs));
				break;
			default:
				num = lhs.totalMicroseconds.CompareTo(rhs.totalMicroseconds);
				break;
			}
			int num2 = num;
			if (!this.SortAscending)
			{
				num2 = -num2;
			}
			if (num2 != 0)
			{
				return num2;
			}
			return string.CompareOrdinal(ProfilerTreeModel.LabelForRow(lhs), ProfilerTreeModel.LabelForRow(rhs));
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x0018F32F File Offset: 0x0018D52F
		public void SetWindow(bool spikes, bool followNewest, int newestOrdinal, int oldestOrdinal, FrameProfiler.FrameSnapshot fallback)
		{
			this.pending.spikes = spikes;
			this.pending.followNewest = followNewest;
			this.pending.newestOrdinal = newestOrdinal;
			this.pending.oldestOrdinal = oldestOrdinal;
			this.pending.fallbackSnapshot = fallback;
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06003EAF RID: 16047 RVA: 0x0018F36F File Offset: 0x0018D56F
		// (set) Token: 0x06003EB0 RID: 16048 RVA: 0x0018F377 File Offset: 0x0018D577
		public ProfilerWindowRequest EmittedRequest { get; private set; }

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06003EB1 RID: 16049 RVA: 0x0018F380 File Offset: 0x0018D580
		public ProfilerWindowRequest SubmittedRequest
		{
			get
			{
				return this.submitted;
			}
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x0018F388 File Offset: 0x0018D588
		private int[] SelectedThreadIds()
		{
			if (this.submittedThreadVersion == ProfilerThreadRegistry.Version && this.submittedSelectionVersion == ProfilerThreadRegistry.SelectionVersion)
			{
				return this.submittedThreadIds;
			}
			this.submittedThreadVersion = ProfilerThreadRegistry.Version;
			this.submittedSelectionVersion = ProfilerThreadRegistry.SelectionVersion;
			if (ProfilerThreadRegistry.AllSelected)
			{
				return this.submittedThreadIds = null;
			}
			this.threadIdScratch.Clear();
			foreach (ProfilerThreadRegistry.Entry entry in ProfilerThreadRegistry.Entries)
			{
				if (ProfilerThreadRegistry.IsSelected(entry.threadId))
				{
					this.threadIdScratch.Add(entry.threadId);
				}
			}
			this.threadIdScratch.Sort();
			if (this.submittedThreadIds != null && CollectionsMarshal.AsSpan<int>(this.threadIdScratch).SequenceEqual(this.submittedThreadIds))
			{
				return this.submittedThreadIds;
			}
			return this.submittedThreadIds = this.threadIdScratch.ToArray();
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x0018F48C File Offset: 0x0018D68C
		private void SubmitRequestIfChanged()
		{
			this.pending.clipStartNs = this.clipStartNs;
			this.pending.clipEndNs = this.clipEndNs;
			this.pending.foldRecursion = this.foldRecursion;
			this.pending.selectedThreadIds = this.SelectedThreadIds();
			if (this.submitted != null && this.submitted.SameWindowAs(this.pending))
			{
				return;
			}
			this.submitted = this.pending;
			this.pending = this.submitted.Clone();
			ProfilerWindowAggregator.Submit(this.submitted);
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x0018F521 File Offset: 0x0018D721
		private ProfilerTreeModel.Row RentRow(ProfilerLabelId labelId)
		{
			ProfilerTreeModel.Row row = (this.rowPool.Count > 0) ? this.rowPool.Pop() : new ProfilerTreeModel.Row();
			row.Reset(labelId);
			return row;
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x0018F54C File Offset: 0x0018D74C
		public void ReleaseRows(List<ProfilerTreeModel.Row> rows)
		{
			for (int i = 0; i < rows.Count; i++)
			{
				this.rowPool.Push(rows[i]);
			}
			rows.Clear();
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x0018F582 File Offset: 0x0018D782
		private List<ProfilerTreeModel.Row> RentRowList()
		{
			if (this.rowListPool.Count <= 0)
			{
				return new List<ProfilerTreeModel.Row>();
			}
			return this.rowListPool.Pop();
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x0018F5A3 File Offset: 0x0018D7A3
		private void ReturnRowList(List<ProfilerTreeModel.Row> list)
		{
			list.Clear();
			this.rowListPool.Push(list);
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06003EB8 RID: 16056 RVA: 0x0018F5B7 File Offset: 0x0018D7B7
		// (set) Token: 0x06003EB9 RID: 16057 RVA: 0x0018F5BF File Offset: 0x0018D7BF
		public int ExpansionVersion { get; private set; }

		// Token: 0x06003EBA RID: 16058 RVA: 0x0018F5C8 File Offset: 0x0018D7C8
		public void SetExpanded(ulong pathKey, bool expanded)
		{
			if (expanded ? this.expandedKeys.Add(pathKey) : this.expandedKeys.Remove(pathKey))
			{
				int expansionVersion = this.ExpansionVersion;
				this.ExpansionVersion = expansionVersion + 1;
			}
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x0018F604 File Offset: 0x0018D804
		public void ExpandAll()
		{
			this.expandAllPending = true;
			int expansionVersion = this.ExpansionVersion;
			this.ExpansionVersion = expansionVersion + 1;
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x0018F628 File Offset: 0x0018D828
		public void ExpandSubtree(ulong pathKey)
		{
			this.expandedKeys.Add(pathKey);
			this.subtreePendingKeys.Add(pathKey);
			int expansionVersion = this.ExpansionVersion;
			this.ExpansionVersion = expansionVersion + 1;
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x0018F660 File Offset: 0x0018D860
		public void CollapseAll()
		{
			this.expandAllPending = false;
			if (this.expandedKeys.Count == 0)
			{
				return;
			}
			this.expandedKeys.Clear();
			int expansionVersion = this.ExpansionVersion;
			this.ExpansionVersion = expansionVersion + 1;
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x0018F69D File Offset: 0x0018D89D
		private bool IsExpanded(ulong pathKey)
		{
			return this.expandAllPending || this.expandedKeys.Contains(pathKey);
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06003EBF RID: 16063 RVA: 0x0018F6B5 File Offset: 0x0018D8B5
		// (set) Token: 0x06003EC0 RID: 16064 RVA: 0x0018F6BD File Offset: 0x0018D8BD
		public bool Inverted { get; set; }

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06003EC1 RID: 16065 RVA: 0x0018F6C6 File Offset: 0x0018D8C6
		// (set) Token: 0x06003EC2 RID: 16066 RVA: 0x0018F6D0 File Offset: 0x0018D8D0
		public bool FoldRecursion
		{
			get
			{
				return this.foldRecursion;
			}
			set
			{
				if (this.foldRecursion == value)
				{
					return;
				}
				this.foldRecursion = value;
				int expansionVersion = this.ExpansionVersion;
				this.ExpansionVersion = expansionVersion + 1;
			}
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x0018F6FE File Offset: 0x0018D8FE
		public void BuildAllLevelRows()
		{
			this.Build(ProfilerTreeModel.View.Flamegraph);
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x0018F707 File Offset: 0x0018D907
		public void BuildTreeRows()
		{
			this.Build(ProfilerTreeModel.View.CallTree);
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x0018F710 File Offset: 0x0018D910
		public void BuildPieRows()
		{
			this.Build(ProfilerTreeModel.View.Pie);
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x0018F71C File Offset: 0x0018D91C
		private void Build(ProfilerTreeModel.View view)
		{
			this.ReleaseRows(this.DisplayRows);
			this.SubmitRequestIfChanged();
			using (ProfilerWindowAggregator.ResultLock.EnterScope())
			{
				this.EmittedRequest = (this.BeginEmit() ? this.result.request : null);
				if (this.EmittedRequest == null)
				{
					return;
				}
				int divisor = Math.Max(1, this.result.FramesIncluded);
				bool flag = this.Inverted && view != ProfilerTreeModel.View.Pie;
				if (flag)
				{
					this.EmitInverted(divisor, view == ProfilerTreeModel.View.CallTree, true);
				}
				else
				{
					this.CollectRoots(this.rootPathScratch);
					List<int> list = this.rootPathScratch;
					ulong parentKey = 14695981039346656037UL;
					ProfilerTreeModel.Descent descent = ProfilerTreeModel.Descent.All;
					if (view != ProfilerTreeModel.View.CallTree)
					{
						if (view == ProfilerTreeModel.View.Pie)
						{
							descent = ProfilerTreeModel.Descent.None;
							list = this.ResolveDrillPath(list, ref parentKey);
						}
					}
					else
					{
						descent = ProfilerTreeModel.Descent.Expanded;
					}
					this.EmitLevel(list, parentKey, 0, divisor, true, descent);
				}
				if (view == ProfilerTreeModel.View.CallTree)
				{
					this.ApplyBaseline(flag);
				}
			}
			this.expandAllPending = false;
			this.subtreePendingKeys.Clear();
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x0018F828 File Offset: 0x0018DA28
		public void BuildFullTreeRows(List<ProfilerTreeModel.Row> into)
		{
			using (ProfilerWindowAggregator.ResultLock.EnterScope())
			{
				if (this.BeginEmit())
				{
					int divisor = Math.Max(1, this.result.FramesIncluded);
					this.emitTarget = into;
					try
					{
						if (this.Inverted)
						{
							this.EmitInverted(divisor, false, false);
						}
						else
						{
							this.CollectRoots(this.rootPathScratch);
							this.EmitLevel(this.rootPathScratch, 14695981039346656037UL, 0, divisor, true, ProfilerTreeModel.Descent.Full);
						}
					}
					finally
					{
						this.emitTarget = this.DisplayRows;
					}
				}
			}
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x0018F8D0 File Offset: 0x0018DAD0
		private bool BeginEmit()
		{
			this.result = ProfilerWindowAggregator.Current;
			if (this.result.request == null)
			{
				return false;
			}
			this.pathTable = ProfilerPathTable.Entries;
			this.pathTableCount = Math.Min(ProfilerPathTable.Count, this.result.byPathId.Length);
			return true;
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x0018F920 File Offset: 0x0018DB20
		public void CollectMatchingPaths(ProfilerSearchModel search, [TupleElementNames(new string[]
		{
			"totalMicroseconds",
			"pathId"
		})] List<ValueTuple<float, int>> into)
		{
			this.CollectPaths(into, (ProfilerPathTable.Entry[] table, int pathId) => table[pathId].parentPathId != 0 && search.Matches(table[pathId].labelId));
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x0018F950 File Offset: 0x0018DB50
		public void CollectPathsWithLabel(ProfilerLabelId labelId, [TupleElementNames(new string[]
		{
			"totalMicroseconds",
			"pathId"
		})] List<ValueTuple<float, int>> into)
		{
			this.CollectPaths(into, (ProfilerPathTable.Entry[] table, int pathId) => table[pathId].labelId == labelId);
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x0018F980 File Offset: 0x0018DB80
		private void CollectPaths([TupleElementNames(new string[]
		{
			"totalMicroseconds",
			"pathId"
		})] List<ValueTuple<float, int>> into, Func<ProfilerPathTable.Entry[], int, bool> matches)
		{
			into.Clear();
			using (ProfilerWindowAggregator.ResultLock.EnterScope())
			{
				if (this.BeginEmit())
				{
					for (int i = 0; i < this.result.touchedCount; i++)
					{
						int num = this.result.touchedPathIds[i];
						if (this.Present(num) && matches(this.pathTable, num))
						{
							into.Add(new ValueTuple<float, int>((float)this.result.byPathId[num].totalMicroseconds, num));
						}
					}
				}
			}
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x0018FA24 File Offset: 0x0018DC24
		public void AppendLabelChain(int pathId, List<ProfilerLabelId> into)
		{
			into.Clear();
			ProfilerPathTable.Entry[] entries = ProfilerPathTable.Entries;
			if (pathId >= ProfilerPathTable.Count)
			{
				return;
			}
			if (this.foldRecursion)
			{
				pathId = entries[pathId].foldedPathId;
			}
			int num = pathId;
			while (entries[num].parentPathId != 0)
			{
				into.Add(entries[num].labelId);
				num = entries[num].parentPathId;
			}
			into.Reverse();
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x0018FA93 File Offset: 0x0018DC93
		private bool Present(int pathId)
		{
			return pathId < this.pathTableCount && this.result.IsListed(pathId) && this.result.byPathId[pathId].entries > 0;
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x0018FAC8 File Offset: 0x0018DCC8
		private void CollectRoots(List<int> into)
		{
			into.Clear();
			for (int i = this.pathTable[0].firstChild; i >= 0; i = this.pathTable[i].nextSibling)
			{
				if (this.Present(i))
				{
					into.Add(i);
				}
			}
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x0018FB18 File Offset: 0x0018DD18
		private bool HasPresentChild(List<int> parents)
		{
			for (int i = 0; i < parents.Count; i++)
			{
				for (int j = this.pathTable[parents[i]].firstChild; j >= 0; j = this.pathTable[j].nextSibling)
				{
					if (this.Present(j))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x0018FB74 File Offset: 0x0018DD74
		private List<int> ResolveDrillPath(List<int> roots, ref ulong key)
		{
			List<int> list = roots;
			for (int i = 0; i < this.Path.Count; i++)
			{
				ProfilerLabelId profilerLabelId = this.Path[i];
				List<int> list2 = (list == this.drillPathScratch) ? this.rootPathScratch : this.drillPathScratch;
				list2.Clear();
				for (int j = 0; j < list.Count; j++)
				{
					for (int k = this.pathTable[list[j]].firstChild; k >= 0; k = this.pathTable[k].nextSibling)
					{
						if (this.pathTable[k].labelId == profilerLabelId && this.Present(k))
						{
							list2.Add(k);
						}
					}
				}
				if (list2.Count == 0)
				{
					this.Path.RemoveRange(i, this.Path.Count - i);
					break;
				}
				list = list2;
				key = ProfilerTreeModel.CombineKey(key, profilerLabelId);
			}
			return list;
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0018FC70 File Offset: 0x0018DE70
		private int EmitLevel(List<int> parents, ulong parentKey, int depth, int divisor, bool unprofiled, ProfilerTreeModel.Descent descent)
		{
			List<ProfilerTreeModel.Row> list = this.RentRowList();
			double num = 0.0;
			long num2 = 0L;
			for (int i = 0; i < parents.Count; i++)
			{
				int num3 = parents[i];
				if (unprofiled)
				{
					ref PathAccumulator ptr = ref this.result.byPathId[num3];
					num += ptr.selfMicroseconds;
					num2 += ptr.selfAllocBytes;
				}
				for (int j = this.pathTable[num3].firstChild; j >= 0; j = this.pathTable[j].nextSibling)
				{
					if (this.Present(j))
					{
						ProfilerLabelId labelId = this.pathTable[j].labelId;
						int num4 = this.FindLevelRow(list, labelId);
						if (num4 < 0)
						{
							num4 = list.Count;
							ProfilerTreeModel.Row row = this.RentRow(labelId);
							row.pathKey = ProfilerTreeModel.CombineKey(parentKey, labelId);
							row.parentKey = parentKey;
							this.PropagateSubtreePending(parentKey, row.pathKey);
							list.Add(row);
							this.IndexLevelRow(list, labelId, num4);
						}
						ref PathAccumulator ptr2 = ref this.result.byPathId[j];
						ProfilerTreeModel.Row row2 = list[num4];
						row2.pathIds.Add(j);
						row2.totalMicroseconds += (float)ptr2.totalMicroseconds;
						row2.selfMicroseconds += (float)ptr2.selfMicroseconds;
						row2.calls += ptr2.calls;
						row2.allocBytes += ptr2.allocBytes;
					}
				}
			}
			if (unprofiled && (num > 0.0 || num2 > 0L))
			{
				ProfilerTreeModel.Row row3 = this.RentRow(ProfilerLabelId.None);
				row3.pathKey = ProfilerTreeModel.CombineKey(parentKey, ProfilerLabelId.None);
				row3.parentKey = parentKey;
				row3.totalMicroseconds = (float)num;
				row3.selfMicroseconds = (float)num;
				row3.allocBytes = num2;
				list.Add(row3);
			}
			this.DivideRows(list, divisor);
			list.Sort(this.rowComparison);
			int count = list.Count;
			for (int k = 0; k < count; k++)
			{
				ProfilerTreeModel.Row row4 = list[k];
				row4.depth = depth;
				this.emitTarget.Add(row4);
				if (row4.labelId.IsValid)
				{
					switch (descent)
					{
					case ProfilerTreeModel.Descent.None:
						row4.hasChildren = this.HasPresentChild(row4.pathIds);
						break;
					case ProfilerTreeModel.Descent.Expanded:
					case ProfilerTreeModel.Descent.Full:
						row4.hasChildren = this.HasPresentChild(row4.pathIds);
						if (row4.hasChildren && (descent == ProfilerTreeModel.Descent.Full || this.IsExpanded(row4.pathKey)))
						{
							this.MarkExpanded(row4);
							this.EmitLevel(row4.pathIds, row4.pathKey, depth + 1, divisor, true, descent);
						}
						break;
					case ProfilerTreeModel.Descent.All:
						if (this.EmitLevel(row4.pathIds, row4.pathKey, depth + 1, divisor, false, descent) > 0)
						{
							row4.hasChildren = true;
							this.MarkExpanded(row4);
						}
						break;
					}
				}
			}
			this.ReturnRowList(list);
			return count;
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x0018FF90 File Offset: 0x0018E190
		private int FindLevelRow(List<ProfilerTreeModel.Row> rows, ProfilerLabelId labelId)
		{
			if (rows.Count <= 16)
			{
				for (int i = 0; i < rows.Count; i++)
				{
					if (rows[i].labelId == labelId)
					{
						return i;
					}
				}
				return -1;
			}
			int num;
			if (!this.levelLookup.TryGetValue(labelId, out num))
			{
				return -1;
			}
			return num;
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x0018FFE4 File Offset: 0x0018E1E4
		private void IndexLevelRow(List<ProfilerTreeModel.Row> rows, ProfilerLabelId labelId, int rowIndex)
		{
			if (rows.Count <= 16)
			{
				return;
			}
			if (rows.Count == 17)
			{
				this.levelLookup.Clear();
				for (int i = 0; i < rows.Count; i++)
				{
					this.levelLookup.Add(rows[i].labelId, i);
				}
				return;
			}
			this.levelLookup.Add(labelId, rowIndex);
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x00190048 File Offset: 0x0018E248
		private void DivideRows(List<ProfilerTreeModel.Row> rows, int divisor)
		{
			if (divisor <= 1)
			{
				return;
			}
			for (int i = 0; i < rows.Count; i++)
			{
				ProfilerTreeModel.Row row = rows[i];
				row.totalMicroseconds /= (float)divisor;
				row.selfMicroseconds /= (float)divisor;
				row.calls = Mathf.RoundToInt((float)row.calls / (float)divisor);
				row.allocBytes /= (long)divisor;
			}
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x001900B3 File Offset: 0x0018E2B3
		private void PropagateSubtreePending(ulong parentKey, ulong pathKey)
		{
			if (this.subtreePendingKeys.Count == 0 || !this.subtreePendingKeys.Contains(parentKey))
			{
				return;
			}
			this.subtreePendingKeys.Add(pathKey);
			this.expandedKeys.Add(pathKey);
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x001900EB File Offset: 0x0018E2EB
		private void MarkExpanded(ProfilerTreeModel.Row row)
		{
			row.expanded = true;
			if (this.expandAllPending)
			{
				this.expandedKeys.Add(row.pathKey);
			}
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x00190110 File Offset: 0x0018E310
		public ulong ExpandTo(List<ProfilerLabelId> pathLabelIds)
		{
			ulong num = 14695981039346656037UL;
			for (int i = 0; i < pathLabelIds.Count; i++)
			{
				if (i > 0)
				{
					this.expandedKeys.Add(num);
				}
				num = ProfilerTreeModel.CombineKey(num, pathLabelIds[i]);
			}
			int expansionVersion = this.ExpansionVersion;
			this.ExpansionVersion = expansionVersion + 1;
			return num;
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x00190168 File Offset: 0x0018E368
		private ProfilerTreeModel.Row InvertedRowFor(List<ProfilerTreeModel.Row> rows, ulong key, ulong parentKey, ProfilerLabelId labelId)
		{
			ProfilerTreeModel.Row row;
			if (this.rowsByInvertedKey.TryGetValue(key, out row))
			{
				return row;
			}
			row = this.RentRow(labelId);
			row.pathKey = key;
			row.parentKey = parentKey;
			this.PropagateSubtreePending(parentKey, key);
			this.rowsByInvertedKey.Add(key, row);
			rows.Add(row);
			return row;
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x001901BC File Offset: 0x0018E3BC
		private static void AccumulateInverted(ProfilerTreeModel.Row row, in PathAccumulator leaf)
		{
			float num = (float)leaf.selfMicroseconds;
			row.totalMicroseconds += num;
			row.selfMicroseconds += num;
			row.allocBytes += leaf.selfAllocBytes;
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x00190200 File Offset: 0x0018E400
		private void EmitInverted(int divisor, bool gate, bool prune = true)
		{
			List<ProfilerTreeModel.Row> list = this.RentRowList();
			this.rowsByInvertedKey.Clear();
			List<ValueTuple<int, ProfilerTreeModel.Row, ulong, int>> list2 = this.invertedChains;
			list2.Clear();
			float num = 0f;
			for (int i = 0; i < this.result.touchedCount; i++)
			{
				int num2 = this.result.touchedPathIds[i];
				if (this.Present(num2))
				{
					ref PathAccumulator ptr = ref this.result.byPathId[num2];
					int parentPathId = this.pathTable[num2].parentPathId;
					if (parentPathId != 0 && (ptr.selfMicroseconds > 0.0 || ptr.selfAllocBytes > 0L))
					{
						ProfilerLabelId labelId = this.pathTable[num2].labelId;
						ulong num3 = ProfilerTreeModel.CombineKey(88172645463325252UL, labelId);
						ProfilerTreeModel.Row row = this.InvertedRowFor(list, num3, 88172645463325252UL, labelId);
						row.pathIds.Add(num2);
						ProfilerTreeModel.AccumulateInverted(row, ptr);
						row.calls += ptr.calls;
						num += (float)ptr.selfMicroseconds;
						list2.Add(new ValueTuple<int, ProfilerTreeModel.Row, ulong, int>(num2, row, num3, parentPathId));
					}
				}
			}
			float num4 = prune ? (num * 0.00024414062f) : 0f;
			List<ValueTuple<int, ProfilerTreeModel.Row, ulong, int>> list3 = this.invertedChainsNext;
			while (list2.Count > 0)
			{
				list3.Clear();
				for (int j = 0; j < list2.Count; j++)
				{
					ValueTuple<int, ProfilerTreeModel.Row, ulong, int> valueTuple = list2[j];
					int item = valueTuple.Item1;
					ProfilerTreeModel.Row item2 = valueTuple.Item2;
					ulong item3 = valueTuple.Item3;
					int item4 = valueTuple.Item4;
					if (item4 != 0)
					{
						if (gate)
						{
							item2.hasChildren = true;
							if (!this.IsExpanded(item2.pathKey))
							{
								goto IL_240;
							}
						}
						else if (item2.totalMicroseconds < num4)
						{
							goto IL_240;
						}
						ProfilerLabelId labelId2 = this.pathTable[item4].labelId;
						ulong num5 = ProfilerTreeModel.CombineKey(item3, labelId2);
						ProfilerTreeModel.Row row2 = this.InvertedRowFor(list, num5, item3, labelId2);
						row2.pathIds.Add(item);
						ProfilerTreeModel.AccumulateInverted(row2, this.result.byPathId[item]);
						item2.hasChildren = true;
						list3.Add(new ValueTuple<int, ProfilerTreeModel.Row, ulong, int>(item, row2, num5, this.pathTable[item4].parentPathId));
					}
					IL_240:;
				}
				List<ValueTuple<int, ProfilerTreeModel.Row, ulong, int>> list4 = list3;
				list3 = list2;
				list2 = list4;
			}
			this.invertedChains = list2;
			this.invertedChainsNext = list3;
			this.DivideRows(list, divisor);
			this.EmitPathRows(list, 88172645463325252UL);
			this.ReturnRowList(list);
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x001904A0 File Offset: 0x0018E6A0
		private void EmitPathRows(List<ProfilerTreeModel.Row> rows, ulong rootKey)
		{
			int count = rows.Count;
			if (this.siblingSortKeys.Length < count)
			{
				this.siblingSortKeys = new ulong[Math.Max(count, this.siblingSortKeys.Length * 2)];
				this.siblingSortRows = new ProfilerTreeModel.Row[this.siblingSortKeys.Length];
			}
			for (int i = 0; i < count; i++)
			{
				this.siblingSortKeys[i] = rows[i].parentKey;
				this.siblingSortRows[i] = rows[i];
			}
			Array.Sort<ulong, ProfilerTreeModel.Row>(this.siblingSortKeys, this.siblingSortRows, 0, count);
			this.childRangesByParentKey.Clear();
			int num2;
			for (int j = 0; j < count; j = num2)
			{
				ulong num = this.siblingSortKeys[j];
				num2 = j + 1;
				while (num2 < count && this.siblingSortKeys[num2] == num)
				{
					num2++;
				}
				if (num2 - j > 1)
				{
					this.siblingSortRows.AsSpan(j, num2 - j).Sort(this.rowComparison);
				}
				this.childRangesByParentKey.Add(num, new ValueTuple<int, int>(j, num2));
			}
			this.EmitPathLevel(this.siblingSortRows, rootKey, 0);
			Array.Clear(this.siblingSortRows, 0, count);
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x001905C0 File Offset: 0x0018E7C0
		private void EmitPathLevel(ProfilerTreeModel.Row[] rows, ulong parentKey, int depth)
		{
			ValueTuple<int, int> valueTuple;
			if (!this.childRangesByParentKey.TryGetValue(parentKey, out valueTuple))
			{
				return;
			}
			for (int i = valueTuple.Item1; i < valueTuple.Item2; i++)
			{
				ProfilerTreeModel.Row row = rows[i];
				row.depth = depth;
				this.emitTarget.Add(row);
				if (row.labelId.IsValid && this.childRangesByParentKey.ContainsKey(row.pathKey))
				{
					this.MarkExpanded(row);
					this.EmitPathLevel(rows, row.pathKey, depth + 1);
				}
			}
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x00190644 File Offset: 0x0018E844
		private void ApplyBaseline(bool inverted)
		{
			if (!this.result.hasBaseline)
			{
				return;
			}
			float[] array = inverted ? this.result.baselineSelfByPathId : this.result.baselineTotalByPathId;
			for (int i = 0; i < this.DisplayRows.Count; i++)
			{
				ProfilerTreeModel.Row row = this.DisplayRows[i];
				if (row.labelId.IsValid)
				{
					float num = 0f;
					bool flag = false;
					for (int j = 0; j < row.pathIds.Count; j++)
					{
						int num2 = row.pathIds[j];
						if (num2 < array.Length && !float.IsNaN(array[num2]))
						{
							num += array[num2];
							flag = true;
						}
					}
					row.baselineTotalMicroseconds = (flag ? num : float.NaN);
				}
			}
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x0019070E File Offset: 0x0018E90E
		public static string LabelForRow(ProfilerTreeModel.Row row)
		{
			if (!row.labelId.IsValid)
			{
				return "Unprofiled";
			}
			return FrameProfiler.GetLabel(row.labelId);
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x00190730 File Offset: 0x0018E930
		public static void AppendAllocBytes(StringBuilder builder, long bytes, string separator = " ")
		{
			if (bytes <= 0L)
			{
				return;
			}
			if (bytes < 1024L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted<long>(bytes);
				defaultInterpolatedStringHandler.AppendFormatted(separator);
				defaultInterpolatedStringHandler.AppendLiteral("B");
				builder.Append(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			if (bytes < 1048576L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler2.AppendFormatted<float>((float)bytes / 1024f, "0.0");
				defaultInterpolatedStringHandler2.AppendFormatted(separator);
				defaultInterpolatedStringHandler2.AppendLiteral("KB");
				builder.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler3.AppendFormatted<float>((float)bytes / 1048576f, "0.00");
			defaultInterpolatedStringHandler3.AppendFormatted(separator);
			defaultInterpolatedStringHandler3.AppendLiteral("MB");
			builder.Append(defaultInterpolatedStringHandler3.ToStringAndClear());
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x00190808 File Offset: 0x0018EA08
		public unsafe static void AppendMs(StringBuilder builder, float ms, string format = "0.0", string separator = " ")
		{
			float num = Mathf.Abs(ms);
			float num2 = ms;
			string value;
			if (num >= 1000f)
			{
				num2 = ms / 1000f;
				value = "s";
				format = "0.00";
			}
			else if (num >= 1f)
			{
				value = "ms";
			}
			else if (num >= 0.001f)
			{
				num2 = ms * 1000f;
				value = "µs";
				format = "0.0";
			}
			else if (num > 0f)
			{
				num2 = ms * 1000000f;
				value = "ns";
				format = ((num2 >= 10f || num2 <= -10f) ? "0" : "0.##");
			}
			else
			{
				value = "ms";
				format = "0";
			}
			Span<char> destination = new Span<char>(stackalloc byte[(UIntPtr)128], 64);
			int length;
			if (num2.TryFormat(destination, out length, format, null))
			{
				builder.Append(destination.Slice(0, length));
			}
			else
			{
				builder.Append(num2);
			}
			builder.Append(separator).Append(value);
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x00190900 File Offset: 0x0018EB00
		public static void AppendFrameMs(StringBuilder builder, float ms, string format = "0.0")
		{
			ProfilerTreeModel.AppendMs(builder, ms, format, " ");
			builder.Append(" (").Append((ms > 0f) ? Mathf.RoundToInt(1000f / ms) : 0).Append(" FPS)");
		}

		// Token: 0x04002958 RID: 10584
		public const ulong RootPathKey = 14695981039346656037UL;

		// Token: 0x0400295B RID: 10587
		private readonly Comparison<ProfilerTreeModel.Row> rowComparison;

		// Token: 0x0400295C RID: 10588
		public readonly List<ProfilerLabelId> Path = new List<ProfilerLabelId>();

		// Token: 0x0400295D RID: 10589
		public readonly List<ProfilerTreeModel.Row> DisplayRows = new List<ProfilerTreeModel.Row>();

		// Token: 0x0400295E RID: 10590
		private List<ProfilerTreeModel.Row> emitTarget;

		// Token: 0x0400295F RID: 10591
		public long clipStartNs = -1L;

		// Token: 0x04002960 RID: 10592
		public long clipEndNs = -1L;

		// Token: 0x04002961 RID: 10593
		private ProfilerWindowRequest pending = new ProfilerWindowRequest
		{
			followNewest = true,
			newestOrdinal = -1,
			oldestOrdinal = -1
		};

		// Token: 0x04002962 RID: 10594
		private ProfilerWindowRequest submitted;

		// Token: 0x04002964 RID: 10596
		private int[] submittedThreadIds;

		// Token: 0x04002965 RID: 10597
		private int submittedThreadVersion = -1;

		// Token: 0x04002966 RID: 10598
		private int submittedSelectionVersion = -1;

		// Token: 0x04002967 RID: 10599
		private readonly List<int> threadIdScratch = new List<int>();

		// Token: 0x04002968 RID: 10600
		private readonly Stack<ProfilerTreeModel.Row> rowPool = new Stack<ProfilerTreeModel.Row>();

		// Token: 0x04002969 RID: 10601
		private readonly Stack<List<ProfilerTreeModel.Row>> rowListPool = new Stack<List<ProfilerTreeModel.Row>>();

		// Token: 0x0400296A RID: 10602
		private readonly Dictionary<ProfilerLabelId, int> levelLookup = new Dictionary<ProfilerLabelId, int>();

		// Token: 0x0400296B RID: 10603
		private readonly HashSet<ulong> expandedKeys = new HashSet<ulong>();

		// Token: 0x0400296D RID: 10605
		private bool expandAllPending;

		// Token: 0x0400296E RID: 10606
		private readonly HashSet<ulong> subtreePendingKeys = new HashSet<ulong>();

		// Token: 0x0400296F RID: 10607
		public const ulong InvertedRootPathKey = 88172645463325252UL;

		// Token: 0x04002971 RID: 10609
		private bool foldRecursion = true;

		// Token: 0x04002972 RID: 10610
		private ProfilerWindowResult result;

		// Token: 0x04002973 RID: 10611
		private ProfilerPathTable.Entry[] pathTable = Array.Empty<ProfilerPathTable.Entry>();

		// Token: 0x04002974 RID: 10612
		private int pathTableCount;

		// Token: 0x04002975 RID: 10613
		private readonly List<int> rootPathScratch = new List<int>();

		// Token: 0x04002976 RID: 10614
		private readonly List<int> drillPathScratch = new List<int>();

		// Token: 0x04002977 RID: 10615
		private const int LevelLookupThreshold = 16;

		// Token: 0x04002978 RID: 10616
		private const float InvertedMinRowFraction = 0.00024414062f;

		// Token: 0x04002979 RID: 10617
		private readonly Dictionary<ulong, ProfilerTreeModel.Row> rowsByInvertedKey = new Dictionary<ulong, ProfilerTreeModel.Row>();

		// Token: 0x0400297A RID: 10618
		[TupleElementNames(new string[]
		{
			"start",
			"end"
		})]
		private readonly Dictionary<ulong, ValueTuple<int, int>> childRangesByParentKey = new Dictionary<ulong, ValueTuple<int, int>>();

		// Token: 0x0400297B RID: 10619
		[TupleElementNames(new string[]
		{
			"leafPathId",
			"row",
			"key",
			"callerPathId"
		})]
		private List<ValueTuple<int, ProfilerTreeModel.Row, ulong, int>> invertedChains = new List<ValueTuple<int, ProfilerTreeModel.Row, ulong, int>>();

		// Token: 0x0400297C RID: 10620
		[TupleElementNames(new string[]
		{
			"leafPathId",
			"row",
			"key",
			"callerPathId"
		})]
		private List<ValueTuple<int, ProfilerTreeModel.Row, ulong, int>> invertedChainsNext = new List<ValueTuple<int, ProfilerTreeModel.Row, ulong, int>>();

		// Token: 0x0400297D RID: 10621
		private ulong[] siblingSortKeys = Array.Empty<ulong>();

		// Token: 0x0400297E RID: 10622
		private ProfilerTreeModel.Row[] siblingSortRows = Array.Empty<ProfilerTreeModel.Row>();

		// Token: 0x0400297F RID: 10623
		public const string UnprofiledLabel = "Unprofiled";

		// Token: 0x02002AA6 RID: 10918
		public enum SortColumn
		{
			// Token: 0x0400AEBE RID: 44734
			Total,
			// Token: 0x0400AEBF RID: 44735
			Self,
			// Token: 0x0400AEC0 RID: 44736
			Calls,
			// Token: 0x0400AEC1 RID: 44737
			Alloc,
			// Token: 0x0400AEC2 RID: 44738
			Label
		}

		// Token: 0x02002AA7 RID: 10919
		public sealed class Row
		{
			// Token: 0x17002BEB RID: 11243
			// (get) Token: 0x06010726 RID: 67366 RVA: 0x005C82FF File Offset: 0x005C64FF
			public bool CanDrill
			{
				get
				{
					return this.labelId.IsValid;
				}
			}

			// Token: 0x17002BEC RID: 11244
			// (get) Token: 0x06010727 RID: 67367 RVA: 0x005C830C File Offset: 0x005C650C
			public bool HasBaseline
			{
				get
				{
					return !float.IsNaN(this.baselineTotalMicroseconds);
				}
			}

			// Token: 0x06010728 RID: 67368 RVA: 0x005C831C File Offset: 0x005C651C
			internal void Reset(ProfilerLabelId labelId)
			{
				this.labelId = labelId;
				this.depth = 0;
				this.pathKey = 0UL;
				this.totalMicroseconds = 0f;
				this.selfMicroseconds = 0f;
				this.calls = 0;
				this.allocBytes = 0L;
				this.hasChildren = false;
				this.expanded = false;
				this.baselineTotalMicroseconds = float.NaN;
				this.parentKey = 0UL;
				this.pathIds.Clear();
			}

			// Token: 0x0400AEC3 RID: 44739
			public ProfilerLabelId labelId;

			// Token: 0x0400AEC4 RID: 44740
			public int depth;

			// Token: 0x0400AEC5 RID: 44741
			public ulong pathKey;

			// Token: 0x0400AEC6 RID: 44742
			public float totalMicroseconds;

			// Token: 0x0400AEC7 RID: 44743
			public float selfMicroseconds;

			// Token: 0x0400AEC8 RID: 44744
			public int calls;

			// Token: 0x0400AEC9 RID: 44745
			public long allocBytes;

			// Token: 0x0400AECA RID: 44746
			public bool hasChildren;

			// Token: 0x0400AECB RID: 44747
			public bool expanded;

			// Token: 0x0400AECC RID: 44748
			public float baselineTotalMicroseconds = float.NaN;

			// Token: 0x0400AECD RID: 44749
			internal ulong parentKey;

			// Token: 0x0400AECE RID: 44750
			public readonly List<int> pathIds = new List<int>();
		}

		// Token: 0x02002AA8 RID: 10920
		public struct WindowSummary
		{
			// Token: 0x0400AECF RID: 44751
			public int framesIncluded;

			// Token: 0x0400AED0 RID: 44752
			public float averageFrameMs;

			// Token: 0x0400AED1 RID: 44753
			public float averageTickerDelta;

			// Token: 0x0400AED2 RID: 44754
			public float averageNodeCount;

			// Token: 0x0400AED3 RID: 44755
			public float averageOverflowedScopes;

			// Token: 0x0400AED4 RID: 44756
			public long averageAllocBytes;

			// Token: 0x0400AED5 RID: 44757
			public float averageAllocCount;

			// Token: 0x0400AED6 RID: 44758
			public int gen0Collections;

			// Token: 0x0400AED7 RID: 44759
			public int gen1Collections;

			// Token: 0x0400AED8 RID: 44760
			public int gen2Collections;

			// Token: 0x0400AED9 RID: 44761
			public long averagePromotedBytes;
		}

		// Token: 0x02002AA9 RID: 10921
		private enum View
		{
			// Token: 0x0400AEDB RID: 44763
			Flamegraph,
			// Token: 0x0400AEDC RID: 44764
			CallTree,
			// Token: 0x0400AEDD RID: 44765
			Pie
		}

		// Token: 0x02002AAA RID: 10922
		private enum Descent
		{
			// Token: 0x0400AEDF RID: 44767
			None,
			// Token: 0x0400AEE0 RID: 44768
			Expanded,
			// Token: 0x0400AEE1 RID: 44769
			Full,
			// Token: 0x0400AEE2 RID: 44770
			All
		}
	}
}
