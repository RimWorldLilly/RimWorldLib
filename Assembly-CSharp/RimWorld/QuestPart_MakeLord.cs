using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200109C RID: 4252
	public abstract class QuestPart_MakeLord : QuestPart
	{
		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x06006A6D RID: 27245 RVA: 0x00256D3F File Offset: 0x00254F3F
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_MakeLord.<get_QuestLookTargets>d__8 <get_QuestLookTargets>d__ = new QuestPart_MakeLord.<get_QuestLookTargets>d__8(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06006A6E RID: 27246 RVA: 0x00256D4F File Offset: 0x00254F4F
		public Map Map
		{
			get
			{
				if (this.mapOfPawn != null)
				{
					return this.mapOfPawn.MapHeld;
				}
				if (this.mapParent != null)
				{
					return this.mapParent.Map;
				}
				return null;
			}
		}

		// Token: 0x06006A6F RID: 27247
		protected abstract Lord MakeLord();

		// Token: 0x06006A70 RID: 27248 RVA: 0x00256D7C File Offset: 0x00254F7C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				if (this.Map == null)
				{
					this.mapOfPawn = null;
					this.mapParent = this.quest.TryFindNewSuitableMapParentForRetarget();
				}
				if (this.Map != null)
				{
					this.pawns.RemoveAll((Pawn x) => x.MapHeld != this.Map);
					for (int i = 0; i < this.pawns.Count; i++)
					{
						Lord lord = this.pawns[i].GetLord();
						if (lord != null)
						{
							lord.Notify_PawnLost(this.pawns[i], PawnLostCondition.ForcedByQuest, null);
						}
					}
					Lord lord2 = this.MakeLord();
					for (int j = 0; j < this.pawns.Count; j++)
					{
						if (!this.pawns[j].Dead)
						{
							lord2.AddPawn(this.pawns[j]);
						}
					}
				}
			}
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
		}

		// Token: 0x06006A71 RID: 27249 RVA: 0x00256EBE File Offset: 0x002550BE
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06006A72 RID: 27250 RVA: 0x00256ED0 File Offset: 0x002550D0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Pawn>(ref this.mapOfPawn, "mapOfPawn", false);
			Scribe_Values.Look<bool>(ref this.excludeFromLookTargets, "excludeFromLookTargets", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006A73 RID: 27251 RVA: 0x00256F98 File Offset: 0x00255198
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
			this.mapParent = this.pawns[0].Map.Parent;
		}

		// Token: 0x0400439E RID: 17310
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400439F RID: 17311
		public string inSignal;

		// Token: 0x040043A0 RID: 17312
		public Faction faction;

		// Token: 0x040043A1 RID: 17313
		public MapParent mapParent;

		// Token: 0x040043A2 RID: 17314
		public string inSignalRemovePawn;

		// Token: 0x040043A3 RID: 17315
		public Pawn mapOfPawn;

		// Token: 0x040043A4 RID: 17316
		public bool excludeFromLookTargets;
	}
}
