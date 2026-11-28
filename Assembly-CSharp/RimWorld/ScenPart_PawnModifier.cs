using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001792 RID: 6034
	public class ScenPart_PawnModifier : ScenPart
	{
		// Token: 0x0600964F RID: 38479 RVA: 0x003696C0 File Offset: 0x003678C0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.chance, "chance", 0f, false);
			Scribe_Values.Look<PawnGenerationContext>(ref this.context, "context", PawnGenerationContext.All, false);
			Scribe_Values.Look<bool>(ref this.hideOffMap, "hideOffMap", false, false);
		}

		// Token: 0x06009650 RID: 38480 RVA: 0x00369710 File Offset: 0x00367910
		protected void DoPawnModifierEditInterface(Rect rect)
		{
			Rect rect2 = rect.TopHalf();
			Rect rect3 = rect2.LeftPart(0.333f).Rounded();
			Rect rect4 = rect2.RightPart(0.666f).Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect3, "chance".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldPercent(rect4, ref this.chance, ref this.chanceBuf, 0f, 1f);
			Rect rect5 = rect.BottomHalf();
			Rect rect6 = rect5.LeftPart(0.333f).Rounded();
			Rect rect7 = rect5.RightPart(0.666f).Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect6, "context".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonText(rect7, this.context.ToStringHuman(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(PawnGenerationContext)))
				{
					PawnGenerationContext localCont2 = (PawnGenerationContext)obj;
					PawnGenerationContext localCont = localCont2;
					list.Add(new FloatMenuOption(localCont.ToStringHuman(), delegate()
					{
						this.context = localCont;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06009651 RID: 38481 RVA: 0x00369894 File Offset: 0x00367A94
		public override void Randomize()
		{
			this.chance = GenMath.RoundedHundredth(Rand.Range(0.05f, 1f));
			this.context = PawnGenerationContextUtility.GetRandom();
			this.hideOffMap = false;
		}

		// Token: 0x06009652 RID: 38482 RVA: 0x003698C2 File Offset: 0x00367AC2
		public override void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
			if (!this.context.Includes(context))
			{
				return;
			}
			if (this.hideOffMap && context == PawnGenerationContext.PlayerStarter)
			{
				return;
			}
			if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
			{
				this.ModifyNewPawn(pawn);
			}
		}

		// Token: 0x06009653 RID: 38483 RVA: 0x00369901 File Offset: 0x00367B01
		public override void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
			if (!this.context.Includes(context))
			{
				return;
			}
			if (this.hideOffMap && context == PawnGenerationContext.PlayerStarter)
			{
				return;
			}
			if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
			{
				this.ModifyPawnPostGenerate(pawn, redressed);
			}
		}

		// Token: 0x06009654 RID: 38484 RVA: 0x00369944 File Offset: 0x00367B44
		public override void PostMapGenerate(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			if (this.hideOffMap && this.context.Includes(PawnGenerationContext.PlayerStarter))
			{
				foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
				{
					if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
					{
						this.ModifyHideOffMapStartingPawnPostMapGenerate(pawn);
					}
				}
			}
		}

		// Token: 0x06009655 RID: 38485 RVA: 0x003699D4 File Offset: 0x00367BD4
		protected virtual void ModifyNewPawn(Pawn p)
		{
		}

		// Token: 0x06009656 RID: 38486 RVA: 0x003699D6 File Offset: 0x00367BD6
		protected virtual void ModifyPawnPostGenerate(Pawn p, bool redressed)
		{
		}

		// Token: 0x06009657 RID: 38487 RVA: 0x003699D8 File Offset: 0x00367BD8
		protected virtual void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
		}

		// Token: 0x06009658 RID: 38488 RVA: 0x003699DA File Offset: 0x00367BDA
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.context.GetHashCode() ^ this.chance.GetHashCode() ^ ((this.hideOffMap > false) ? 1 : 0);
		}

		// Token: 0x04005A45 RID: 23109
		protected float chance = 1f;

		// Token: 0x04005A46 RID: 23110
		protected PawnGenerationContext context;

		// Token: 0x04005A47 RID: 23111
		protected bool hideOffMap;

		// Token: 0x04005A48 RID: 23112
		private string chanceBuf;
	}
}
