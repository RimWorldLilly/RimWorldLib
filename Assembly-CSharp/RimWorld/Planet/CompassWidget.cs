using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023E6 RID: 9190
	[StaticConstructorOnStartup]
	public static class CompassWidget
	{
		// Token: 0x17002876 RID: 10358
		// (get) Token: 0x0600E574 RID: 58740 RVA: 0x00553828 File Offset: 0x00551A28
		private static float Angle
		{
			get
			{
				Vector2 vector = GenWorldUI.WorldToUIPosition(Find.WorldGrid.NorthPolePos);
				Vector2 a = new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
				vector.y = (float)UI.screenHeight - vector.y;
				return a.AngleTo(vector);
			}
		}

		// Token: 0x0600E575 RID: 58741 RVA: 0x0055387C File Offset: 0x00551A7C
		public static void CompassOnGUI(ref float curBaseY)
		{
			CompassWidget.CompassOnGUI(new Vector2((float)UI.screenWidth - 10f - 32f, curBaseY - 10f - 32f));
			curBaseY -= 84f;
		}

		// Token: 0x0600E576 RID: 58742 RVA: 0x005538B4 File Offset: 0x00551AB4
		private static void CompassOnGUI(Vector2 center)
		{
			Widgets.DrawTextureRotated(center, CompassWidget.CompassTex, CompassWidget.Angle, 1f, null);
			Rect rect = new Rect(center.x - 32f, center.y - 32f, 64f, 64f);
			TooltipHandler.TipRegionByKey(rect, "CompassTip");
			if (Widgets.ButtonInvisible(rect, true))
			{
				Find.WorldCameraDriver.RotateSoNorthIsUp(true);
			}
		}

		// Token: 0x040091BB RID: 37307
		private const float Padding = 10f;

		// Token: 0x040091BC RID: 37308
		private const float Size = 64f;

		// Token: 0x040091BD RID: 37309
		private static readonly Texture2D CompassTex = ContentFinder<Texture2D>.Get("UI/Misc/Compass", true);
	}
}
