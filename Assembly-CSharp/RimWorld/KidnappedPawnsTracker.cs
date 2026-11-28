using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001610 RID: 5648
	public class KidnappedPawnsTracker : IExposable
	{
		// Token: 0x17001863 RID: 6243
		// (get) Token: 0x06008CBD RID: 36029 RVA: 0x003322E1 File Offset: 0x003304E1
		public List<Pawn> KidnappedPawnsListForReading
		{
			get
			{
				return this.kidnappedPawns;
			}
		}

		// Token: 0x06008CBE RID: 36030 RVA: 0x003322E9 File Offset: 0x003304E9
		public KidnappedPawnsTracker(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x06008CBF RID: 36031 RVA: 0x00332304 File Offset: 0x00330504
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.kidnappedPawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Collections.Look<Pawn>(ref this.kidnappedPawns, "kidnappedPawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x06008CC0 RID: 36032 RVA: 0x0033235C File Offset: 0x0033055C
		public void Kidnap(Pawn pawn, Pawn kidnapper)
		{
			if (this.kidnappedPawns.Contains(pawn))
			{
				Log.Error("Tried to kidnap already kidnapped pawn " + ((pawn != null) ? pawn.ToString() : null));
				return;
			}
			if (pawn.Faction == this.faction)
			{
				Log.Error("Tried to kidnap pawn with the same faction: " + ((pawn != null) ? pawn.ToString() : null));
				return;
			}
			pawn.PreKidnapped(kidnapper);
			pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
			this.kidnappedPawns.Add(pawn);
			if (!Find.WorldPawns.Contains(pawn))
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				if (!Find.WorldPawns.Contains(pawn))
				{
					Log.Error("WorldPawns discarded kidnapped pawn.");
					this.kidnappedPawns.Remove(pawn);
				}
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(pawn, null, PawnDiedOrDownedThoughtsKind.Lost);
				BillUtility.Notify_ColonistUnavailable(pawn);
				if (kidnapper != null)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelPawnsKidnapped".Translate(pawn.Named("PAWN")), "LetterPawnsKidnapped".Translate(pawn.Named("PAWN"), kidnapper.Faction.Named("FACTION")), LetterDefOf.NegativeEvent, null, 0, true);
				}
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "Kidnapped", pawn.Named("SUBJECT"), kidnapper.Named("KIDNAPPER"));
			Find.GameEnder.CheckOrUpdateGameOver();
		}

		// Token: 0x06008CC1 RID: 36033 RVA: 0x003324BC File Offset: 0x003306BC
		public void RemoveKidnappedPawn(Pawn pawn)
		{
			if (this.kidnappedPawns.Remove(pawn))
			{
				if (pawn.Faction == Faction.OfPlayer)
				{
					PawnDiedOrDownedThoughtsUtility.RemoveLostThoughts(pawn);
					return;
				}
			}
			else
			{
				Log.Warning("Tried to remove kidnapped pawn " + ((pawn != null) ? pawn.ToString() : null) + " but he's not here.");
			}
		}

		// Token: 0x06008CC2 RID: 36034 RVA: 0x0033250C File Offset: 0x0033070C
		public void LogKidnappedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.faction.Name + ":");
			for (int i = 0; i < this.kidnappedPawns.Count; i++)
			{
				stringBuilder.AppendLine(this.kidnappedPawns[i].Name.ToStringFull);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06008CC3 RID: 36035 RVA: 0x0033257C File Offset: 0x0033077C
		public void KidnappedPawnsTrackerTick()
		{
			for (int i = this.kidnappedPawns.Count - 1; i >= 0; i--)
			{
				if (this.kidnappedPawns[i].DestroyedOrNull())
				{
					this.kidnappedPawns.RemoveAt(i);
				}
			}
			if (Find.TickManager.TicksGame % 15051 == 0)
			{
				for (int j = this.kidnappedPawns.Count - 1; j >= 0; j--)
				{
					if (Rand.MTBEventOccurs(30f, 60000f, 15051f))
					{
						this.kidnappedPawns[j].SetFaction(this.faction, null);
						this.kidnappedPawns.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x0400563D RID: 22077
		private Faction faction;

		// Token: 0x0400563E RID: 22078
		private List<Pawn> kidnappedPawns = new List<Pawn>();

		// Token: 0x0400563F RID: 22079
		private const int TryRecruitInterval = 15051;

		// Token: 0x04005640 RID: 22080
		private const float RecruitMTBDays = 30f;
	}
}
