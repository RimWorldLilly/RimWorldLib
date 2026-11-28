using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F29 RID: 7977
	public class ShortcutKeys
	{
		// Token: 0x0600CB8A RID: 52106 RVA: 0x004B000C File Offset: 0x004AE20C
		public void ShortcutKeysOnGUI()
		{
			if (Current.ProgramState == ProgramState.Playing && !WorldComponent_GravshipController.CutsceneInProgress)
			{
				if (KeyBindingDefOf.NextColonist.KeyDownEvent)
				{
					ThingSelectionUtility.SelectNextColonist();
					Event.current.Use();
				}
				if (KeyBindingDefOf.PreviousColonist.KeyDownEvent)
				{
					ThingSelectionUtility.SelectPreviousColonist();
					Event.current.Use();
				}
			}
		}
	}
}
