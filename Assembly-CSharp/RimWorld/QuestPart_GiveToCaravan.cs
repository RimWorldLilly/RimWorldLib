using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001115 RID: 4373
	public class QuestPart_GiveToCaravan : QuestPart
	{
		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x06006D5A RID: 27994 RVA: 0x00264E59 File Offset: 0x00263059
		// (set) Token: 0x06006D5B RID: 27995 RVA: 0x00264E74 File Offset: 0x00263074
		public IEnumerable<Thing> Things
		{
			get
			{
				return this.items.Concat(this.pawns.Cast<Thing>());
			}
			set
			{
				this.items.Clear();
				this.pawns.Clear();
				if (value != null)
				{
					foreach (Thing thing in value)
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							this.pawns.Add(pawn);
						}
						else
						{
							this.items.Add(thing);
						}
					}
				}
			}
		}

		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x06006D5C RID: 27996 RVA: 0x00264EF4 File Offset: 0x002630F4
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_GiveToCaravan.<get_QuestLookTargets>d__8 <get_QuestLookTargets>d__ = new QuestPart_GiveToCaravan.<get_QuestLookTargets>d__8(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x06006D5D RID: 27997 RVA: 0x00264F04 File Offset: 0x00263104
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, true, false);
			}
		}

		// Token: 0x06006D5E RID: 27998 RVA: 0x00264F14 File Offset: 0x00263114
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				Caravan caravan = this.caravan;
				if (caravan == null)
				{
					signal.args.TryGetArg<Caravan>("CARAVAN", out caravan);
				}
				if (caravan != null && this.Things.Any<Thing>())
				{
					for (int i = 0; i < this.pawns.Count; i++)
					{
						if (this.pawns[i].Faction != Faction.OfPlayer)
						{
							this.pawns[i].SetFaction(Faction.OfPlayer, null);
						}
						caravan.AddPawn(this.pawns[i], true);
					}
					for (int j = 0; j < this.items.Count; j++)
					{
						CaravanInventoryUtility.GiveThing(caravan, this.items[j]);
					}
					this.items.Clear();
				}
			}
		}

		// Token: 0x06006D5F RID: 27999 RVA: 0x00265028 File Offset: 0x00263228
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].def == ThingDefOf.PsychicAmplifier)
				{
					Find.History.Notify_PsylinkAvailable();
					return;
				}
			}
		}

		// Token: 0x06006D60 RID: 28000 RVA: 0x00265074 File Offset: 0x00263274
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06006D61 RID: 28001 RVA: 0x00265084 File Offset: 0x00263284
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].Destroy(DestroyMode.Vanish);
			}
			this.items.Clear();
		}

		// Token: 0x06006D62 RID: 28002 RVA: 0x002650CC File Offset: 0x002632CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Caravan>(ref this.caravan, "caravan", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006D63 RID: 28003 RVA: 0x0026518C File Offset: 0x0026338C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x06006D64 RID: 28004 RVA: 0x002651BC File Offset: 0x002633BC
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040045A7 RID: 17831
		public string inSignal;

		// Token: 0x040045A8 RID: 17832
		public Caravan caravan;

		// Token: 0x040045A9 RID: 17833
		private List<Thing> items = new List<Thing>();

		// Token: 0x040045AA RID: 17834
		private List<Pawn> pawns = new List<Pawn>();
	}
}
