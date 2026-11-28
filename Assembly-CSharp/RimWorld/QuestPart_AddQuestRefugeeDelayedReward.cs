using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A8 RID: 4264
	public class QuestPart_AddQuestRefugeeDelayedReward : QuestPart_AddQuest
	{
		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x06006AB5 RID: 27317 RVA: 0x00257D4A File Offset: 0x00255F4A
		public override QuestScriptDef QuestDef
		{
			get
			{
				return QuestScriptDefOf.RefugeeDelayedReward;
			}
		}

		// Token: 0x06006AB6 RID: 27318 RVA: 0x00257D54 File Offset: 0x00255F54
		public override Slate GetSlate()
		{
			Slate slate = new Slate();
			slate.Set<FloatRange>("marketValueRange", this.marketValueRange, false);
			slate.Set<Faction>("faction", this.faction, false);
			for (int i = 0; i < this.lodgers.Count; i++)
			{
				if (!this.lodgers[i].Dead && this.lodgers[i].Faction != Faction.OfPlayer && !this.lodgers[i].IsPrisoner)
				{
					slate.Set<Pawn>("rewardGiver", this.lodgers[i], false);
					break;
				}
			}
			return slate;
		}

		// Token: 0x06006AB7 RID: 27319 RVA: 0x00257DFA File Offset: 0x00255FFA
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06006AB8 RID: 27320 RVA: 0x00257E0C File Offset: 0x0025600C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.lodgers.Contains(item))
			{
				this.lodgers.Remove(item);
			}
		}

		// Token: 0x06006AB9 RID: 27321 RVA: 0x00257E64 File Offset: 0x00256064
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Collections.Look<Pawn>(ref this.lodgers, "lodgers", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_Values.Look<FloatRange>(ref this.marketValueRange, "marketValueRange", default(FloatRange), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lodgers.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040043D1 RID: 17361
		public List<Pawn> lodgers = new List<Pawn>();

		// Token: 0x040043D2 RID: 17362
		public FloatRange marketValueRange;

		// Token: 0x040043D3 RID: 17363
		public Faction faction;

		// Token: 0x040043D4 RID: 17364
		public string inSignalRemovePawn;
	}
}
