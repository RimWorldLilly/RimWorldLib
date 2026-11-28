using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A3 RID: 4259
	public class QuestPart_AddContentsToShuttle : QuestPart
	{
		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06006A8A RID: 27274 RVA: 0x00257462 File Offset: 0x00255662
		// (set) Token: 0x06006A8B RID: 27275 RVA: 0x0025747C File Offset: 0x0025567C
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
						if (thing.Destroyed)
						{
							Log.Error("Tried to add a destroyed thing to QuestPart_AddContentsToShuttle: " + thing.ToStringSafe<Thing>());
						}
						else
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
		}

		// Token: 0x1700123D RID: 4669
		// (get) Token: 0x06006A8C RID: 27276 RVA: 0x0025751C File Offset: 0x0025571C
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				QuestPart_AddContentsToShuttle.<get_Hyperlinks>d__8 <get_Hyperlinks>d__ = new QuestPart_AddContentsToShuttle.<get_Hyperlinks>d__8(-2);
				<get_Hyperlinks>d__.<>4__this = this;
				return <get_Hyperlinks>d__;
			}
		}

		// Token: 0x1700123E RID: 4670
		// (get) Token: 0x06006A8D RID: 27277 RVA: 0x0025752C File Offset: 0x0025572C
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_AddContentsToShuttle.<get_QuestLookTargets>d__10 <get_QuestLookTargets>d__ = new QuestPart_AddContentsToShuttle.<get_QuestLookTargets>d__10(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006A8E RID: 27278 RVA: 0x0025753C File Offset: 0x0025573C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.shuttle != null)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				this.items.RemoveAll((Thing x) => x.Destroyed);
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].IsWorldPawn())
					{
						Find.WorldPawns.RemovePawn(this.pawns[i]);
					}
				}
				CompTransporter compTransporter = this.shuttle.TryGetComp<CompTransporter>();
				compTransporter.innerContainer.TryAddRangeOrTransfer(this.pawns, true, false);
				compTransporter.innerContainer.TryAddRangeOrTransfer(this.items, true, false);
				this.items.Clear();
			}
		}

		// Token: 0x06006A8F RID: 27279 RVA: 0x00257641 File Offset: 0x00255841
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06006A90 RID: 27280 RVA: 0x0025764F File Offset: 0x0025584F
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x06006A91 RID: 27281 RVA: 0x00257660 File Offset: 0x00255860
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!this.items[i].Destroyed)
				{
					this.items[i].Destroy(DestroyMode.Vanish);
				}
			}
			this.items.Clear();
		}

		// Token: 0x06006A92 RID: 27282 RVA: 0x002576BC File Offset: 0x002558BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006A93 RID: 27283 RVA: 0x0025777C File Offset: 0x0025597C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x040043BA RID: 17338
		public string inSignal;

		// Token: 0x040043BB RID: 17339
		public Thing shuttle;

		// Token: 0x040043BC RID: 17340
		private List<Thing> items = new List<Thing>();

		// Token: 0x040043BD RID: 17341
		private List<Pawn> pawns = new List<Pawn>();
	}
}
