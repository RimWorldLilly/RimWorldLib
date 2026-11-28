using System;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CBE RID: 7358
	public class Dialog_NamePlayerGravship : Dialog_GiveName
	{
		// Token: 0x17002267 RID: 8807
		// (get) Token: 0x0600BCF7 RID: 48375 RVA: 0x0043A185 File Offset: 0x00438385
		protected override int FirstCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x0600BCF8 RID: 48376 RVA: 0x0043A18C File Offset: 0x0043838C
		public Dialog_NamePlayerGravship(Building_GravEngine engine)
		{
			if (!ModLister.CheckOdyssey("Grav engines"))
			{
				return;
			}
			this.engine = engine;
			this.nameGenerator = (() => NameGenerator.GenerateName(RulePackDefOf.NamerGravship, new Predicate<string>(this.IsValidName), false, null, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerGravshipMessage";
			this.gainedNameMessageKey = "PlayerGravshipGainsName";
			this.invalidNameMessageKey = "PlayerGravshipNameIsInvalid";
		}

		// Token: 0x0600BCF9 RID: 48377 RVA: 0x0043A1F7 File Offset: 0x004383F7
		protected override bool IsValidName(string s)
		{
			return s.Length != 0 && s.Length <= 64 && !GrammarResolver.ContainsSpecialChars(s);
		}

		// Token: 0x0600BCFA RID: 48378 RVA: 0x0043A21A File Offset: 0x0043841A
		protected override void Named(string s)
		{
			this.engine.RenamableLabel = s;
			this.engine.nameHidden = false;
		}

		// Token: 0x04006C02 RID: 27650
		private readonly Building_GravEngine engine;
	}
}
