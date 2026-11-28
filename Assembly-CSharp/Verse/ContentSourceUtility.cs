using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000694 RID: 1684
	[StaticConstructorOnStartup]
	public static class ContentSourceUtility
	{
		// Token: 0x06003519 RID: 13593 RVA: 0x001596F0 File Offset: 0x001578F0
		public static Texture2D GetIcon(this ContentSource s)
		{
			switch (s)
			{
			case ContentSource.Undefined:
				return BaseContent.BadTex;
			case ContentSource.OfficialModsFolder:
				return ContentSourceUtility.ContentSourceIcon_OfficialModsFolder;
			case ContentSource.ModsFolder:
				return ContentSourceUtility.ContentSourceIcon_ModsFolder;
			case ContentSource.SteamWorkshop:
				return ContentSourceUtility.ContentSourceIcon_SteamWorkshop;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x00159728 File Offset: 0x00157928
		public static void DrawContentSource(Rect r, ContentSource source, Action clickAction = null)
		{
			Rect rect = new Rect(r.x, r.y + r.height / 2f - 12f, 24f, 24f);
			GUI.DrawTexture(rect, source.GetIcon());
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => "Source".Translate() + ": " + source.HumanLabel(), (int)(r.x + r.y * 56161f));
				Widgets.DrawHighlight(rect);
			}
			if (clickAction != null && Widgets.ButtonInvisible(rect, true))
			{
				clickAction();
			}
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x001597CE File Offset: 0x001579CE
		public static string HumanLabel(this ContentSource s)
		{
			return ("ContentSource_" + s.ToString()).Translate();
		}

		// Token: 0x040022D2 RID: 8914
		public const float IconSize = 24f;

		// Token: 0x040022D3 RID: 8915
		private static readonly Texture2D ContentSourceIcon_OfficialModsFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/OfficialModsFolder", true);

		// Token: 0x040022D4 RID: 8916
		private static readonly Texture2D ContentSourceIcon_ModsFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/ModsFolder", true);

		// Token: 0x040022D5 RID: 8917
		private static readonly Texture2D ContentSourceIcon_SteamWorkshop = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/SteamWorkshop", true);
	}
}
