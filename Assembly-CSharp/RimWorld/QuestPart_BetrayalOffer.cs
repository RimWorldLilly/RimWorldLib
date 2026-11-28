using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200107B RID: 4219
	public class QuestPart_BetrayalOffer : QuestPartActivable
	{
		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x060069B3 RID: 27059 RVA: 0x00253D4B File Offset: 0x00251F4B
		public override string ExpiryInfoPart
		{
			get
			{
				return "QuestBetrayalOffer".Translate(this.PawnsAliveCount, this.extraFaction.faction.Name);
			}
		}

		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x060069B4 RID: 27060 RVA: 0x00253D7C File Offset: 0x00251F7C
		public override string ExpiryInfoPartTip
		{
			get
			{
				return "QuestBetrayalOfferTip".Translate(this.asker.NameFullColored, this.extraFaction.faction.Name);
			}
		}

		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x060069B5 RID: 27061 RVA: 0x00253DB2 File Offset: 0x00251FB2
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_BetrayalOffer.<get_QuestLookTargets>d__13 <get_QuestLookTargets>d__ = new QuestPart_BetrayalOffer.<get_QuestLookTargets>d__13(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x060069B6 RID: 27062 RVA: 0x00253DC4 File Offset: 0x00251FC4
		private int PawnsAliveCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].Destroyed)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x060069B7 RID: 27063 RVA: 0x00253E04 File Offset: 0x00252004
		private bool AnyPawnDespawnedButAlive
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].SpawnedOrAnyParentSpawned && !this.pawns[i].Dead)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x060069B8 RID: 27064 RVA: 0x00253E50 File Offset: 0x00252050
		private bool AllSurvivingPawnsRecruited
		{
			get
			{
				foreach (Pawn pawn in this.pawns)
				{
					if (!pawn.Destroyed && (!pawn.Faction.IsPlayerSafe() || pawn.GetExtraFaction(this.extraFaction.factionType, this.quest) == this.extraFaction.faction))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x060069B9 RID: 27065 RVA: 0x00253EDC File Offset: 0x002520DC
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			if (!this.AnyPawnDespawnedButAlive && this.PawnsAliveCount > 0)
			{
				if (!this.outSignalEnabled.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalEnabled, receivedArgs, false));
					return;
				}
			}
			else
			{
				base.Complete();
			}
		}

		// Token: 0x060069BA RID: 27066 RVA: 0x00253F2C File Offset: 0x0025212C
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				if (this.AnyPawnDespawnedButAlive)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalFailure, signal.args, false));
					base.Complete();
					return;
				}
				if (this.PawnsAliveCount == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalSuccess, signal.args, false));
					base.Complete();
					return;
				}
				if (this.AllSurvivingPawnsRecruited)
				{
					string tag = this.outSignalFailureRecruited ?? this.outSignalFailure;
					Find.SignalManager.SendSignal(new Signal(tag, signal.args, false));
					base.Complete();
				}
			}
		}

		// Token: 0x060069BB RID: 27067 RVA: 0x00253FE2 File Offset: 0x002521E2
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.extraFaction.faction == f)
			{
				this.extraFaction.faction = null;
			}
		}

		// Token: 0x060069BC RID: 27068 RVA: 0x00253FFE File Offset: 0x002521FE
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			base.Notify_PawnKilled(pawn, dinfo);
			if (pawn == this.asker && base.State == QuestPartState.Enabled)
			{
				this.Disable();
			}
		}

		// Token: 0x060069BD RID: 27069 RVA: 0x00254020 File Offset: 0x00252220
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ExtraFaction>(ref this.extraFaction, "extraFaction", Array.Empty<object>());
			Scribe_References.Look<Pawn>(ref this.asker, "asker", false);
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignalSuccess, "outSignalSuccess", null, false);
			Scribe_Values.Look<string>(ref this.outSignalFailure, "outSignalFailure", null, false);
			Scribe_Values.Look<string>(ref this.outSignalFailureRecruited, "outSignalFailureRecruited", null, false);
			Scribe_Values.Look<string>(ref this.outSignalEnabled, "outSignalEnabled", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
				if (this.asker == null && base.State == QuestPartState.Enabled)
				{
					this.Disable();
				}
			}
		}

		// Token: 0x0400432D RID: 17197
		public ExtraFaction extraFaction;

		// Token: 0x0400432E RID: 17198
		public Pawn asker;

		// Token: 0x0400432F RID: 17199
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04004330 RID: 17200
		public List<string> inSignals = new List<string>();

		// Token: 0x04004331 RID: 17201
		public string outSignalEnabled;

		// Token: 0x04004332 RID: 17202
		public string outSignalSuccess;

		// Token: 0x04004333 RID: 17203
		public string outSignalFailure;

		// Token: 0x04004334 RID: 17204
		public string outSignalFailureRecruited;
	}
}
