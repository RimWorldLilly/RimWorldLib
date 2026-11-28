using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020019D0 RID: 6608
	public class CompUniqueWeapon : ThingComp
	{
		// Token: 0x17001D0A RID: 7434
		// (get) Token: 0x0600A636 RID: 42550 RVA: 0x003B45D3 File Offset: 0x003B27D3
		public CompProperties_UniqueWeapon Props
		{
			get
			{
				return (CompProperties_UniqueWeapon)this.props;
			}
		}

		// Token: 0x17001D0B RID: 7435
		// (get) Token: 0x0600A637 RID: 42551 RVA: 0x003B45E0 File Offset: 0x003B27E0
		public List<WeaponTraitDef> TraitsListForReading
		{
			get
			{
				return this.traits;
			}
		}

		// Token: 0x17001D0C RID: 7436
		// (get) Token: 0x0600A638 RID: 42552 RVA: 0x003B45E8 File Offset: 0x003B27E8
		public bool IgnoreAccuracyMaluses
		{
			get
			{
				if (this.ignoreAccuracyMaluses == null)
				{
					this.ignoreAccuracyMaluses = new bool?(false);
					using (List<WeaponTraitDef>.Enumerator enumerator = this.TraitsListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.ignoresAccuracyMaluses)
							{
								this.ignoreAccuracyMaluses = new bool?(true);
								break;
							}
						}
					}
				}
				return this.ignoreAccuracyMaluses.Value;
			}
		}

		// Token: 0x0600A639 RID: 42553 RVA: 0x003B466C File Offset: 0x003B286C
		public override void PostExposeData()
		{
			Scribe_Collections.Look<WeaponTraitDef>(ref this.traits, "traits", LookMode.Def, Array.Empty<object>());
			Scribe_Defs.Look<ColorDef>(ref this.color, "color");
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.traits == null)
				{
					this.traits = new List<WeaponTraitDef>();
				}
				this.Setup(true);
			}
		}

		// Token: 0x0600A63A RID: 42554 RVA: 0x003B46D3 File Offset: 0x003B28D3
		public override string TransformLabel(string label)
		{
			CompStyleable compStyleable = this.parent.compStyleable;
			bool flag;
			if (compStyleable == null)
			{
				flag = (null != null);
			}
			else
			{
				Precept_ThingStyle sourcePrecept = compStyleable.SourcePrecept;
				flag = (((sourcePrecept != null) ? sourcePrecept.Label : null) != null);
			}
			if (flag)
			{
				return label;
			}
			if (!this.name.NullOrEmpty())
			{
				return this.name;
			}
			return label;
		}

		// Token: 0x0600A63B RID: 42555 RVA: 0x003B4714 File Offset: 0x003B2914
		public override void PostPostMake()
		{
			if (!ModLister.CheckOdyssey("Unique Weapons"))
			{
				return;
			}
			this.InitializeTraits();
			CompQuality compQuality;
			if (this.parent.TryGetComp(out compQuality))
			{
				compQuality.SetQuality(QualityUtility.GenerateQuality(QualityGenerator.Super), new ArtGenerationContext?(ArtGenerationContext.Outsider));
			}
			List<string> list = new List<string>();
			this.color = (from c in DefDatabase<ColorDef>.AllDefs
			where c.colorType == ColorType.Weapon && c.randomlyPickable
			select c).RandomElement<ColorDef>();
			foreach (WeaponTraitDef weaponTraitDef in this.TraitsListForReading)
			{
				if (weaponTraitDef.forcedColor != null)
				{
					this.color = weaponTraitDef.forcedColor;
				}
				if (weaponTraitDef.traitAdjectives != null)
				{
					list.AddRange(weaponTraitDef.traitAdjectives);
				}
			}
			GrammarRequest request = default(GrammarRequest);
			request.Rules.Add(new Rule_String("weapon_type", this.Props.namerLabels.RandomElement<string>()));
			request.Rules.Add(new Rule_String("color", this.color.label));
			request.Rules.Add(new Rule_String("trait_adjective", list.RandomElement<string>()));
			foreach (Rule item in TaleData_Pawn.GenerateRandom(true).GetRules("ANYPAWN", request.Constants))
			{
				request.Rules.Add(item);
			}
			request.Includes.Add(RulePackDefOf.NamerUniqueWeapon);
			this.name = NameGenerator.GenerateName(request, null, false, "r_weapon_name", null).StripTags();
			CompArt compArt;
			if (this.parent.TryGetComp(out compArt))
			{
				compArt.Title = this.name;
			}
		}

		// Token: 0x0600A63C RID: 42556 RVA: 0x003B4908 File Offset: 0x003B2B08
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
				int randomInRange = CompUniqueWeapon.NumTraitsRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					IEnumerable<WeaponTraitDef> source = allDefs.Where(new Func<WeaponTraitDef, bool>(this.CanAddTrait));
					if (source.Any<WeaponTraitDef>())
					{
						this.AddTrait(source.RandomElementByWeight((WeaponTraitDef x) => x.commonality));
					}
				}
				this.Setup(false);
			}
		}

		// Token: 0x0600A63D RID: 42557 RVA: 0x003B49E0 File Offset: 0x003B2BE0
		public bool CanAddTrait(WeaponTraitDef trait)
		{
			if (!ModLister.CheckOdyssey("Unique Weapons"))
			{
				return false;
			}
			if (!this.Props.weaponCategories.Contains(trait.weaponCategory))
			{
				return false;
			}
			if (this.TraitsListForReading.Empty<WeaponTraitDef>() && !trait.canGenerateAlone)
			{
				return false;
			}
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				foreach (WeaponTraitDef other in this.traits)
				{
					if (trait.Overlaps(other))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600A63E RID: 42558 RVA: 0x003B4A88 File Offset: 0x003B2C88
		public void AddTrait(WeaponTraitDef traitDef)
		{
			if (!ModLister.CheckOdyssey("Unique Weapons"))
			{
				return;
			}
			this.traits.Add(traitDef);
		}

		// Token: 0x0600A63F RID: 42559 RVA: 0x003B4AA4 File Offset: 0x003B2CA4
		public void Setup(bool fromSave)
		{
			foreach (WeaponTraitDef weaponTraitDef in this.traits)
			{
				if (weaponTraitDef.abilityProps != null)
				{
					CompEquippableAbilityReloadable compEquippableAbilityReloadable = this.parent.TryGetComp<CompEquippableAbilityReloadable>();
					compEquippableAbilityReloadable.props = weaponTraitDef.abilityProps;
					if (!fromSave)
					{
						compEquippableAbilityReloadable.Notify_PropsChanged();
					}
				}
			}
		}

		// Token: 0x0600A640 RID: 42560 RVA: 0x003B4B1C File Offset: 0x003B2D1C
		public override void Notify_Equipped(Pawn pawn)
		{
			if (!ModLister.CheckOdyssey("Unique weapon"))
			{
				return;
			}
			foreach (WeaponTraitDef weaponTraitDef in this.traits)
			{
				weaponTraitDef.Worker.Notify_Equipped(pawn);
			}
		}

		// Token: 0x0600A641 RID: 42561 RVA: 0x003B4B80 File Offset: 0x003B2D80
		public void Notify_EquipmentLost(Pawn pawn)
		{
			foreach (WeaponTraitDef weaponTraitDef in this.traits)
			{
				weaponTraitDef.Worker.Notify_EquipmentLost(pawn);
			}
		}

		// Token: 0x0600A642 RID: 42562 RVA: 0x003B4BD8 File Offset: 0x003B2DD8
		public override float GetStatOffset(StatDef stat)
		{
			float num = 0f;
			foreach (WeaponTraitDef weaponTraitDef in this.TraitsListForReading)
			{
				num += weaponTraitDef.statOffsets.GetStatOffsetFromList(stat);
			}
			return num;
		}

		// Token: 0x0600A643 RID: 42563 RVA: 0x003B4C3C File Offset: 0x003B2E3C
		public override float GetStatFactor(StatDef stat)
		{
			float num = 1f;
			foreach (WeaponTraitDef weaponTraitDef in this.TraitsListForReading)
			{
				num *= weaponTraitDef.statFactors.GetStatFactorFromList(stat);
			}
			return num;
		}

		// Token: 0x0600A644 RID: 42564 RVA: 0x003B4CA0 File Offset: 0x003B2EA0
		public override void GetStatsExplanation(StatDef stat, StringBuilder sb, string whitespace = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (WeaponTraitDef weaponTraitDef in this.TraitsListForReading)
			{
				float statOffsetFromList = weaponTraitDef.statOffsets.GetStatOffsetFromList(stat);
				if (!Mathf.Approximately(statOffsetFromList, 0f))
				{
					stringBuilder.AppendLine(whitespace + "    " + weaponTraitDef.LabelCap + ": " + stat.Worker.ValueToString(statOffsetFromList, false, ToStringNumberSense.Offset));
				}
				float statFactorFromList = weaponTraitDef.statFactors.GetStatFactorFromList(stat);
				if (!Mathf.Approximately(statFactorFromList, 1f))
				{
					stringBuilder.AppendLine(whitespace + "    " + weaponTraitDef.LabelCap + ": " + stat.Worker.ValueToString(statFactorFromList, false, ToStringNumberSense.Factor));
				}
			}
			if (stringBuilder.Length == 0)
			{
				return;
			}
			sb.AppendLine(whitespace + "StatsReport_WeaponTraits".Translate() + ":");
			sb.Append(stringBuilder.ToString());
		}

		// Token: 0x0600A645 RID: 42565 RVA: 0x003B4DEC File Offset: 0x003B2FEC
		public override Color? ForceColor()
		{
			ColorDef colorDef = this.color;
			if (colorDef == null)
			{
				return null;
			}
			return new Color?(colorDef.color);
		}

		// Token: 0x0600A646 RID: 42566 RVA: 0x003B4E18 File Offset: 0x003B3018
		public override string CompInspectStringExtra()
		{
			if (this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				return null;
			}
			return "WeaponTraits".Translate() + ": " + (from x in this.traits
			select x.label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x0600A647 RID: 42567 RVA: 0x003B4E88 File Offset: 0x003B3088
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompUniqueWeapon.<SpecialDisplayStats>d__26 <SpecialDisplayStats>d__ = new CompUniqueWeapon.<SpecialDisplayStats>d__26(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600A648 RID: 42568 RVA: 0x003B4E98 File Offset: 0x003B3098
		public override string CompTipStringExtra()
		{
			return "WeaponTraits".Translate() + ": " + (from x in this.TraitsListForReading
			select x.label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x04006213 RID: 25107
		private List<WeaponTraitDef> traits = new List<WeaponTraitDef>();

		// Token: 0x04006214 RID: 25108
		private ColorDef color;

		// Token: 0x04006215 RID: 25109
		private string name;

		// Token: 0x04006216 RID: 25110
		private CompStyleable styleable;

		// Token: 0x04006217 RID: 25111
		private static readonly IntRange NumTraitsRange = new IntRange(1, 3);

		// Token: 0x04006218 RID: 25112
		private bool? ignoreAccuracyMaluses;
	}
}
