using System;

namespace Verse
{
	// Token: 0x0200052F RID: 1327
	public static class Scribe_Defs
	{
		// Token: 0x0600272C RID: 10028 RVA: 0x000F4FAC File Offset: 0x000F31AC
		public static void Look<T>(ref T value, string label) where T : Def, new()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				string text;
				if (value == null)
				{
					text = "null";
				}
				else
				{
					text = value.defName;
				}
				Scribe_Values.Look<string>(ref text, label, "null", false);
				return;
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = ScribeExtractor.DefFromNode<T>(Scribe.loader.curXmlParent[label]);
			}
		}
	}
}
