using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001116 RID: 4374
	public class QuestPart_JoinPlayer : QuestPart
	{
		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x06006D67 RID: 28007 RVA: 0x002651F2 File Offset: 0x002633F2
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_JoinPlayer.<get_QuestLookTargets>d__7 <get_QuestLookTargets>d__ = new QuestPart_JoinPlayer.<get_QuestLookTargets>d__7(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x170012C4 RID: 4804
		// (get) Token: 0x06006D68 RID: 28008 RVA: 0x00265202 File Offset: 0x00263402
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x06006D69 RID: 28009 RVA: 0x0026521C File Offset: 0x0026341C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				if (this.joinPlayer)
				{
					using (List<Pawn>.Enumerator enumerator = this.pawns.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Pawn pawn = enumerator.Current;
							if (pawn.IsColonist || pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony)
							{
								RecruitUtility.Recruit(pawn, Faction.OfPlayer, null);
							}
							else if (pawn.Faction != Faction.OfPlayer)
							{
								pawn.SetFaction(Faction.OfPlayer, null);
							}
						}
						return;
					}
				}
				if (this.makePrisoners)
				{
					for (int i = 0; i < this.pawns.Count; i++)
					{
						if (this.pawns[i].RaceProps.Humanlike)
						{
							if (!this.pawns[i].IsPrisonerOfColony)
							{
								this.pawns[i].guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Prisoner);
							}
							HealthUtility.TryAnesthetize(this.pawns[i]);
						}
					}
				}
			}
		}

		// Token: 0x06006D6A RID: 28010 RVA: 0x00265374 File Offset: 0x00263574
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06006D6B RID: 28011 RVA: 0x00265382 File Offset: 0x00263582
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x06006D6C RID: 28012 RVA: 0x00265394 File Offset: 0x00263594
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.makePrisoners, "makePrisoners", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040045AB RID: 17835
		public string inSignal;

		// Token: 0x040045AC RID: 17836
		public string outSignalResult;

		// Token: 0x040045AD RID: 17837
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040045AE RID: 17838
		public bool joinPlayer;

		// Token: 0x040045AF RID: 17839
		public bool makePrisoners;

		// Token: 0x040045B0 RID: 17840
		public MapParent mapParent;
	}
}
