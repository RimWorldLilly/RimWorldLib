using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001F4 RID: 500
	public class PlayLog : IExposable
	{
		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x0004A249 File Offset: 0x00048449
		public List<LogEntry> AllEntries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0004A251 File Offset: 0x00048451
		public int LastTick
		{
			get
			{
				if (this.entries.Count == 0)
				{
					return 0;
				}
				return this.entries[0].Tick;
			}
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x0004A273 File Offset: 0x00048473
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			this.ReduceToCapacity();
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x0004A288 File Offset: 0x00048488
		private void ReduceToCapacity()
		{
			while (this.entries.Count > 150)
			{
				this.RemoveEntry(this.entries[this.entries.Count - 1]);
			}
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0004A2BC File Offset: 0x000484BC
		public void ExposeData()
		{
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0004A2D4 File Offset: 0x000484D4
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.entries.Count - 1; i >= 0; i--)
			{
				if (this.entries[i].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						string[] array = new string[5];
						array[0] = "Discarding pawn ";
						array[1] = ((p != null) ? p.ToString() : null);
						array[2] = ", but he is referenced by a play log entry ";
						int num = 3;
						LogEntry logEntry = this.entries[i];
						array[num] = ((logEntry != null) ? logEntry.ToString() : null);
						array[4] = ".";
						Log.Warning(string.Concat(array));
					}
					this.RemoveEntry(this.entries[i]);
				}
			}
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0004A37C File Offset: 0x0004857C
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0004A3B4 File Offset: 0x000485B4
		public void Notify_IdeoRemoved(Ideo ideo)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].Notify_IdeoRemoved(ideo);
			}
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0004A3E9 File Offset: 0x000485E9
		private void RemoveEntry(LogEntry entry)
		{
			this.entries.Remove(entry);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0004A3F8 File Offset: 0x000485F8
		public bool AnyEntryConcerns(Pawn p)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000E37 RID: 3639
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04000E38 RID: 3640
		private const int Capacity = 150;
	}
}
