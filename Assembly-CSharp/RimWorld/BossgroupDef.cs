using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F46 RID: 3910
	public class BossgroupDef : Def
	{
		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x060064C8 RID: 25800 RVA: 0x0023FA54 File Offset: 0x0023DC54
		public BossgroupWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (BossgroupWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x060064C9 RID: 25801 RVA: 0x0023FA94 File Offset: 0x0023DC94
		public string LeaderDescription
		{
			get
			{
				return this.leaderDescription.Formatted(this.boss.kindDef.Named("LEADERKIND"));
			}
		}

		// Token: 0x060064CA RID: 25802 RVA: 0x0023FABC File Offset: 0x0023DCBC
		public int GetWaveIndex(int timesSummoned)
		{
			int num = timesSummoned;
			if (num >= this.waves.Count)
			{
				Rand.PushState(Gen.HashCombine<int>(Find.World.info.Seed, timesSummoned));
				num = Rand.Range(this.repeatWaveStartIndex, this.waves.Count);
				Rand.PopState();
			}
			return num;
		}

		// Token: 0x060064CB RID: 25803 RVA: 0x0023FB10 File Offset: 0x0023DD10
		public BossGroupWave GetWave(int index)
		{
			return this.waves[this.GetWaveIndex(index)];
		}

		// Token: 0x060064CC RID: 25804 RVA: 0x0023FB24 File Offset: 0x0023DD24
		public string GetWaveDescription(int waveIndex)
		{
			BossGroupWave wave = this.GetWave(this.GetWaveIndex(waveIndex));
			this.tmpEntries.Clear();
			string text = GenLabel.BestKindLabel(this.boss.kindDef, Gender.None, false, -1).CapitalizeFirst();
			if (!wave.bossApparel.NullOrEmpty<ThingDef>())
			{
				text = "BossWithApparel".Translate(text.Named("BOSS"), (from a in wave.bossApparel
				select a.label).ToCommaList(true, false).Named("APPAREL"));
			}
			this.tmpEntries.Add(text + " x" + wave.bossCount.ToString());
			foreach (PawnKindDefCount pawnKindDefCount in wave.escorts)
			{
				this.tmpEntries.Add(GenLabel.BestKindLabel(pawnKindDefCount.kindDef, Gender.None, false, -1).CapitalizeFirst() + " x" + pawnKindDefCount.count.ToString());
			}
			return this.tmpEntries.ToLineList("  - ");
		}

		// Token: 0x060064CD RID: 25805 RVA: 0x0023FC6C File Offset: 0x0023DE6C
		public override IEnumerable<string> ConfigErrors()
		{
			BossgroupDef.<ConfigErrors>d__16 <ConfigErrors>d__ = new BossgroupDef.<ConfigErrors>d__16(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003A55 RID: 14933
		public BossDef boss;

		// Token: 0x04003A56 RID: 14934
		public List<BossGroupWave> waves = new List<BossGroupWave>();

		// Token: 0x04003A57 RID: 14935
		public int repeatWaveStartIndex;

		// Token: 0x04003A58 RID: 14936
		public Type workerClass = typeof(BossgroupWorker);

		// Token: 0x04003A59 RID: 14937
		[MustTranslate]
		public string leaderDescription;

		// Token: 0x04003A5A RID: 14938
		public QuestScriptDef quest;

		// Token: 0x04003A5B RID: 14939
		public ThingDef rewardDef;

		// Token: 0x04003A5C RID: 14940
		private BossgroupWorker workerInt;

		// Token: 0x04003A5D RID: 14941
		private List<string> tmpEntries = new List<string>();
	}
}
