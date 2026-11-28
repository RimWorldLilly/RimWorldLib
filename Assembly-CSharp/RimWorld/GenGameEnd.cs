using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F1C RID: 7964
	public static class GenGameEnd
	{
		// Token: 0x0600CB23 RID: 52003 RVA: 0x004AA414 File Offset: 0x004A8614
		public static void EndGameDialogMessage(string msg, bool allowKeepPlaying = true)
		{
			GenGameEnd.EndGameDialogMessage(msg, allowKeepPlaying, Color.clear);
		}

		// Token: 0x0600CB24 RID: 52004 RVA: 0x004AA424 File Offset: 0x004A8624
		public static void EndGameDialogMessage(string msg, bool allowKeepPlaying, Color screenFillColor)
		{
			DiaNode diaNode = new DiaNode(msg);
			if (allowKeepPlaying)
			{
				DiaOption diaOption = new DiaOption("GameOverKeepWatching".Translate());
				diaOption.resolveTree = true;
				diaNode.options.Add(diaOption);
				DiaOption diaOption2 = new DiaOption("GameOverCreateNewWanderers".Translate());
				diaOption2.resolveTree = true;
				diaOption2.action = delegate()
				{
					Find.WindowStack.Add(new Dialog_ChooseNewWanderers());
				};
				diaNode.options.Add(diaOption2);
			}
			DiaOption diaOption3 = new DiaOption("GameOverMainMenu".Translate());
			diaOption3.action = delegate()
			{
				GenScene.GoToMainMenu();
			};
			diaOption3.resolveTree = true;
			diaNode.options.Add(diaOption3);
			Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(diaNode, true, false, null);
			dialog_NodeTree.screenFillColor = screenFillColor;
			dialog_NodeTree.silenceAmbientSound = !allowKeepPlaying;
			dialog_NodeTree.closeOnAccept = allowKeepPlaying;
			dialog_NodeTree.closeOnCancel = allowKeepPlaying;
			Find.WindowStack.Add(dialog_NodeTree);
			Find.Archive.Add(new ArchivedDialog(diaNode.text, null, null));
		}
	}
}
