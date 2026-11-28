using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D22 RID: 7458
	public class FloatMenuOptionProvider_DraftedAttack : FloatMenuOptionProvider
	{
		// Token: 0x1700233F RID: 9023
		// (get) Token: 0x0600C0F3 RID: 49395 RVA: 0x004658B8 File Offset: 0x00463AB8
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002340 RID: 9024
		// (get) Token: 0x0600C0F4 RID: 49396 RVA: 0x004658BB File Offset: 0x00463ABB
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002341 RID: 9025
		// (get) Token: 0x0600C0F5 RID: 49397 RVA: 0x004658BE File Offset: 0x00463ABE
		protected override bool Multiselect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002342 RID: 9026
		// (get) Token: 0x0600C0F6 RID: 49398 RVA: 0x004658C1 File Offset: 0x00463AC1
		protected override bool MechanoidCanDo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C0F7 RID: 49399 RVA: 0x004658C4 File Offset: 0x00463AC4
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_DraftedAttack.<GetOptionsFor>d__8 <GetOptionsFor>d__ = new FloatMenuOptionProvider_DraftedAttack.<GetOptionsFor>d__8(-2);
			<GetOptionsFor>d__.<>4__this = this;
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}

		// Token: 0x0600C0F8 RID: 49400 RVA: 0x004658E4 File Offset: 0x00463AE4
		private static bool CanTarget(Thing clickedThing)
		{
			if (clickedThing.def.noRightClickDraftAttack && clickedThing.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			if (clickedThing.def.IsNonDeconstructibleAttackableBuilding)
			{
				return true;
			}
			BuildingProperties building = clickedThing.def.building;
			if (building != null && building.quickTargetable)
			{
				return true;
			}
			if (!clickedThing.def.destroyable)
			{
				return false;
			}
			if (clickedThing.HostileTo(Faction.OfPlayer))
			{
				return true;
			}
			Pawn pawn = clickedThing as Pawn;
			return pawn != null && pawn.NonHumanlikeOrWildMan();
		}

		// Token: 0x0600C0F9 RID: 49401 RVA: 0x0046596A File Offset: 0x00463B6A
		private static IEnumerable<FloatMenuOption> GetSingleSelectAttackOptions(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_DraftedAttack.<GetSingleSelectAttackOptions>d__10 <GetSingleSelectAttackOptions>d__ = new FloatMenuOptionProvider_DraftedAttack.<GetSingleSelectAttackOptions>d__10(-2);
			<GetSingleSelectAttackOptions>d__.<>3__clickedThing = clickedThing;
			<GetSingleSelectAttackOptions>d__.<>3__context = context;
			return <GetSingleSelectAttackOptions>d__;
		}

		// Token: 0x0600C0FA RID: 49402 RVA: 0x00465984 File Offset: 0x00463B84
		private FloatMenuOption GetMultiselectAttackOption(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_DraftedAttack.tmpPawns.Clear();
			string text = null;
			bool flag = clickedThing.HostileTo(Faction.OfPlayer);
			string text2 = null;
			foreach (Pawn pawn in context.ValidSelectedPawns)
			{
				if (FloatMenuOptionProvider_DraftedAttack.GetAttackAction(pawn, clickedThing, out text, out text2) != null)
				{
					FloatMenuOptionProvider_DraftedAttack.tmpPawns.Add(pawn);
				}
			}
			if (FloatMenuOptionProvider_DraftedAttack.tmpPawns.Count != 0)
			{
				FleckDef fleck = FloatMenuUtility.UseRangedAttack(FloatMenuOptionProvider_DraftedAttack.tmpPawns[0]) ? FleckDefOf.FeedbackShoot : FleckDefOf.FeedbackMelee;
				FloatMenuOption floatMenuOption = new FloatMenuOption(text ?? "Attack".Translate(clickedThing.Label, clickedThing), delegate()
				{
					FleckMaker.Static(clickedThing.DrawPos, clickedThing.Map, fleck, 1f);
					foreach (Pawn pawn2 in FloatMenuOptionProvider_DraftedAttack.tmpPawns)
					{
						string text3;
						string text4;
						Action attackAction = FloatMenuOptionProvider_DraftedAttack.GetAttackAction(pawn2, clickedThing, out text3, out text4);
						if (attackAction != null)
						{
							attackAction();
						}
					}
				}, MenuOptionPriority.AttackEnemy, null, null, 0f, null, null, true, 0);
				floatMenuOption.Priority = (flag ? MenuOptionPriority.AttackEnemy : MenuOptionPriority.VeryLow);
				bool autoTakeable;
				if (!flag)
				{
					BuildingProperties building = clickedThing.def.building;
					autoTakeable = (building != null && building.quickTargetable);
				}
				else
				{
					autoTakeable = true;
				}
				floatMenuOption.autoTakeable = autoTakeable;
				floatMenuOption.autoTakeablePriority = 40f;
				return floatMenuOption;
			}
			if (text2 != null)
			{
				return new FloatMenuOption((text ?? "Attack".Translate(clickedThing.Label, clickedThing)) + ": " + text2, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return null;
		}

		// Token: 0x0600C0FB RID: 49403 RVA: 0x00465B30 File Offset: 0x00463D30
		private static Action GetAttackAction(Pawn pawn, Thing target, out string label, out string failStr)
		{
			failStr = null;
			label = "Attack".Translate(target.Label, target);
			if (ModsConfig.BiotechActive && pawn.IsColonyMech && !MechanitorUtility.InMechanitorCommandRange(pawn, target))
			{
				failStr = "OutOfCommandRange".Translate();
				return null;
			}
			Action rangedAttackAction = FloatMenuOptionProvider_DraftedAttack.GetRangedAttackAction(pawn, target, out label, out failStr);
			if (rangedAttackAction != null)
			{
				return rangedAttackAction;
			}
			return FloatMenuOptionProvider_DraftedAttack.GetMeleeAttackAction(pawn, target, out label, out failStr);
		}

		// Token: 0x0600C0FC RID: 49404 RVA: 0x00465BAC File Offset: 0x00463DAC
		private static Action GetRangedAttackAction(Pawn pawn, Thing target, out string label, out string failStr)
		{
			failStr = null;
			label = "FireAt".Translate(target.Label, target);
			if (!FloatMenuUtility.UseRangedAttack(pawn))
			{
				return null;
			}
			label = "FireAt".Translate(target.Label, target);
			return FloatMenuUtility.GetRangedAttackAction(pawn, target, out failStr);
		}

		// Token: 0x0600C0FD RID: 49405 RVA: 0x00465C18 File Offset: 0x00463E18
		private static Action GetMeleeAttackAction(Pawn pawn, Thing target, out string label, out string failStr)
		{
			failStr = null;
			label = "Attack".Translate(target.Label, target);
			Pawn pawn2 = target as Pawn;
			if (pawn2 != null && pawn2.Downed)
			{
				label = "MeleeAttackToDeath".Translate(target.Label, target);
			}
			else
			{
				label = "MeleeAttack".Translate(target.Label, target);
			}
			return FloatMenuUtility.GetMeleeAttackAction(pawn, target, out failStr, false);
		}

		// Token: 0x0600C0FE RID: 49406 RVA: 0x00465CB0 File Offset: 0x00463EB0
		[Obsolete]
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			return base.GetSingleOptionFor(clickedThing, context);
		}

		// Token: 0x0600C0FF RID: 49407 RVA: 0x00465CBC File Offset: 0x00463EBC
		[Obsolete]
		private static FloatMenuOption GetSingleSelectAttackOption(Thing clickedThing, FloatMenuContext context)
		{
			string text;
			string text2;
			Action action = FloatMenuOptionProvider_DraftedAttack.GetAttackAction(context.FirstSelectedPawn, clickedThing, out text, out text2);
			FleckDef fleck = FloatMenuUtility.UseRangedAttack(context.FirstSelectedPawn) ? FleckDefOf.FeedbackShoot : FleckDefOf.FeedbackMelee;
			if (action != null)
			{
				return new FloatMenuOption(text ?? "Attack".Translate(clickedThing.Label, clickedThing), delegate()
				{
					FleckMaker.Static(clickedThing.DrawPos, clickedThing.Map, fleck, 1f);
					action();
				}, MenuOptionPriority.AttackEnemy, null, null, 0f, null, null, true, 0);
			}
			if (!text2.NullOrEmpty())
			{
				return new FloatMenuOption((text ?? "Attack".Translate(clickedThing.Label, clickedThing)) + ": " + text2, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return null;
		}

		// Token: 0x04006ED7 RID: 28375
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();
	}
}
