using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CBF RID: 7359
	public class Dialog_NamePlayerSettlement : Dialog_GiveName
	{
		// Token: 0x17002268 RID: 8808
		// (get) Token: 0x0600BCFC RID: 48380 RVA: 0x0043A251 File Offset: 0x00438451
		protected override int FirstCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x0600BCFD RID: 48381 RVA: 0x0043A258 File Offset: 0x00438458
		public Dialog_NamePlayerSettlement(Settlement settlement)
		{
			this.settlement = settlement;
			if (settlement.HasMap && settlement.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				this.suggestingPawn = settlement.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			}
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, new Predicate<string>(this.IsValidName), false, null, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionBaseMessage";
			this.gainedNameMessageKey = "PlayerFactionBaseGainsName";
			this.invalidNameMessageKey = "PlayerFactionBaseNameIsInvalid";
		}

		// Token: 0x0600BCFE RID: 48382 RVA: 0x0043A2EB File Offset: 0x004384EB
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.settlement.Map != null)
			{
				Current.Game.CurrentMap = this.settlement.Map;
			}
		}

		// Token: 0x0600BCFF RID: 48383 RVA: 0x0043A315 File Offset: 0x00438515
		protected override bool IsValidName(string s)
		{
			return NamePlayerSettlementDialogUtility.IsValidName(s);
		}

		// Token: 0x0600BD00 RID: 48384 RVA: 0x0043A31D File Offset: 0x0043851D
		protected override void Named(string s)
		{
			NamePlayerSettlementDialogUtility.Named(this.settlement, s);
		}

		// Token: 0x04006C03 RID: 27651
		private Settlement settlement;
	}
}
