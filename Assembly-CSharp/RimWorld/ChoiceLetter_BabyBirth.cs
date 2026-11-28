using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D51 RID: 7505
	public class ChoiceLetter_BabyBirth : ChoiceLetter
	{
		// Token: 0x170023D9 RID: 9177
		// (get) Token: 0x0600C241 RID: 49729 RVA: 0x0046CBE4 File Offset: 0x0046ADE4
		public override bool CanShowInLetterStack
		{
			get
			{
				Pawn pawn = this.pawn;
				bool flag;
				if (pawn == null)
				{
					flag = false;
				}
				else
				{
					Faction faction = pawn.Faction;
					flag = ((faction != null) ? new bool?(faction.IsPlayer) : null).GetValueOrDefault();
				}
				if (flag)
				{
					Pawn pawn2 = this.pawn;
					if (pawn2 != null && pawn2.babyNamingDeadline >= 0)
					{
						return base.CanShowInLetterStack;
					}
				}
				return false;
			}
		}

		// Token: 0x170023DA RID: 9178
		// (get) Token: 0x0600C242 RID: 49730 RVA: 0x0046CC48 File Offset: 0x0046AE48
		public override bool ShouldAutomaticallyOpenLetter
		{
			get
			{
				Pawn pawn = this.pawn;
				return pawn != null && pawn.babyNamingDeadline >= 0 && base.ShouldAutomaticallyOpenLetter;
			}
		}

		// Token: 0x170023DB RID: 9179
		// (get) Token: 0x0600C243 RID: 49731 RVA: 0x0046CC6C File Offset: 0x0046AE6C
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				ChoiceLetter_BabyBirth.<get_Choices>d__6 <get_Choices>d__ = new ChoiceLetter_BabyBirth.<get_Choices>d__6(-2);
				<get_Choices>d__.<>4__this = this;
				return <get_Choices>d__;
			}
		}

		// Token: 0x0600C244 RID: 49732 RVA: 0x0046CC7C File Offset: 0x0046AE7C
		public void Start()
		{
			this.pawn = (this.lookTargets.TryGetPrimaryTarget().Thing as Pawn);
			if (this.pawn.babyNamingDeadline >= Find.TickManager.TicksGame)
			{
				base.StartTimeout(this.pawn.babyNamingDeadline - Find.TickManager.TicksGame);
			}
		}

		// Token: 0x0600C245 RID: 49733 RVA: 0x0046CCDC File Offset: 0x0046AEDC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			if (Scribe.mode != LoadSaveMode.Saving && this.pawn == null)
			{
				this.pawn = (this.lookTargets.TryGetPrimaryTarget().Thing as Pawn);
			}
		}

		// Token: 0x0600C246 RID: 49734 RVA: 0x0046CD30 File Offset: 0x0046AF30
		private void Rename()
		{
			string initialFirstNameOverride = null;
			NameTriple nameTriple = this.pawn.Name as NameTriple;
			if (nameTriple != null && nameTriple.First == "Baby".Translate().CapitalizeFirst())
			{
				Rand.PushState();
				Name name;
				try
				{
					Rand.Seed = this.pawn.thingIDNumber;
					Pawn pawn = this.pawn;
					NameStyle style = NameStyle.Full;
					string forcedLastName = null;
					bool forceNoNick = false;
					Pawn_GeneTracker genes = this.pawn.genes;
					name = PawnBioAndNameGenerator.GeneratePawnName(pawn, style, forcedLastName, forceNoNick, (genes != null) ? genes.Xenotype : null);
				}
				finally
				{
					Rand.PopState();
				}
				NameTriple nameTriple2 = name as NameTriple;
				initialFirstNameOverride = ((nameTriple2 != null) ? nameTriple2.First : ((NameSingle)name).Name);
			}
			Find.WindowStack.Add(this.pawn.NamePawnDialog(initialFirstNameOverride));
		}

		// Token: 0x04006F11 RID: 28433
		private Pawn pawn;
	}
}
