using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200158E RID: 5518
	public class Pawn_AbilityTracker : IExposable
	{
		// Token: 0x1700171E RID: 5918
		// (get) Token: 0x0600873A RID: 34618 RVA: 0x0030DEC4 File Offset: 0x0030C0C4
		public List<Ability> AllAbilitiesForReading
		{
			get
			{
				if (this.allAbilitiesCachedDirty)
				{
					this.allAbilitiesCached.Clear();
					this.allAbilitiesCached.AddRange(this.abilities);
					if (this.pawn.health != null)
					{
						foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
						{
							if (!hediff.def.abilities.NullOrEmpty<AbilityDef>())
							{
								this.allAbilitiesCached.AddRange(hediff.AllAbilitiesForReading);
							}
						}
					}
					if (this.pawn.equipment != null)
					{
						ThingWithComps primary = this.pawn.equipment.Primary;
						CompEquippableAbility compEquippableAbility = (primary != null) ? primary.TryGetComp<CompEquippableAbility>() : null;
						if (compEquippableAbility != null && compEquippableAbility.AbilityForReading != null)
						{
							this.allAbilitiesCached.Add(compEquippableAbility.AbilityForReading);
						}
					}
					if (this.pawn.apparel != null)
					{
						foreach (Apparel apparel in this.pawn.apparel.WornApparel)
						{
							this.allAbilitiesCached.AddRange(apparel.AllAbilitiesForReading);
						}
					}
					if (ModsConfig.AnomalyActive && this.pawn.IsMutant)
					{
						this.allAbilitiesCached.AddRange(this.pawn.mutant.AllAbilitiesForReading);
					}
					if (this.pawn.royalty != null)
					{
						this.allAbilitiesCached.AddRange(this.pawn.royalty.AllAbilitiesForReading);
					}
					if (ModsConfig.IdeologyActive)
					{
						Ideo ideo = this.pawn.Ideo;
						Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(this.pawn) : null;
						if (precept_Role != null && precept_Role.Active && !precept_Role.AbilitiesFor(this.pawn).NullOrEmpty<Ability>())
						{
							foreach (Ability ability in precept_Role.AbilitiesFor(this.pawn))
							{
								bool flag = false;
								if (!ability.def.requiredMemes.NullOrEmpty<MemeDef>())
								{
									foreach (MemeDef item in ability.def.requiredMemes)
									{
										if (!this.pawn.Ideo.memes.Contains(item))
										{
											flag = true;
											break;
										}
									}
								}
								if (!flag)
								{
									this.allAbilitiesCached.Add(ability);
								}
							}
						}
					}
					if (ModsConfig.AnomalyActive && this.pawn.IsMutant && this.pawn.mutant.Def.abilityWhitelist.Any<AbilityDef>())
					{
						this.allAbilitiesCached = (from a in this.allAbilitiesCached
						where this.pawn.mutant.Def.abilityWhitelist.Contains(a.def)
						select a).ToList<Ability>();
					}
					this.allAbilitiesCached.SortBy(delegate(Ability a)
					{
						AbilityCategoryDef category = a.def.category;
						if (category == null)
						{
							return 0;
						}
						return category.displayOrder;
					}, (Ability a) => a.def.displayOrder, (Ability a) => a.def.level);
					this.allAbilitiesCachedDirty = false;
				}
				return this.allAbilitiesCached;
			}
		}

		// Token: 0x0600873B RID: 34619 RVA: 0x0030E264 File Offset: 0x0030C464
		public Pawn_AbilityTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600873C RID: 34620 RVA: 0x0030E29C File Offset: 0x0030C49C
		public void AbilitiesTick()
		{
			for (int i = 0; i < this.AllAbilitiesForReading.Count; i++)
			{
				this.AllAbilitiesForReading[i].AbilityTick();
			}
		}

		// Token: 0x0600873D RID: 34621 RVA: 0x0030E2D0 File Offset: 0x0030C4D0
		public void GainAbility(AbilityDef def)
		{
			if (!this.abilities.Any((Ability a) => a.def == def))
			{
				this.abilities.Add(AbilityUtility.MakeAbility(def, this.pawn));
			}
			this.Notify_TemporaryAbilitiesChanged();
		}

		// Token: 0x0600873E RID: 34622 RVA: 0x0030E328 File Offset: 0x0030C528
		public void RemoveAbility(AbilityDef def)
		{
			Ability ability = this.abilities.FirstOrDefault((Ability x) => x.def == def);
			if (ability != null)
			{
				this.abilities.Remove(ability);
			}
			this.Notify_TemporaryAbilitiesChanged();
		}

		// Token: 0x0600873F RID: 34623 RVA: 0x0030E370 File Offset: 0x0030C570
		public Ability GetAbility(AbilityDef def, bool includeTemporary = false)
		{
			List<Ability> list = includeTemporary ? this.AllAbilitiesForReading : this.abilities;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == def)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06008740 RID: 34624 RVA: 0x0030E3B8 File Offset: 0x0030C5B8
		public List<Ability> AICastableAbilities(LocalTargetInfo target, bool offensive)
		{
			this.tmpAbilities.Clear();
			foreach (Ability ability in this.AllAbilitiesForReading)
			{
				if (ability.def.ai_IsOffensive == offensive && (ability.AICanTargetNow(target) || ability.AICanTargetNow(this.pawn)))
				{
					this.tmpAbilities.Add(ability);
				}
			}
			return this.tmpAbilities;
		}

		// Token: 0x06008741 RID: 34625 RVA: 0x0030E44C File Offset: 0x0030C64C
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_AbilityTracker.<GetGizmos>d__13 <GetGizmos>d__ = new Pawn_AbilityTracker.<GetGizmos>d__13(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06008742 RID: 34626 RVA: 0x0030E45C File Offset: 0x0030C65C
		public void Notify_TemporaryAbilitiesChanged()
		{
			this.allAbilitiesCachedDirty = true;
		}

		// Token: 0x06008743 RID: 34627 RVA: 0x0030E468 File Offset: 0x0030C668
		public void ExposeData()
		{
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, new object[]
			{
				this.pawn
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.abilities.RemoveAll((Ability a) => a.def == null || a.def == AbilityDefOf.Speech);
			}
		}

		// Token: 0x0400539C RID: 21404
		public Pawn pawn;

		// Token: 0x0400539D RID: 21405
		public List<Ability> abilities = new List<Ability>();

		// Token: 0x0400539E RID: 21406
		private bool allAbilitiesCachedDirty = true;

		// Token: 0x0400539F RID: 21407
		private List<Ability> allAbilitiesCached = new List<Ability>();

		// Token: 0x040053A0 RID: 21408
		private readonly List<Ability> tmpAbilities = new List<Ability>();
	}
}
