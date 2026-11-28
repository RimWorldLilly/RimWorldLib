using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001DF RID: 479
	public class BattleLog : IExposable
	{
		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x00044A23 File Offset: 0x00042C23
		public List<Battle> Battles
		{
			get
			{
				return this.battles;
			}
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00044A2C File Offset: 0x00042C2C
		public void Add(LogEntry entry)
		{
			Battle battle = null;
			foreach (Thing thing in entry.GetConcerns())
			{
				Battle battleActive = ((Pawn)thing).records.BattleActive;
				if (battle == null)
				{
					battle = battleActive;
				}
				else if (battleActive != null)
				{
					battle = ((battle.Importance > battleActive.Importance) ? battle : battleActive);
				}
			}
			if (battle == null)
			{
				battle = Battle.Create();
				this.battles.Insert(0, battle);
			}
			foreach (Thing thing2 in entry.GetConcerns())
			{
				Pawn pawn = (Pawn)thing2;
				Battle battleActive2 = pawn.records.BattleActive;
				if (battleActive2 != null && battleActive2 != battle)
				{
					battle.Absorb(battleActive2);
					this.battles.Remove(battleActive2);
				}
				pawn.records.EnterBattle(battle);
			}
			battle.Add(entry);
			this.cachedActiveEntries = null;
			this.ReduceToCapacity();
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00044B38 File Offset: 0x00042D38
		private void ReduceToCapacity()
		{
			int num = this.battles.Count((Battle btl) => btl.AbsorbedBy == null);
			while (num > 20 && this.battles[this.battles.Count - 1].LastEntryTimestamp + Mathf.Max(420000, 5000) < Find.TickManager.TicksGame)
			{
				if (this.battles[this.battles.Count - 1].AbsorbedBy == null)
				{
					num--;
				}
				this.battles.RemoveAt(this.battles.Count - 1);
				this.cachedActiveEntries = null;
			}
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00044BF2 File Offset: 0x00042DF2
		public void ExposeData()
		{
			Scribe_Collections.Look<Battle>(ref this.battles, "battles", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.battles == null)
			{
				this.battles = new List<Battle>();
			}
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00044C28 File Offset: 0x00042E28
		public bool AnyEntryConcerns(Pawn p)
		{
			for (int i = 0; i < this.battles.Count; i++)
			{
				if (this.battles[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00044C64 File Offset: 0x00042E64
		public bool IsEntryActive(LogEntry log)
		{
			if (this.cachedActiveEntries == null)
			{
				this.cachedActiveEntries = new HashSet<LogEntry>();
				for (int i = 0; i < this.battles.Count; i++)
				{
					List<LogEntry> entries = this.battles[i].Entries;
					for (int j = 0; j < entries.Count; j++)
					{
						this.cachedActiveEntries.Add(entries[j]);
					}
				}
			}
			return this.cachedActiveEntries.Contains(log);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00044CDC File Offset: 0x00042EDC
		public void RemoveEntry(LogEntry log)
		{
			int num = 0;
			while (num < this.battles.Count && !this.battles[num].Entries.Remove(log))
			{
				num++;
			}
			this.cachedActiveEntries = null;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00044D20 File Offset: 0x00042F20
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.battles.Count - 1; i >= 0; i--)
			{
				this.battles[i].Notify_PawnDiscarded(p, silentlyRemoveReferences);
			}
			this.cachedActiveEntries = null;
		}

		// Token: 0x04000D84 RID: 3460
		private List<Battle> battles = new List<Battle>();

		// Token: 0x04000D85 RID: 3461
		private const int BattleHistoryLength = 20;

		// Token: 0x04000D86 RID: 3462
		private HashSet<LogEntry> cachedActiveEntries;
	}
}
