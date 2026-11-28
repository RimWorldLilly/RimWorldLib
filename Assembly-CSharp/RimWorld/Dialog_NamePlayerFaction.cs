using System;

namespace RimWorld
{
	// Token: 0x02001CBB RID: 7355
	public class Dialog_NamePlayerFaction : Dialog_GiveName
	{
		// Token: 0x17002264 RID: 8804
		// (get) Token: 0x0600BCE6 RID: 48358 RVA: 0x00439ED0 File Offset: 0x004380D0
		protected override int FirstCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x0600BCE7 RID: 48359 RVA: 0x00439ED4 File Offset: 0x004380D4
		public Dialog_NamePlayerFaction()
		{
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, new Predicate<string>(this.IsValidName), false, null, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.gainedNameMessageKey = "PlayerFactionGainsName";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
		}

		// Token: 0x0600BCE8 RID: 48360 RVA: 0x00439F2B File Offset: 0x0043812B
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x0600BCE9 RID: 48361 RVA: 0x00439F33 File Offset: 0x00438133
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}
	}
}
