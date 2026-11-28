using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015B8 RID: 5560
	public class Pawn_FoodRestrictionTracker : IExposable
	{
		// Token: 0x170017C3 RID: 6083
		// (get) Token: 0x060089CD RID: 35277 RVA: 0x0031E690 File Offset: 0x0031C890
		// (set) Token: 0x060089CE RID: 35278 RVA: 0x0031E6E6 File Offset: 0x0031C8E6
		public FoodPolicy CurrentFoodPolicy
		{
			get
			{
				if (this.pawn.IsMutant && this.pawn.mutant.Def.disablePolicies)
				{
					return null;
				}
				if (this.curPolicy == null)
				{
					this.curPolicy = Current.Game.foodRestrictionDatabase.DefaultFoodRestriction();
				}
				return this.curPolicy;
			}
			set
			{
				this.curPolicy = value;
			}
		}

		// Token: 0x170017C4 RID: 6084
		// (get) Token: 0x060089CF RID: 35279 RVA: 0x0031E6F0 File Offset: 0x0031C8F0
		public bool Configurable
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && !this.pawn.Destroyed && (this.pawn.Faction == Faction.OfPlayer || this.pawn.HostFaction == Faction.OfPlayer);
			}
		}

		// Token: 0x060089D0 RID: 35280 RVA: 0x0031E744 File Offset: 0x0031C944
		public Pawn_FoodRestrictionTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060089D1 RID: 35281 RVA: 0x0031E753 File Offset: 0x0031C953
		public Pawn_FoodRestrictionTracker()
		{
		}

		// Token: 0x060089D2 RID: 35282 RVA: 0x0031E75C File Offset: 0x0031C95C
		public FoodPolicy GetCurrentRespectedRestriction(Pawn getter = null)
		{
			if (!this.Configurable)
			{
				return null;
			}
			if (this.pawn.Faction != Faction.OfPlayer && (getter == null || getter.Faction != Faction.OfPlayer))
			{
				return null;
			}
			if (this.pawn.InMentalState)
			{
				return null;
			}
			return this.CurrentFoodPolicy;
		}

		// Token: 0x060089D3 RID: 35283 RVA: 0x0031E7AC File Offset: 0x0031C9AC
		public bool BabyFoodAllowed(ThingDef food)
		{
			this.TrySetupAllowedBabyFoodTypes();
			if (!ITab_Pawn_Feeding.BabyConsumableFoods.Contains(food))
			{
				return false;
			}
			if (!this.allowedBabyFoodTypes.ContainsKey(food))
			{
				this.allowedBabyFoodTypes.Add(food, true);
			}
			return this.allowedBabyFoodTypes[food];
		}

		// Token: 0x060089D4 RID: 35284 RVA: 0x0031E7EA File Offset: 0x0031C9EA
		public void SetBabyFoodAllowed(ThingDef food, bool allowed)
		{
			this.TrySetupAllowedBabyFoodTypes();
			if (!ITab_Pawn_Feeding.BabyConsumableFoods.Contains(food))
			{
				return;
			}
			this.allowedBabyFoodTypes.SetOrAdd(food, allowed);
		}

		// Token: 0x060089D5 RID: 35285 RVA: 0x0031E810 File Offset: 0x0031CA10
		private void TrySetupAllowedBabyFoodTypes()
		{
			if (this.allowedBabyFoodTypes == null)
			{
				this.allowedBabyFoodTypes = new Dictionary<ThingDef, bool>();
				foreach (ThingDef key in ITab_Pawn_Feeding.BabyConsumableFoods)
				{
					this.allowedBabyFoodTypes.Add(key, true);
				}
			}
		}

		// Token: 0x060089D6 RID: 35286 RVA: 0x0031E87C File Offset: 0x0031CA7C
		public void ExposeData()
		{
			Scribe_References.Look<FoodPolicy>(ref this.curPolicy, "curRestriction", false);
			Scribe_Collections.Look<ThingDef, bool>(ref this.allowedBabyFoodTypes, "allowedBabyFoodTypes", LookMode.Def, LookMode.Value);
		}

		// Token: 0x040054DA RID: 21722
		public Pawn pawn;

		// Token: 0x040054DB RID: 21723
		private FoodPolicy curPolicy;

		// Token: 0x040054DC RID: 21724
		private Dictionary<ThingDef, bool> allowedBabyFoodTypes;
	}
}
