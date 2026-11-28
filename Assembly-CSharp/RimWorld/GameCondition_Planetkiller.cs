using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200114D RID: 4429
	public class GameCondition_Planetkiller : GameCondition
	{
		// Token: 0x1700130E RID: 4878
		// (get) Token: 0x06006F33 RID: 28467 RVA: 0x0026EBD8 File Offset: 0x0026CDD8
		public override string TooltipString
		{
			get
			{
				Vector2 location;
				if (Find.CurrentMap != null)
				{
					location = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
				}
				else
				{
					location = default(Vector2);
				}
				string text = this.def.LabelCap;
				text += "\n";
				text = text + "\n" + this.Description;
				text = string.Concat(new string[]
				{
					text,
					"\n",
					"ImpactDate".Translate().CapitalizeFirst(),
					": ",
					GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.startTick + base.Duration), location).Colorize(ColoredText.DateTimeColor)
				});
				return string.Concat(new string[]
				{
					text,
					"\n",
					"TimeLeft".Translate().CapitalizeFirst(),
					": ",
					base.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor)
				});
			}
		}

		// Token: 0x06006F34 RID: 28468 RVA: 0x0026ECF4 File Offset: 0x0026CEF4
		public override void GameConditionTick()
		{
			base.GameConditionTick();
			if (base.TicksLeft <= 179)
			{
				Find.ActiveLesson.Deactivate();
				if (base.TicksLeft == 179)
				{
					SoundDefOf.PlanetkillerImpact.PlayOneShotOnCamera(null);
				}
				if (base.TicksLeft == 90)
				{
					ScreenFader.StartFade(GameCondition_Planetkiller.FadeColor, 1f);
				}
			}
		}

		// Token: 0x06006F35 RID: 28469 RVA: 0x0026ED4F File Offset: 0x0026CF4F
		public override void End()
		{
			base.End();
			this.Impact();
		}

		// Token: 0x06006F36 RID: 28470 RVA: 0x0026ED5D File Offset: 0x0026CF5D
		private void Impact()
		{
			ScreenFader.SetColor(Color.clear);
			GenGameEnd.EndGameDialogMessage("GameOverPlanetkillerImpact".Translate(Find.World.info.name), false, GameCondition_Planetkiller.FadeColor);
		}

		// Token: 0x04004716 RID: 18198
		private const int SoundDuration = 179;

		// Token: 0x04004717 RID: 18199
		private const int FadeDuration = 90;

		// Token: 0x04004718 RID: 18200
		private static readonly Color FadeColor = Color.white;
	}
}
