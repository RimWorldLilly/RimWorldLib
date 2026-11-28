using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001DE RID: 478
	public class Battle : IExposable, ILoadReferenceable
	{
		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x000444A7 File Offset: 0x000426A7
		public int Importance
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x000444B4 File Offset: 0x000426B4
		public int CreationTimestamp
		{
			get
			{
				return this.creationTimestamp;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x000444BC File Offset: 0x000426BC
		public int LastEntryTimestamp
		{
			get
			{
				if (this.entries.Count <= 0)
				{
					return 0;
				}
				return this.entries[this.entries.Count - 1].Timestamp;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x000444EB File Offset: 0x000426EB
		public Battle AbsorbedBy
		{
			get
			{
				return this.absorbedBy;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x000444F3 File Offset: 0x000426F3
		public List<LogEntry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00044519 File Offset: 0x00042719
		public static Battle Create()
		{
			return new Battle
			{
				loadID = Find.UniqueIDsManager.GetNextBattleID(),
				creationTimestamp = Find.TickManager.TicksGame
			};
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00044540 File Offset: 0x00042740
		public string GetName()
		{
			if (this.battleName.NullOrEmpty())
			{
				HashSet<Faction> hashSet = new HashSet<Faction>(from p in this.concerns
				select p.Faction);
				GrammarRequest request = default(GrammarRequest);
				if (this.concerns.Count == 1)
				{
					if (hashSet.Count((Faction f) => f != null) < 2)
					{
						request.Includes.Add(RulePackDefOf.Battle_Solo);
						request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT1", this.concerns.First<Pawn>(), null, true, true));
						goto IL_1D9;
					}
				}
				if (this.concerns.Count == 2)
				{
					request.Includes.Add(RulePackDefOf.Battle_Duel);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT1", this.concerns.First<Pawn>(), null, true, true));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT2", this.concerns.Last<Pawn>(), null, true, true));
				}
				else if (hashSet.Count == 1)
				{
					request.Includes.Add(RulePackDefOf.Battle_Internal);
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION1", hashSet.First<Faction>(), request.Constants, true));
				}
				else if (hashSet.Count == 2)
				{
					request.Includes.Add(RulePackDefOf.Battle_War);
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION1", hashSet.First<Faction>(), request.Constants, true));
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION2", hashSet.Last<Faction>(), request.Constants, true));
				}
				else
				{
					request.Includes.Add(RulePackDefOf.Battle_Brawl);
				}
				IL_1D9:
				this.battleName = GrammarResolver.Resolve("r_battlename", request, null, false, null, null, null, true);
			}
			return this.battleName;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00044744 File Offset: 0x00042944
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			foreach (Thing thing in entry.GetConcerns())
			{
				if (thing is Pawn)
				{
					this.concerns.Add(thing as Pawn);
				}
			}
			this.battleName = null;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x000447B8 File Offset: 0x000429B8
		public void Absorb(Battle battle)
		{
			this.creationTimestamp = Mathf.Min(this.creationTimestamp, battle.creationTimestamp);
			this.entries.AddRange(battle.entries);
			this.concerns.AddRange(battle.concerns);
			this.entries = (from e in this.entries
			orderby e.Age
			select e).ToList<LogEntry>();
			battle.entries.Clear();
			battle.concerns.Clear();
			battle.absorbedBy = this;
			this.battleName = null;
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00044857 File Offset: 0x00042A57
		public bool Concerns(Pawn pawn)
		{
			return this.concerns.Contains(pawn);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00044868 File Offset: 0x00042A68
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			if (!this.concerns.Contains(p))
			{
				return;
			}
			for (int i = this.entries.Count - 1; i >= 0; i--)
			{
				if (this.entries[i].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						string[] array = new string[5];
						array[0] = "Discarding pawn ";
						array[1] = ((p != null) ? p.ToString() : null);
						array[2] = ", but he is referenced by a battle log entry ";
						int num = 3;
						LogEntry logEntry = this.entries[i];
						array[num] = ((logEntry != null) ? logEntry.ToString() : null);
						array[4] = ".";
						Log.Warning(string.Concat(array));
					}
					this.entries.RemoveAt(i);
				}
			}
			this.concerns.Remove(p);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00044920 File Offset: 0x00042B20
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.creationTimestamp, "creationTimestamp", 0, false);
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<Battle>(ref this.absorbedBy, "absorbedBy", false);
			Scribe_Values.Look<string>(ref this.battleName, "battleName", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.concerns.Clear();
				foreach (Pawn item in this.entries.SelectMany((LogEntry e) => e.GetConcerns()).OfType<Pawn>())
				{
					this.concerns.Add(item);
				}
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00044A0C File Offset: 0x00042C0C
		public string GetUniqueLoadID()
		{
			return "Battle_" + this.loadID.ToString();
		}

		// Token: 0x04000D7D RID: 3453
		public const int TicksForBattleExit = 5000;

		// Token: 0x04000D7E RID: 3454
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04000D7F RID: 3455
		private string battleName;

		// Token: 0x04000D80 RID: 3456
		private Battle absorbedBy;

		// Token: 0x04000D81 RID: 3457
		private HashSet<Pawn> concerns = new HashSet<Pawn>();

		// Token: 0x04000D82 RID: 3458
		private int loadID;

		// Token: 0x04000D83 RID: 3459
		private int creationTimestamp;
	}
}
