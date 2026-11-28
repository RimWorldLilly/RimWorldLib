using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C3 RID: 4291
	public class QuestPart_ExtraFaction : QuestPartActivable
	{
		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x06006B3D RID: 27453 RVA: 0x0025A210 File Offset: 0x00258410
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_ExtraFaction.<get_InvolvedFactions>d__7 <get_InvolvedFactions>d__ = new QuestPart_ExtraFaction.<get_InvolvedFactions>d__7(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006B3E RID: 27454 RVA: 0x0025A220 File Offset: 0x00258420
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			base.Notify_PawnKilled(pawn, dinfo);
			if (pawn.HomeFaction != null && pawn.Faction != pawn.HomeFaction)
			{
				pawn.SetFaction(pawn.HomeFaction, null);
			}
		}

		// Token: 0x06006B3F RID: 27455 RVA: 0x0025A250 File Offset: 0x00258450
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (!(signal.tag == this.inSignalRemovePawn))
			{
				List<string> list = this.inSignalsRemovePawn;
				if (list == null || !list.Contains(signal.tag))
				{
					return;
				}
			}
			Pawn pawn;
			if (signal.args.TryGetArg<Pawn>("SUBJECT", out pawn) && this.affectedPawns.Contains(pawn))
			{
				this.affectedPawns.Remove(pawn);
				this.extraFaction.faction.Notify_MemberLeftExtraFaction(pawn);
			}
		}

		// Token: 0x06006B40 RID: 27456 RVA: 0x0025A2D2 File Offset: 0x002584D2
		public override bool QuestPartReserves(Faction f)
		{
			return this.extraFaction.faction == f;
		}

		// Token: 0x06006B41 RID: 27457 RVA: 0x0025A2E4 File Offset: 0x002584E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ExtraFaction>(ref this.extraFaction, "extraFaction", Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.affectedPawns, "affectedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.areHelpers, "areHelpers", false, false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_Collections.Look<string>(ref this.inSignalsRemovePawn, "inSignalsRemovePawn", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				ExtraFaction extraFaction = this.extraFaction;
				if (((extraFaction != null) ? extraFaction.faction : null) == null)
				{
					this.affectedPawns.Clear();
					return;
				}
				this.affectedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006B42 RID: 27458 RVA: 0x0025A3AF File Offset: 0x002585AF
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.affectedPawns.Replace(replace, with);
		}

		// Token: 0x06006B43 RID: 27459 RVA: 0x0025A3BF File Offset: 0x002585BF
		public override void Cleanup()
		{
			base.Cleanup();
			this.SetRelationsGainTickForPawns();
		}

		// Token: 0x06006B44 RID: 27460 RVA: 0x0025A3CD File Offset: 0x002585CD
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.extraFaction.faction == faction)
			{
				this.affectedPawns.Clear();
				this.extraFaction.faction = null;
			}
		}

		// Token: 0x06006B45 RID: 27461 RVA: 0x0025A3F4 File Offset: 0x002585F4
		protected override void Disable()
		{
			base.Disable();
			this.SetRelationsGainTickForPawns();
		}

		// Token: 0x06006B46 RID: 27462 RVA: 0x0025A404 File Offset: 0x00258604
		private void SetRelationsGainTickForPawns()
		{
			foreach (Pawn pawn in this.affectedPawns)
			{
				if (pawn.mindState != null)
				{
					pawn.mindState.SetNoAidRelationsGainUntilTick(Find.TickManager.TicksGame + 1800000);
				}
			}
		}

		// Token: 0x0400442D RID: 17453
		public ExtraFaction extraFaction;

		// Token: 0x0400442E RID: 17454
		public List<Pawn> affectedPawns = new List<Pawn>();

		// Token: 0x0400442F RID: 17455
		public bool areHelpers;

		// Token: 0x04004430 RID: 17456
		public string inSignalRemovePawn;

		// Token: 0x04004431 RID: 17457
		public List<string> inSignalsRemovePawn;

		// Token: 0x04004432 RID: 17458
		private const int RelationsGainAvailableInTicks = 1800000;
	}
}
