using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001029 RID: 4137
	public class Archive : IExposable
	{
		// Token: 0x170011DD RID: 4573
		// (get) Token: 0x06006833 RID: 26675 RVA: 0x0024DA20 File Offset: 0x0024BC20
		public List<IArchivable> ArchivablesListForReading
		{
			get
			{
				return this.archivables;
			}
		}

		// Token: 0x06006834 RID: 26676 RVA: 0x0024DA28 File Offset: 0x0024BC28
		public void ExposeData()
		{
			Scribe_Collections.Look<IArchivable>(ref this.archivables, "archivables", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<IArchivable>(ref this.pinnedArchivables, "pinnedArchivables", LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.archivables.RemoveAll((IArchivable x) => x == null);
				this.pinnedArchivables.RemoveWhere((IArchivable x) => x == null);
			}
		}

		// Token: 0x06006835 RID: 26677 RVA: 0x0024DABC File Offset: 0x0024BCBC
		public bool Add(IArchivable archivable)
		{
			if (archivable == null)
			{
				Log.Error("Tried to add null archivable.");
				return false;
			}
			if (this.Contains(archivable))
			{
				return false;
			}
			this.archivables.Add(archivable);
			this.archivables.SortBy((IArchivable x) => x.CreatedTicksGame);
			this.CheckCullArchivables();
			return true;
		}

		// Token: 0x06006836 RID: 26678 RVA: 0x0024DB20 File Offset: 0x0024BD20
		public bool Remove(IArchivable archivable)
		{
			if (!this.Contains(archivable))
			{
				return false;
			}
			this.archivables.Remove(archivable);
			this.pinnedArchivables.Remove(archivable);
			return true;
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x0024DB48 File Offset: 0x0024BD48
		public bool Contains(IArchivable archivable)
		{
			return this.archivables.Contains(archivable);
		}

		// Token: 0x06006838 RID: 26680 RVA: 0x0024DB56 File Offset: 0x0024BD56
		public void Pin(IArchivable archivable)
		{
			if (!this.Contains(archivable))
			{
				return;
			}
			if (this.IsPinned(archivable))
			{
				return;
			}
			this.pinnedArchivables.Add(archivable);
		}

		// Token: 0x06006839 RID: 26681 RVA: 0x0024DB79 File Offset: 0x0024BD79
		public void Unpin(IArchivable archivable)
		{
			if (!this.Contains(archivable))
			{
				return;
			}
			if (!this.IsPinned(archivable))
			{
				return;
			}
			this.pinnedArchivables.Remove(archivable);
		}

		// Token: 0x0600683A RID: 26682 RVA: 0x0024DB9C File Offset: 0x0024BD9C
		public bool IsPinned(IArchivable archivable)
		{
			return this.pinnedArchivables.Contains(archivable);
		}

		// Token: 0x0600683B RID: 26683 RVA: 0x0024DBAC File Offset: 0x0024BDAC
		private void CheckCullArchivables()
		{
			int num = 0;
			for (int i = 0; i < this.archivables.Count; i++)
			{
				if (!this.IsPinned(this.archivables[i]) && this.archivables[i].CanCullArchivedNow)
				{
					num++;
				}
			}
			int num2 = num - 200;
			int num3 = 0;
			while (num3 < this.archivables.Count && num2 > 0)
			{
				if (!this.IsPinned(this.archivables[num3]) && this.archivables[num3].CanCullArchivedNow && this.Remove(this.archivables[num3]))
				{
					num2--;
					num3--;
				}
				num3++;
			}
		}

		// Token: 0x0600683C RID: 26684 RVA: 0x0024DC64 File Offset: 0x0024BE64
		public void Notify_MapRemoved(Map map)
		{
			for (int i = 0; i < this.archivables.Count; i++)
			{
				LookTargets lookTargets = this.archivables[i].LookTargets;
				if (lookTargets != null)
				{
					lookTargets.Notify_MapRemoved(map);
				}
			}
		}

		// Token: 0x0400425F RID: 16991
		private List<IArchivable> archivables = new List<IArchivable>();

		// Token: 0x04004260 RID: 16992
		private HashSet<IArchivable> pinnedArchivables = new HashSet<IArchivable>();

		// Token: 0x04004261 RID: 16993
		public const int MaxNonPinnedArchivables = 200;
	}
}
