using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001875 RID: 6261
	public class CompEquippableAbility : CompEquippable
	{
		// Token: 0x17001AC6 RID: 6854
		// (get) Token: 0x06009C9B RID: 40091 RVA: 0x0038600A File Offset: 0x0038420A
		private CompProperties_EquippableAbility Props
		{
			get
			{
				return this.props as CompProperties_EquippableAbility;
			}
		}

		// Token: 0x17001AC7 RID: 6855
		// (get) Token: 0x06009C9C RID: 40092 RVA: 0x00386017 File Offset: 0x00384217
		public Ability AbilityForReading
		{
			get
			{
				if (this.ability == null && this.Props.abilityDef != null)
				{
					this.ability = AbilityUtility.MakeAbility(this.Props.abilityDef, base.Holder);
				}
				return this.ability;
			}
		}

		// Token: 0x06009C9D RID: 40093 RVA: 0x00386050 File Offset: 0x00384250
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (base.Holder != null)
			{
				this.AbilityForReading.pawn = base.Holder;
				this.AbilityForReading.verb.caster = base.Holder;
			}
		}

		// Token: 0x06009C9E RID: 40094 RVA: 0x00386088 File Offset: 0x00384288
		public virtual void UsedOnce()
		{
		}

		// Token: 0x06009C9F RID: 40095 RVA: 0x0038608A File Offset: 0x0038428A
		public override void Notify_Equipped(Pawn pawn)
		{
			if (this.AbilityForReading == null)
			{
				return;
			}
			this.AbilityForReading.pawn = pawn;
			this.AbilityForReading.verb.caster = pawn;
			pawn.abilities.Notify_TemporaryAbilitiesChanged();
		}

		// Token: 0x06009CA0 RID: 40096 RVA: 0x003860BD File Offset: 0x003842BD
		public override void Notify_Unequipped(Pawn pawn)
		{
			if (this.AbilityForReading == null)
			{
				return;
			}
			pawn.abilities.Notify_TemporaryAbilitiesChanged();
		}

		// Token: 0x06009CA1 RID: 40097 RVA: 0x003860D4 File Offset: 0x003842D4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<Ability>(ref this.ability, "ability", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Holder != null && this.AbilityForReading != null)
			{
				this.AbilityForReading.pawn = base.Holder;
				this.AbilityForReading.verb.caster = base.Holder;
			}
		}

		// Token: 0x04005CF8 RID: 23800
		private Ability ability;
	}
}
