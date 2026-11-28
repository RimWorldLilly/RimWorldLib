using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000712 RID: 1810
	[StaticConstructorOnStartup]
	public static class CustomCursor
	{
		// Token: 0x0600392C RID: 14636 RVA: 0x0016EDB5 File Offset: 0x0016CFB5
		public static void Activate()
		{
			Cursor.SetCursor(CustomCursor.CursorTex, CustomCursor.CursorHotspot, CursorMode.Auto);
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x0016EDC7 File Offset: 0x0016CFC7
		public static void Deactivate()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}

		// Token: 0x040025A5 RID: 9637
		private static readonly Texture2D CursorTex = ContentFinder<Texture2D>.Get("UI/Cursors/CursorCustom", true);

		// Token: 0x040025A6 RID: 9638
		private static Vector2 CursorHotspot = new Vector2(3f, 3f);
	}
}
