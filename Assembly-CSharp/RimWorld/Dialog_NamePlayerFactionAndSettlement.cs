using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CBD RID: 7357
	public class Dialog_NamePlayerFactionAndSettlement : Dialog_GiveName
	{
		// Token: 0x17002265 RID: 8805
		// (get) Token: 0x0600BCED RID: 48365 RVA: 0x0043A00A File Offset: 0x0043820A
		protected override int FirstCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x17002266 RID: 8806
		// (get) Token: 0x0600BCEE RID: 48366 RVA: 0x0043A00E File Offset: 0x0043820E
		protected override int SecondCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x0600BCEF RID: 48367 RVA: 0x0043A014 File Offset: 0x00438214
		public Dialog_NamePlayerFactionAndSettlement(Settlement settlement)
		{
			this.settlement = settlement;
			if (settlement.HasMap && settlement.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				this.suggestingPawn = settlement.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			}
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, new Predicate<string>(this.IsValidName), false, null, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
			this.useSecondName = true;
			this.secondNameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, new Predicate<string>(this.IsValidSecondName), false, null, null, null));
			this.curSecondName = this.secondNameGenerator();
			this.secondNameMessageKey = "NamePlayerFactionBaseMessage_NameFactionContinuation";
			this.invalidSecondNameMessageKey = "PlayerFactionBaseNameIsInvalid";
			this.gainedNameMessageKey = "PlayerFactionAndBaseGainsName";
		}

		// Token: 0x0600BCF0 RID: 48368 RVA: 0x0043A0E7 File Offset: 0x004382E7
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.settlement.Map != null)
			{
				Current.Game.CurrentMap = this.settlement.Map;
			}
		}

		// Token: 0x0600BCF1 RID: 48369 RVA: 0x0043A111 File Offset: 0x00438311
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x0600BCF2 RID: 48370 RVA: 0x0043A119 File Offset: 0x00438319
		protected override bool IsValidSecondName(string s)
		{
			return NamePlayerSettlementDialogUtility.IsValidName(s);
		}

		// Token: 0x0600BCF3 RID: 48371 RVA: 0x0043A121 File Offset: 0x00438321
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}

		// Token: 0x0600BCF4 RID: 48372 RVA: 0x0043A129 File Offset: 0x00438329
		protected override void NamedSecond(string s)
		{
			NamePlayerSettlementDialogUtility.Named(this.settlement, s);
		}

		// Token: 0x04006C01 RID: 27649
		private Settlement settlement;
	}
}
