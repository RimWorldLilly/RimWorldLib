using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A9 RID: 4265
	public class QuestPart_AddQuest_RefugeeBetrayal : QuestPart_AddQuest
	{
		// Token: 0x17001246 RID: 4678
		// (get) Token: 0x06006ABB RID: 27323 RVA: 0x00257F10 File Offset: 0x00256110
		public override QuestScriptDef QuestDef
		{
			get
			{
				return QuestScriptDefOf.RefugeeBetrayal;
			}
		}

		// Token: 0x06006ABC RID: 27324 RVA: 0x00257F18 File Offset: 0x00256118
		public override Slate GetSlate()
		{
			Slate slate = new Slate();
			slate.Set<List<Pawn>>("lodgers", this.lodgers, false);
			slate.Set<ExtraFaction>("refugeeFaction", this.refugeeFaction, false);
			slate.Set<Pawn>("factionOpponent", this.factionOpponent, false);
			slate.Set<Pawn>("asker", this.asker, false);
			slate.Set<int>("lodgerCount", this.lodgers.Count, false);
			slate.Set<Faction>("faction", this.refugeeFaction.faction, false);
			slate.Set<Map>("map", this.mapParent.Map, false);
			return slate;
		}

		// Token: 0x06006ABD RID: 27325 RVA: 0x00257FB7 File Offset: 0x002561B7
		public override void Notify_FactionRemoved(Faction f)
		{
			ExtraFaction extraFaction = this.refugeeFaction;
			if (((extraFaction != null) ? extraFaction.faction : null) == f)
			{
				this.refugeeFaction = null;
			}
		}

		// Token: 0x06006ABE RID: 27326 RVA: 0x00257FD8 File Offset: 0x002561D8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.lodgers.Contains(item))
			{
				this.lodgers.Remove(item);
			}
		}

		// Token: 0x06006ABF RID: 27327 RVA: 0x00258030 File Offset: 0x00256230
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ExtraFaction>(ref this.refugeeFaction, "refugeeFaction", Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.lodgers, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_References.Look<Pawn>(ref this.asker, "asker", false);
			Scribe_References.Look<Pawn>(ref this.factionOpponent, "factionOpponent", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lodgers.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040043D5 RID: 17365
		public List<Pawn> lodgers = new List<Pawn>();

		// Token: 0x040043D6 RID: 17366
		public Pawn factionOpponent;

		// Token: 0x040043D7 RID: 17367
		public Pawn asker;

		// Token: 0x040043D8 RID: 17368
		public ExtraFaction refugeeFaction;

		// Token: 0x040043D9 RID: 17369
		public MapParent mapParent;

		// Token: 0x040043DA RID: 17370
		public string inSignalRemovePawn;
	}
}
