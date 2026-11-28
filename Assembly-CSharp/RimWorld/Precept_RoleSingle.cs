using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200174E RID: 5966
	public class Precept_RoleSingle : Precept_Role
	{
		// Token: 0x1700196B RID: 6507
		// (get) Token: 0x06009499 RID: 38041 RVA: 0x00362D69 File Offset: 0x00360F69
		public Pawn ChosenPawnValue
		{
			get
			{
				IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
				if (ideoRoleInstance == null)
				{
					return null;
				}
				return ideoRoleInstance.pawn;
			}
		}

		// Token: 0x1700196C RID: 6508
		// (get) Token: 0x0600949A RID: 38042 RVA: 0x00362D7C File Offset: 0x00360F7C
		public List<Ability> AbilitiesForReading
		{
			get
			{
				IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
				if (ideoRoleInstance == null)
				{
					return null;
				}
				return ideoRoleInstance.abilities;
			}
		}

		// Token: 0x0600949B RID: 38043 RVA: 0x00362D8F File Offset: 0x00360F8F
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			this.chosenPawn = new IdeoRoleInstance(this);
			base.Init(ideo, generatingFor);
		}

		// Token: 0x0600949C RID: 38044 RVA: 0x00362DA5 File Offset: 0x00360FA5
		public override void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
			base.Notify_MemberChangedFaction(p, oldFaction, newFaction);
			if (p == this.ChosenPawnValue && oldFaction.IsPlayer)
			{
				this.Assign(null, false);
			}
		}

		// Token: 0x0600949D RID: 38045 RVA: 0x00362DC9 File Offset: 0x00360FC9
		public override IEnumerable<Pawn> ChosenPawns()
		{
			Precept_RoleSingle.<ChosenPawns>d__7 <ChosenPawns>d__ = new Precept_RoleSingle.<ChosenPawns>d__7(-2);
			<ChosenPawns>d__.<>4__this = this;
			return <ChosenPawns>d__;
		}

		// Token: 0x0600949E RID: 38046 RVA: 0x00362DDC File Offset: 0x00360FDC
		public override void RecacheActivity()
		{
			int colonistBelieverCountCached = this.ideo.ColonistBelieverCountCached;
			bool flag = Faction.OfPlayer.ideos.Has(this.ideo);
			if (this.active && this.def.deactivationBelieverCount >= 0 && colonistBelieverCountCached <= this.def.deactivationBelieverCount && !this.def.leaderRole)
			{
				this.active = false;
				if (flag)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRoleInactive".Translate(this.name).CapitalizeFirst(), "LetterLabelRoleInactiveDesc".Translate(this.ideo.memberName, this.def.deactivationBelieverCount, this.def.activationBelieverCount, this.Named("ROLE")).CapitalizeFirst(), LetterDefOf.NeutralEvent, null, 0, true);
				}
				if (this.ChosenPawnValue != null)
				{
					if (flag)
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRoleLost".Translate(this.ChosenPawnValue.Named("PAWN"), this.Named("ROLE")), "LetterRoleLostDesc".Translate(this.ChosenPawnValue.Named("PAWN"), this.Named("ROLE")) + " " + "LetterRoleLostReasonLowBelieversDesc".Translate(this.ideo.memberName).CapitalizeFirst(), LetterDefOf.NeutralEvent, this.ChosenPawnValue, null, null, null, null, 0, true);
					}
					base.Notify_PawnUnassigned(this.ChosenPawnValue);
					this.chosenPawn.pawn = null;
				}
			}
			if (!this.active && this.def.activationBelieverCount >= 0 && (colonistBelieverCountCached >= this.def.activationBelieverCount || this.def.leaderRole))
			{
				this.active = true;
				if (flag && !this.def.leaderRole && this.def.activationBelieverCount > 0 && Find.TickManager.TicksGame > 1)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRoleActive".Translate(this.name).CapitalizeFirst(), "LetterLabelRoleActiveDesc".Translate(this.ideo.memberName, this.def.activationBelieverCount, this.Named("ROLE")).CapitalizeFirst(), LetterDefOf.NeutralEvent, null, 0, true);
				}
			}
			if (this.ChosenPawnValue != null && !base.ValidatePawn(this.ChosenPawnValue))
			{
				base.Notify_PawnUnassigned(this.ChosenPawnValue);
				this.chosenPawn.pawn = null;
			}
		}

		// Token: 0x0600949F RID: 38047 RVA: 0x003630A0 File Offset: 0x003612A0
		public override void Assign(Pawn p, bool addThoughts)
		{
			if (p != this.ChosenPawnValue)
			{
				if (p != null && !base.ValidatePawn(p))
				{
					Log.Error("Invalid pawn assigned to " + base.LabelCap + " role. pawn=" + p.GetUniqueLoadID());
				}
				if (this.ChosenPawnValue != null && addThoughts)
				{
					if (p != null)
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRoleLost".Translate(this.ChosenPawnValue.Named("PAWN"), this.Named("ROLE")), "LetterRoleLostDesc".Translate(this.ChosenPawnValue.Named("PAWN"), this.Named("ROLE")) + " " + "LetterRoleLostReasonUnassignedDesc".Translate(this.ChosenPawnValue.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NeutralEvent, this.ChosenPawnValue, null, null, null, null, 0, true);
					}
					this.ChosenPawnValue.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleLost, this), null);
				}
				Pawn chosenPawnValue = this.ChosenPawnValue;
				this.chosenPawn.pawn = p;
				base.Notify_PawnUnassigned(chosenPawnValue);
				base.Notify_PawnAssigned(p);
				if (this.def.leaderRole && Current.ProgramState == ProgramState.Playing)
				{
					Faction ofPlayer = Faction.OfPlayer;
					if (ofPlayer != null)
					{
						ofPlayer.leader = p;
						foreach (Ideo ideo in ofPlayer.ideos.AllIdeos)
						{
							foreach (Precept precept in ideo.PreceptsListForReading)
							{
								if (precept != this)
								{
									Precept_Role precept_Role = precept as Precept_Role;
									if (precept_Role != null && precept_Role.def.leaderRole)
									{
										precept_Role.Assign(null, true);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060094A0 RID: 38048 RVA: 0x003632B4 File Offset: 0x003614B4
		public override void FillOrUpdateAbilities()
		{
			if (!this.def.grantedAbilities.NullOrEmpty<AbilityDef>())
			{
				IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
				Pawn chosenPawnValue = this.ChosenPawnValue;
				IdeoRoleInstance ideoRoleInstance2 = this.chosenPawn;
				ideoRoleInstance.abilities = base.FillOrUpdateAbilityList(chosenPawnValue, (ideoRoleInstance2 != null) ? ideoRoleInstance2.abilities : null);
			}
		}

		// Token: 0x060094A1 RID: 38049 RVA: 0x003632F1 File Offset: 0x003614F1
		public override List<Ability> AbilitiesFor(Pawn p)
		{
			return this.chosenPawn.abilities;
		}

		// Token: 0x060094A2 RID: 38050 RVA: 0x003632FE File Offset: 0x003614FE
		public override Pawn ChosenPawnSingle()
		{
			IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
			if (ideoRoleInstance == null)
			{
				return null;
			}
			return ideoRoleInstance.pawn;
		}

		// Token: 0x060094A3 RID: 38051 RVA: 0x00363311 File Offset: 0x00361511
		public override bool IsAssigned(Pawn p)
		{
			IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
			return ((ideoRoleInstance != null) ? ideoRoleInstance.pawn : null) == p;
		}

		// Token: 0x060094A4 RID: 38052 RVA: 0x00363328 File Offset: 0x00361528
		public override void Unassign(Pawn p, bool generateThoughts)
		{
			this.Assign(null, generateThoughts);
		}

		// Token: 0x060094A5 RID: 38053 RVA: 0x00363334 File Offset: 0x00361534
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<IdeoRoleInstance>(ref this.chosenPawn, "chosenPawn", new object[]
			{
				this
			});
			if (GameDataSaveLoader.IsSavingOrLoadingExternalIdeo && Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.chosenPawn = new IdeoRoleInstance(this);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ChosenPawnValue != null && !base.ValidatePawn(this.ChosenPawnValue))
				{
					Pawn pawn = this.chosenPawn.pawn;
					this.chosenPawn.pawn = null;
					base.Notify_PawnUnassigned(pawn);
				}
				this.chosenPawn.sourceRole = this;
				this.FillOrUpdateAbilities();
			}
		}

		// Token: 0x060094A6 RID: 38054 RVA: 0x003633CB File Offset: 0x003615CB
		public override void CopyTo(Precept precept)
		{
			base.CopyTo(precept);
			((Precept_RoleSingle)precept).chosenPawn = this.chosenPawn;
		}

		// Token: 0x0400599E RID: 22942
		public IdeoRoleInstance chosenPawn;
	}
}
