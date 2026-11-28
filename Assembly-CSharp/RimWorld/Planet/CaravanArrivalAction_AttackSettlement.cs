using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002374 RID: 9076
	public class CaravanArrivalAction_AttackSettlement : CaravanArrivalAction
	{
		// Token: 0x170027A2 RID: 10146
		// (get) Token: 0x0600E145 RID: 57669 RVA: 0x00539FB4 File Offset: 0x005381B4
		public override string Label
		{
			get
			{
				return "AttackSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x170027A3 RID: 10147
		// (get) Token: 0x0600E146 RID: 57670 RVA: 0x00539FD5 File Offset: 0x005381D5
		public override string ReportString
		{
			get
			{
				return "CaravanAttacking".Translate(this.settlement.Label);
			}
		}

		// Token: 0x0600E147 RID: 57671 RVA: 0x00539FF6 File Offset: 0x005381F6
		public CaravanArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x0600E148 RID: 57672 RVA: 0x00539FFE File Offset: 0x005381FE
		public CaravanArrivalAction_AttackSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600E149 RID: 57673 RVA: 0x0053A010 File Offset: 0x00538210
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_AttackSettlement.CanAttack(caravan, this.settlement);
		}

		// Token: 0x0600E14A RID: 57674 RVA: 0x0053A05E File Offset: 0x0053825E
		public override void Arrived(Caravan caravan)
		{
			SettlementUtility.Attack(caravan, this.settlement);
		}

		// Token: 0x0600E14B RID: 57675 RVA: 0x0053A06C File Offset: 0x0053826C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x0600E14C RID: 57676 RVA: 0x0053A088 File Offset: 0x00538288
		public static FloatMenuAcceptanceReport CanAttack(Caravan caravan, Settlement settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Attackable)
			{
				return false;
			}
			if (settlement.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(settlement.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return true;
		}

		// Token: 0x0600E14D RID: 57677 RVA: 0x0053A0E8 File Offset: 0x005382E8
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_AttackSettlement>(() => CaravanArrivalAction_AttackSettlement.CanAttack(caravan, settlement), () => new CaravanArrivalAction_AttackSettlement(settlement), "AttackSettlement".Translate(settlement.Label), caravan, settlement.Tile, settlement, settlement.Faction.AllyOrNeutralTo(Faction.OfPlayer) ? delegate(Action action)
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmAttackFriendlyFaction".Translate(settlement.LabelCap, settlement.Faction.Name), delegate()
				{
					action();
				}, false, null, WindowLayer.Dialog));
			} : null);
		}

		// Token: 0x04009039 RID: 36921
		private Settlement settlement;
	}
}
