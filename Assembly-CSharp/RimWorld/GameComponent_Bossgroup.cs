using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002026 RID: 8230
	public class GameComponent_Bossgroup : GameComponent
	{
		// Token: 0x1700257B RID: 9595
		// (get) Token: 0x0600D058 RID: 53336 RVA: 0x004C957B File Offset: 0x004C777B
		public List<BossDef> AllBosses
		{
			get
			{
				if (this.allBosses.NullOrEmpty<BossDef>())
				{
					this.allBosses = new List<BossDef>();
					this.allBosses.AddRange(DefDatabase<BossDef>.AllDefs);
				}
				return this.allBosses;
			}
		}

		// Token: 0x0600D059 RID: 53337 RVA: 0x004C95AB File Offset: 0x004C77AB
		public GameComponent_Bossgroup(Game game)
		{
		}

		// Token: 0x0600D05A RID: 53338 RVA: 0x004C95D4 File Offset: 0x004C77D4
		public bool ReservedByBossgroup(PawnKindDef bossgroupKidDef)
		{
			BossDef bossForKind = this.GetBossForKind(bossgroupKidDef);
			if (bossForKind != null)
			{
				if (bossForKind.appearAfterTicks < Find.TickManager.TicksGame)
				{
					return false;
				}
				if (!this.IsDefeated(bossForKind))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D05B RID: 53339 RVA: 0x004C960C File Offset: 0x004C780C
		public bool IsDefeated(BossDef bossDef)
		{
			return this.killedBosses.Contains(bossDef);
		}

		// Token: 0x0600D05C RID: 53340 RVA: 0x004C961C File Offset: 0x004C781C
		private BossDef GetBossForKind(PawnKindDef kindDef)
		{
			return this.AllBosses.FirstOrDefault((BossDef b) => b.kindDef == kindDef);
		}

		// Token: 0x0600D05D RID: 53341 RVA: 0x004C964D File Offset: 0x004C784D
		public int NumTimesCalledBossgroup(BossgroupDef bossgroupDef)
		{
			if (!this.timesCalledBossgroups.ContainsKey(bossgroupDef))
			{
				this.timesCalledBossgroups.Add(bossgroupDef, 0);
			}
			return this.timesCalledBossgroups[bossgroupDef];
		}

		// Token: 0x0600D05E RID: 53342 RVA: 0x004C9676 File Offset: 0x004C7876
		public void ResetProgress()
		{
			this.timesCalledBossgroups.Clear();
		}

		// Token: 0x0600D05F RID: 53343 RVA: 0x004C9684 File Offset: 0x004C7884
		public void Notify_BossgroupCalled(BossgroupDef bossgroupDef)
		{
			this.lastBossgroupCalled = Find.TickManager.TicksGame;
			if (this.timesCalledBossgroups.ContainsKey(bossgroupDef))
			{
				Dictionary<BossgroupDef, int> dictionary = this.timesCalledBossgroups;
				int num = dictionary[bossgroupDef];
				dictionary[bossgroupDef] = num + 1;
				return;
			}
			this.timesCalledBossgroups.Add(bossgroupDef, 0);
		}

		// Token: 0x0600D060 RID: 53344 RVA: 0x004C96D8 File Offset: 0x004C78D8
		public virtual void Notify_PawnKilled(Pawn pawn)
		{
			BossDef bossForKind = this.GetBossForKind(pawn.kindDef);
			if (bossForKind != null && !this.killedBosses.Contains(bossForKind))
			{
				this.killedBosses.Add(bossForKind);
			}
		}

		// Token: 0x0600D061 RID: 53345 RVA: 0x004C970F File Offset: 0x004C790F
		public void DebugResetDefeatedPawns()
		{
			this.killedBosses.Clear();
		}

		// Token: 0x0600D062 RID: 53346 RVA: 0x004C971C File Offset: 0x004C791C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastBossgroupCalled, "lastBossgroupCalled", -9999999, false);
			Scribe_Collections.Look<BossDef>(ref this.killedBosses, "killedBosses", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<BossgroupDef, int>(ref this.timesCalledBossgroups, "timesCalledBossgroups", LookMode.Def, LookMode.Value, ref this.bossgroupTmp, ref this.calledTmp, true, false, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				HashSet<PawnKindDef> hashSet = null;
				Scribe_Collections.Look<PawnKindDef>(ref hashSet, "killedBossgroupMechs", LookMode.Undefined);
				if (hashSet != null)
				{
					if (this.killedBosses == null)
					{
						this.killedBosses = new List<BossDef>();
					}
					foreach (PawnKindDef kindDef in hashSet)
					{
						BossDef bossForKind = this.GetBossForKind(kindDef);
						if (bossForKind != null && !this.killedBosses.Contains(bossForKind))
						{
							this.killedBosses.Add(bossForKind);
						}
					}
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.timesCalledBossgroups == null)
				{
					this.timesCalledBossgroups = new Dictionary<BossgroupDef, int>();
				}
				if (this.killedBosses == null)
				{
					this.killedBosses = new List<BossDef>();
				}
			}
		}

		// Token: 0x0400849D RID: 33949
		public int lastBossgroupCalled = -9999999;

		// Token: 0x0400849E RID: 33950
		private Dictionary<BossgroupDef, int> timesCalledBossgroups = new Dictionary<BossgroupDef, int>();

		// Token: 0x0400849F RID: 33951
		private List<BossDef> allBosses;

		// Token: 0x040084A0 RID: 33952
		private List<BossDef> killedBosses = new List<BossDef>();

		// Token: 0x040084A1 RID: 33953
		private List<BossgroupDef> bossgroupTmp;

		// Token: 0x040084A2 RID: 33954
		private List<int> calledTmp;
	}
}
