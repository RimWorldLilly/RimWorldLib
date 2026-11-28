using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D12 RID: 7442
	public abstract class FloatMenuOptionProvider
	{
		// Token: 0x170022FC RID: 8956
		// (get) Token: 0x0600C072 RID: 49266
		protected abstract bool Drafted { get; }

		// Token: 0x170022FD RID: 8957
		// (get) Token: 0x0600C073 RID: 49267
		protected abstract bool Undrafted { get; }

		// Token: 0x170022FE RID: 8958
		// (get) Token: 0x0600C074 RID: 49268
		protected abstract bool Multiselect { get; }

		// Token: 0x170022FF RID: 8959
		// (get) Token: 0x0600C075 RID: 49269 RVA: 0x00463554 File Offset: 0x00461754
		protected virtual bool RequiresManipulation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002300 RID: 8960
		// (get) Token: 0x0600C076 RID: 49270 RVA: 0x00463557 File Offset: 0x00461757
		protected virtual bool MechanoidCanDo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002301 RID: 8961
		// (get) Token: 0x0600C077 RID: 49271 RVA: 0x0046355A File Offset: 0x0046175A
		protected virtual bool CanSelfTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002302 RID: 8962
		// (get) Token: 0x0600C078 RID: 49272 RVA: 0x0046355D File Offset: 0x0046175D
		public virtual bool CanTargetDespawned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002303 RID: 8963
		// (get) Token: 0x0600C079 RID: 49273 RVA: 0x00463560 File Offset: 0x00461760
		protected virtual bool IgnoreFogged
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C07A RID: 49274 RVA: 0x00463564 File Offset: 0x00461764
		public virtual bool SelectedPawnValid(Pawn pawn, FloatMenuContext context)
		{
			return (!pawn.IsMutant || pawn.mutant.Def.whitelistedFloatMenuProviders == null || pawn.mutant.Def.whitelistedFloatMenuProviders.Contains(FloatMenuMakerMap.currentProvider.GetType())) && (this.Drafted || !pawn.Drafted) && (this.Undrafted || pawn.Drafted) && (this.MechanoidCanDo || !pawn.RaceProps.IsMechanoid) && (!this.RequiresManipulation || pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
		}

		// Token: 0x0600C07B RID: 49275 RVA: 0x0046360C File Offset: 0x0046180C
		public virtual bool TargetThingValid(Thing thing, FloatMenuContext context)
		{
			if (!this.CanTargetDespawned && !thing.Spawned)
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			return pawn == null || this.TargetPawnValid(pawn, context);
		}

		// Token: 0x0600C07C RID: 49276 RVA: 0x00463642 File Offset: 0x00461842
		public virtual bool TargetPawnValid(Pawn pawn, FloatMenuContext context)
		{
			return this.CanSelfTarget || pawn != context.FirstSelectedPawn;
		}

		// Token: 0x0600C07D RID: 49277 RVA: 0x00463658 File Offset: 0x00461858
		public virtual bool Applies(FloatMenuContext context)
		{
			return (this.Multiselect || !context.IsMultiselect) && (!this.IgnoreFogged || !context.ClickedCell.Fogged(context.map)) && this.AppliesInt(context);
		}

		// Token: 0x0600C07E RID: 49278 RVA: 0x00463695 File Offset: 0x00461895
		protected virtual bool AppliesInt(FloatMenuContext context)
		{
			return true;
		}

		// Token: 0x0600C07F RID: 49279 RVA: 0x00463698 File Offset: 0x00461898
		public virtual IEnumerable<FloatMenuOption> GetOptions(FloatMenuContext context)
		{
			FloatMenuOptionProvider.<GetOptions>d__21 <GetOptions>d__ = new FloatMenuOptionProvider.<GetOptions>d__21(-2);
			<GetOptions>d__.<>4__this = this;
			<GetOptions>d__.<>3__context = context;
			return <GetOptions>d__;
		}

		// Token: 0x0600C080 RID: 49280 RVA: 0x004636AF File Offset: 0x004618AF
		public virtual IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider.<GetOptionsFor>d__22 <GetOptionsFor>d__ = new FloatMenuOptionProvider.<GetOptionsFor>d__22(-2);
			<GetOptionsFor>d__.<>4__this = this;
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}

		// Token: 0x0600C081 RID: 49281 RVA: 0x004636CD File Offset: 0x004618CD
		public virtual IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			FloatMenuOptionProvider.<GetOptionsFor>d__23 <GetOptionsFor>d__ = new FloatMenuOptionProvider.<GetOptionsFor>d__23(-2);
			<GetOptionsFor>d__.<>4__this = this;
			<GetOptionsFor>d__.<>3__clickedPawn = clickedPawn;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}

		// Token: 0x0600C082 RID: 49282 RVA: 0x004636EB File Offset: 0x004618EB
		protected virtual FloatMenuOption GetSingleOption(FloatMenuContext context)
		{
			return null;
		}

		// Token: 0x0600C083 RID: 49283 RVA: 0x004636EE File Offset: 0x004618EE
		protected virtual FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			return null;
		}

		// Token: 0x0600C084 RID: 49284 RVA: 0x004636F1 File Offset: 0x004618F1
		protected virtual FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			return null;
		}
	}
}
