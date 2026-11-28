using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001507 RID: 5383
	public class FactionPermit : IExposable
	{
		// Token: 0x17001661 RID: 5729
		// (get) Token: 0x060083D3 RID: 33747 RVA: 0x002F65BF File Offset: 0x002F47BF
		public RoyalTitleDef Title
		{
			get
			{
				return this.title;
			}
		}

		// Token: 0x17001662 RID: 5730
		// (get) Token: 0x060083D4 RID: 33748 RVA: 0x002F65C7 File Offset: 0x002F47C7
		public RoyalTitlePermitDef Permit
		{
			get
			{
				return this.permit;
			}
		}

		// Token: 0x17001663 RID: 5731
		// (get) Token: 0x060083D5 RID: 33749 RVA: 0x002F65CF File Offset: 0x002F47CF
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x17001664 RID: 5732
		// (get) Token: 0x060083D6 RID: 33750 RVA: 0x002F65D7 File Offset: 0x002F47D7
		public int LastUsedTick
		{
			get
			{
				return this.lastUsedTick;
			}
		}

		// Token: 0x17001665 RID: 5733
		// (get) Token: 0x060083D7 RID: 33751 RVA: 0x002F65DF File Offset: 0x002F47DF
		public bool OnCooldown
		{
			get
			{
				return this.LastUsedTick > 0 && Find.TickManager.TicksGame < this.LastUsedTick + this.permit.CooldownTicks;
			}
		}

		// Token: 0x060083D8 RID: 33752 RVA: 0x002F660A File Offset: 0x002F480A
		public FactionPermit()
		{
		}

		// Token: 0x060083D9 RID: 33753 RVA: 0x002F6619 File Offset: 0x002F4819
		public FactionPermit(Faction faction, RoyalTitleDef title, RoyalTitlePermitDef permit)
		{
			this.title = title;
			this.faction = faction;
			this.permit = permit;
		}

		// Token: 0x060083DA RID: 33754 RVA: 0x002F663D File Offset: 0x002F483D
		public void Notify_Used()
		{
			this.lastUsedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060083DB RID: 33755 RVA: 0x002F664F File Offset: 0x002F484F
		public void ResetCooldown()
		{
			this.lastUsedTick = -1;
		}

		// Token: 0x060083DC RID: 33756 RVA: 0x002F6658 File Offset: 0x002F4858
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.title, "title");
			Scribe_Defs.Look<RoyalTitlePermitDef>(ref this.permit, "permit");
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", -1, false);
		}

		// Token: 0x0400517A RID: 20858
		private RoyalTitleDef title;

		// Token: 0x0400517B RID: 20859
		private RoyalTitlePermitDef permit;

		// Token: 0x0400517C RID: 20860
		private Faction faction;

		// Token: 0x0400517D RID: 20861
		private int lastUsedTick = -1;
	}
}
