using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B8F RID: 7055
	public class Tradeable_Pawn : Tradeable
	{
		// Token: 0x170020AB RID: 8363
		// (get) Token: 0x0600B517 RID: 46359 RVA: 0x003FCB9D File Offset: 0x003FAD9D
		public override Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.AnyPawn, null);
			}
		}

		// Token: 0x170020AC RID: 8364
		// (get) Token: 0x0600B518 RID: 46360 RVA: 0x003FCBAC File Offset: 0x003FADAC
		public override string Label
		{
			get
			{
				string text = base.Label;
				if (this.AnyPawn.Name != null && !this.AnyPawn.Name.Numerical && !this.AnyPawn.RaceProps.Humanlike)
				{
					text = text + ", " + this.AnyPawn.def.label;
				}
				return string.Concat(new string[]
				{
					text,
					" (",
					this.AnyPawn.GetGenderLabel(),
					", ",
					Mathf.FloorToInt(this.AnyPawn.ageTracker.AgeBiologicalYearsFloat).ToString(),
					")"
				});
			}
		}

		// Token: 0x170020AD RID: 8365
		// (get) Token: 0x0600B519 RID: 46361 RVA: 0x003FCC64 File Offset: 0x003FAE64
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyPawn.MainDesc(true, true) + "\n\n" + this.AnyPawn.DescriptionDetailed;
			}
		}

		// Token: 0x170020AE RID: 8366
		// (get) Token: 0x0600B51A RID: 46362 RVA: 0x003FCC96 File Offset: 0x003FAE96
		private Pawn AnyPawn
		{
			get
			{
				return (Pawn)this.AnyThing;
			}
		}

		// Token: 0x0600B51B RID: 46363 RVA: 0x003FCCA4 File Offset: 0x003FAEA4
		public override void ResolveTrade()
		{
			if (base.ActionToDo == TradeAction.PlayerSells)
			{
				List<Pawn> list = this.thingsColony.Take(base.CountToTransferToDestination).Cast<Pawn>().ToList<Pawn>();
				for (int i = 0; i < list.Count; i++)
				{
					bool flag = GuestUtility.IsSellingToSlavery(list[i], TradeSession.trader.Faction);
					TradeSession.trader.GiveSoldThingToTrader(list[i], 1, TradeSession.playerNegotiator);
					if (flag)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SoldSlave, TradeSession.playerNegotiator.Named(HistoryEventArgsNames.Doer)), true);
					}
				}
				return;
			}
			if (base.ActionToDo == TradeAction.PlayerBuys)
			{
				List<Pawn> list2 = this.thingsTrader.Take(base.CountToTransferToSource).Cast<Pawn>().ToList<Pawn>();
				for (int j = 0; j < list2.Count; j++)
				{
					TradeSession.trader.GiveSoldThingToPlayer(list2[j], 1, TradeSession.playerNegotiator);
				}
			}
		}
	}
}
