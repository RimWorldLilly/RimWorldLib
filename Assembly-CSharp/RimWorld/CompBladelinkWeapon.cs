using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183D RID: 6205
	public class CompBladelinkWeapon : CompBiocodable
	{
		// Token: 0x17001A81 RID: 6785
		// (get) Token: 0x06009B4D RID: 39757 RVA: 0x0037FEA3 File Offset: 0x0037E0A3
		public List<WeaponTraitDef> TraitsListForReading
		{
			get
			{
				return this.traits;
			}
		}

		// Token: 0x17001A82 RID: 6786
		// (get) Token: 0x06009B4E RID: 39758 RVA: 0x0037FEAB File Offset: 0x0037E0AB
		public int TicksSinceLastKill
		{
			get
			{
				if (this.lastKillTick < 0)
				{
					return 0;
				}
				return Find.TickManager.TicksAbs - this.lastKillTick;
			}
		}

		// Token: 0x17001A83 RID: 6787
		// (get) Token: 0x06009B4F RID: 39759 RVA: 0x0037FECC File Offset: 0x0037E0CC
		public override bool Biocodable
		{
			get
			{
				if (!this.traits.NullOrEmpty<WeaponTraitDef>())
				{
					for (int i = 0; i < this.traits.Count; i++)
					{
						if (this.traits[i].neverBond)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x06009B50 RID: 39760 RVA: 0x0037FF12 File Offset: 0x0037E112
		public override void PostPostMake()
		{
			this.InitializeTraits();
		}

		// Token: 0x06009B51 RID: 39761 RVA: 0x0037FF1A File Offset: 0x0037E11A
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			this.UnCode();
		}

		// Token: 0x06009B52 RID: 39762 RVA: 0x0037FF22 File Offset: 0x0037E122
		public override void Notify_MapRemoved()
		{
			this.UnCode();
		}

		// Token: 0x06009B53 RID: 39763 RVA: 0x0037FF2C File Offset: 0x0037E12C
		private void InitializeTraits()
		{
			IEnumerable<WeaponTraitDef> allDefs = DefDatabase<WeaponTraitDef>.AllDefs;
			if (this.traits == null)
			{
				this.traits = new List<WeaponTraitDef>();
			}
			Map mapBeingGenerated = MapGenerator.mapBeingGenerated;
			using (new RandBlock((mapBeingGenerated != null) ? mapBeingGenerated.NextGenSeed : this.parent.HashOffset()))
			{
				int randomInRange = CompBladelinkWeapon.TraitsRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					IEnumerable<WeaponTraitDef> source = allDefs.Where(new Func<WeaponTraitDef, bool>(this.CanAddTrait));
					if (source.Any<WeaponTraitDef>())
					{
						this.traits.Add(source.RandomElementByWeight((WeaponTraitDef x) => x.commonality));
					}
				}
			}
		}

		// Token: 0x06009B54 RID: 39764 RVA: 0x00380000 File Offset: 0x0037E200
		private bool CanAddTrait(WeaponTraitDef trait)
		{
			if (trait.weaponCategory != WeaponCategoryDefOf.BladeLink)
			{
				return false;
			}
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					if (trait.Overlaps(this.traits[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06009B55 RID: 39765 RVA: 0x00380058 File Offset: 0x0037E258
		public override void Notify_Equipped(Pawn pawn)
		{
			if (!ModLister.CheckRoyalty("Persona weapon"))
			{
				return;
			}
			base.Notify_Equipped(pawn);
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_Equipped(pawn);
				}
			}
		}

		// Token: 0x06009B56 RID: 39766 RVA: 0x003800B4 File Offset: 0x0037E2B4
		public override void CodeFor(Pawn pawn)
		{
			if (!base.Biocodable)
			{
				return;
			}
			if (pawn.IsColonistPlayerControlled && base.CodedPawn == null)
			{
				Find.LetterStack.ReceiveLetter("LetterBladelinkWeaponBondedLabel".Translate(pawn.Named("PAWN"), this.parent.Named("WEAPON")), "LetterBladelinkWeaponBonded".Translate(pawn.Named("PAWN"), this.parent.Named("WEAPON")), LetterDefOf.PositiveEvent, new LookTargets(pawn), null, null, null, null, 0, true);
			}
			base.CodeFor(pawn);
		}

		// Token: 0x06009B57 RID: 39767 RVA: 0x00380148 File Offset: 0x0037E348
		protected override void OnCodedFor(Pawn pawn)
		{
			this.lastKillTick = GenTicks.TicksAbs;
			pawn.equipment.bondedWeapon = this.parent;
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_Bonded(pawn);
				}
			}
		}

		// Token: 0x06009B58 RID: 39768 RVA: 0x003801AC File Offset: 0x0037E3AC
		public override void Notify_KilledPawn(Pawn pawn)
		{
			this.lastKillTick = Find.TickManager.TicksAbs;
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_KilledPawn(pawn);
				}
			}
		}

		// Token: 0x06009B59 RID: 39769 RVA: 0x00380204 File Offset: 0x0037E404
		public void Notify_EquipmentLost(Pawn pawn)
		{
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_EquipmentLost(pawn);
				}
			}
		}

		// Token: 0x06009B5A RID: 39770 RVA: 0x0038024C File Offset: 0x0037E44C
		public void Notify_WieldedOtherWeapon()
		{
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_OtherWeaponWielded(this);
				}
			}
		}

		// Token: 0x06009B5B RID: 39771 RVA: 0x00380294 File Offset: 0x0037E494
		public override void UnCode()
		{
			if (base.CodedPawn != null)
			{
				base.CodedPawn.equipment.bondedWeapon = null;
				if (!this.traits.NullOrEmpty<WeaponTraitDef>())
				{
					for (int i = 0; i < this.traits.Count; i++)
					{
						this.traits[i].Worker.Notify_Unbonded(base.CodedPawn);
					}
				}
			}
			base.UnCode();
			this.lastKillTick = -1;
		}

		// Token: 0x06009B5C RID: 39772 RVA: 0x00380308 File Offset: 0x0037E508
		public override string CompInspectStringExtra()
		{
			string text = "";
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				text += "Stat_Thing_PersonaWeaponTrait_Label".Translate() + ": " + (from x in this.traits
				select x.label).ToCommaList(false, false).CapitalizeFirst();
			}
			if (this.Biocodable)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				if (base.CodedPawn == null)
				{
					text += "NotBonded".Translate();
				}
				else
				{
					text += "BondedWith".Translate(base.CodedPawnLabel.ApplyTag(TagType.Name, null)).Resolve();
				}
			}
			return text;
		}

		// Token: 0x06009B5D RID: 39773 RVA: 0x003803EC File Offset: 0x0037E5EC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastKillTick, "lastKillTick", -1, false);
			Scribe_Collections.Look<WeaponTraitDef>(ref this.traits, "traits", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Scribe_Values.Look<bool>(ref this.oldBonded, "bonded", false, false);
				Scribe_Values.Look<string>(ref this.oldBondedPawnLabel, "bondedPawnLabel", null, false);
				Scribe_References.Look<Pawn>(ref this.oldBondedPawn, "bondedPawn", true);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.oldBonded)
				{
					this.CodeFor(this.oldBondedPawn);
				}
				if (this.traits == null)
				{
					this.traits = new List<WeaponTraitDef>();
				}
				if (this.oldBondedPawn != null)
				{
					if (string.IsNullOrEmpty(this.oldBondedPawnLabel) || !this.oldBonded)
					{
						this.codedPawnLabel = this.oldBondedPawn.Name.ToStringFull;
						this.biocoded = true;
					}
					if (this.oldBondedPawn.equipment.bondedWeapon == null)
					{
						this.oldBondedPawn.equipment.bondedWeapon = this.parent;
						return;
					}
					if (this.oldBondedPawn.equipment.bondedWeapon != this.parent)
					{
						this.UnCode();
					}
				}
			}
		}

		// Token: 0x06009B5E RID: 39774 RVA: 0x00380517 File Offset: 0x0037E717
		public override string TransformLabel(string label)
		{
			return label;
		}

		// Token: 0x06009B5F RID: 39775 RVA: 0x0038051A File Offset: 0x0037E71A
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompBladelinkWeapon.<SpecialDisplayStats>d__27 <SpecialDisplayStats>d__ = new CompBladelinkWeapon.<SpecialDisplayStats>d__27(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x04005C53 RID: 23635
		private int lastKillTick = -1;

		// Token: 0x04005C54 RID: 23636
		private List<WeaponTraitDef> traits = new List<WeaponTraitDef>();

		// Token: 0x04005C55 RID: 23637
		private static readonly IntRange TraitsRange = new IntRange(1, 2);

		// Token: 0x04005C56 RID: 23638
		private bool oldBonded;

		// Token: 0x04005C57 RID: 23639
		private string oldBondedPawnLabel;

		// Token: 0x04005C58 RID: 23640
		private Pawn oldBondedPawn;
	}
}
