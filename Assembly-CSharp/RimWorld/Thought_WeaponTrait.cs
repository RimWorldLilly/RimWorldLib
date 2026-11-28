using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E56 RID: 3670
	public class Thought_WeaponTrait : Thought_Memory
	{
		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x060061B8 RID: 25016 RVA: 0x00233D73 File Offset: 0x00231F73
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || !this.HasWeapon;
			}
		}

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x060061B9 RID: 25017 RVA: 0x00233D88 File Offset: 0x00231F88
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.Name.Named("WEAPON")).CapitalizeFirst();
			}
		}

		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x060061BA RID: 25018 RVA: 0x00233DC4 File Offset: 0x00231FC4
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Name.Named("WEAPON")).CapitalizeFirst();
			}
		}

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x060061BB RID: 25019 RVA: 0x00233DFE File Offset: 0x00231FFE
		protected bool HasWeapon
		{
			get
			{
				return this.weapon != null && !this.weapon.Destroyed;
			}
		}

		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x060061BC RID: 25020 RVA: 0x00233E18 File Offset: 0x00232018
		private string Name
		{
			get
			{
				CompGeneratedNames compGeneratedNames = this.weapon.TryGetComp<CompGeneratedNames>();
				if (compGeneratedNames != null)
				{
					return compGeneratedNames.Name;
				}
				return this.weapon.LabelNoCount;
			}
		}

		// Token: 0x060061BD RID: 25021 RVA: 0x00233E46 File Offset: 0x00232046
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<ThingWithComps>(ref this.weapon, "weapon", false);
		}

		// Token: 0x04003607 RID: 13831
		public ThingWithComps weapon;
	}
}
