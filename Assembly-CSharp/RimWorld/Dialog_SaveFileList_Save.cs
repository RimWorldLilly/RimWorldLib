using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD7 RID: 7383
	public class Dialog_SaveFileList_Save : Dialog_SaveFileList
	{
		// Token: 0x1700228C RID: 8844
		// (get) Token: 0x0600BDCE RID: 48590 RVA: 0x00442EB5 File Offset: 0x004410B5
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BDCF RID: 48591 RVA: 0x00442EB8 File Offset: 0x004410B8
		public Dialog_SaveFileList_Save()
		{
			this.interactButLabel = "OverwriteButton".Translate();
			if (Faction.OfPlayer.HasName)
			{
				this.typingName = Faction.OfPlayer.Name;
				return;
			}
			this.typingName = SaveGameFilesUtility.UnusedDefaultFileName(Faction.OfPlayer.def.LabelCap);
		}

		// Token: 0x0600BDD0 RID: 48592 RVA: 0x00442F1C File Offset: 0x0044111C
		protected override void DoFileInteraction(string mapName)
		{
			mapName = GenFile.SanitizedFileName(mapName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveGame(mapName);
			}, "SavingLongEvent", false, null, true, false, null);
			Messages.Message("SavedAs".Translate(mapName), MessageTypeDefOf.SilentInput, false);
			PlayerKnowledgeDatabase.Save();
			this.Close(true);
		}
	}
}
