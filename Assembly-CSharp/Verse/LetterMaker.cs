using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000616 RID: 1558
	public static class LetterMaker
	{
		// Token: 0x060030B6 RID: 12470 RVA: 0x001381C3 File Offset: 0x001363C3
		public static Letter MakeLetter(LetterDef def)
		{
			Letter letter = (Letter)Activator.CreateInstance(def.letterClass);
			letter.def = def;
			letter.ID = Find.UniqueIDsManager.GetNextLetterID();
			return letter;
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x001381EC File Offset: 0x001363EC
		public static ChoiceLetter MakeLetter(TaggedString label, TaggedString text, LetterDef def, Faction relatedFaction = null, Quest quest = null)
		{
			if (!typeof(ChoiceLetter).IsAssignableFrom(def.letterClass))
			{
				Log.Error(((def != null) ? def.ToString() : null) + " is not a choice letter.");
				return null;
			}
			ChoiceLetter choiceLetter = (ChoiceLetter)LetterMaker.MakeLetter(def);
			choiceLetter.Label = label;
			choiceLetter.Text = text;
			choiceLetter.relatedFaction = relatedFaction;
			choiceLetter.quest = quest;
			return choiceLetter;
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x00138256 File Offset: 0x00136456
		public static ChoiceLetter MakeLetter(TaggedString label, TaggedString text, LetterDef def, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null)
		{
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, def, null, null);
			choiceLetter.lookTargets = lookTargets;
			choiceLetter.relatedFaction = relatedFaction;
			choiceLetter.quest = quest;
			choiceLetter.hyperlinkThingDefs = hyperlinkThingDefs;
			return choiceLetter;
		}
	}
}
